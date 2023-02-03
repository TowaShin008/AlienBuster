using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Util;

public class JumpEnemy : MonoBehaviour
{
    //�v���C���[�̃|�W�V����
    [SerializeField] private GameObject playerObject;

    Rigidbody rigidbody;

    private bool deadFlag;

    [SerializeField] private int hp = 5;

    //[SerializeField] private float speed = 0.05f;

    [SerializeField] private GameObject gun;
    [SerializeField] private GameObject firingPoint;
    [SerializeField] private GameObject bullet;
    [SerializeField] private float bulletSpeed = 60.0f;
    const int shotDelayMaxTime = 60;
    private int shotDelayTime = shotDelayMaxTime;

    [SerializeField] private float bulletDestroyTime = 0.8f;

    //�W�����v��(�����)
    [SerializeField, Min(0)] float jumpPower = 5.0f;
    //���E�ւ̃W�����v�́i�O���ɂ��j
    [SerializeField, Min(0)] float aroundJumpPower = 0.15f;
    //�W�����v�̑��x�Ȑ�
    [SerializeField] AnimationCurve jumpCurve = new();
    //�W�����v�̍ő厞��
    [SerializeField, Min(0)] float maxJumpTime = 1.0f;
    private float jumpTime = 0;


    [SerializeField, Min(0)] int jampDelayMaxTime = 120;
    private int jumpDelayTime = 0;

    private bool onTheGroundFlag = false;
    private bool jumping = false;

    //�����G�t�F�N�g
    [SerializeField] GameObject explosion;
    [SerializeField] private Vector3 explosionSize = new Vector3(1.0f, 1.0f, 1.0f);

    int randomValue = 0;

    public GameObject ufo;

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
    //�q�b�g�����ɐ�����΂Ȃ��悤��
    [SerializeField, Min(0)] int hitStopMaxTime = 10;
    private int hitStopTime = 10;
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
        rigidbody.drag = 0;

        rigidbody.isKinematic = false;

        jumpDelayTime = jampDelayMaxTime;
        hitStopTime = hitStopMaxTime;
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


            if (hitStopTime > 0)
            {
                hitStopTime--;
            }


            if (hitStopTime <= 0)
            {
                rigidbody.isKinematic = false;

                // �W�����v�̊J�n����
                if (onTheGroundFlag == true && jumpDelayTime <= 0)
                {
                    jumping = true;
                    jumpDelayTime = jampDelayMaxTime;
                    onTheGroundFlag = false;
                    randomValue = Random.Range(0, 3);
                    rigidbody.drag = 0;
                }

                if (jumpDelayTime > 0 && onTheGroundFlag == true)
                {
                    jumpDelayTime--;
                    rigidbody.drag = 50;
                }

                // �W�����v���̏���
                if (jumping)
                {
                    jumpTime += Time.deltaTime;
                    if (jumpTime >= maxJumpTime)
                    {
                        Debug.Log(jumpTime);
                        jumping = false;
                        jumpTime = 0;
                    }

                }
            }
            //�X�e�[�W�O�ɏo���ۂ̃|�W�V�����̏C������
            StageOutProcessing();

            if (hp <= 0)
            {
                deadFlag = true;
                ufo.GetComponent<EnemySpawnManager>().DecrimentEnemyCount();
            }

            if (deadFlag)
            {
                DestroyEnemyUfoCounter.EnemyCounterPlus();
                DropWeapon();
                GameObject newExplosion = Instantiate(explosion, this.gameObject.transform.position, Quaternion.Euler(0, 0, 0));
                newExplosion.transform.localScale = explosionSize;
                Destroy(newExplosion, 1.0f);
                Destroy(gameObject);
            }
        }
    }

    private void FixedUpdate()
    {
        //�W�����v����
        Jump();
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
    /// �W�����v����
    /// </summary>
    void Jump()
    {
        if (!jumping)
        {
            return;
        }

        rigidbody.velocity = new Vector3(rigidbody.velocity.x, 0, rigidbody.velocity.z);
        // �W�����v�̑��x���A�j���[�V�����J�[�u����擾
        float t = jumpTime / maxJumpTime;
        float power = jumpPower * jumpCurve.Evaluate(t);
        if (t >= 1)
        {
            jumping = false;
            jumpTime = 0;
        }
        rigidbody.AddForce(power * Vector3.up, ForceMode.Impulse);

        switch (randomValue)
        {
            case 0:
                rigidbody.AddForce(aroundJumpPower * transform.right, ForceMode.Impulse);
                break;
            case 1:
                rigidbody.AddForce(aroundJumpPower * -(transform.right), ForceMode.Impulse);
                break;
            case 2:
                rigidbody.AddForce(aroundJumpPower * transform.forward, ForceMode.Impulse);
                break;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        string gameObjectName = collision.gameObject.tag;
        if (gameObjectName != Constants.normalBulletName.ToString() && gameObjectName != Constants.rocketBombName.ToString() && gameObjectName != Constants.sniperBulletName.ToString() && gameObjectName == Constants.enemyBulletName.ToString()) { return; }

        if (gameObjectName == Constants.normalBulletName.ToString())
        {
            rigidbody.isKinematic = true;
            hitStopTime = hitStopMaxTime;
            hp -= Constants.normalBulletDamage;
            damageAudioSource.PlayOneShot(damageAudioClip);
        }
        else if (gameObjectName == Constants.rocketBombName.ToString())
        {
            rigidbody.isKinematic = true;
            hitStopTime = hitStopMaxTime;
            hp -= Constants.rocketBombDamage;
            damageAudioSource.PlayOneShot(damageAudioClip);
        }
        else if (gameObjectName == Constants.sniperBulletName.ToString())
        {
            rigidbody.isKinematic = true;
            hitStopTime = hitStopMaxTime;
            hp -= Constants.sniperBulletDamage;
            damageAudioSource.PlayOneShot(damageAudioClip);
        }

        if (gameObjectName == Constants.fieldName.ToString())
        {
            onTheGroundFlag = true;
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
        int randomValue = Random.Range(1, 6);

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
