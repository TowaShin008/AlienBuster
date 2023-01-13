using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Util;

public class SniperScript : MonoBehaviour
{
    public GameObject bullet;

    public float speed = 0.1f;

    [SerializeField]
    private GameObject normalGunPosition;
    [SerializeField]
    private GameObject holdGunPosition;
    //public Transform defaultPos;
    //public Transform aimPos;
    [SerializeField]
    private GameObject firingPoint;

    public List<MeshRenderer> sniperMesh;

    public Image sniperEdge;
    public Image sniperGaugeEdge;
    public Image sniperGauge;
    Vector3 defScale;

    //���ʉ�
    public AudioClip shotSound;
    AudioSource audioSource;

    private float bulletSpeed = 120.0f;

    [SerializeField]
    Vector3 muzzleFlashScale;
    [SerializeField]
    GameObject muzzleFlashPrefab;

    GameObject muzzleFlash;

    [SerializeField] float dispersion = 0.02f; // �΂���
    [SerializeField] float verticalToHorizontalRatio = 1.5f; // �΂���̏c����

    [SerializeField]
    const int remainingMaxBullet = 20;
    int remainingBullets = remainingMaxBullet;

    MagazineScript magazineScript = null;

    void Start()
    {
        //���̃R���|�[�l���g�擾
        audioSource = GetComponent<AudioSource>();
        remainingBullets = remainingMaxBullet;
        MagazineInitialize();
        magazineScript.ReloadEnable(true);
    }

    // Update is called once per frame
    void Update()
    {
        float lTri = Input.GetAxis(Constants.lTriggerName.ToString());

        if (Input.GetMouseButton(1) || lTri > 0)
        {//�e���\���鏈��
            HoldGun();
        }
        else
        {
            transform.position = normalGunPosition.transform.position;
            //transform.rotation = Quaternion.RotateTowards(transform.rotation, defaultPos.rotation, speed);
            sniperEdge.transform.localScale = Vector2.MoveTowards(sniperEdge.transform.localScale, new Vector2(5.0f, 5.0f), speed * 5.0f);
            for (int i = 0; i < sniperMesh.Count; i++)
            {
                sniperMesh[i].material.color = Color.white;
            }

            Color32 color = sniperGauge.color;
            Color32 color2 = sniperGaugeEdge.color;
            color.a = 0;
            color2.a = 0;
            sniperGauge.color = color;
            sniperGaugeEdge.color = color2;
        }

        //�Q�[�W
        defScale = sniperGauge.transform.localScale;
        if (defScale.y <= 0.25f)
        {
            defScale.y += 0.003f;
        }
        sniperGauge.transform.localScale = defScale;
    }

    public void HoldGun()
    {
        sniperEdge.enabled = true;
        sniperGaugeEdge.enabled = true;
        sniperGauge.enabled = true;
        transform.position = holdGunPosition.transform.position;
        //transform.rotation = Quaternion.RotateTowards(transform.rotation, aimPos.rotation, speed);
        sniperEdge.transform.localScale = Vector2.MoveTowards(sniperEdge.transform.localScale, new Vector2(1.0f, 1.0f), speed + 5.0f);
        if (sniperEdge.transform.localScale.x == 1.0f)
        {
            for (int i = 0; i < sniperMesh.Count; i++)
            {
                sniperMesh[i].material.color = new Color32(255, 255, 255, 0);
            }

            Color32 color = sniperGauge.color;
            Color32 color2 = sniperGaugeEdge.color;
            color.a = 180;
            color2.a = 180;
            sniperGauge.color = color;
            sniperGaugeEdge.color = color2;
        }
    }
    /// <summary>
    /// �ˌ�����
    /// </summary>
    /// <param name="arg_cameraRotation">�J�����̉�]��</param>
    public bool Shot(Quaternion arg_cameraRotation,bool arg_holdFlag)
    {
        if (!magazineScript.CheckBullets())
        {
            magazineScript.SetRemainingBulletsSize(remainingBullets);
            return true;
        }
        //float rTri = Input.GetAxis("R_Trigger");
        if (defScale.y >= 0.25f)
        {
            magazineScript.DecrementMagazine();
            //if (Input.GetMouseButtonDown(0) || rTri > 0)
            {//�e�̔��ˏ���
                if (remainingBullets > 0)
                {
                    remainingBullets--;
                }
                //�e�̉�
                audioSource.PlayOneShot(shotSound);
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
                newBullet.name = "SniperBullet";
                // �o���������{�[����0.8�b��ɏ���
                Destroy(newBullet, 1.0f);

                defScale.y = 0;
                sniperGauge.transform.localScale = defScale;

                MuzzleFashProcessing();
            }
        }

        if (remainingBullets <= 0)
        {
            Initialize();
            return false;
        }

        return true;
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
    /// �c�e���̃��Z�b�g
    /// </summary>
    public void ResetRemainigBullet()
    {
        remainingBullets = remainingMaxBullet;
    }

    public void Initialize()
	{
        ResetRemainigBullet();
        magazineScript.SetRemainingBulletsSize(remainingMaxBullet);
        magazineScript.SetMagazineSize(2);
        magazineScript.SetReloadTime(120);
        transform.position = normalGunPosition.transform.position;
        //transform.rotation = Quaternion.RotateTowards(transform.rotation, defaultPos.rotation, speed);
        sniperEdge.transform.localScale = new Vector2(5.0f, 5.0f);
        for (int i = 0; i < sniperMesh.Count; i++)
        {
            sniperMesh[i].material.color = Color.white;
        }

        Color32 color = sniperGauge.color;
        Color32 color2 = sniperGaugeEdge.color;
        color.a = 0;
        color2.a = 0;
        sniperGauge.color = color;
        sniperGaugeEdge.color = color2;
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
}
