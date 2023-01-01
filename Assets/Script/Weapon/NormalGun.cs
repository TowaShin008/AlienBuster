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

    //���ʉ�
    public AudioClip gunSound;
    AudioSource audioSource;

    [SerializeField]
    Vector3 muzzleFlashScale;
    [SerializeField]
    GameObject muzzleFlashPrefab;

    GameObject muzzleFlash;

    [SerializeField] float dispersion = 0.02f; // �΂���
    [SerializeField] float verticalToHorizontalRatio = 1.5f; // �΂���̏c����

    MagazineScript magazineScript = null;

    void Start()
    {
        //���̃R���|�[�l���g�擾
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
    /// �e�̔��ˏ���
    /// </summary>
    /// <param name="arg_cameraRotation">�J�����̉�]��</param>
    public void Shot(Quaternion arg_cameraRotation,bool arg_holdFlag)
    {
        if (!magazineScript.CheckBullets()) return;

        if (shotDelayTime <= 0)
        {
            magazineScript.DecrementMagazine();
            //�e�̉�
            audioSource.PlayOneShot(gunSound);
            //�e�̔��ˏ���
            // �e�𔭎˂���ꏊ���擾
            var bulletPosition = firingPoint.transform.position;
            // ��Ŏ擾�����ꏊ�ɁA"bullet"��Prefab���o��������
            GameObject newBullet = Instantiate(bullet, bulletPosition, arg_cameraRotation);

            // �c�̂΂��
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
                // ���̂΂��
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

            // �e�̔��˕�����newBall��z����(���[�J�����W)�����A�e�I�u�W�F�N�g��rigidbody�ɏՌ��͂�������
            newBullet.GetComponent<Rigidbody>().AddForce(direction * bulletSpeed, ForceMode.Impulse);
            // �o���������{�[���̖��O��"bullet"�ɕύX
            newBullet.name = bullet.name;
            // �o���������{�[����0.8�b��ɏ���
            Destroy(newBullet, 1.5f);

            shotDelayTime = shotDelayMaxTime;

            MuzzleFashProcessing();
        }
    }
    /// <summary>
    /// �}�Y���t���b�V�����o
    /// </summary>
    private void MuzzleFashProcessing()
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
    /// �}�K�W���̏�����
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
