using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketLauncher : MonoBehaviour
{
    [SerializeField]
    private GameObject rocketBomb;
    private float bulletSpeed = 30.0f;
    //���10�b��1��
    const int shotDelayMaxTime = 100;
    private int shotDelayTime = 0;

    [SerializeField]
    private GameObject firingPoint;

    Quaternion recoilgun;
    Quaternion recoil;
    Quaternion recoilback;
    bool lerp = false;
    bool lerpback = false;

    [SerializeField] float angle = -45.0f;
    Vector3 axis = Vector3.right;
    [SerializeField] float interpolant = 1.5f;
    float sec;

    public AudioClip shotSound;
    AudioSource audioSource;

    [SerializeField]
    private GameObject gunModel;

    private bool shotAgainFlag = true;

    // Start is called before the first frame update
    void Start()
    {
        //���̃R���|�[�l���g�擾
        audioSource = GetComponent<AudioSource>();
        recoil = Quaternion.AngleAxis(10.0f, new Vector3(0.0f, 0.0f, 1.0f));
        recoilback = Quaternion.AngleAxis(10.0f, new Vector3(0.0f, 0.0f, 1.0f));
        shotAgainFlag = true;
        recoilback = gunModel.transform.localRotation;
    }

    // Update is called once per frame
    void Update()
    {
        if (lerp)
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

        if (shotDelayTime > 0)
        {
            shotDelayTime--;
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
                shotAgainFlag = true;
            }
        }
    }
    /// <summary>
    /// �e�̔��ˏ���
    /// </summary>
    /// <param name="arg_firingPoint">�e�̃|�W�V����</param>
    /// <param name="arg_cameraRotation">�J�����̉�]��</param>
    public void Shot(Quaternion arg_cameraRotation)
    {
        if (shotDelayTime <= 0 && lerpback == false && shotAgainFlag)
        {// �e�̔��ˏ���
            recoilgun = gunModel.transform.localRotation;
            recoil = Quaternion.AngleAxis(angle, axis) * gunModel.transform.localRotation;
            //recoilback = recoilgun;
            //�e�̉�
            audioSource.PlayOneShot(shotSound);
			// �e�𔭎˂���ꏊ���擾
			var bulletPosition = firingPoint.transform.position;
            // ��Ŏ擾�����ꏊ�ɁA"grenade"��Prefab���o��������
            GameObject newBall = Instantiate(rocketBomb, bulletPosition, arg_cameraRotation);
            // �o���������{�[����forward(z������)
            var direction = newBall.transform.forward;
            // �e�̔��˕�����newBall��z����(���[�J�����W)�����A�e�I�u�W�F�N�g��rigidbody�ɏՌ��͂�������
            newBall.GetComponent<Rigidbody>().AddForce(direction * bulletSpeed, ForceMode.Impulse);
            Invoke("Explode", 2.0f); // �O���l�[�h�𔭎˂��Ă���1.5�b��ɔ���������
            // �o���������{�[���̖��O��"bullet"�ɕύX
            newBall.name = rocketBomb.name;
            // �o���������{�[����2�b��ɏ���
            //Destroy(newBall, 1.5f);
            shotAgainFlag = false;

            shotDelayTime = shotDelayMaxTime;

            lerp = true;
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
