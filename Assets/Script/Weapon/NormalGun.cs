using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalGun : MonoBehaviour
{
    [SerializeField]
    private GameObject bullet;
    public float bulletSpeed = 30.0f;
    const int shotDelayMaxTime = 20;
    private int shotDelayTime = 0;
    [SerializeField]
    private GameObject firingPoint;

    //効果音
    public AudioClip gunSound;
    AudioSource audioSource;

    [SerializeField]
    Vector3 muzzleFlashScale;
    [SerializeField]
    GameObject muzzleFlashPrefab;

    GameObject muzzleFlash;

    [SerializeField] float dispersion = 0.02f; // ばらつき具合
    [SerializeField] float verticalToHorizontalRatio = 1.5f; // ばらつきの縦横比

    MagazineScript magazineScript = null;

    void Start()
    {
        //音のコンポーネント取得
        audioSource = GetComponent<AudioSource>();
        MagazineInitialize();
        magazineScript.ReloadEnable(true);
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
    /// <param name="arg_cameraRotation">カメラの回転量</param>
    public void Shot(Quaternion arg_cameraRotation,bool arg_holdFlag)
    {
        if (!magazineScript.CheckBullets()) return;

        if (shotDelayTime <= 0)
        {
            magazineScript.DecrementMagazine();
            //銃の音
            audioSource.PlayOneShot(gunSound);
            //弾の発射処理
            // 弾を発射する場所を取得
            var bulletPosition = firingPoint.transform.position;
            // 上で取得した場所に、"bullet"のPrefabを出現させる
            GameObject newBullet = Instantiate(bullet, bulletPosition, arg_cameraRotation);

            // 縦のばらつき
            float v = Random.Range(-dispersion * verticalToHorizontalRatio, dispersion * verticalToHorizontalRatio);
            Vector3 direction;

            if (arg_holdFlag)
            {
                direction = newBullet.transform.forward;
            }
            else
			{
                if (v >= 0)
                {
                    direction = Vector3.Slerp(newBullet.transform.forward, newBullet.transform.up, v);
                }
                else
                {
                    direction = Vector3.Slerp(newBullet.transform.forward, -newBullet.transform.up, -v);
                }
                // 横のばらつき
                float h = Random.Range(-dispersion, dispersion);
                if (h >= 0)
                {
                    direction = Vector3.Slerp(direction, newBullet.transform.right, h);
                }
                else
                {
                    direction = Vector3.Slerp(direction, -newBullet.transform.right, -h);
                }
            }

            // 弾の発射方向にnewBallのz方向(ローカル座標)を入れ、弾オブジェクトのrigidbodyに衝撃力を加える
            newBullet.GetComponent<Rigidbody>().AddForce(direction * bulletSpeed, ForceMode.Impulse);
            // 出現させたボールの名前を"bullet"に変更
            newBullet.name = bullet.name;
            // 出現させたボールを0.8秒後に消す
            Destroy(newBullet, 1.5f);

            shotDelayTime = shotDelayMaxTime;

            MuzzleFashProcessing();
        }
    }
    /// <summary>
    /// マズルフラッシュ演出
    /// </summary>
    private void MuzzleFashProcessing()
    {
        //マズルフラッシュON
        if (muzzleFlashPrefab != null)
        {
            if (muzzleFlash != null)
            {
                muzzleFlash.SetActive(true);
            }
            else
            {
                muzzleFlash = Instantiate(muzzleFlashPrefab, firingPoint.transform.position, firingPoint.transform.rotation);
                muzzleFlash.transform.SetParent(firingPoint.gameObject.transform);
                muzzleFlash.transform.localScale = muzzleFlashScale;
            }
        }

        //マズルフラッシュ終了演出
        StartCoroutine(MuzzleFlashEndProcessing());
    }
    /// <summary>
    /// マズルフラッシュ終了演出
    /// </summary>
    /// <returns>インターフェイス</returns>
    IEnumerator MuzzleFlashEndProcessing()
    {
        yield return new WaitForSeconds(0.15f);
        //マズルフラッシュOFF
        if (muzzleFlash != null)
        {
            muzzleFlash.SetActive(false);
        }
    }
    /// <summary>
    /// マガジンの初期化
    /// </summary>
    void MagazineInitialize()
    {
        this.gameObject.AddComponent<MagazineScript>();
        magazineScript = this.gameObject.GetComponent<MagazineScript>();

        magazineScript.ReloadEnable(false);
        magazineScript.SetMagazineSize(10);
        magazineScript.SetReloadTime(120);
    }
    public void Initialize()
    {
        magazineScript.SetRemainingBulletsSize(0);
        magazineScript.SetMagazineSize(10);
        magazineScript.SetReloadTime(120);
    }
}
