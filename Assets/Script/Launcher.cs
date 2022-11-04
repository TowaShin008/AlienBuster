using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Launcher : MonoBehaviour
{
    [SerializeField]
    private GameObject gunModel;
    [SerializeField]
    private GameObject normalGunPosition;
    [SerializeField]
    private GameObject holdGunPosition;
    [SerializeField]
    private GameObject firingPoint;
    [SerializeField]
    private GameObject grenade;
    [SerializeField]
    private GameObject Explosion;
    private float bulletSpeed = 750.0f;
    //大体10秒に1発
    const int shotDelayMaxTime = 100;
    private int shotDelayTime = shotDelayMaxTime;

    public GameObject cam;
    Quaternion cameraRot, characterRot;

    const float shakingNormalSpeed = 10.0f;
    const float shakingMaxSpeed = 15.0f;
    float shakingSpeed = shakingNormalSpeed;
    Rigidbody rb_grenade;

    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        //移動処理
        MoveProcessing();
        shotDelayTime--;
        if (Input.GetMouseButton(1))
        {//銃を構える処理
            HoldGun();
        }
        else if (Input.GetMouseButton(0))
        {//弾の発射処理
            gunModel.transform.position = normalGunPosition.transform.position;
            if (shotDelayTime > 0)
            {

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
    }

    private void MoveProcessing()
    {
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D))
        {

            if (Input.GetMouseButton(1) == false)
            {//呼吸演出処理
                BreathProcessing();
            }
        }
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
        // 弾を発射する場所を取得
        var bulletPosition = firingPoint.transform.position;
        // 上で取得した場所に、"grenade"のPrefabを出現させる
        GameObject newBall = Instantiate(grenade, bulletPosition, cam.transform.rotation);
        // 出現させたボールのforward(z軸方向)
        var direction = newBall.transform.forward;
        // 弾の発射方向にnewBallのz方向(ローカル座標)を入れ、弾オブジェクトのrigidbodyに衝撃力を加える
        newBall.GetComponent<Rigidbody>().AddForce(Vector3.Lerp(direction, transform.up, 0.1f) * bulletSpeed, ForceMode.Impulse);    
        // 出現させたボールの名前を"grenade"に変更
        newBall.name = grenade.name;       
        // 出現させたボールを2秒後に消す
        Destroy(newBall, 2.5f);
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

}