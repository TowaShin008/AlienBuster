using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Launcher : MonoBehaviour
{
    [SerializeField]
    private GameObject grenade;
    private float bulletSpeed = 30.0f;
    //大体10秒に1発
    const int shotDelayMaxTime = 100;
    private int shotDelayTime = shotDelayMaxTime;

    //public GameObject cam;
    //Quaternion cameraRot, characterRot;

    //const float shakingNormalSpeed = 10.0f;
    //const float shakingMaxSpeed = 15.0f;
    //float shakingSpeed = shakingNormalSpeed;
    //Rigidbody rb_grenade;

    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        if (shotDelayTime > 0)
        {
            shotDelayTime--;
        }
    }
    /// <summary>
    /// 弾の発射処理
    /// </summary>
    /// <param name="arg_firingPoint">銃のポジション</param>
    /// <param name="arg_cameraRotation">カメラの回転量</param>
    public void Shot(Vector3 arg_firingPoint, Quaternion arg_cameraRotation)
    {
        if (shotDelayTime <= 0)
        {
            //弾の発射処理
            // 弾を発射する場所を取得
            var bulletPosition = arg_firingPoint;
            // 上で取得した場所に、"grenade"のPrefabを出現させる
            GameObject newBall = Instantiate(grenade, bulletPosition, arg_cameraRotation);
            // 出現させたボールのforward(z軸方向)
            var direction = newBall.transform.forward;
            // 弾の発射方向にnewBallのz方向(ローカル座標)を入れ、弾オブジェクトのrigidbodyに衝撃力を加える
            newBall.GetComponent<Rigidbody>().AddForce(direction * bulletSpeed, ForceMode.Impulse);
            Invoke("Explode", 3.0f); // グレネードを発射してから1.5秒後に爆発させる
                                     // 出現させたボールの名前を"bullet"に変更
            newBall.name = grenade.name;

            // 出現させたボールを2秒後に消す
            Destroy(newBall, 2.9f);

            shotDelayTime = shotDelayMaxTime;
        }
    }
    /// <summary>
    /// 爆破演出
    /// </summary>
    void Explode()
    {
        GameObject[] cubes = GameObject.FindGameObjectsWithTag("Enemy"); //「Enemy」タグのついたオブジェクトを全て検索して配列にいれる

        if (cubes.Length == 0) return; // 「Enemy」タグがついたオブジェクトがなければ何もしない。

        foreach (GameObject cube in cubes) // 配列に入れた一つひとつのオブジェクト
        {
            if (cube.GetComponent<Rigidbody>()) // Rigidbodyがあれば、グレネードを中心とした爆発の力を加える
            {
                cube.GetComponent<Rigidbody>().AddExplosionForce(100f, transform.position, 30f, 5f, ForceMode.Impulse);
            }
        }
    }
}