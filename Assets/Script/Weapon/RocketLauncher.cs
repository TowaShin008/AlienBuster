using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketLauncher : MonoBehaviour
{
    [SerializeField]
    private GameObject rocketBomb;
    private float bulletSpeed = 30.0f;
    //大体10秒に1発
    const int shotDelayMaxTime = 100;
    private int shotDelayTime = 0;

    [SerializeField]
    private GameObject firingPoint;

    Quaternion recoilgun;
    Quaternion recoil;
    Quaternion recoilback;
    bool lerp = false;
    bool lerpback = false;

    [SerializeField] float angle = -45.0f;
    Vector3 axis = Vector3.right;
    [SerializeField] float interpolant = 1.5f;
    float sec;

    public AudioClip shotSound;
    AudioSource audioSource;

    [SerializeField]
    private GameObject gunModel;

    private bool shotAgainFlag = true;

    // Start is called before the first frame update
    void Start()
    {
        //音のコンポーネント取得
        audioSource = GetComponent<AudioSource>();
        recoil = Quaternion.AngleAxis(10.0f, new Vector3(0.0f, 0.0f, 1.0f));
        recoilback = Quaternion.AngleAxis(10.0f, new Vector3(0.0f, 0.0f, 1.0f));
        shotAgainFlag = true;
        recoilback = gunModel.transform.localRotation;
    }

    // Update is called once per frame
    void Update()
    {
        if (lerp)
        {
            sec += Time.deltaTime;
            gunModel.transform.localRotation = Quaternion.Lerp(recoilgun, recoil, sec * interpolant);
            if (gunModel.transform.localRotation == recoil)
            {
                sec = 0;
                lerp = false;
                lerpback = true;
                recoilgun = gunModel.transform.localRotation;
            }
        }

        Recoilback();

        if (shotDelayTime > 0)
        {
            shotDelayTime--;
        }
    }
    private void Recoilback()
    {
        if (lerpback == true)
        {
            sec += Time.deltaTime;
            gunModel.transform.localRotation = Quaternion.Lerp(recoilgun, recoilback, sec * interpolant);
            if (gunModel.transform.localRotation == recoilback)
            {
                sec = 0;
                lerp = false;
                lerpback = false;
                shotAgainFlag = true;
            }
        }
    }
    /// <summary>
    /// 弾の発射処理
    /// </summary>
    /// <param name="arg_firingPoint">銃のポジション</param>
    /// <param name="arg_cameraRotation">カメラの回転量</param>
    public void Shot(Quaternion arg_cameraRotation)
    {
        if (shotDelayTime <= 0 && lerpback == false && shotAgainFlag)
        {// 弾の発射処理
            recoilgun = gunModel.transform.localRotation;
            recoil = Quaternion.AngleAxis(angle, axis) * gunModel.transform.localRotation;
            //recoilback = recoilgun;
            //銃の音
            audioSource.PlayOneShot(shotSound);
			// 弾を発射する場所を取得
			var bulletPosition = firingPoint.transform.position;
            // 上で取得した場所に、"grenade"のPrefabを出現させる
            GameObject newBall = Instantiate(rocketBomb, bulletPosition, arg_cameraRotation);
            // 出現させたボールのforward(z軸方向)
            var direction = newBall.transform.forward;
            // 弾の発射方向にnewBallのz方向(ローカル座標)を入れ、弾オブジェクトのrigidbodyに衝撃力を加える
            newBall.GetComponent<Rigidbody>().AddForce(direction * bulletSpeed, ForceMode.Impulse);
            Invoke("Explode", 2.0f); // グレネードを発射してから1.5秒後に爆発させる
            // 出現させたボールの名前を"bullet"に変更
            newBall.name = rocketBomb.name;
            // 出現させたボールを2秒後に消す
            //Destroy(newBall, 1.5f);
            shotAgainFlag = false;

            shotDelayTime = shotDelayMaxTime;

            lerp = true;
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
                cube.GetComponent<Rigidbody>().AddExplosionForce(30f, transform.position, 30f, 5f, ForceMode.Impulse);
            }
        }
    }
}
