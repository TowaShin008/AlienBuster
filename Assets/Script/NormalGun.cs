using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalGun : MonoBehaviour
{
    [SerializeField]
    private GameObject bullet;
    //private float bulletSpeed = 60.0f;
    private float bulletSpeed = 30.0f;
    //const int shotDelayMaxTime = 10;
    const int shotDelayMaxTime = 5;
    private int shotDelayTime = shotDelayMaxTime;
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
    public void Shot(Vector3 arg_firingPoint, Quaternion arg_cameraRotation)
	{
        if (shotDelayTime <= 0)
        {
            //�e�̔��ˏ���
            // �e�𔭎˂���ꏊ���擾
            var bulletPosition = arg_firingPoint;
            // ��Ŏ擾�����ꏊ�ɁA"bullet"��Prefab���o��������
            GameObject newBall = Instantiate(bullet, bulletPosition, arg_cameraRotation);
            // �o���������{�[����forward(z������)
            var direction = newBall.transform.forward;
            // �e�̔��˕�����newBall��z����(���[�J�����W)�����A�e�I�u�W�F�N�g��rigidbody�ɏՌ��͂�������
            newBall.GetComponent<Rigidbody>().AddForce(direction * bulletSpeed, ForceMode.Impulse);
            // �o���������{�[���̖��O��"bullet"�ɕύX
            newBall.name = bullet.name;
            // �o���������{�[����0.8�b��ɏ���
            Destroy(newBall, 0.8f);

            shotDelayTime = shotDelayMaxTime;
        }
    }
}
