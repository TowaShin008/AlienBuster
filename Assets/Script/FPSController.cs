using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Util;
public class FPSController : MonoBehaviour
{
    //�ʏ�e
    [SerializeField]
    private GameObject normalGun;
    //�O���l�[�g�����`���[
    [SerializeField]
    private GameObject rocketLauncher;
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
    [SerializeField]
    private GameObject reticle;

    //�v���C���[�ړ����x
    const float normalSpeed = 5.0f;
    const float sprintSpeed = 15.0f;
    float speed = normalSpeed;
    bool sprintFlag = false;

    //�X�e�b�v���̖��C�������Ȃ�܂ł̗P�\����
    const int stepMaxTime = 120;
    int stepTime = 0;
    float stepPower = 5.0f;

    //LSHIFT�̓��͎��Ԃ��i�[����ϐ�
    int chargeCount = 0;

    //�v���C���[�̃X�^�~�i
    const int maxStamina = 300;
    int stamina = maxStamina;

    //�J�����֘A�ϐ�
    public Camera cam;
    Quaternion cameraRot, characterRot;

    Rigidbody rigidbody;

    //XY�����̎��_���x
    public float Xsensityvity, Ysensityvity;

    //�c�@
    int remain;
    const int maxHP = 3;
    int hp = maxHP;
    bool deadFlag;
    bool holdFlag = false;

    //�e�̗h�ꉉ�o�̕ϐ�
    const float shakingNormalSpeed = 10.0f;
    const float shakingMaxSpeed = 15.0f;
    float shakingSpeed = shakingNormalSpeed;

    int gunType = 1;
    //�X�i�C�p�[���C�t����UI
    public Image sniperEdge;
    public Image sniperGaugeEdge;
    public Image sniperGauge;

    [SerializeField]
    private AudioSource jumpAudioSource;
    [SerializeField]
    private AudioSource stepAudioSource;
    [SerializeField]
    AudioSource getItemAudioSource;

    //�|�[�Y�Ɏg�����̒B
    [SerializeField]
    GameObject pauseObject;
    Vector3 savePosition;
    Quaternion saveCamera;
    Quaternion saveplayerRotation;

    //�Y�[��
    const float defaultCameraFov = 60; // �W���̎���p
    float defaultZoomCameraFov = 30;�@//�g�厞�̎���p
    float cameraFov = 60; //���݂̎���p
    const float zoomTime = 10; //�g��܂ł̎���

    // Start is called before the first frame update
    void Start()
    {
        holdFlag = false;
        cameraRot = cam.transform.localRotation;
        characterRot = transform.localRotation;
        deadFlag = false;
        rigidbody = GetComponent<Rigidbody>();
        //�J�[�\���̕\��
        Cursor.visible = false;
        //�J�[�\���̃��b�N
        Cursor.lockState = CursorLockMode.Locked;
        Physics.gravity = new Vector3(0.0f, -6.0f, 0.0f);

        //�c�@
        remain = 1;
        hp = maxHP;
        normalGun.SetActive(false);
        rocketLauncher.SetActive(false);
        sniperRifle.SetActive(false);
        shotGun.SetActive(false);

        if (gunType == 1)
		{
            normalGun.SetActive(true);
            normalGun.GetComponent<NormalGun>().Initialize();
        }
        else if (gunType == 2)
        {
            rocketLauncher.SetActive(true);
            rocketLauncher.GetComponent<RocketLauncher>().Initialize();
        }
        else if (gunType == 3)
        {
            sniperRifle.SetActive(true);
            sniperRifle.GetComponent<SniperScript>().Initialize();
        }
        else if (gunType == 4)
        {
            shotGun.SetActive(true);
            shotGun.GetComponent<ShotGun>().Initialize();
        }
        //�X�i�C�p�[���C�t����UI
        sniperEdge.enabled = false;
        sniperGaugeEdge.enabled = false;
        sniperGauge.enabled = false;

        //�ʒu�̕ۑ�
        savePosition =new Vector3(0,0,0);
        saveCamera = new Quaternion(0,0,0,0);
        saveplayerRotation = new Quaternion(0, 0, 0, 0);
        reticle.GetComponent<DistanceOfPlayer>().SetPlayerGunType(gunType);
    }

