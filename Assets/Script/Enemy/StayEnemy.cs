using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Util;

public class StayEnemy : MonoBehaviour
{
    //�v���C���[�̃|�W�V����
    [SerializeField] private GameObject playerObject;

    Rigidbody rigidbody;

    private bool deadFlag;

    [SerializeField] private int hp = 5;

    [SerializeField] private float speed = 0.1f;

    [SerializeField] private GameObject gun;
    [SerializeField] private GameObject firingPoint;
    [SerializeField] private GameObject homingMissile;
    [SerializeField] private float bulletSpeed = 60.0f;
    const int shotDelayMaxTime = 180;
    private int shotDelayTime = shotDelayMaxTime;

    [SerializeField] private float bulletDestroyTime = 0.8f;

    //�����G�t�F�N�g
    [SerializeField] GameObject explosion;
    [SerializeField] private Vector3 explosionSize = new Vector3(1.0f, 1.0f, 1.0f);

    //�h���b�v���镐��
    [SerializeField]
    private GameObject rocketLauncherItem;
    [SerializeField]
    private GameObject sniperRifleItem;
    [SerializeField]
    private GameObject shotGunItem;

    bool stop;
    [SerializeField]
    GameObject pauseObject;
    //�_���[�W��se
    AudioSource damageAudioSource;
    [SerializeField]
    AudioClip damageAudioClip;
    // Start is called before the first frame update
    void Start()
    {
        stop = false;
        deadFlag = false;
        rigidbody = GetComponent<Rigidbody>();
        rigidbody.drag = 50;
        damageAudioSource = GetComponent<AudioSource>();
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

            if (hp <= 0)
            {
                deadFlag = true;
            }

            StageOutProcessing();

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

    private void OnCollisionEnter(Collision collision)
    {
        string gameObjectName = collision.gameObject.tag;
        if (gameObjectName != Constants.normalBulletName.ToString() && gameObjectName != Constants.rocketBombName.ToString() && gameObjectName != Constants.sniperBulletName.ToString() && gameObjectName == Constants.enemyBulletName.ToString()) { return; }

        if (gameObjectName == Constants.normalBulletName.ToString())
        {
            hp -= Constants.normalBulletDamage;
            damageAudioSource.PlayOneShot(damageAudioClip);
        }
        else if (gameObjectName == Constants.rocketBombName.ToString())
        {
            hp -= Constants.rocketBombDamage;
            damageAudioSource.PlayOneShot(damageAudioClip);
        }
        else if (gameObjectName == Constants.sniperBulletName.ToString())
        {
            hp -= Constants.sniperBulletDamage;
            damageAudioSource.PlayOneShot(damageAudioClip);
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
    /// �e�̔��ˏ���
    /// </summary>
    private void Shot()
    {
        // �e�𔭎˂���ꏊ���擾
        var bulletPosition = firingPoint.transform.position;
        // ��Ŏ擾�����ꏊ�ɁA"grenade"��Prefab���o��������
        GameObject newBall = Instantiate(homingMissile, bulletPosition, gun.transform.rotation);
        //// �o���������{�[����forward(z������)
        //var direction = newBall.transform.up;
        //// �e�̔��˕�����newBall��z����(���[�J�����W)�����A�e�I�u�W�F�N�g��rigidbody�ɏՌ��͂�������
        //newBall.GetComponent<Rigidbody>().AddForce(direction * bulletSpeed, ForceMode.Impulse);
        Invoke("Explode", 6.0f); // �O���l�[�h�𔭎˂��Ă���1.5�b��ɔ���������
                                 // �o���������{�[���̖��O��"bullet"�ɕύX
        newBall.name = homingMissile.name;
    }
    /// <summary>
    /// ���j���o
    /// </summary>
    void Explode()
    {
        GameObject[] cubes = GameObject.FindGameObjectsWithTag(Constants.enemyName.ToString()); //�uEnemy�v�^�O�̂����I�u�W�F�N�g��S�Č������Ĕz��ɂ����

        if (cubes.Length == 0) return; // �uEnemy�v�^�O�������I�u�W�F�N�g���Ȃ���Ή������Ȃ��B

        foreach (GameObject cube in cubes) // �z��ɓ��ꂽ��ЂƂ̃I�u�W�F�N�g
        {
            if (cube.GetComponent<Rigidbody>()) // Rigidbody������΁A�O���l�[�h�𒆐S�Ƃ��������̗͂�������
            {
                cube.GetComponent<Rigidbody>().AddExplosionForce(30f, transform.position, 30f, 5f, ForceMode.Impulse);
            }
        }
    }
    /// <summary>
    /// ����̃h���b�v����
    /// </summary>
    private void DropWeapon()
    {
        //�o��������G�������_���ɑI��
        int randomValue = Random.Range(2, 6);

        int playerGunType = playerObject.GetComponent<FPSController>().GetGunType();

        if (randomValue == playerGunType)
        {
            return;
        }

        if (randomValue == 2)
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