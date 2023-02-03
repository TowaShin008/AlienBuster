using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Util;

public class JumpEnemy : MonoBehaviour
{
    //プレイヤーのポジション
    [SerializeField] private GameObject playerObject;

    Rigidbody rigidbody;

    private bool deadFlag;

    [SerializeField] private int hp = 5;

    //[SerializeField] private float speed = 0.05f;

    [SerializeField] private GameObject gun;
    [SerializeField] private GameObject firingPoint;
    [SerializeField] private GameObject bullet;
    [SerializeField] private float bulletSpeed = 60.0f;
    const int shotDelayMaxTime = 60;
    private int shotDelayTime = shotDelayMaxTime;

    [SerializeField] private float bulletDestroyTime = 0.8f;

    //ジャンプ力(上方向)
    [SerializeField, Min(0)] float jumpPower = 5.0f;
    //左右へのジャンプ力（前方にも）
    [SerializeField, Min(0)] float aroundJumpPower = 0.15f;
    //ジャンプの速度曲線
    [SerializeField] AnimationCurve jumpCurve = new();
    //ジャンプの最大時間
    [SerializeField, Min(0)] float maxJumpTime = 1.0f;
    private float jumpTime = 0;


    [SerializeField, Min(0)] int jampDelayMaxTime = 120;
    private int jumpDelayTime = 0;

    private bool onTheGroundFlag = false;
    private bool jumping = false;

    //爆発エフェクト
    [SerializeField] GameObject explosion;
    [SerializeField] private Vector3 explosionSize = new Vector3(1.0f, 1.0f, 1.0f);

    int randomValue = 0;

    public GameObject ufo;

    //ドロップする武器
    [SerializeField]
    private GameObject rocketLauncherItem;
    [SerializeField]
    private GameObject sniperRifleItem;
    [SerializeField]
    private GameObject shotGunItem;

    bool stop;
    [SerializeField]
    GameObject pauseObject;
    //ヒット時後ろに吹っ飛ばないように
    [SerializeField, Min(0)] int hitStopMaxTime = 10;
    private int hitStopTime = 10;
    //ダメージ時se
    AudioSource damageAudioSource;
    [SerializeField]
    AudioClip damageAudioClip;

    // Start is called before the first frame update
    void Start()
    {
        stop = false;
        deadFlag = false;
        rigidbody = GetComponent<Rigidbody>();
        rigidbody.drag = 0;

        rigidbody.isKinematic = false;

        jumpDelayTime = jampDelayMaxTime;
        hitStopTime = hitStopMaxTime;
        damageAudioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (pauseObject.activeSelf)
        {
            stop = false;
        }
        else
        {
            stop = true;
        }

        transform.LookAt(playerObject.transform);

        if (stop)
		{
            //弾の発射処理
            gun.transform.position = gun.transform.position;
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


            if (hitStopTime > 0)
            {
                hitStopTime--;
            }


            if (hitStopTime <= 0)
            {
                rigidbody.isKinematic = false;

                // ジャンプの開始判定
                if (onTheGroundFlag == true && jumpDelayTime <= 0)
                {
                    jumping = true;
                    jumpDelayTime = jampDelayMaxTime;
                    onTheGroundFlag = false;
                    randomValue = Random.Range(0, 3);
                    rigidbody.drag = 0;
                }

                if (jumpDelayTime > 0 && onTheGroundFlag == true)
                {
                    jumpDelayTime--;
                    rigidbody.drag = 50;
                }

                // ジャンプ中の処理
                if (jumping)
                {
                    jumpTime += Time.deltaTime;
                    if (jumpTime >= maxJumpTime)
                    {
                        Debug.Log(jumpTime);
                        jumping = false;
                        jumpTime = 0;
                    }

                }
            }
            //ステージ外に出た際のポジションの修正処理
            StageOutProcessing();

            if (hp <= 0)
            {
                deadFlag = true;
                ufo.GetComponent<EnemySpawnManager>().DecrimentEnemyCount();
            }

            if (deadFlag)
            {
                DestroyEnemyUfoCounter.EnemyCounterPlus();
                DropWeapon();
                GameObject newExplosion = Instantiate(explosion, this.gameObject.transform.position, Quaternion.Euler(0, 0, 0));
                newExplosion.transform.localScale = explosionSize;
                Destroy(newExplosion, 1.0f);
                Destroy(gameObject);
            }
        }
    }

