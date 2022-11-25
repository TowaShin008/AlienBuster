using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Jump: MonoBehaviour
{
    //�v���C���[�̃|�W�V����
    [SerializeField] private GameObject playerObject;

    Rigidbody rigidbody;

    private bool stopFlag;
    private bool deadFlag;

    [SerializeField] private int hp = 5;

    //[SerializeField] private float speed = 0.05f;

    [SerializeField] private GameObject gunModel;
    [SerializeField] private GameObject gunPosition;
    [SerializeField] private GameObject firingPoint;
    [SerializeField] private GameObject bullet;
    [SerializeField] private float bulletSpeed = 60.0f;
    const int shotDelayMaxTime = 30;
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

    private bool onTheGroundFlog = false;
    private bool jumping = false;

    //�����G�t�F�N�g
    [SerializeField] GameObject explosion;
    [SerializeField] private Vector3 explosionSize = new Vector3(1.0f, 1.0f, 1.0f);

    int randomValue = 0;

    // Start is called before the first frame update
    void Start()
    {
        stopFlag = false;
        deadFlag = false;
        rigidbody = GetComponent<Rigidbody>();
        rigidbody.drag = 0;

        jumpDelayTime = jampDelayMaxTime;
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
        
        // �W�����v�̊J�n����
        if (onTheGroundFlog == true && jumpDelayTime <= 0)
        {
            jumping = true;
            jumpDelayTime = jampDelayMaxTime;
            onTheGroundFlog = false;
            randomValue = Random.Range(0, 3);
        }

        if (jumpDelayTime > 0 && onTheGroundFlog == true)
        {
            jumpDelayTime--;
        }

        // �W�����v���̏���
        if (jumping)
        {
            jumpTime += Time.deltaTime;
            if (jumpTime >= maxJumpTime)
            {
                jumping = false;
                jumpTime = 0;
            }
           
        }       

        if (hp <= 0)
        {
            deadFlag = true;
        }

        if (deadFlag)
        {
            GameObject newExplosion = Instantiate(explosion, this.gameObject.transform.position, Quaternion.Euler(0, 0, 0));
            newExplosion.transform.localScale = explosionSize;
            Destroy(newExplosion, 1.0f);
            Destroy(gameObject);
        }
    }

    private void FixedUpdate()
    {
        Jump();
    }
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
        if (gameObjectName != "Bullet" && gameObjectName != "Grenade" && gameObjectName == "EnemyBullet" && gameObjectName == "Field") { return; }

        if (gameObjectName == "Bullet")
        {
            hp--;
        }
        else if (gameObjectName == "Grenade")
        {
            hp -= 10;
        }

        if(gameObjectName == "Field")
        {
            onTheGroundFlog = true;
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
