using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSController : MonoBehaviour
{
    //プレイヤー移動速度
    const float normalSpeed = 5.0f;
    const float sprintSpeed = 15.0f;
    float speed = normalSpeed;

    //ステップ時の摩擦がなくなる猶予時間
    const int stepMaxTime = 120;
    int stepTime = stepMaxTime;

    //LSHIFTの入力時間を格納する変数
    int chargeCount = 0;

    //プレイヤーのスタミナ
    const int maxStamina = 300;
    int stamina = maxStamina;

    //カメラ関連変数
    public GameObject cam;
    Quaternion cameraRot, characterRot;

    Rigidbody rigidbody;

    //XY方向の視点感度
    public float Xsensityvity, Ysensityvity;

    bool deadFlag;

    //銃の揺れ演出の変数
    const float shakingNormalSpeed = 10.0f;
    const float shakingMaxSpeed = 15.0f;
    float shakingSpeed = shakingNormalSpeed;

    [SerializeField]
    private GameObject gunModel;
    [SerializeField]
    private GameObject normalGunPosition;
    [SerializeField]
    private GameObject holdGunPosition;
    [SerializeField]
    private GameObject firingPoint;

    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = 60;
        cameraRot = cam.transform.localRotation;
        characterRot = transform.localRotation;
        deadFlag = false;
        rigidbody = GetComponent<Rigidbody>();
        //カーソルの表示
        Cursor.visible = false;
        //カーソルのロック
        Cursor.lockState = CursorLockMode.Locked;
        Physics.gravity = new Vector3(0.0f, -4.0f, 0.0f);
    }

    // Update is called once per frame
    void Update()
    {
        //カメラの移動処理
        MoveCameraProcessing();

        if (Input.GetKey(KeyCode.Space))
		{//ジャンプ処理
            JumpProcessing();
		}

        //加速ゲージのリチャージ処理
        StaminaRechargeProcessing();

        if(Input.GetMouseButton(1))
		{//銃を構える処理
            HoldGun();
		}
        else if (Input.GetMouseButton(0))
        {//弾の発射処理(腰うち)
            gunModel.transform.position = normalGunPosition.transform.position;

            gunModel.GetComponent<NormalGun>().Shot(firingPoint.transform.position, cam.transform.rotation);
		}
		else
		{//マウス入力がない場合は、銃を構えない。
			gunModel.transform.position = normalGunPosition.transform.position;
		}
		//移動処理
		MoveProcessing();

        //カーソルの非表示
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.visible = true;
        }

        Debug.Log(stamina);
    }
    /// <summary>
    /// 移動処理
    /// </summary>
    private void MoveProcessing()
	{
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D))
        {
            var velocity = new Vector3(0, 0, 0);
            //プレイヤー移動処理
            if (Input.GetKey(KeyCode.W))
            {
                velocity += gameObject.transform.rotation * new Vector3(0, 0, speed);
            }
            if (Input.GetKey(KeyCode.A))
            {
                velocity += gameObject.transform.rotation * new Vector3(-speed, 0, 0);
            }
            if (Input.GetKey(KeyCode.S))
            {
                velocity += gameObject.transform.rotation * new Vector3(0, 0, -speed);
            }
            if (Input.GetKey(KeyCode.D))
            {
                velocity += gameObject.transform.rotation * new Vector3(speed, 0, 0);
            }
            gameObject.transform.position += velocity * Time.deltaTime;


            if (Input.GetKey(KeyCode.LeftShift))
            {
                chargeCount++;
                //スプリント処理
                SprintProcessing();
            }

            if (Input.GetKeyUp(KeyCode.LeftShift))
            {
                //ステップ処理
                StepProcessing(velocity);
            }

            if (Input.GetMouseButton(1) == false)
			{//呼吸演出処理
                BreathProcessing();
            }

            rigidbody.drag = 1;
        }
        //ステップをしていないか、ステップ猶予時間でなければ摩擦を強くする
        if (stepTime == 0)
        {
            rigidbody.drag = 50;
        }
        else
		{
            stepTime--;
		}
    }
    /// <summary>
    /// 角度制限関数の作成
    /// </summary>
    /// <param name="q"></param>
    /// <returns>クォータニオン</returns>
    public Quaternion ClampRotation(Quaternion q)
    {
        //q = x,y,z,w (x,y,zはベクトル（量と向き）：wはスカラー（座標とは無関係の量）)

        q.x /= q.w;
        q.y /= q.w;
        q.z /= q.w;
        q.w = 1f;

        float angleX = Mathf.Atan(q.x) * Mathf.Rad2Deg * 2f;

        angleX = Mathf.Clamp(angleX, -90.0f, 90.0f);

        q.x = Mathf.Tan(angleX * Mathf.Deg2Rad * 0.5f);

        return q;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "Enemy")
        {
            Debug.Log("Hit");
            deadFlag = true;
        }
    }
    /// <summary>
    /// カメラの移動処理
    /// </summary>
    private void MoveCameraProcessing()
	{
        //Y軸視点移動
        float yRot = Input.GetAxis("Mouse Y") * Ysensityvity;
        cameraRot *= Quaternion.Euler(-yRot, 0, 0);
        //X軸視点移動
        float xRot = Input.GetAxis("Mouse X") * Xsensityvity;
        characterRot *= Quaternion.Euler(0, xRot, 0);

        transform.localRotation = characterRot;

        //Updateの中で作成した関数を呼ぶ
        cameraRot = ClampRotation(cameraRot);

        cam.transform.localRotation = cameraRot;
    }
    /// <summary>
    /// ステップ処理
    /// </summary>
    public void StepProcessing(Vector3 arg_velocity)
	{
        if (chargeCount < 30)
        {//ステップ処理
            if (stamina >= 100)
            {
                stepTime = stepMaxTime;
                stamina -= 100;
                rigidbody.AddForce(arg_velocity * 400.0f);
            }
        }
        //プレイヤーの初期化
        speed = normalSpeed;
        shakingSpeed = shakingNormalSpeed;
        chargeCount = 0;
    }
    /// <summary>
    /// スプリント処理
    /// </summary>
    public void SprintProcessing()
	{
        if (chargeCount >= 30 && stamina > 0)
        {//スプリント処理
            stamina -= 2;
            speed = sprintSpeed;
            shakingSpeed = shakingMaxSpeed;
        }
	}
    /// <summary>
    /// ジャンプ処理
    /// </summary>
    public void JumpProcessing()
	{
        rigidbody.AddForce(new Vector3(0.0f, 3.0f, 0.0f));
	}
    /// <summary>
    /// 呼吸演出
    /// </summary>
    private void BreathProcessing()
	{
        //マウスのY軸ポジションの取得
        float yRot = gunModel.transform.localRotation.eulerAngles.x;
        //三角関数を使い銃を縦に揺らす
        yRot += Mathf.Sin(Time.time * shakingSpeed) * 0.5f;

        gunModel.transform.rotation *= Quaternion.Euler(-yRot, 0, 0);
    }
    /// <summary>
    /// 銃を構える処理と発射処理
    /// </summary>
    private void HoldGun()
	{
        gunModel.transform.position = holdGunPosition.transform.position;
        if (Input.GetMouseButton(0))
        {//弾の発射処理
            gunModel.GetComponent<NormalGun>().Shot(firingPoint.transform.position, cam.transform.rotation);
        }
    }
    /// <summary>
    /// スタミナのリチャージ処理
    /// </summary>
    private void StaminaRechargeProcessing()
	{
        if (stamina < maxStamina)
		{
            stamina++;
		}
	}
}
