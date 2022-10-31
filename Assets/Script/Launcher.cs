using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Launcher : MonoBehaviour
{
    [SerializeField]
    private GameObject gunModel;
    [SerializeField]
    private GameObject normalGunPosition;
    [SerializeField]
    private GameObject holdGunPosition;
    [SerializeField]
    private GameObject firingPoint;
    [SerializeField]
    private GameObject grenade;
    private float bulletSpeed = 30.0f;
    //���10�b��1��
    const int shotDelayMaxTime = 100;
    private int shotDelayTime = shotDelayMaxTime;

    public GameObject cam;
    Quaternion cameraRot, characterRot;

    const float shakingNormalSpeed = 10.0f;
    const float shakingMaxSpeed = 15.0f;
    float shakingSpeed = shakingNormalSpeed;
    Rigidbody rb_grenade;

    // Start is called before the first frame update
    void Start()
    {
      
    }

    // Update is called once per frame
    void Update()
    {
        //�ړ�����
        MoveProcessing();
        shotDelayTime--;
        if (Input.GetMouseButton(1))
        {//�e���\���鏈��
            HoldGun();
        }
        else if (Input.GetMouseButton(0))
        {//�e�̔��ˏ���
            gunModel.transform.position = normalGunPosition.transform.position;
            if (shotDelayTime > 0)
            {

            }
            else
            {
                //�e�̔��ˏ���
                Shot();
                shotDelayTime = shotDelayMaxTime;
            }
        }
        else
        {
            gunModel.transform.position = normalGunPosition.transform.position;
        }
    }

    private void MoveProcessing()
    {
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D))
        {
            if (Input.GetMouseButton(1) == false)
            {//�ċz���o����
                BreathProcessing();
            }
        }
    }

    /// <summary>
    /// �ċz���o
    /// </summary>
    private void BreathProcessing()
    {
        //�}�E�X��Y���|�W�V�����̎擾
        float yRot = gunModel.transform.localRotation.eulerAngles.x;
        yRot += Mathf.Sin(Time.time * shakingSpeed) * 0.5f;

        gunModel.transform.rotation *= Quaternion.Euler(-yRot, 0, 0);
    }
    /// <summary>
    /// �e�̔��ˏ���
    /// </summary>
    private void Shot()
    {
        // �e�𔭎˂���ꏊ���擾
        var bulletPosition = firingPoint.transform.position;
        // ��Ŏ擾�����ꏊ�ɁA"grenade"��Prefab���o��������
        GameObject newBall = Instantiate(grenade, bulletPosition, cam.transform.rotation);
        // �o���������{�[����forward(z������)
        var direction = newBall.transform.forward;
        // �e�̔��˕�����newBall��z����(���[�J�����W)�����A�e�I�u�W�F�N�g��rigidbody�ɏՌ��͂�������
        newBall.GetComponent<Rigidbody>().AddForce(direction * bulletSpeed, ForceMode.Impulse);

        // �o���������{�[���̖��O��"bullet"�ɕύX
        newBall.name = grenade.name;
        // �o���������{�[����0.8�b��ɏ���
        Destroy(newBall, 3.8f);
    }
    /// <summary>
    /// �e���\���鏈��
    /// </summary>
    private void HoldGun()
    {
        gunModel.transform.position = holdGunPosition.transform.position;
        if (Input.GetMouseButton(0))
        {//�e�̔��ˏ���
            if (shotDelayTime > 0)
            {
                shotDelayTime--;
            }
            else
            {
                //�e�̔��ˏ���
                Shot();
                shotDelayTime = shotDelayMaxTime;
            }
        }
    }
   
}