    // Update is called once per frame
    void Update()
    {
        if (pauseObject.activeSelf)
        {//�|�[�Y���̒�~����
            gameObject.transform.position = savePosition;
            cam.transform.localRotation = saveCamera;
            transform.localRotation = saveplayerRotation;
            jumpAudioSource.Stop();
            stepAudioSource.Stop();
            return;
        }
        
        //���_�ړ�����
        MoveCameraProcessing();

        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Joystick1Button5))
		{
            jumpAudioSource.Play();
        }

        if (Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.Joystick1Button5))
		{//�W�����v����
            JumpProcessing();
		}
        else
		{
            jumpAudioSource.Stop();
        }

        //�ړ�����
        MoveProcessing();

        //�X�e�b�v�Q�[�W�̃��`���[�W����
        StaminaRechargeProcessing();

        float lTri = Input.GetAxis(Constants.lTriggerName.ToString());
        float rTri = Input.GetAxis(Constants.rTriggerName.ToString());

        if (Input.GetMouseButton(1) || lTri > 0)
		{//�e���\���鏈��
			HoldGun();
            ZoomIn();
		}
        else
        {//�}�E�X���͂��Ȃ��ꍇ�́A�e���\���Ȃ��B
            if (gunType == 1)
            {
                normalGun.transform.position = normalGunPosition.transform.position;
                normalGun.GetComponent<NormalGun>().OpaqueRenderingMode();
            }
            else if (gunType == 2)
            {
                rocketLauncher.transform.position = normalGunPosition.transform.position;
                rocketLauncher.GetComponent<RocketLauncher>().OpaqueRenderingMode();
            }
            else if (gunType == 3)
            {
                //sniperRifle.transform.position = normalGunPosition.transform.position;
            }
            else if (gunType == 4)
            {
                shotGun.transform.position = normalGunPosition.transform.position;
                shotGun.GetComponent<ShotGun>().OpaqueRenderingMode();
            }

            ZoomOut();
            holdFlag = false;
        }

        if (Input.GetMouseButton(0) || rTri > 0)
		{//�e�̔��ˏ���
			Shot();
		}


        if (Input.GetKeyDown(KeyCode.Escape))
        {//�J�[�\���̕\��
            Cursor.visible = true;
        }

        //�X�e�[�W�O�̃|�W�V�����C������
        StageOutProcessing();

        if (!pauseObject.activeSelf)
        {
            savePosition = gameObject.transform.position;
            saveCamera = cam.transform.localRotation;
            saveplayerRotation = transform.localRotation;
        }
    }
	/// <summary>
	/// �ړ�����
	/// </summary>
	private void MoveProcessing()
	{
        float lsh = Input.GetAxis(Constants.lStickHorizontalName.ToString());
        float lsv = Input.GetAxis(Constants.lStickVerticalName.ToString());

        sprintFlag = false;

        var velocity = new Vector3(0, 0, 0);
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D) || lsh != 0 || lsv != 0)
        {
            //�ړ�����
            if (Input.GetKey(KeyCode.W) || lsv > 0)
            {
                velocity += gameObject.transform.rotation * new Vector3(0, 0, speed);
            }
            if (Input.GetKey(KeyCode.A) || lsh < 0)
            {
                velocity += gameObject.transform.rotation * new Vector3(-speed, 0, 0);
            }
            if (Input.GetKey(KeyCode.S) || lsv < 0)
            {
                velocity += gameObject.transform.rotation * new Vector3(0, 0, -speed);
            }
            if (Input.GetKey(KeyCode.D) || lsh > 0)
            {
                velocity += gameObject.transform.rotation * new Vector3(speed, 0, 0);
            }
            this.transform.position += velocity * Time.deltaTime;


            if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.Joystick1Button4))
            {
                chargeCount++;
                //�X�v�����g����
                SprintProcessing();
            }

            if (Input.GetKeyUp(KeyCode.LeftShift) || Input.GetKeyUp(KeyCode.Joystick1Button4))
            {//�X�e�b�v����
                stepAudioSource.Play();
                StepProcessing(velocity);
            }

			if (Input.GetMouseButton(1) == false)
			{//�ċz���o����
				BreathProcessing();
			}
        }
        if(sprintFlag == false)
		{
            speed = normalSpeed;
            chargeCount = 0;
        }
        if (stepTime > 0)
        {//�X�e�b�v�����Ă��Ȃ����A�X�e�b�v�P�\���ԂłȂ���΍R�͂���������
            stepTime--;
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
        if (collision.gameObject.tag == Constants.enemyName.ToString() || collision.gameObject.tag == Constants.enemyBulletName.ToString())
        {//�_���[�W���o
            Damage();
        }
    }

	private void OnCollisionStay(Collision collision)
	{
        if (stepTime == 0 && collision.gameObject.CompareTag(Constants.fieldName.ToString()))
        {//�X�e�b�v�����Ă��Ȃ����A�X�e�b�v�P�\���ԂłȂ���Ζ��C����������
            rigidbody.drag = 100;
        }

        if (collision.gameObject.tag == Constants.weaponItemName.ToString())
        {
            getItemAudioSource.Play();
            sniperRifle.GetComponent<SniperScript>().InitializePosition();
            if (collision.gameObject.name == Constants.rocketLauncherItemName.ToString())
            {
                normalGun.SetActive(false);
                rocketLauncher.SetActive(true);
                rocketLauncher.GetComponent<RocketLauncher>().Initialize();
                sniperRifle.SetActive(false);
                shotGun.SetActive(false);
                defaultZoomCameraFov = 30;
                gunType = 2;
                reticle.GetComponent<DistanceOfPlayer>().SetPlayerGunType(gunType);
            }
            else if (collision.gameObject.name == Constants.sniperRifleItemName.ToString())
            {
                normalGun.SetActive(false);
                rocketLauncher.SetActive(false);
                sniperRifle.SetActive(true);
                sniperRifle.GetComponent<SniperScript>().Initialize();
                shotGun.SetActive(false);
                defaultZoomCameraFov = 15;
                gunType = 3;
                reticle.GetComponent<DistanceOfPlayer>().SetPlayerGunType(gunType);
            }
            else if (collision.gameObject.name == Constants.shotGunItemName.ToString())
            {
                normalGun.SetActive(false);
                rocketLauncher.SetActive(false);
                sniperRifle.SetActive(false);
                shotGun.SetActive(true);
                shotGun.GetComponent<ShotGun>().Initialize();
                defaultZoomCameraFov = 30;
                gunType = 4;
                reticle.GetComponent<DistanceOfPlayer>().SetPlayerGunType(gunType);
            }
        }
    }

	private void OnCollisionExit(Collision collision)
	{
        if (collision.gameObject.CompareTag(Constants.fieldName.ToString()))
        {//�X�e�b�v�����Ă��Ȃ����A�X�e�b�v�P�\���ԂłȂ���Ζ��C����������
            rigidbody.drag = 0;
        }
    }
	/// <summary>
	/// �J�����̈ړ�����
	/// </summary>
	private void MoveCameraProcessing()
	{
		//Y�����_�ړ�
		float yRot = Input.GetAxis(Constants.mouseAxisYName.ToString()) * Ysensityvity;
		cameraRot *= Quaternion.Euler(-yRot, 0, 0);
		//X�����_�ړ�
		float xRot = Input.GetAxis(Constants.mouseAxisXName.ToString()) * Xsensityvity;
		characterRot *= Quaternion.Euler(0, xRot, 0);

		float rsh = Input.GetAxis(Constants.rStickHorizontalName.ToString()) * Xsensityvity;
        float rsv = Input.GetAxis(Constants.rStickVerticalName.ToString()) * Ysensityvity;

        cameraRot *= Quaternion.Euler(-rsv, 0, 0);
        characterRot *= Quaternion.Euler(0, rsh, 0);

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
                rigidbody.velocity = Vector3.zero;
                rigidbody.AddForce(arg_velocity * stepPower, ForceMode.Impulse);
                chargeCount = 0;
            }
        }
    }
    /// <summary>
    /// �X�v�����g����
    /// </summary>
    public void SprintProcessing()
	{
        sprintFlag = true;
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
        rigidbody.AddForce(new Vector3(0.0f, 10.0f, 0.0f),ForceMode.Force);
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
        float rocketLauncheryRot = rocketLauncher.transform.localRotation.eulerAngles.x;
        //�O�p�֐����g���e���c�ɗh�炷
        rocketLauncheryRot += Mathf.Sin(Time.time * shakingSpeed) * 0.5f;

        rocketLauncher.transform.rotation *= Quaternion.Euler(-rocketLauncheryRot, 0, 0);

        //�}�E�X��Y���|�W�V�����̎擾
        float sniperRifleyRot = sniperRifle.transform.localRotation.eulerAngles.x;
        //�O�p�֐����g���e���c�ɗh�炷
        sniperRifleyRot += Mathf.Sin(Time.time * shakingSpeed) * 0.5f;

        sniperRifle.transform.rotation *= Quaternion.Euler(-sniperRifleyRot, 0, 0);
    }
    /// <summary>
    /// �e���\���鏈���Ɣ��ˏ���
    /// </summary>
    private void HoldGun()
	{
        if (gunType == 1)
        {
            normalGun.GetComponent<NormalGun>().HoldGun(holdGunPosition.transform.position);
        }
        else if (gunType == 2)
        {
            rocketLauncher.GetComponent<RocketLauncher>().HoldGun(holdGunPosition.transform.position);
        }
        else if (gunType == 3)
        {
            //sniperRifle.GetComponent<NormalGun>().HoldGun(holdGunPosition.transform.position);
        }
        else if (gunType == 4)
        {
            shotGun.GetComponent<ShotGun>().HoldGun(holdGunPosition.transform.position);
        }

        holdFlag = true;
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
            FadeManager.Instance.LoadScene(Constants.endSceneName.ToString(), 0.5f);
        }
    }
    /// <summary>
    /// �v���C���[�̎ˌ�����
    /// </summary>
    private void Shot()
	{
        if (gunType == 1)
        {
            normalGun.GetComponent<NormalGun>().Shot(cam.transform.rotation, holdFlag);
        }
        else if (gunType == 2)
        {
            if (rocketLauncher.GetComponent<RocketLauncher>().Shot(cam.transform.rotation) == false)
			{
                rocketLauncher.SetActive(false);
                normalGun.SetActive(true);
                normalGun.GetComponent<NormalGun>().Initialize();
                gunType = 1;
			}
        }
        else if (gunType == 3)
        {
            if (sniperRifle.GetComponent<SniperScript>().Shot(cam.transform.rotation, holdFlag) == false)
			{
                defaultZoomCameraFov = 30;
                sniperRifle.SetActive(false);
                normalGun.SetActive(true);
                normalGun.GetComponent<NormalGun>().Initialize();
                gunType = 1;
            }
        }
        else if (gunType == 4)
		{
            if (shotGun.GetComponent<ShotGun>().Shot(cam.transform.rotation) == false)
			{
				shotGun.SetActive(false);
				normalGun.SetActive(true);
                normalGun.GetComponent<NormalGun>().Initialize();
                gunType = 1;
			}
        }
    }
    /// <summary>
    /// �v���C���[���X�e�[�W�O�ɏo�Ă��܂����ۂ̃|�W�V�����C������
    /// </summary>
    private void StageOutProcessing()
	{
        //�X�e�[�W�O�ɏo�����Ƀ|�W�V�����𐳂����ʒu�ɖ߂�����
        var currentPosition = gameObject.transform.position;

        if (currentPosition.z > Constants.stageMaxPositionZ)
        {
            currentPosition.z = Constants.stageMaxPositionZ;
        }
        if (currentPosition.z < Constants.stageMinPositionZ)
        {
            currentPosition.z = Constants.stageMinPositionZ;
        }
        if (currentPosition.x > Constants.stageMaxPositionX)
        {
            currentPosition.x = Constants.stageMaxPositionX;
        }
        if (currentPosition.x < Constants.stageMinPositionX)
        {
            currentPosition.x = Constants.stageMinPositionX;
        }
        if (currentPosition.y > Constants.stageMaxPositionY)
        {
            currentPosition.y = Constants.stageMaxPositionY;
            rigidbody.velocity = Vector3.zero;
        }
        if (currentPosition.y < Constants.stageMinPositionY)
        {
            currentPosition.y = Constants.stageMinPositionY;
        }

        gameObject.transform.position = currentPosition;
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
    /// <returns>���݂̃X�^�~�i</returns>
    public int GetStamina()
    {
        return stamina;
    }
    /// <summary>
    /// ���݂�HP�̎擾
    /// </summary>
    /// <returns></returns>
    public int GetHP()
	{
        return hp;
	}
    /// <summary>
    /// �ő�HP�̎擾
    /// </summary>
    /// <returns>�ő�HP</returns>
    public int GetMaxHP()
    {
        return maxHP;
    }
    /// <summary>
    /// �J�����̃Y�[���C������
    /// </summary>
    void ZoomIn()
    {
        if (cameraFov <= defaultZoomCameraFov)
        {
			cameraFov = defaultZoomCameraFov;
			cam.fieldOfView = cameraFov;
			return;
        }
        float n = (defaultCameraFov - defaultZoomCameraFov) / zoomTime;

        cameraFov -= n;

        cam.fieldOfView = cameraFov;
    }
    /// <summary>
    /// �J�����̃Y�[���A�E�g����
    /// </summary>
    void ZoomOut()
    {
        if (cameraFov >= defaultCameraFov)
        {
            cameraFov = defaultCameraFov;
            return;
        }
        float n = (defaultCameraFov - defaultZoomCameraFov) / zoomTime;

        cameraFov += n;

        cam.fieldOfView = cameraFov;
    }
}
