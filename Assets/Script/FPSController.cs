using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FPSController : MonoBehaviour
{
    //�ʏ�e
    [SerializeField]
    private GameObject normalGun;
    //�O���l�[�g�����`���[
    [SerializeField]
    private GameObject grenadeLauncher;
    //�X�i�C�p�[���C�t��
    [SerializeField]
    private GameObject sniperRifle;
    //�V���b�g�K��
    [SerializeField]
    private GameObject shotGun;

    [SerializeField]
    private GameObject normalGunPosition;
    [SerializeField]
    private GameObject holdGunPosition;

    private BoxCollider collider;

    //�v���C���[�ړ����x
    const float normalSpeed = 5.0f;
    const float sprintSpeed = 15.0f;
    float speed = normalSpeed;

    //�X�e�b�v���̖��C�������Ȃ�܂ł̗P�\����
    const int stepMaxTime = 120;
    int stepTime = 0;

    //LSHIFT�̓��͎��Ԃ��i�[����ϐ�
    int chargeCount = 0;

    //�v���C���[�̃X�^�~�i
    const int maxStamina = 300;
    int stamina = maxStamina;

    //�J�����֘A�ϐ�
    public GameObject cam;
    Quaternion cameraRot, characterRot;

    Rigidbody rigidbody;

    //XY�����̎��_���x
    public float Xsensityvity, Ysensityvity;

    //�c�@
    int remain;
    const int maxHP = 3;
    int hp = maxHP;
    bool deadFlag;

    //�e�̗h�ꉉ�o�̕ϐ�
    const float shakingNormalSpeed = 10.0f;
    const float shakingMaxSpeed = 15.0f;
    float shakingSpeed = shakingNormalSpeed;

    int gunType = 1;
    //�X�i�C�p�[���C�t����UI
    public Image sniperEdge;
    public Image sniperGaugeEdge;
    public Image sniperGauge;

    // Start is called before the first frame update
    void Start()
    {
        cameraRot = cam.transform.localRotation;
        characterRot = transform.localRotation;
        deadFlag = false;
        rigidbody = GetComponent<Rigidbody>();
        collider = GetComponent<BoxCollider>();
        //�J�[�\���̕\��
        Cursor.visible = false;
        //�J�[�\���̃��b�N
        Cursor.lockState = CursorLockMode.Locked;
        Physics.gravity = new Vector3(0.0f, -4.0f, 0.0f);

        //�c�@
        remain = 1;
        hp = maxHP;
        normalGun.SetActive(true);
        grenadeLauncher.SetActive(false);
        sniperRifle.SetActive(false);
        shotGun.SetActive(false);
        //�X�i�C�p�[���C�t����UI
        sniperEdge.enabled = false;
        sniperGaugeEdge.enabled = false;
        sniperGauge.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        //���_�ړ�����
        MoveCameraProcessing();

        if (Input.GetKey(KeyCode.Space))
		{//�W�����v����
            JumpProcessing();
		}

        //�X�e�b�v�Q�[�W�̃��`���[�W����
        StaminaRechargeProcessing();

		if (Input.GetMouseButton(1))
		{//�e���\���鏈��
			HoldGun();
		}
		else if (Input.GetMouseButton(0))
		{//�e�̔��ˏ���(������)
			HipShot();
		}
		else
		{//�}�E�X���͂��Ȃ��ꍇ�́A�e���\���Ȃ��B
			normalGun.transform.position = normalGunPosition.transform.position;
            grenadeLauncher.transform.position = normalGunPosition.transform.position;
            //sniperRifle.transform.position = normalGunPosition.transform.position;
            shotGun.transform.position = normalGunPosition.transform.position;
        }

		//�ړ�����
		MoveProcessing();

        if (Input.GetKeyDown(KeyCode.Escape))
        {//�J�[�\���̔�\��
            Cursor.visible = true;
        }

        Debug.Log(stamina);
    }
    /// <summary>
    /// �ړ�����
    /// </summary>
    private void MoveProcessing()
	{
        var velocity = new Vector3(0, 0, 0);
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D))
        {
            //�ړ�����
            if (Input.GetKey(KeyCode.W))
            {
                velocity += gameObject.transform.rotation * new Vector3(0, 0, speed);
            }
            if (Input.GetKey(KeyCode.A))
            {
                velocity += gameObject.transform.rotation * new Vector3(-speed, 0, 0);
            }
            if (Input.GetKey(KeyCode.S))
            {
                velocity += gameObject.transform.rotation * new Vector3(0, 0, -speed);
            }
            if (Input.GetKey(KeyCode.D))
            {
                velocity += gameObject.transform.rotation * new Vector3(speed, 0, 0);
            }
            this.transform.position += velocity * Time.deltaTime;


            if (Input.GetKey(KeyCode.LeftShift))
            {
                chargeCount++;
                //�X�v�����g����
                SprintProcessing();
            }

            if (Input.GetKeyUp(KeyCode.LeftShift))
            {//�X�e�b�v����
                StepProcessing(velocity);
            }

			if (Input.GetMouseButton(1) == false)
			{//�ċz���o����
				BreathProcessing();
			}
        }
        if (stepTime > 0)
        {//�X�e�b�v�����Ă��Ȃ����A�X�e�b�v�P�\���ԂłȂ���΍R�͂���������
            rigidbody.drag = 0;
		}
    }
    /// <summary>
    /// �p�x�����֐��̍쐬
    /// </summary>
    /// <param name="q">�ΏۂƂȂ���̂̃N�H�[�^�j�I��</param>
    /// <returns>�N�H�[�^�j�I��</returns>
    public Quaternion ClampRotation(Quaternion q)
    {
        //q = x,y,z,w (x,y,z�̓x�N�g���i�ʂƌ����j�Fw�̓X�J���[�i���W�Ƃ͖��֌W�̗ʁj)

        q.x /= q.w;
        q.y /= q.w;
        q.z /= q.w;
        q.w = 1f;

        float angleX = Mathf.Atan(q.x) * Mathf.Rad2Deg * 2f;

        angleX = Mathf.Clamp(angleX, -90.0f, 90.0f);

        q.x = Mathf.Tan(angleX * Mathf.Deg2Rad * 0.5f);

        return q;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "Enemy" || collision.gameObject.name == "EnemyBullet")
        {
            Debug.Log("Hit");
            Damage();
        }

        if(collision.gameObject.tag == "WeaponItem")
		{
            if (collision.gameObject.name == "NormalGunItem")
            {
                normalGun.SetActive(true);
                grenadeLauncher.SetActive(false);
                sniperRifle.SetActive(false);
                shotGun.SetActive(false);
                gunType = 1;
            }
            else if (collision.gameObject.name == "RocketLauncherItem")
            {
                normalGun.SetActive(false);
                grenadeLauncher.SetActive(true);
                sniperRifle.SetActive(false);
                shotGun.SetActive(false);
                gunType = 2;
            }
            else if (collision.gameObject.name == "SniperRifleItem")
            {
                normalGun.SetActive(false);
                grenadeLauncher.SetActive(false);
                sniperRifle.SetActive(true);
                shotGun.SetActive(false);
                gunType = 3;
            }
            else if (collision.gameObject.name == "ShotGunItem")
            {
                normalGun.SetActive(false);
                grenadeLauncher.SetActive(false);
                sniperRifle.SetActive(false);
                shotGun.SetActive(true);
                gunType = 4;
            }
        }

        if (stepTime == 0 && collision.gameObject.CompareTag("Field"))
        {//�X�e�b�v�����Ă��Ȃ����A�X�e�b�v�P�\���ԂłȂ���Ζ��C����������
            rigidbody.drag = 100;
        }
    }
    /// <summary>
    /// �J�����̈ړ�����
    /// </summary>
    private void MoveCameraProcessing()
	{
        //Y�����_�ړ�
        float yRot = Input.GetAxis("Mouse Y") * Ysensityvity;
        cameraRot *= Quaternion.Euler(-yRot, 0, 0);
        //X�����_�ړ�
        float xRot = Input.GetAxis("Mouse X") * Xsensityvity;
        characterRot *= Quaternion.Euler(0, xRot, 0);

        if (deadFlag == false)
		{
            transform.localRotation = characterRot;
        }

        //Update�̒��ō쐬�����֐����Ă�
        cameraRot = ClampRotation(cameraRot);

        cam.transform.localRotation = cameraRot;
    }
    /// <summary>
    /// �X�e�b�v����
    /// </summary>
    public void StepProcessing(Vector3 arg_velocity)
	{
        if (chargeCount < 30)
        {//�X�e�b�v����
            if (stamina >= 100)
            {
                stepTime = stepMaxTime;
                stamina -= 100;
                rigidbody.AddForce(arg_velocity * 5.0f, ForceMode.Impulse);
            }
        }
        //�v���C���[�̏�����
        speed = normalSpeed;
        shakingSpeed = shakingNormalSpeed;
        chargeCount = 0;
    }
    /// <summary>
    /// �X�v�����g����
    /// </summary>
    public void SprintProcessing()
	{
        if (chargeCount >= 30 && stamina > 0)
        {//�X�v�����g����
            stamina -= 2;
            speed = sprintSpeed;
            shakingSpeed = shakingMaxSpeed;
        }
	}
    /// <summary>
    /// �W�����v����
    /// </summary>
    public void JumpProcessing()
	{
        rigidbody.drag = 0;
        rigidbody.AddForce(new Vector3(0.0f, 10.0f, 0.0f));
	}
    /// <summary>
    /// �ċz���o
    /// </summary>
    private void BreathProcessing()
	{
        //�}�E�X��Y���|�W�V�����̎擾
        float yRot = normalGun.transform.localRotation.eulerAngles.x;
        //�O�p�֐����g���e���c�ɗh�炷
        yRot += Mathf.Sin(Time.time * shakingSpeed) * 0.5f;

        normalGun.transform.rotation *= Quaternion.Euler(-yRot, 0, 0);

        shotGun.transform.rotation *= Quaternion.Euler(-yRot, 0, 0);

        //�}�E�X��Y���|�W�V�����̎擾
        float grenadeLauncheryRot = grenadeLauncher.transform.localRotation.eulerAngles.x;
        //�O�p�֐����g���e���c�ɗh�炷
        grenadeLauncheryRot += Mathf.Sin(Time.time * shakingSpeed) * 0.5f;

        grenadeLauncher.transform.rotation *= Quaternion.Euler(-grenadeLauncheryRot, 0, 0);

        //�}�E�X��Y���|�W�V�����̎擾
        float sniperRifleyRot = sniperRifle.transform.localRotation.eulerAngles.x;
        //�O�p�֐����g���e���c�ɗh�炷
        sniperRifleyRot += Mathf.Sin(Time.time * shakingSpeed) * 0.5f;

        sniperRifle.transform.rotation *= Quaternion.Euler(-sniperRifleyRot, 0, 0);
    }
    /// <summary>
    /// �����߂���
    /// </summary>
    private void HipShot()
	{
        normalGun.transform.position = normalGunPosition.transform.position;
        grenadeLauncher.transform.position = normalGunPosition.transform.position;
        //sniperRifle.transform.position = normalGunPosition.transform.position;
        shotGun.transform.position = normalGunPosition.transform.position;

        //�e�̔��ˏ���
        Shot();
    }
    /// <summary>
    /// �e���\���鏈���Ɣ��ˏ���
    /// </summary>
    private void HoldGun()
	{
        normalGun.transform.position = holdGunPosition.transform.position;
        grenadeLauncher.transform.position = holdGunPosition.transform.position;
        //sniperRifle.transform.position = holdGunPosition.transform.position;
        shotGun.transform.position = holdGunPosition.transform.position;

        if (Input.GetMouseButton(0))
        {//�e�̔��ˏ���
            Shot();
        }
    }
    /// <summary>
    /// �X�^�~�i�̃��`���[�W����
    /// </summary>
    private void StaminaRechargeProcessing()
	{
        if (stamina < maxStamina)
		{
            stamina++;
		}
	}
    /// <summary>
    /// �_���[�W����
    /// </summary>
    public void Damage()
	{
        hp--;
        if (hp < 1)
		{
            if (hp <= 0)
			{//�c�@�����炷
                DecrimentRemain();
			}
		}
	}
    /// <summary>
    /// �c�@�̌���
    /// </summary>
    public void DecrimentRemain()
	{
        remain--;
        if (remain <= 0)
        {
            deadFlag = true;
            rigidbody.drag = 0;
            FadeManager.Instance.LoadScene("EndScene", 0.5f);
        }
    }
    /// <summary>
    /// �v���C���[�̎ˌ�����
    /// </summary>
    private void Shot()
	{
        if (gunType == 1)
        {
            normalGun.GetComponent<NormalGun>().Shot(cam.transform.rotation);
        }
        else if (gunType == 2)
        {
            grenadeLauncher.GetComponent<RocketLauncher>().Shot(cam.transform.rotation);
        }
        else if (gunType == 3)
        {
            sniperRifle.GetComponent<SniperScript>().Shot(cam.transform.rotation);
        }
        else if (gunType == 4)
		{
            shotGun.GetComponent<ShotGun>().Shot(cam.transform.rotation);
        }
    }
    /// <summary>
    /// �e�ԍ��̃Z�b�g
    /// </summary>
    /// <param name="arg_gunType">�e�ԍ�</param>
    public void SetGunType(int arg_gunType)
	{
        gunType = arg_gunType;
	}
    /// <summary>
    /// �e�ԍ��̃Z�b�g�擾
    /// </summary>
    /// <param name="arg_gunType">�e�ԍ�</param>
    public int GetGunType()
    {
        return gunType;
    }
    /// <summary>
    /// �ő�X�^�~�i�̎擾
    /// </summary>
    /// <returns>�ő�X�^�~�i</returns>
    public int GetMaxStamina()
    {
        return maxStamina;
    }
    /// <summary>
    /// �X�^�~�i�̎擾
    /// </summary>
    /// <returns>�X�^�~�i</returns>
    public int GetStamina()
    {
        return stamina;
    }

    public int GetHP()
	{
        return hp;
	}
    public int GetMaxHP()
    {
        return maxHP;
    }
}
