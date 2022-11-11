using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    //プレイヤーのポジション
    [SerializeField] private GameObject playerObject;

    Rigidbody rigidbody;

    private bool stopFlag;
    private bool deadFlag;

    [SerializeField] private int hp = 5;

    [SerializeField] private float speed = 0.1f;

    [SerializeField] private GameObject gunModel;
    [SerializeField] private GameObject gunPosition;
    [SerializeField] private GameObject firingPoint;
    [SerializeField] private GameObject bullet;
    [SerializeField] private float bulletSpeed = 60.0f;
    const int shotDelayMaxTime = 30;
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

    // Start is called before the first frame update
    void Start()
    {
        stopFlag = false;
        deadFlag = false;
        rigidbody = GetComponent<Rigidbody>();
        rigidbody.drag = 50;
    }

    // Update is called once per frame
    void Update()
    {
        //弾の発射処理
        gunModel.transform.position = gunPosition.transform.position;
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

        transform.LookAt(playerObject.transform);
        transform.position += transform.forward * speed;

        //ステップ処理
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

        if (hp <= 0)
        {
            deadFlag = true;
        }

        if (deadFlag)
        {
            GameObject newExplosion = Instantiate(explosion, this.gameObject.transform.position, Quaternion.Euler(0, 0, 0));
            newExplosion.transform.localScale = explosionSize;
            Destroy(newExplosion, 1.0f);
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        string gameObjectName = collision.gameObject.tag;
        if (gameObjectName != "Bullet"&& gameObjectName != "Grenade" && gameObjectName == "EnemyBullet") { return; }

        if(gameObjectName == "Bullet")
		{
            hp--;
        }
        else if(gameObjectName == "Grenade")
		{
            hp -= 10;
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
        GameObject newBall = Instantiate(bullet, bulletPosition, gunModel.transform.rotation);
        // 出現させたボールのforward(z軸方向)
        var direction = newBall.transform.forward;
        // 弾の発射方向にnewBallのz方向(ローカル座標)を入れ、弾オブジェクトのrigidbodyに衝撃力を加える
        newBall.GetComponent<Rigidbody>().AddForce(direction * bulletSpeed, ForceMode.Impulse);
        // 出現させたボールの名前を"bullet"に変更
        newBall.name = bullet.name;
        // 出現させたボールを0.8秒後に消す
        Destroy(newBall, bulletDestroyTime);
    }
}
