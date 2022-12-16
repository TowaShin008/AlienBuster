using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Util;

public class StepEnemy : MonoBehaviour
{
    //プレイヤーのポジション
    [SerializeField] private GameObject playerObject;

    Rigidbody rigidbody;

    private bool deadFlag;

    [SerializeField] private int hp = 5;

    [SerializeField] private float speed = 0.1f;

    [SerializeField] private GameObject gun;
    [SerializeField] private GameObject firingPoint;
    [SerializeField] private GameObject bullet;
    [SerializeField] private float bulletSpeed = 60.0f;
    const int shotDelayMaxTime = 60;
    private int shotDelayTime = shotDelayMaxTime;

    [SerializeField] private float bulletDestroyTime = 0.8f;

    [SerializeField] private float stepSpeed = 50.0f;
    const int stepMaxTime = 30;
    private int stepTime = stepMaxTime;
    const int stepDelayMaxTime = 120;
    private int stepDelayTime = stepDelayMaxTime;

    //爆発エフェクト
    [SerializeField] GameObject explosion;
    [SerializeField] private Vector3 explosionSize = new Vector3(1.0f, 1.0f, 1.0f);

    public GameObject ufo;

    //ドロップする武器
    [SerializeField]
    private GameObject normalGunItem;
    [SerializeField]
    private GameObject rocketLauncherItem;
    [SerializeField]
    private GameObject sniperRifleItem;
    [SerializeField]
    private GameObject shotGunItem;

    bool stop;
    [SerializeField]
    GameObject pauseObject;
    // Start is called before the first frame update
    void Start()
    {
        stop = false;
        deadFlag = false;
        rigidbody = GetComponent<Rigidbody>();
        rigidbody.drag = 50;
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

            transform.position += transform.forward * speed;

            //ステップ処理
            StepProcessing();

            StageOutProcessing();

            if (hp <= 0)
            {
                deadFlag = true;
                ufo.GetComponent<EnemySpawnManager>().DecrimentEnemyCount();
            }

            if (deadFlag)
            {
                DropWeapon();
                GameObject newExplosion = Instantiate(explosion, this.gameObject.transform.position, Quaternion.Euler(0, 0, 0));
                newExplosion.transform.localScale = explosionSize;
                Destroy(newExplosion, 1.0f);
                Destroy(gameObject);
            }
        }
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
    /// ステップ処理
    /// </summary>
    private void StepProcessing()
    {
        if (stepTime > 0)
        {
            stepTime--;
            transform.RotateAround(playerObject.transform.position, Vector3.up, stepSpeed * Time.deltaTime);
            stepDelayTime = stepDelayMaxTime;
        }
        else
        {
            if (stepDelayTime > 0)
            {
                stepDelayTime--;
            }
            else
            {
                stepTime = stepMaxTime;
                if (Random.value <= 0.5f)
                {
                    stepSpeed *= -1;
                }
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        string gameObjectName = collision.gameObject.tag;
        if (gameObjectName != Constants.normalBulletName && gameObjectName != Constants.rocketBombName && gameObjectName != Constants.sniperBulletName && gameObjectName == Constants.enemyBulletName) { return; }

        if (gameObjectName == Constants.normalBulletName)
        {
            hp -= Constants.normalBulletDamage;
        }
        else if (gameObjectName == Constants.rocketBombName)
        {
            hp -= Constants.rocketBombDamage;
        }
        else if (gameObjectName == Constants.sniperBulletName)
        {
            hp -= Constants.sniperBulletDamage;
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
        var randomValue = Random.Range(1, 10);

        int playerGunType = playerObject.GetComponent<FPSController>().GetGunType();

        if (randomValue == playerGunType)
        {
            return;
        }

        if (randomValue == 1)
        {
            normalGunItem.SetActive(true);
            normalGunItem.transform.position = this.transform.position;
        }
        else if (randomValue == 2)
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
