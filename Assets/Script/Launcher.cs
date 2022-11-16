using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Launcher : MonoBehaviour
{
    //[SerializeField]
    //private GameObject gunModel;
    //[SerializeField]
    //private GameObject normalGunPosition;
    //[SerializeField]
    //private GameObject holdGunPosition;
    //[SerializeField]
    //private GameObject firingPoint;
    [SerializeField]
    private GameObject grenade;

    private float bulletSpeed = 30.0f;
    //���10�b��1��
    const int shotDelayMaxTime = 100;
    private int shotDelayTime = shotDelayMaxTime;

    //public GameObject cam;
    //Quaternion cameraRot, characterRot;

    //const float shakingNormalSpeed = 10.0f;
    //const float shakingMaxSpeed = 15.0f;
    //float shakingSpeed = shakingNormalSpeed;
    //Rigidbody rb_grenade;

    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        ////�ړ�����
        //MoveProcessing();
        //shotDelayTime--;
        //if (Input.GetMouseButton(1))
        //{//�e���\���鏈��
        //    HoldGun();
        //}
        //else if (Input.GetMouseButton(0))
        //{//�e�̔��ˏ���
        //    gunModel.transform.position = normalGunPosition.transform.position;
        //    if (shotDelayTime > 0)
        //    {

        //    }
        //    else
        //    {
        //        //�e�̔��ˏ���
        //        Shot();
        //        shotDelayTime = shotDelayMaxTime;
        //    }
        //}
        //else
        //{
        //    gunModel.transform.position = normalGunPosition.transform.position;
        //}
    }

    //private void MoveProcessing()
    //{
    //    if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D))
    //    {

    //        if (Input.GetMouseButton(1) == false)
    //        {//�ċz���o����
    //            BreathProcessing();
    //        }
    //    }
    //}

    /// <summary>
    /// �ċz���o
    /// </summary>
    //private void BreathProcessing()
    //{
    //    //�}�E�X��Y���|�W�V�����̎擾
    //    float yRot = gunModel.transform.localRotation.eulerAngles.x;
    //    yRot += Mathf.Sin(Time.time * shakingSpeed) * 0.5f;

    //    gunModel.transform.rotation *= Quaternion.Euler(-yRot, 0, 0);
    //}
    /// <summary>
    /// �e�̔��ˏ���
    /// </summary>
    public void Shot(Vector3 arg_firingPoint, Quaternion arg_cameraRotation)
    {
        if (shotDelayTime > 0)
        {
            shotDelayTime--;
        }
        else
        {
            //�e�̔��ˏ���
            // �e�𔭎˂���ꏊ���擾
            var bulletPosition = arg_firingPoint;
            // ��Ŏ擾�����ꏊ�ɁA"grenade"��Prefab���o��������
            GameObject newBall = Instantiate(grenade, bulletPosition, arg_cameraRotation);
            // �o���������{�[����forward(z������)
            var direction = newBall.transform.forward;
            // �e�̔��˕�����newBall��z����(���[�J�����W)�����A�e�I�u�W�F�N�g��rigidbody�ɏՌ��͂�������
            newBall.GetComponent<Rigidbody>().AddForce(direction * bulletSpeed, ForceMode.Impulse);
            Invoke("Explode", 2.0f); // �O���l�[�h�𔭎˂��Ă���1.5�b��ɔ���������
                                     // �o���������{�[���̖��O��"bullet"�ɕύX
            newBall.name = grenade.name;

            // �o���������{�[����2�b��ɏ���
            Destroy(newBall, 1.5f);

            shotDelayTime = shotDelayMaxTime;
        }
    }
    /// <summary>
    /// �e���\���鏈��
    /// </summary>
    //private void HoldGun()
    //{
    //    gunModel.transform.position = holdGunPosition.transform.position;
    //    if (Input.GetMouseButton(0))
    //    {//�e�̔��ˏ���
    //        if (shotDelayTime > 0)
    //        {
    //            shotDelayTime--;
    //        }
    //        else
    //        {
    //            //�e�̔��ˏ���
    //            Shot();
    //            shotDelayTime = shotDelayMaxTime;
    //        }
    //    }
    //}
    void Explode()
    {
        GameObject[] cubes = GameObject.FindGameObjectsWithTag("Enemy"); //�uEnemy�v�^�O�̂����I�u�W�F�N�g��S�Č������Ĕz��ɂ����

        if (cubes.Length == 0) return; // �uEnemy�v�^�O�������I�u�W�F�N�g���Ȃ���Ή������Ȃ��B

        foreach (GameObject cube in cubes) // �z��ɓ��ꂽ��ЂƂ̃I�u�W�F�N�g
        {
            if (cube.GetComponent<Rigidbody>()) // Rigidbody������΁A�O���l�[�h�𒆐S�Ƃ��������̗͂�������
            {
                cube.GetComponent<Rigidbody>().AddExplosionForce(100f, transform.position, 30f, 5f, ForceMode.Impulse);
            }
        }
    }
}