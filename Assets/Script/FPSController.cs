using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSController : MonoBehaviour
{
    const float normalSpeed = 5.0f;
    const float sprintSpeed = 15.0f;
    float speed = normalSpeed;

    const int accelMaxCount = 2;
    int accelCount = accelMaxCount;

    const int accelMaxTime = 120;
    int accelTime = accelMaxTime;

    const int accelRechargeMaxCount = 180;
    int accelRechargeCount = accelRechargeMaxCount;

    public GameObject cam;
    Quaternion cameraRot, characterRot;

    Rigidbody rigidbody;

    //XY方向の視点感度
    public float Xsensityvity, Ysensityvity;

    bool deadFlag;

    //変数の宣言(角度の制限用)
    float minX;
    float maxX;

    int chargeCount = 0;

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
    [SerializeField]
    private GameObject bullet;
    private float bulletSpeed = 60.0f;
    const int shotDelayMaxTime = 10;
    private int shotDelayTime = shotDelayMaxTime;

    private bool landingFlag;

    //ショットガン用変数
    [SerializeField]
    float randomDiffusion = 10;
    [SerializeField]
    int bulletCount = 10;
    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = 60;
        cameraRot = cam.transform.localRotation;
        characterRot = transform.localRotation;
        deadFlag = false;
        const float normalMaxX = 90.0f;
        const float normalMinX = -90.0f;
        minX = normalMinX;
        maxX = normalMaxX;
        rigidbody = GetComponent<Rigidbody>();
        //カーソルの表示
        Cursor.visible = false;
        //カーソルのロック
        Cursor.lockState = CursorLockMode.Locked;
        landingFlag = true;
        Physics.gravity = new Vector3(0.0f, -4.0f, 0.0f);
    }

    // Update is called once per frame
    void Update()
    {
        //カメラの移動処理
        MoveCameraProcessing();

        if(Input.GetKey(KeyCode.Space))
		{//ジャンプ処理
            JumpProcessing();
		}

        if(Input.GetKey(KeyCode.LeftShift))
		{
            chargeCount++;
            if(chargeCount>=30)
            {//スプリント処理
                SprintProcessing();
            }
        }

        //加速ゲージのリチャージ処理
        AccelRechargeProcessing();

        //カーソルの非表示
        if (Input.GetKeyDown(KeyCode.Escape))
		{
            Cursor.visible = true;
		}

        if(Input.GetMouseButton(1))
		{//銃を構える処理
            HoldGun();
		}
        else if (Input.GetMouseButton(0))
        {//弾の発射処理
            gunModel.transform.position = normalGunPosition.transform.position;
            if(shotDelayTime>0)
			{
                shotDelayTime--;
			}
            else
			{
                //弾の発射処理
                Shot();
                shotDelayTime = shotDelayMaxTime;
            }
        }
        else
        {
            gunModel.transform.position = normalGunPosition.transform.position;
        }
        //移動処理
        MoveProcessing();
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

            if (Input.GetKeyUp(KeyCode.LeftShift))
            {//加速処理
                AccelProcessing(velocity);
            }

            if (Input.GetMouseButton(1) == false)
			{//呼吸演出処理
                BreathProcessing();
            }

            rigidbody.drag = 1;
        }

        if (accelTime == 0)
        {
            rigidbody.drag = 50;
        }
        else
		{
            accelTime--;
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

        angleX = Mathf.Clamp(angleX, minX, maxX);

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
        //X軸視点
        float xRot = Input.GetAxis("Mouse X") * Xsensityvity;
        characterRot *= Quaternion.Euler(0, xRot, 0);

        transform.localRotation = characterRot;

        //Updateの中で作成した関数を呼ぶ
        cameraRot = ClampRotation(cameraRot);

        cam.transform.localRotation = cameraRot;
    }
    /// <summary>
    /// 加速処理
    /// </summary>
    public void AccelProcessing(Vector3 arg_velocity)
	{
        if (chargeCount < 30)
        {//加速処理
            if (accelCount > 0)
            {
                accelTime = accelMaxTime;
                accelCount--;
                rigidbody.AddForce(arg_velocity * 400.0f);
                Debug.Log(accelCount);
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
        speed = sprintSpeed;
        shakingSpeed = shakingMaxSpeed;
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
        yRot += Mathf.Sin(Time.time * shakingSpeed) * 0.5f;

        gunModel.transform.rotation *= Quaternion.Euler(-yRot, 0, 0);
    }
    /// <summary>
    /// 弾の発射処理
    /// </summary>
    private void Shot()
	{
        for (int n = 0; n < bulletCount; n++)
        {
            // 弾を発射する場所を取得
            var bulletPosition = firingPoint.transform.position;
            // 上で取得した場所に、"bullet"のPrefabを出現させる
            GameObject newBall = Instantiate(bullet, bulletPosition, cam.transform.rotation);
            //ランダムの方向に拡散
            float randomX = Random.Range(randomDiffusion, -randomDiffusion);
            float randomY = Random.Range(randomDiffusion, -randomDiffusion);
            float randomZ = Random.Range(randomDiffusion, - randomDiffusion);

            // 出現させたボールのforward(z軸方向)
            var direction = new Vector3(randomX,randomY,randomZ);
            // 弾の発射方向にnewBallのz方向(ローカル座標)を入れ、弾オブジェクトのrigidbodyに衝撃力を加える
            newBall.GetComponent<Rigidbody>().AddForce(direction , ForceMode.Impulse);
            newBall.GetComponent<Rigidbody>().AddForce(transform.forward * bulletSpeed, ForceMode.Impulse);
            // 出現させたボールの名前を"bullet"に変更
            newBall.name = bullet.name;
            // 出現させたボールを0.8秒後に消す
            Destroy(newBall, 0.8f);
        }
    }
    /// <summary>
    /// 銃を構える処理
    /// </summary>
    private void HoldGun()
	{
        gunModel.transform.position = holdGunPosition.transform.position;
        if (Input.GetMouseButton(0))
        {//弾の発射処理
            if (shotDelayTime > 0)
            {
                shotDelayTime--;
            }
            else
            {
                //弾の発射処理
                Shot();
                shotDelayTime = shotDelayMaxTime;
            }
        }
    }
    /// <summary>
    /// 加速ゲージのリチャージ処理
    /// </summary>
    private void AccelRechargeProcessing()
	{
        if (accelCount < accelMaxCount)
		{
            accelRechargeCount--;
            //リチャージカウントがゼロかどうか
            bool isAccelRechargeCountEmpty = accelRechargeCount <= 0;

            if (isAccelRechargeCountEmpty)
			{
                accelCount++;
                accelRechargeCount = accelRechargeMaxCount;

                Debug.Log(accelCount);
            }
		}
	}
}
