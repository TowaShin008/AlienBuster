using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotGun : MonoBehaviour
{
    [SerializeField]
    private GameObject bullet;
    [SerializeField]
    private GameObject firingPoint;

    private float bulletSpeed = 30.0f;
    const int shotDelayMaxTime = 100;
    private int shotDelayTime = 0;

    [SerializeField]
    float randomDiffusion = 10;
    [SerializeField]
    int bulletCount = 10;

    [SerializeField]
    private Transform bulletExitPosition;
    [SerializeField]
    private float exitSpeed = 0.1f;
    [SerializeField]
    private float exitRotate = 360.0f;

    //効果音
    public AudioClip shotSound;
    AudioSource audioSource;
    public AudioClip bulletSound;

    [SerializeField]
    Vector3 muzzleFlashScale;
    [SerializeField]
    GameObject muzzleFlashPrefab;

    GameObject muzzleFlash;

    void Start()
    {
        //音のコンポーネント取得
        audioSource = GetComponent<AudioSource>();
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
    public void Shot(Quaternion arg_cameraRotation)
    {
        if (shotDelayTime <= 0)
        {
            //銃の音
            audioSource.PlayOneShot(shotSound);
            audioSource.PlayOneShot(bulletSound);
            var bulletInstance = Instantiate(bullet, bulletExitPosition.position, arg_cameraRotation);
            var bulletRigit = bulletInstance.GetComponent<Rigidbody>();
            bulletRigit.AddForce(bulletExitPosition.forward * exitSpeed);
            bulletRigit.AddTorque(Random.insideUnitSphere * exitRotate);
            Destroy(bulletInstance, 3f);


            for (int n = 0; n < bulletCount; n++)
            {
                var bulletPosition = firingPoint.transform.position;

                GameObject newBall = Instantiate(bullet, bulletPosition, arg_cameraRotation);

                float randomX = Random.Range(randomDiffusion, -randomDiffusion);
                float randomY = Random.Range(randomDiffusion, -randomDiffusion);
                float randomZ = Random.Range(randomDiffusion, -randomDiffusion);


                var direction = new Vector3(randomX, randomY, randomZ);
                Rigidbody newbulletRb = newBall.GetComponent<Rigidbody>();

                newbulletRb.AddForce(direction, ForceMode.Impulse);
                newbulletRb.AddForce(newBall.transform.forward * bulletSpeed, ForceMode.Impulse);

                newBall.name = bullet.name;

                Destroy(newBall, 0.8f);
            }

            MuzzleFlashProcessing();

            shotDelayTime = shotDelayMaxTime;
        }
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
}
