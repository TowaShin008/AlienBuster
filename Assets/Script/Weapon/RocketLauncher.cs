using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketLauncher : MonoBehaviour
{
    [SerializeField]
    private GameObject grenade;
    private float bulletSpeed = 30.0f;
    //���10�b��1��
    const int shotDelayMaxTime = 100;
    private int shotDelayTime = 0;

    [SerializeField]
    private GameObject firingPoint;

    // Start is called before the first frame update
    void Start()
    {

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
    /// <param name="arg_firingPoint">�e�̃|�W�V����</param>
    /// <param name="arg_cameraRotation">�J�����̉�]��</param>
    public void Shot(Quaternion arg_cameraRotation)
    {
        if (shotDelayTime <= 0)
        {
            // �e�̔��ˏ���
            // �e�𔭎˂���ꏊ���擾
            var bulletPosition = firingPoint.transform.position;
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
    /// ���j���o
    /// </summary>
    void Explode()
    {
        GameObject[] cubes = GameObject.FindGameObjectsWithTag("Enemy"); //�uEnemy�v�^�O�̂����I�u�W�F�N�g��S�Č������Ĕz��ɂ����

        if (cubes.Length == 0) return; // �uEnemy�v�^�O�������I�u�W�F�N�g���Ȃ���Ή������Ȃ��B

        foreach (GameObject cube in cubes) // �z��ɓ��ꂽ��ЂƂ̃I�u�W�F�N�g
        {
            if (cube.GetComponent<Rigidbody>()) // Rigidbody������΁A�O���l�[�h�𒆐S�Ƃ��������̗͂�������
            {
                cube.GetComponent<Rigidbody>().AddExplosionForce(30f, transform.position, 30f, 5f, ForceMode.Impulse);
            }
        }
    }
}
