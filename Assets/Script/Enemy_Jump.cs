using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Jump: MonoBehaviour
{
    //プレイヤーのポジション
    [SerializeField] private GameObject playerObject;

    Rigidbody rigidbody;

    private bool stopFlag;
    private bool deadFlag;

    [SerializeField] private int hp = 5;

    //[SerializeField] private float speed = 0.05f;

    [SerializeField] private GameObject gunModel;
    [SerializeField] private GameObject gunPosition;
    [SerializeField] private GameObject firingPoint;
    [SerializeField] private GameObject bullet;
    [SerializeField] private float bulletSpeed = 60.0f;
    const int shotDelayMaxTime = 30;
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

    private bool onTheGroundFlog = false;
    private bool jumping = false;

    //爆発エフェクト
    [SerializeField] GameObject explosion;
    [SerializeField] private Vector3 explosionSize = new Vector3(1.0f, 1.0f, 1.0f);

    int randomValue = 0;

    // Start is called before the first frame update
    void Start()
    {
        stopFlag = false;
        deadFlag = false;
        rigidbody = GetComponent<Rigidbody>();
        rigidbody.drag = 0;

        jumpDelayTime = jampDelayMaxTime;
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
        
        // ジャンプの開始判定
        if (onTheGroundFlog == true && jumpDelayTime <= 0)
        {
            jumping = true;
            jumpDelayTime = jampDelayMaxTime;
            onTheGroundFlog = false;
            randomValue = Random.Range(0, 3);
        }

        if (jumpDelayTime > 0 && onTheGroundFlog == true)
        {
            jumpDelayTime--;
        }

        // ジャンプ中の処理
        if (jumping)
        {
            jumpTime += Time.deltaTime;
            if (jumpTime >= maxJumpTime)
            {
                jumping = false;
                jumpTime = 0;
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

    private void FixedUpdate()
    {
        Jump();
    }
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
        if (gameObjectName != "Bullet" && gameObjectName != "Grenade" && gameObjectName == "EnemyBullet" && gameObjectName == "Field") { return; }

        if (gameObjectName == "Bullet")
        {
            hp--;
        }
        else if (gameObjectName == "Grenade")
        {
            hp -= 10;
        }

        if(gameObjectName == "Field")
        {
            onTheGroundFlog = true;
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
