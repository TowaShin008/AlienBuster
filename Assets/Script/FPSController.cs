using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSController : MonoBehaviour
{
    const float normalSpeed = 5.0f;
    const float sprintSpeed = 15.0f;
    float speed = normalSpeed;

    const int accelMaxCount = 2;
    int accelCount = accelMaxCount;

    const int accelMaxTime = 120;
    int accelTime = accelMaxTime;

    const int accelRechargeMaxCount = 180;
    int accelRechargeCount = accelRechargeMaxCount;

    public GameObject cam;
    Quaternion cameraRot, characterRot;

    Rigidbody rigidbody;

    //XY�����̎��_���x
    public float Xsensityvity, Ysensityvity;

    bool deadFlag;

    //�ϐ��̐錾(�p�x�̐����p)
    float minX;
    float maxX;

    int chargeCount = 0;

    const float shakingNormalSpeed = 10.0f;
    const float shakingMaxSpeed = 15.0f;
    float shakingSpeed = shakingNormalSpeed;

    private bool landingFlag;

    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = 60;
        cameraRot = cam.transform.localRotation;
        characterRot = transform.localRotation;
        deadFlag = false;
        const float normalMaxX = 90.0f;
        const float normalMinX = -90.0f;
        minX = normalMinX;
        maxX = normalMaxX;
        rigidbody = GetComponent<Rigidbody>();
        //�J�[�\���̕\��
        Cursor.visible = false;
        //�J�[�\���̃��b�N
        Cursor.lockState = CursorLockMode.Locked;
        landingFlag = true;
        Physics.gravity = new Vector3(0.0f, -4.0f, 0.0f);
    }

    // Update is called once per frame
    void Update()
    {
        //�J�����̈ړ�����
        MoveCameraProcessing();

        if(Input.GetKey(KeyCode.Space))
		{//�W�����v����
            JumpProcessing();
		}

        if(Input.GetKey(KeyCode.LeftShift))
		{
            chargeCount++;
            if(chargeCount>=30)
            {//�X�v�����g����
                SprintProcessing();
            }
        }

        //�����Q�[�W�̃��`���[�W����
        AccelRechargeProcessing();

        //�J�[�\���̔�\��
        if (Input.GetKeyDown(KeyCode.Escape))
		{
            Cursor.visible = true;
		}

        
        //�ړ�����
        MoveProcessing();
    }
    /// <summary>
    /// �ړ�����
    /// </summary>
    private void MoveProcessing()
	{
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D))
        {
            var velocity = new Vector3(0, 0, 0);
            //�v���C���[�ړ�����
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
            gameObject.transform.position += velocity * Time.deltaTime;

            if (Input.GetKeyUp(KeyCode.LeftShift))
            {//��������
                AccelProcessing(velocity);
            }

            //if (Input.GetMouseButton(1) == false)
            //{//�ċz���o����
            //    BreathProcessing();
            //}

            rigidbody.drag = 1;
        }

        if (accelTime == 0)
        {
            rigidbody.drag = 50;
        }
        else
		{
            accelTime--;
		}
    }
    /// <summary>
    /// �p�x�����֐��̍쐬
    /// </summary>
    /// <param name="q"></param>
    /// <returns>�N�H�[�^�j�I��</returns>
    public Quaternion ClampRotation(Quaternion q)
    {
        //q = x,y,z,w (x,y,z�̓x�N�g���i�ʂƌ����j�Fw�̓X�J���[�i���W�Ƃ͖��֌W�̗ʁj)

        q.x /= q.w;
        q.y /= q.w;
        q.z /= q.w;
        q.w = 1f;

        float angleX = Mathf.Atan(q.x) * Mathf.Rad2Deg * 2f;

        angleX = Mathf.Clamp(angleX, minX, maxX);

        q.x = Mathf.Tan(angleX * Mathf.Deg2Rad * 0.5f);

        return q;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "Enemy")
        {
            Debug.Log("Hit");
            deadFlag = true;
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
        //X�����_
        float xRot = Input.GetAxis("Mouse X") * Xsensityvity;
        characterRot *= Quaternion.Euler(0, xRot, 0);

        transform.localRotation = characterRot;

        //Update�̒��ō쐬�����֐����Ă�
        cameraRot = ClampRotation(cameraRot);

        cam.transform.localRotation = cameraRot;
    }
    /// <summary>
    /// ��������
    /// </summary>
    public void AccelProcessing(Vector3 arg_velocity)
	{
        if (chargeCount < 30)
        {//��������
            if (accelCount > 0)
            {
                accelTime = accelMaxTime;
                accelCount--;
                rigidbody.AddForce(arg_velocity * 400.0f);
                Debug.Log(accelCount);
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
        speed = sprintSpeed;
        shakingSpeed = shakingMaxSpeed;
	}
    /// <summary>
    /// �W�����v����
    /// </summary>
    public void JumpProcessing()
	{
        rigidbody.AddForce(new Vector3(0.0f, 3.0f, 0.0f));
	}

    /// <summary>
    /// �����Q�[�W�̃��`���[�W����
    /// </summary>
    private void AccelRechargeProcessing()
	{
        if (accelCount < accelMaxCount)
		{
            accelRechargeCount--;
            //���`���[�W�J�E���g���[�����ǂ���
            bool isAccelRechargeCountEmpty = accelRechargeCount <= 0;

            if (isAccelRechargeCountEmpty)
			{
                accelCount++;
                accelRechargeCount = accelRechargeMaxCount;

                Debug.Log(accelCount);
            }
		}
	}
}
