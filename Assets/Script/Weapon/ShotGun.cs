using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Util;

public class ShotGun : MonoBehaviour
{
    public MeshRenderer mesh;
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

    MagazineScript magazineScript = null;

    void Start()
    {
        //���̃R���|�[�l���g�擾
        audioSource = GetComponent<AudioSource>();
        remainingBullets = remainingMaxBullet;
        MagazineInitialize();
        magazineScript.ReloadEnable(true);
        OpaqueRenderingMode();
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
    /// �e���\���鏈��
    /// </summary>
    public void HoldGun(Vector3 arg_holdGunPosition)
    {
        FadeRenderingMode();
        Color color = mesh.material.color;
        color.a = 0.2f;
        mesh.material.color = color;

        this.transform.position = arg_holdGunPosition;
    }
    /// <summary>
    /// �ˌ�����
    /// </summary>
    /// <param name="arg_cameraRotation">�J�����̉�]��</param>
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
            //�}�Y���t���b�V�����o
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
    /// <summary>
    /// �}�K�W���̏�����
    /// </summary>
    void MagazineInitialize()
    {
        this.gameObject.AddComponent<MagazineScript>();
        magazineScript = this.gameObject.GetComponent<MagazineScript>();

        magazineScript.ReloadEnable(false);
        magazineScript.SetMagazineSize(Constants.shotGunMagazineSize);
        magazineScript.SetReloadTime(Constants.shotGunReloadTime);
    }
    /// <summary>
    /// ����������
    /// </summary>
    public void Initialize()
    {
        OpaqueRenderingMode();
        ResetRemainigBullet();
        magazineScript.SetRemainingBulletsSize(remainingMaxBullet);
        magazineScript.SetMagazineSize(Constants.shotGunMagazineSize);
        magazineScript.SetReloadTime(Constants.shotGunReloadTime);
    }
    /// <summary>
    /// �ʏ�`�揈��
    /// </summary>
    public void OpaqueRenderingMode()
    {
        mesh.material.SetOverrideTag("RenderType", "");
        mesh.material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
        mesh.material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
        mesh.material.SetInt("_ZWrite", 1);
        mesh.material.DisableKeyword("_ALPHATEST_ON");
        mesh.material.DisableKeyword("_ALPHABLEND_ON");
        mesh.material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
        mesh.material.renderQueue = -1;
    }
    /// <summary>
    /// ���ߕ`�揈��
    /// </summary>
    public void FadeRenderingMode()
    {
        mesh.material.SetOverrideTag("RenderType", "Transparent");
        mesh.material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
        mesh.material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
        mesh.material.SetInt("_ZWrite", 0);
        mesh.material.DisableKeyword("_ALPHATEST_ON");
        mesh.material.EnableKeyword("_ALPHABLEND_ON");
        mesh.material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
        mesh.material.renderQueue = 3000;
    }
}
