using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSController : MonoBehaviour
{
    [SerializeField]
    private GameObject normalGun;
    [SerializeField]
    private GameObject normalGunPosition;
    [SerializeField]
    private GameObject holdGunPosition;
    [SerializeField]
    private GameObject firingPoint;
    [SerializeField]
    private PhysicMaterial slip;
    [SerializeField]
    private PhysicMaterial nonSlip;

    private BoxCollider collider;

    //�v���C���[�ړ����x
    const float normalSpeed = 5.0f;
    const float sprintSpeed = 15.0f;
    float speed = normalSpeed;

    //�X�e�b�v���̖��C�������Ȃ�܂ł̗P�\����
    const int stepMaxTime = 120;
    int stepTime = stepMaxTime;

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
    int hp;
    bool deadFlag;

    //�e�̗h�ꉉ�o�̕ϐ�
    const float shakingNormalSpeed = 10.0f;
    const float shakingMaxSpeed = 15.0f;
    float shakingSpeed = shakingNormalSpeed;

    // Start is called before the first frame update
    void Start()
    {
        //�t���[�����[�g�̌Œ�
        Application.targetFrameRate = 60;
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
        remain = 3;
        hp = 3;
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
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D))
        {
            var velocity = new Vector3(0, 0, 0);
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

			collider.material = slip;
        }
        if (stepTime == 0)
        {//�X�e�b�v�����Ă��Ȃ����A�X�e�b�v�P�\���ԂłȂ���Ζ��C����������
            collider.material = nonSlip;
        }
        else
		{
            stepTime--;
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
        if (collision.gameObject.name == "Enemy")
        {
            Debug.Log("Hit");
            Damage();
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

        transform.localRotation = characterRot;

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
    }
    /// <summary>
    /// �����߂���
    /// </summary>
    private void HipShot()
	{
        normalGun.transform.position = normalGunPosition.transform.position;

        normalGun.GetComponent<NormalGun>().Shot(firingPoint.transform.position, cam.transform.rotation);
    }
    /// <summary>
    /// �e���\���鏈���Ɣ��ˏ���
    /// </summary>
    private void HoldGun()
	{
        normalGun.transform.position = holdGunPosition.transform.position;
        if (Input.GetMouseButton(0))
        {//�e�̔��ˏ���
            normalGun.GetComponent<NormalGun>().Shot(firingPoint.transform.position, cam.transform.rotation);
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
            if (hp == 0)
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
        if (remain == 0)
        {
            deadFlag = true;
        }
    }
}
