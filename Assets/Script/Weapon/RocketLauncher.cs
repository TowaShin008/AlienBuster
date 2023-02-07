using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Util;

public class RocketLauncher : MonoBehaviour
{
    public MeshRenderer mesh;
    [SerializeField]
    private GameObject rocketBomb;
    private float bulletSpeed = 30.0f;
    //���10�b��1��
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

    [SerializeField]
    const int remainingMaxBullet = 10;
    int remainingBullets = remainingMaxBullet;

    MagazineScript magazineScript = null;

    // Start is called before the first frame update
    void Start()
    {
        //���̃R���|�[�l���g�擾
        audioSource = GetComponent<AudioSource>();
        recoil = Quaternion.AngleAxis(10.0f, new Vector3(0.0f, 0.0f, 1.0f));
        recoilback = Quaternion.AngleAxis(10.0f, new Vector3(0.0f, 0.0f, 1.0f));
        shotAgainFlag = true;
        recoilback = gunModel.transform.localRotation;
        remainingBullets = remainingMaxBullet;
        MagazineInitialize();
        magazineScript.ReloadEnable(true);
        OpaqueRenderingMode();
    }

    // Update is called once per frame
    void Update()
    {
        if (lerp)
        {
            //���R�C�������i�C�[�W���O�����t���j
            lerp = false;
            lerpback = true;
            gunModel.transform.DOLocalRotateQuaternion(recoil, interpolant)
                              .SetEase(Ease.InOutQuart).OnComplete(Recoilback);
        }

        //Recoilback();

        if (shotDelayTime > 0)
        {
            shotDelayTime--;
        }
    }
    private void Recoilback()
    {
        if (lerpback == true)
        {
            gunModel.transform.DOLocalRotateQuaternion(recoilback, interpolant)
                          .SetEase(Ease.InOutQuart);
            lerp = false;
            lerpback = false;
            shotAgainFlag = true;
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
    /// �e�̔��ˏ���
    /// </summary>
    /// <param name="arg_cameraRotation">�J�����̉�]��</param>
    public bool Shot(Quaternion arg_cameraRotation)
    {
        if (!magazineScript.CheckBullets())
		{
            magazineScript.SetRemainingBulletsSize(remainingBullets);
            return true;
        }

        if (shotDelayTime <= 0 && lerpback == false && shotAgainFlag)
        {// �e�̔��ˏ���
            magazineScript.DecrementMagazine();
            if (remainingBullets>0)
			{
                remainingBullets--;
            }
            recoilgun = gunModel.transform.localRotation;
            recoil = Quaternion.AngleAxis(angle, axis) * gunModel.transform.localRotation;
            //recoilback = recoilgun;
            //�e�̉�
            audioSource.PlayOneShot(shotSound);
			// �e�𔭎˂���ꏊ���擾
			var bulletPosition = firingPoint.transform.position;
            // ��Ŏ擾�����ꏊ�ɁA"grenade"��Prefab���o��������
            GameObject newBall = Instantiate(rocketBomb, bulletPosition, arg_cameraRotation);
            // �o���������{�[����forward(z������)
            var direction = newBall.transform.forward;
            // �e�̔��˕�����newBall��z����(���[�J�����W)�����A�e�I�u�W�F�N�g��rigidbody�ɏՌ��͂�������
            newBall.GetComponent<Rigidbody>().AddForce(direction * bulletSpeed, ForceMode.Impulse);
            Invoke("Explode", 2.0f); // �O���l�[�h�𔭎˂��Ă���1.5�b��ɔ���������
            // �o���������{�[���̖��O��"bullet"�ɕύX
            newBall.name = rocketBomb.name;
            // �o���������{�[����2�b��ɏ���
            //Destroy(newBall, 1.5f);
            shotAgainFlag = false;

            shotDelayTime = shotDelayMaxTime;

            lerp = true;
        }
        if(remainingBullets<=0)
		{
            return false;
		}

        return true;
    }
    /// <summary>
    /// ���j���o
    /// </summary>
    void Explode()
    {
        GameObject[] cubes = GameObject.FindGameObjectsWithTag(Constants.enemyName.ToString()); //�uEnemy�v�^�O�̂����I�u�W�F�N�g��S�Č������Ĕz��ɂ����

        if (cubes.Length == 0) return; // �uEnemy�v�^�O�������I�u�W�F�N�g���Ȃ���Ή������Ȃ��B

        foreach (GameObject cube in cubes) // �z��ɓ��ꂽ��ЂƂ̃I�u�W�F�N�g
        {
            if (cube.GetComponent<Rigidbody>()) // Rigidbody������΁A�O���l�[�h�𒆐S�Ƃ��������̗͂�������
            {
                cube.GetComponent<Rigidbody>().AddExplosionForce(30f, transform.position, 30f, 5f, ForceMode.Impulse);
            }
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
        magazineScript.SetMagazineSize(Constants.rocketLauncherMagazineSize);
        magazineScript.SetReloadTime(Constants.rocketLauncherReloadTime);
    }
    /// <summary>
    /// ����������
    /// </summary>
    public void Initialize()
	{
        OpaqueRenderingMode();
        ResetRemainigBullet();
        magazineScript.SetRemainingBulletsSize(remainingMaxBullet);
        magazineScript.SetMagazineSize(Constants.rocketLauncherMagazineSize);
        magazineScript.SetReloadTime(Constants.rocketLauncherReloadTime);
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
