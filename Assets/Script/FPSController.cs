using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Util;
public class FPSController : MonoBehaviour
{
    //通常銃
    [SerializeField]
    private GameObject normalGun;
    //グレネートランチャー
    [SerializeField]
    private GameObject rocketLauncher;
    //スナイパーライフル
    [SerializeField]
    private GameObject sniperRifle;
    //ショットガン
    [SerializeField]
    private GameObject shotGun;

    [SerializeField]
    private GameObject normalGunPosition;
    [SerializeField]
    private GameObject holdGunPosition;

    private BoxCollider collider;

    //プレイヤー移動速度
    const float normalSpeed = 5.0f;
    const float sprintSpeed = 15.0f;
    float speed = normalSpeed;

    //ステップ時の摩擦が強くなるまでの猶予時間
    const int stepMaxTime = 120;
    int stepTime = 0;

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

    //残機
    int remain;
    const int maxHP = 3;
    int hp = maxHP;
    bool deadFlag;

    //銃の揺れ演出の変数
    const float shakingNormalSpeed = 10.0f;
    const float shakingMaxSpeed = 15.0f;
    float shakingSpeed = shakingNormalSpeed;

    int gunType = 1;
    //スナイパーライフルのUI
    public Image sniperEdge;
    public Image sniperGaugeEdge;
    public Image sniperGauge;

    [SerializeField]
    private AudioSource jumpAudioSource;
    [SerializeField]
    private AudioSource stepAudioSource;

