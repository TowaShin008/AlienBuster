using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalGun : MonoBehaviour
{
    [SerializeField]
    private GameObject bullet;
    private float bulletSpeed = 30.0f;
    const int shotDelayMaxTime = 5;
    private int shotDelayTime = 0;
    [SerializeField]
    private GameObject firingPoint;


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
    public void Shot(Quaternion arg_cameraRotation)
	{
        if (shotDelayTime <= 0)
        {
            //弾の発射処理
            // 弾を発射する場所を取得
            var bulletPosition = firingPoint.transform.position;
            // 上で取得した場所に、"bullet"のPrefabを出現させる
            GameObject newBall = Instantiate(bullet, bulletPosition, arg_cameraRotation);
            // 出現させたボールのforward(z軸方向)
            var direction = newBall.transform.forward;
            // 弾の発射方向にnewBallのz方向(ローカル座標)を入れ、弾オブジェクトのrigidbodyに衝撃力を加える
            newBall.GetComponent<Rigidbody>().AddForce(direction * bulletSpeed, ForceMode.Impulse);
            // 出現させたボールの名前を"bullet"に変更
            newBall.name = bullet.name;
            // 出現させたボールを0.8秒後に消す
            Destroy(newBall, 0.8f);

            shotDelayTime = shotDelayMaxTime;
        }
    }
}
