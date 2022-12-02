using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Launcher : MonoBehaviour
{
    [SerializeField] private GameObject gunModel;
    [SerializeField] private GameObject normalGunPosition;
    [SerializeField] private GameObject holdGunPosition;
    [SerializeField] private GameObject firingPoint;
    [SerializeField] private GameObject grenade;
    [SerializeField] private float bulletSpeed = 30.0f;
    //���10�b��1��
    [SerializeField] private int shotDelayMaxTime = 100;   
    [SerializeField] private float DestroyTime = 2.0f; 
    private int shotDelayTime ;

    Quaternion recoilgun;
    Quaternion recoil;
    Quaternion recoilback;
    bool lerp = false;
    bool lerpback = false;

    [SerializeField] float angle = 90;
    Vector3 axis = Vector3.right;
    [SerializeField] float interpolant = 0.8f;
    float sec;

    const float shakingNormalSpeed = 10.0f;
    const float shakingMaxSpeed = 15.0f;
    float shakingSpeed = shakingNormalSpeed;
    Rigidbody rb_grenade;

    // Start is called before the first frame update
    void Start()
    {
        shotDelayTime = shotDelayMaxTime;
    }

    // Update is called once per frame
    void Update()
    {
       
        
        if (lerp == true)
        {
            sec += Time.deltaTime;
            gunModel.transform.localRotation = Quaternion.Lerp(recoilgun, recoil, sec * interpolant);
            if (gunModel.transform.localRotation == recoil)
            {
                sec = 0;
                lerp = false;
                lerpback = true;
                recoilgun = gunModel.transform.localRotation;
            }
        }

        Recoilback();

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
                //Invoke("Recoilback", 0.5f);
            }
        }
        else
        {
            gunModel.transform.position = normalGunPosition.transform.position;
           
        }
    }

    private void Recoilback()
    {
        if (lerpback == true)
        {
            sec += Time.deltaTime;
            gunModel.transform.localRotation = Quaternion.Lerp(recoilgun, recoilback, sec * interpolant);
            if (gunModel.transform.localRotation == recoilback)
            {
                sec = 0;
                lerp = false;
                lerpback = false;
            }
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
        recoilgun = gunModel.transform.localRotation;
        recoil = Quaternion.AngleAxis(angle, axis) * gunModel.transform.localRotation;
        recoilback = recoilgun;

        // �e�𔭎˂���ꏊ���擾
        var bulletPosition = firingPoint.transform.position;
        // ��Ŏ擾�����ꏊ�ɁA"grenade"��Prefab���o��������
        GameObject newBall = Instantiate(grenade, bulletPosition, firingPoint.transform.rotation); //cam.transform.rotation);//  
        // �o���������{�[����forward(z������)
        var direction = newBall.transform.forward;
        //newBall.transform.rotation = Quaternion.AngleAxis(90, Vector3.forward) * newBall.transform.rotation;
        // �e�̔��˕�����newBall��z����(���[�J�����W)�����A�e�I�u�W�F�N�g��rigidbody�ɏՌ��͂�������Lerp��float�̑��₷�ƕ������ɂȂ�
        newBall.GetComponent<Rigidbody>().AddForce(Vector3.Lerp(direction, transform.up, 0.0f) * bulletSpeed, ForceMode.VelocityChange);    
        // �o���������{�[���̖��O��"grenade"�ɕύX
        newBall.name = grenade.name;       
        // �o���������{�[����2�b��ɏ���
        Destroy(newBall, DestroyTime);
        //���R�C���̕\���i�e�̂݁j
        //gunModel.transform.localRotation = gunModel.transform.localRotation * recoil;
        lerp = true;
        
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