    //ポーズに使うもの達
    [SerializeField]
    GameObject pauseObject;
    Vector3 savePosition;
    Quaternion saveCamera;
    Quaternion saveplayerRotation;
    // Start is called before the first frame update
    void Start()
    {
        cameraRot = cam.transform.localRotation;
        characterRot = transform.localRotation;
        deadFlag = false;
        rigidbody = GetComponent<Rigidbody>();
        collider = GetComponent<BoxCollider>();
        //カーソルの表示
        Cursor.visible = false;
        //カーソルのロック
        Cursor.lockState = CursorLockMode.Locked;
        Physics.gravity = new Vector3(0.0f, -6.0f, 0.0f);

        //残機
        remain = 1;
        hp = maxHP;
        normalGun.SetActive(true);
        rocketLauncher.SetActive(false);
        sniperRifle.SetActive(false);
        shotGun.SetActive(false);
        //スナイパーライフルのUI
        sniperEdge.enabled = false;
        sniperGaugeEdge.enabled = false;
        sniperGauge.enabled = false;

        //位置の保存
        savePosition =new Vector3(0,0,0);
        saveCamera = new Quaternion(0,0,0,0);
        saveplayerRotation = new Quaternion(0, 0, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (pauseObject.activeSelf)
        {
            gameObject.transform.position = savePosition;
            cam.transform.localRotation = saveCamera;
            transform.localRotation = saveplayerRotation;
            jumpAudioSource.Stop();
            stepAudioSource.Stop();
            return;
        }
        
        //視点移動処理
        MoveCameraProcessing();
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown("joystick button 5"))
		{
            jumpAudioSource.Play();
        }

        if (Input.GetKey(KeyCode.Space) || Input.GetKey("joystick button 5"))
		{//ジャンプ処理
            JumpProcessing();
		}
        else
		{
            jumpAudioSource.Stop();
        }

        //ステップゲージのリチャージ処理
        StaminaRechargeProcessing();

        float lTri = Input.GetAxis("L_Trigger");
        float rTri = Input.GetAxis("R_Trigger");

        if (Input.GetMouseButton(1) || lTri > 0)
		{//銃を構える処理
			HoldGun();
		}
        else
        {//マウス入力がない場合は、銃を構えない。
            normalGun.transform.position = normalGunPosition.transform.position;
            rocketLauncher.transform.position = normalGunPosition.transform.position;
            //sniperRifle.transform.position = normalGunPosition.transform.position;
            shotGun.transform.position = normalGunPosition.transform.position;
        }

        if (Input.GetMouseButton(0) || rTri > 0)
		{//弾の発射処理
			Shot();
		}


		//移動処理
		MoveProcessing();

        if (Input.GetKeyDown(KeyCode.Escape))
        {//カーソルの非表示
            Cursor.visible = true;
        }

        var currentPosition = gameObject.transform.position;

        if (currentPosition.z > Constants.stageMaxPositionZ)
		{
            currentPosition.z = Constants.stageMaxPositionZ;
        }
        if (currentPosition.z < Constants.stageMinPositionZ)
        {
            currentPosition.z = Constants.stageMinPositionZ;
        }
        if (currentPosition.x > Constants.stageMaxPositionX)
        {
            currentPosition.x = Constants.stageMaxPositionX;
        }
        if (currentPosition.x < Constants.stageMinPositionX)
        {
            currentPosition.x = Constants.stageMinPositionX;
        }

        gameObject.transform.position = currentPosition;
        if (!pauseObject.activeSelf)
        {
            savePosition = gameObject.transform.position;
            saveCamera = cam.transform.localRotation;
            saveplayerRotation = transform.localRotation;
        }
        // Debug.Log(stamina);
    }
    /// <summary>
    /// 移動処理
    /// </summary>
    private void MoveProcessing()
	{
        float lsh = Input.GetAxis("L_Stick_H");
        float lsv = Input.GetAxis("L_Stick_V");

        var velocity = new Vector3(0, 0, 0);
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D) || lsh != 0 || lsv != 0)
        {
            //移動処理
            if (Input.GetKey(KeyCode.W) || lsv > 0)
            {
                velocity += gameObject.transform.rotation * new Vector3(0, 0, speed);
            }
            if (Input.GetKey(KeyCode.A) || lsh < 0)
            {
                velocity += gameObject.transform.rotation * new Vector3(-speed, 0, 0);
            }
            if (Input.GetKey(KeyCode.S) || lsv < 0)
            {
                velocity += gameObject.transform.rotation * new Vector3(0, 0, -speed);
            }
            if (Input.GetKey(KeyCode.D) || lsh > 0)
            {
                velocity += gameObject.transform.rotation * new Vector3(speed, 0, 0);
            }
            this.transform.position += velocity * Time.deltaTime;


            if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey("joystick button 4"))
            {
                chargeCount++;
                //スプリント処理
                SprintProcessing();
            }

            if (Input.GetKeyUp(KeyCode.LeftShift) || Input.GetKeyUp("joystick button 4"))
            {//ステップ処理
                stepAudioSource.Play();
                StepProcessing(velocity);
            }

			if (Input.GetMouseButton(1) == false)
			{//呼吸演出処理
				BreathProcessing();
			}
        }
        if (stepTime > 0)
        {//ステップをしていないか、ステップ猶予時間でなければ抗力を強くする
            stepTime--;
            rigidbody.drag = 0;
		}
    }
    /// <summary>
    /// 角度制限関数の作成
    /// </summary>
    /// <param name="q">対象となるもののクォータニオン</param>
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
        if (collision.gameObject.tag == "Enemy" || collision.gameObject.tag == "EnemyBullet")
        {
            Debug.Log("Hit");
            Damage();
        }

        if(collision.gameObject.tag == "WeaponItem")
		{
            if (collision.gameObject.name == "NormalGunItem")
            {
                normalGun.SetActive(true);
                rocketLauncher.SetActive(false);
                sniperRifle.SetActive(false);
                shotGun.SetActive(false);
                gunType = 1;
            }
            else if (collision.gameObject.name == "RocketLauncherItem")
            {
                normalGun.SetActive(false);
                rocketLauncher.SetActive(true);
                sniperRifle.SetActive(false);
                shotGun.SetActive(false);
                gunType = 2;
            }
            else if (collision.gameObject.name == "SniperRifleItem")
            {
                normalGun.SetActive(false);
                rocketLauncher.SetActive(false);
                sniperRifle.SetActive(true);
                shotGun.SetActive(false);
                gunType = 3;
            }
            else if (collision.gameObject.name == "ShotGunItem")
            {
                normalGun.SetActive(false);
                rocketLauncher.SetActive(false);
                sniperRifle.SetActive(false);
                shotGun.SetActive(true);
                gunType = 4;
            }
        }
    }

	private void OnCollisionStay(Collision collision)
	{
        if (stepTime == 0 && collision.gameObject.CompareTag("Field"))
        {//ステップをしていないか、ステップ猶予時間でなければ摩擦を強くする
            rigidbody.drag = 100;
        }
    }

	private void OnCollisionExit(Collision collision)
	{
        if (collision.gameObject.CompareTag("Field"))
        {//ステップをしていないか、ステップ猶予時間でなければ摩擦を強くする
            rigidbody.drag = 0;
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

		//      if (deadFlag == false)
		//{
		//          transform.localRotation = characterRot;
		//      }

		float rsh = Input.GetAxis("R_Stick_H") * Xsensityvity;
        float rsv = Input.GetAxis("R_Stick_V") * Ysensityvity;

        cameraRot *= Quaternion.Euler(-rsv, 0, 0);
        characterRot *= Quaternion.Euler(0, rsh, 0);

        if (deadFlag == false)
        {
            transform.localRotation = characterRot;
        }

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
                rigidbody.AddForce(arg_velocity * 5.0f, ForceMode.Impulse);
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
        rigidbody.drag = 0;
        rigidbody.AddForce(new Vector3(0.0f, 10.0f, 0.0f));
	}
    /// <summary>
    /// 呼吸演出
    /// </summary>
    private void BreathProcessing()
	{
        //マウスのY軸ポジションの取得
        float yRot = normalGun.transform.localRotation.eulerAngles.x;
        //三角関数を使い銃を縦に揺らす
        yRot += Mathf.Sin(Time.time * shakingSpeed) * 0.5f;

        normalGun.transform.rotation *= Quaternion.Euler(-yRot, 0, 0);

        shotGun.transform.rotation *= Quaternion.Euler(-yRot, 0, 0);

        //マウスのY軸ポジションの取得
        float rocketLauncheryRot = rocketLauncher.transform.localRotation.eulerAngles.x;
        //三角関数を使い銃を縦に揺らす
        rocketLauncheryRot += Mathf.Sin(Time.time * shakingSpeed) * 0.5f;

        rocketLauncher.transform.rotation *= Quaternion.Euler(-rocketLauncheryRot, 0, 0);

        //マウスのY軸ポジションの取得
        float sniperRifleyRot = sniperRifle.transform.localRotation.eulerAngles.x;
        //三角関数を使い銃を縦に揺らす
        sniperRifleyRot += Mathf.Sin(Time.time * shakingSpeed) * 0.5f;

        sniperRifle.transform.rotation *= Quaternion.Euler(-sniperRifleyRot, 0, 0);
    }
    /// <summary>
    /// 腰だめうち
    /// </summary>
 //   private void HipShot()
	//{
 //       normalGun.transform.position = normalGunPosition.transform.position;
 //       rocketLauncher.transform.position = normalGunPosition.transform.position;
 //       //sniperRifle.transform.position = normalGunPosition.transform.position;
 //       shotGun.transform.position = normalGunPosition.transform.position;

 //       //弾の発射処理
 //       Shot();
 //   }
    /// <summary>
    /// 銃を構える処理と発射処理
    /// </summary>
    private void HoldGun()
	{
        normalGun.transform.position = holdGunPosition.transform.position;
        rocketLauncher.transform.position = holdGunPosition.transform.position;
        //sniperRifle.transform.position = holdGunPosition.transform.position;
        shotGun.transform.position = holdGunPosition.transform.position;

        //if (Input.GetMouseButton(0) || arg_tri > 0)
        //{//弾の発射処理
        //    Shot();
        //}
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
    /// <summary>
    /// ダメージ処理
    /// </summary>
    public void Damage()
	{
        hp--;
        if (hp < 1)
		{
            if (hp <= 0)
			{//残機を減らす
                DecrimentRemain();
			}
		}
	}
    /// <summary>
    /// 残機の減少
    /// </summary>
    public void DecrimentRemain()
	{
        remain--;
        if (remain <= 0)
        {
            deadFlag = true;
            rigidbody.drag = 0;
            FadeManager.Instance.LoadScene("EndScene", 0.5f);
        }
    }
    /// <summary>
    /// プレイヤーの射撃処理
    /// </summary>
    private void Shot()
	{
        if (gunType == 1)
        {
            normalGun.GetComponent<NormalGun>().Shot(cam.transform.rotation);
        }
        else if (gunType == 2)
        {
            rocketLauncher.GetComponent<RocketLauncher>().Shot(cam.transform.rotation);
        }
        else if (gunType == 3)
        {
            sniperRifle.GetComponent<SniperScript>().Shot(cam.transform.rotation);
        }
        else if (gunType == 4)
		{
            shotGun.GetComponent<ShotGun>().Shot(cam.transform.rotation);
        }
    }
    /// <summary>
    /// 銃番号のセット
    /// </summary>
    /// <param name="arg_gunType">銃番号</param>
    public void SetGunType(int arg_gunType)
	{
        gunType = arg_gunType;
	}
    /// <summary>
    /// 銃番号のセット取得
    /// </summary>
    /// <param name="arg_gunType">銃番号</param>
    public int GetGunType()
    {
        return gunType;
    }
    /// <summary>
    /// 最大スタミナの取得
    /// </summary>
    /// <returns>最大スタミナ</returns>
    public int GetMaxStamina()
    {
        return maxStamina;
    }
    /// <summary>
    /// スタミナの取得
    /// </summary>
    /// <returns>スタミナ</returns>
    public int GetStamina()
    {
        return stamina;
    }

    public int GetHP()
	{
        return hp;
	}
    public int GetMaxHP()
    {
        return maxHP;
    }
}
