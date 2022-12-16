using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Util;

public class StepEnemy : MonoBehaviour
{
    //�v���C���[�̃|�W�V����
    [SerializeField] private GameObject playerObject;

    Rigidbody rigidbody;

    private bool deadFlag;

    [SerializeField] private int hp = 5;

    [SerializeField] private float speed = 0.1f;

    [SerializeField] private GameObject gun;
    [SerializeField] private GameObject firingPoint;
    [SerializeField] private GameObject bullet;
    [SerializeField] private float bulletSpeed = 60.0f;
    const int shotDelayMaxTime = 60;
    private int shotDelayTime = shotDelayMaxTime;

    [SerializeField] private float bulletDestroyTime = 0.8f;

    [SerializeField] private float stepSpeed = 50.0f;
    const int stepMaxTime = 30;
    private int stepTime = stepMaxTime;
    const int stepDelayMaxTime = 120;
    private int stepDelayTime = stepDelayMaxTime;

    //�����G�t�F�N�g
    [SerializeField] GameObject explosion;
    [SerializeField] private Vector3 explosionSize = new Vector3(1.0f, 1.0f, 1.0f);

    public GameObject ufo;

    //�h���b�v���镐��
    [SerializeField]
    private GameObject normalGunItem;
    [SerializeField]
    private GameObject rocketLauncherItem;
    [SerializeField]
    private GameObject sniperRifleItem;
    [SerializeField]
    private GameObject shotGunItem;

    bool stop;
    [SerializeField]
    GameObject pauseObject;
    // Start is called before the first frame update
    void Start()
    {
        stop = false;
        deadFlag = false;
        rigidbody = GetComponent<Rigidbody>();
        rigidbody.drag = 50;
    }

    // Update is called once per frame
    void Update()
    {
        if (pauseObject.activeSelf)
        {
            stop = false;
        }
        else
        {
            stop = true;
        }

        transform.LookAt(playerObject.transform);

        if (stop)
		{
            //�e�̔��ˏ���
            gun.transform.position = gun.transform.position;
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

            transform.position += transform.forward * speed;

            //�X�e�b�v����
            StepProcessing();

            StageOutProcessing();

            if (hp <= 0)
            {
                deadFlag = true;
                ufo.GetComponent<EnemySpawnManager>().DecrimentEnemyCount();
            }

            if (deadFlag)
            {
                DropWeapon();
                GameObject newExplosion = Instantiate(explosion, this.gameObject.transform.position, Quaternion.Euler(0, 0, 0));
                newExplosion.transform.localScale = explosionSize;
                Destroy(newExplosion, 1.0f);
                Destroy(gameObject);
            }
        }
    }
    /// <summary>
    ///�X�e�[�W�O�ɏo�Ă��܂����ۂ̃|�W�V�����C������
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

        gameObject.transform.position = currentPosition;
    }
    /// <summary>
    /// �X�e�b�v����
    /// </summary>
    private void StepProcessing()
    {
        if (stepTime > 0)
        {
            stepTime--;
            transform.RotateAround(playerObject.transform.position, Vector3.up, stepSpeed * Time.deltaTime);
            stepDelayTime = stepDelayMaxTime;
        }
        else
        {
            if (stepDelayTime > 0)
            {
                stepDelayTime--;
            }
            else
            {
                stepTime = stepMaxTime;
                if (Random.value <= 0.5f)
                {
                    stepSpeed *= -1;
                }
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        string gameObjectName = collision.gameObject.tag;
        if (gameObjectName != Constants.normalBulletName && gameObjectName != Constants.rocketBombName && gameObjectName != Constants.sniperBulletName && gameObjectName == Constants.enemyBulletName) { return; }

        if (gameObjectName == Constants.normalBulletName)
        {
            hp -= Constants.normalBulletDamage;
        }
        else if (gameObjectName == Constants.rocketBombName)
        {
            hp -= Constants.rocketBombDamage;
        }
        else if (gameObjectName == Constants.sniperBulletName)
        {
            hp -= Constants.sniperBulletDamage;
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
        GameObject newBall = Instantiate(bullet, bulletPosition, gun.transform.rotation);
        // �o���������{�[����forward(z������)
        var direction = newBall.transform.forward;
        // �e�̔��˕�����newBall��z����(���[�J�����W)�����A�e�I�u�W�F�N�g��rigidbody�ɏՌ��͂�������
        newBall.GetComponent<Rigidbody>().AddForce(direction * bulletSpeed, ForceMode.Impulse);
        // �o���������{�[���̖��O��"bullet"�ɕύX
        newBall.name = bullet.name;
        // �o���������{�[����0.8�b��ɏ���
        Destroy(newBall, bulletDestroyTime);
    }
    /// <summary>
    /// ����̃h���b�v����
    /// </summary>
    private void DropWeapon()
    {
        //�o��������G�������_���ɑI��
        var randomValue = Random.Range(1, 10);

        int playerGunType = playerObject.GetComponent<FPSController>().GetGunType();

        if (randomValue == playerGunType)
        {
            return;
        }

        if (randomValue == 1)
        {
            normalGunItem.SetActive(true);
            normalGunItem.transform.position = this.transform.position;
        }
        else if (randomValue == 2)
        {
            rocketLauncherItem.SetActive(true);
            rocketLauncherItem.transform.position = this.transform.position;
        }
        else if (randomValue == 3)
        {
            sniperRifleItem.SetActive(true);
            sniperRifleItem.transform.position = this.transform.position;
        }
        else if (randomValue == 4)
        {
            shotGunItem.SetActive(true);
            shotGunItem.transform.position = this.transform.position;
        }
    }
}