    private void FixedUpdate()
    {
        //ジャンプ処理
        Jump();
    }
    /// <summary>
    ///ステージ外に出てしまった際のポジション修正処理
    /// </summary>
    private void StageOutProcessing()
    {
        //ステージ外に出た時にポジションを正しい位置に戻す処理
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
    }
    /// <summary>
    /// ジャンプ処理
    /// </summary>
    void Jump()
    {
        if (!jumping)
        {
            return;
        }

        rigidbody.velocity = new Vector3(rigidbody.velocity.x, 0, rigidbody.velocity.z);
        // ジャンプの速度をアニメーションカーブから取得
        float t = jumpTime / maxJumpTime;
        float power = jumpPower * jumpCurve.Evaluate(t);
        if (t >= 1)
        {
            jumping = false;
            jumpTime = 0;
        }
        rigidbody.AddForce(power * Vector3.up, ForceMode.Impulse);

        switch (randomValue)
        {
            case 0:
                rigidbody.AddForce(aroundJumpPower * transform.right, ForceMode.Impulse);
                break;
            case 1:
                rigidbody.AddForce(aroundJumpPower * -(transform.right), ForceMode.Impulse);
                break;
            case 2:
                rigidbody.AddForce(aroundJumpPower * transform.forward, ForceMode.Impulse);
                break;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        string gameObjectName = collision.gameObject.tag;
        if (gameObjectName != Constants.normalBulletName.ToString() && gameObjectName != Constants.rocketBombName.ToString() && gameObjectName != Constants.sniperBulletName.ToString() && gameObjectName == Constants.enemyBulletName.ToString()) { return; }

        if (gameObjectName == Constants.normalBulletName.ToString())
        {
            rigidbody.isKinematic = true;
            hitStopTime = hitStopMaxTime;
            hp -= Constants.normalBulletDamage;
            damageAudioSource.PlayOneShot(damageAudioClip);
        }
        else if (gameObjectName == Constants.rocketBombName.ToString())
        {
            rigidbody.isKinematic = true;
            hitStopTime = hitStopMaxTime;
            hp -= Constants.rocketBombDamage;
            damageAudioSource.PlayOneShot(damageAudioClip);
        }
        else if (gameObjectName == Constants.sniperBulletName.ToString())
        {
            rigidbody.isKinematic = true;
            hitStopTime = hitStopMaxTime;
            hp -= Constants.sniperBulletDamage;
            damageAudioSource.PlayOneShot(damageAudioClip);
        }

        if (gameObjectName == Constants.fieldName.ToString())
        {
            onTheGroundFlag = true;
        }
    }
    /// <summary>
    /// 弾の発射処理
    /// </summary>
    private void Shot()
    {
        // 弾を発射する場所を取得
        var bulletPosition = firingPoint.transform.position;
        // 上で取得した場所に、"bullet"のPrefabを出現させる     
        GameObject newBall = Instantiate(bullet, bulletPosition, gun.transform.rotation);
        // 出現させたボールのforward(z軸方向)
        var direction = newBall.transform.forward;
        // 弾の発射方向にnewBallのz方向(ローカル座標)を入れ、弾オブジェクトのrigidbodyに衝撃力を加える
        newBall.GetComponent<Rigidbody>().AddForce(direction * bulletSpeed, ForceMode.Impulse);
        // 出現させたボールの名前を"bullet"に変更
        newBall.name = bullet.name;
        // 出現させたボールを0.8秒後に消す
        Destroy(newBall, bulletDestroyTime);
    }
    /// <summary>
    /// 武器のドロップ処理
    /// </summary>
    private void DropWeapon()
    {
        //出現させる敵をランダムに選ぶ
        int randomValue = Random.Range(1, 6);

        int playerGunType = playerObject.GetComponent<FPSController>().GetGunType();

        if (randomValue == playerGunType)
        {
            return;
        }

        if (randomValue == 2)
        {
            rocketLauncherItem.SetActive(true);
            rocketLauncherItem.transform.position = this.transform.position;
        }
        else if (randomValue == 3)
        {
            sniperRifleItem.SetActive(true);
            sniperRifleItem.transform.position = this.transform.position;
        }
        else if (randomValue == 4)
        {
            shotGunItem.SetActive(true);
            shotGunItem.transform.position = this.transform.position;
        }
    }
}
