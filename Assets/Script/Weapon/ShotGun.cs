using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotGun : MonoBehaviour
{
    [SerializeField]
    private GameObject bullet;
    [SerializeField]
    private GameObject firingPoint;

    private float bulletSpeed = 80.0f;
    const int shotDelayMaxTime = 100;
    private int shotDelayTime = 0;

    [SerializeField]
    float randomDiffusion = 200.0f;
    [SerializeField]
    int bulletCount = 10;

    //効果音
    public AudioClip shotSound;
    AudioSource audioSource;
    public AudioClip bulletSound;

    [SerializeField]
    Vector3 muzzleFlashScale;
    [SerializeField]
    GameObject muzzleFlashPrefab;

    GameObject muzzleFlash;

    [SerializeField]
    const int remainingMaxBullet = 30;
    int remainingBullets = remainingMaxBullet;

    MagazineScript magazineScript = null;

    void Start()
    {
        //音のコンポーネント取得
        audioSource = GetComponent<AudioSource>();
        remainingBullets = remainingMaxBullet;
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
    /// 射撃処理
    /// </summary>
    /// <param name="arg_cameraRotation">カメラの回転量</param>
    public bool Shot(Quaternion arg_cameraRotation)
    {
        if (!magazineScript.CheckBullets())
        {
            magazineScript.SetRemainingBulletsSize(remainingBullets);
            return true;
        }

        if (shotDelayTime <= 0)
        {
            magazineScript.DecrementMagazine();
            if (remainingBullets > 0)
            {
                remainingBullets--;
            }
            //銃の音
            audioSource.PlayOneShot(shotSound);
            audioSource.PlayOneShot(bulletSound);


            for (int n = 0; n < bulletCount; n++)
            {
                var bulletPosition = firingPoint.transform.position;

                GameObject newBall = Instantiate(bullet, bulletPosition, arg_cameraRotation);

                float randomX = Random.Range(randomDiffusion, -randomDiffusion);
                float randomY = Random.Range(randomDiffusion, -randomDiffusion);
                float randomZ = Random.Range(randomDiffusion, -randomDiffusion);


                var direction = new Vector3(randomX, randomY, randomZ);
                Rigidbody newbulletRb = newBall.GetComponent<Rigidbody>();

                newbulletRb.AddForce(direction);
                newbulletRb.AddForce(newBall.transform.forward * bulletSpeed,ForceMode.Impulse);

                newBall.name = bullet.name;

                Destroy(newBall, 0.8f);
            }
            //マズルフラッシュ演出
            MuzzleFlashProcessing();

            shotDelayTime = shotDelayMaxTime;
        }

        if (remainingBullets <= 0)
        {
            return false;
        }

        return true;
    }
    /// <summary>
    /// マズルフラッシュ演出
    /// </summary>
    private void MuzzleFlashProcessing()
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
    /// 残弾数のリセット
    /// </summary>
    public void ResetRemainigBullet()
    {
        remainingBullets = remainingMaxBullet;
    }
    /// <summary>
    /// マガジンの初期化
    /// </summary>
    void MagazineInitialize()
    {
        this.gameObject.AddComponent<MagazineScript>();
        magazineScript = this.gameObject.GetComponent<MagazineScript>();

        magazineScript.ReloadEnable(false);
        magazineScript.SetMagazineSize(2);
        magazineScript.SetReloadTime(120);
    }
    public void Initialize()
    {
        ResetRemainigBullet();
        magazineScript.SetRemainingBulletsSize(remainingMaxBullet);
        magazineScript.SetMagazineSize(2);
        magazineScript.SetReloadTime(120);
    }
}
