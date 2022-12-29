using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotGun : MonoBehaviour
{
    [SerializeField]
    private GameObject bullet;
    [SerializeField]
    private GameObject firingPoint;

    private float bulletSpeed = 60.0f;
    const int shotDelayMaxTime = 100;
    private int shotDelayTime = 0;

    [SerializeField]
    float randomDiffusion = 200.0f;
    [SerializeField]
    int bulletCount = 10;

    //���ʉ�
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

    void Start()
    {
        //���̃R���|�[�l���g�擾
        audioSource = GetComponent<AudioSource>();
        remainingBullets = remainingMaxBullet;
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
    /// �ˌ�����
    /// </summary>
    /// <param name="arg_cameraRotation">�J�����̉�]��</param>
    public bool Shot(Quaternion arg_cameraRotation)
    {
        if (shotDelayTime <= 0)
        {
            if (remainingBullets > 0)
            {
                remainingBullets--;
            }
            //�e�̉�
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
    /// �}�Y���t���b�V�����o
    /// </summary>
    private void MuzzleFlashProcessing()
    {
        //�}�Y���t���b�V��ON
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

        //�}�Y���t���b�V���I�����o
        StartCoroutine(MuzzleFlashEndProcessing());
    }
    /// <summary>
    /// �}�Y���t���b�V���I�����o
    /// </summary>
    /// <returns>�C���^�[�t�F�C�X</returns>
    IEnumerator MuzzleFlashEndProcessing()
    {
        yield return new WaitForSeconds(0.15f);
        //�}�Y���t���b�V��OFF
        if (muzzleFlash != null)
        {
            muzzleFlash.SetActive(false);
        }
    }
    /// <summary>
    /// �c�e���̃��Z�b�g
    /// </summary>
    public void ResetRemainigBullet()
    {
        remainingBullets = remainingMaxBullet;
    }
}
