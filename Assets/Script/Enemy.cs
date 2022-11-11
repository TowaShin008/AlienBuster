using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    //�v���C���[�̃|�W�V����
    [SerializeField] private GameObject playerObject;

    Rigidbody rigidbody;

    private bool stopFlag;
    private bool deadFlag;

    [SerializeField] private int hp = 5;

    [SerializeField] private float speed = 0.1f;

    [SerializeField] private GameObject gunModel;
    [SerializeField] private GameObject gunPosition;
    [SerializeField] private GameObject firingPoint;
    [SerializeField] private GameObject bullet;
    [SerializeField] private float bulletSpeed = 60.0f;
    const int shotDelayMaxTime = 30;
    private int shotDelayTime = shotDelayMaxTime;

    [SerializeField] private float bulletDestroyTime = 0.8f;

    //�����G�t�F�N�g
    [SerializeField] GameObject explosion;

    // Start is called before the first frame update
    void Start()
    {
        stopFlag = false;
        deadFlag = false;
        rigidbody = GetComponent<Rigidbody>();
        rigidbody.drag = 50;
    }

    // Update is called once per frame
    void Update()
    {
        //�e�̔��ˏ���
        gunModel.transform.position = gunPosition.transform.position;
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

        transform.LookAt(playerObject.transform);
        transform.position += transform.forward * speed;

        if (hp <= 0)
        {
            deadFlag = true;
        }

        if (deadFlag)
        {
            Instantiate(explosion, this.gameObject.transform.position, Quaternion.Euler(0, 0, 0)); // ���ǉ�
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        string gameObjectName = collision.gameObject.tag;
        if (gameObjectName != "Bullet"&& gameObjectName != "Grenade" && gameObjectName == "EnemyBullet") { return; }

        if(gameObjectName == "Bullet")
		{
            hp--;
        }
        else if(gameObjectName == "Grenade")
		{
            hp -= 10;
		}
    }
    /// <summary>
    /// �e�̔��ˏ���
    /// </summary>
    private void Shot()
    {
        // �e�𔭎˂���ꏊ���擾
        var bulletPosition = firingPoint.transform.position;
        // ��Ŏ擾�����ꏊ�ɁA"bullet"��Prefab���o��������     
        GameObject newBall = Instantiate(bullet, bulletPosition, gunModel.transform.rotation);
        // �o���������{�[����forward(z������)
        var direction = newBall.transform.forward;
        // �e�̔��˕�����newBall��z����(���[�J�����W)�����A�e�I�u�W�F�N�g��rigidbody�ɏՌ��͂�������
        newBall.GetComponent<Rigidbody>().AddForce(direction * bulletSpeed, ForceMode.Impulse);
        // �o���������{�[���̖��O��"bullet"�ɕύX
        newBall.name = bullet.name;
        // �o���������{�[����0.8�b��ɏ���

        Destroy(newBall, bulletDestroyTime);
    }
}
