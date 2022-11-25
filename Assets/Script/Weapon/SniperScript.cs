using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SniperScript : MonoBehaviour
{
    public GameObject bullet;

    public float speed = 0.1f;

    [SerializeField]
    private GameObject normalGunPosition;
    [SerializeField]
    private GameObject holdGunPosition;
    //public Transform defaultPos;
    //public Transform aimPos;
    [SerializeField]
    private GameObject firingPoint;

    public List<MeshRenderer> sniperMesh;

    public Image sniperEdge;
    public Image sniperGaugeEdge;
    public Image sniperGauge;
    Vector3 defScale;

    private float bulletSpeed = 60.0f;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(1))
        {//�e���\���鏈��
            HoldGun();
        }
        else
        {
			transform.position = normalGunPosition.transform.position;
			//transform.rotation = Quaternion.RotateTowards(transform.rotation, defaultPos.rotation, speed);
			sniperEdge.transform.localScale = Vector2.MoveTowards(sniperEdge.transform.localScale, new Vector2(5.0f, 5.0f), speed * 5.0f);
            for (int i = 0; i < sniperMesh.Count; i++)
            {
                sniperMesh[i].material.color = new Color32(255, 255, 255, 255);
            }

            Color32 color = sniperGauge.color;
            Color32 color2 = sniperGaugeEdge.color;
            color.a = 0;
            color2.a = 0;
            sniperGauge.color = color;
            sniperGaugeEdge.color = color2;
        }

        //�Q�[�W
        defScale = sniperGauge.transform.localScale;
        if (defScale.y <= 0.25f)
        {
            defScale.y += 0.005f;
        }
        sniperGauge.transform.localScale = defScale;
    }

    public void HoldGun()
	{
        sniperEdge.enabled = true;
        sniperGaugeEdge.enabled = true;
        sniperGauge.enabled = true;
        transform.position = holdGunPosition.transform.position;
        //transform.rotation = Quaternion.RotateTowards(transform.rotation, aimPos.rotation, speed);
        sniperEdge.transform.localScale = Vector2.MoveTowards(sniperEdge.transform.localScale, new Vector2(1.0f, 1.0f), speed + 5.0f);
        if (sniperEdge.transform.localScale.x == 1.0f)
        {
            for (int i = 0; i < sniperMesh.Count; i++)
            {
                sniperMesh[i].material.color = new Color32(255, 255, 255, 0);
            }

            Color32 color = sniperGauge.color;
            Color32 color2 = sniperGaugeEdge.color;
            color.a = 180;
            color2.a = 180;
            sniperGauge.color = color;
            sniperGaugeEdge.color = color2;
        }
    }

    public void Shot(Quaternion arg_cameraRotation)
	{
        if (defScale.y >= 0.25f)
        {
            if (Input.GetMouseButtonDown(0))
            {//�e�̔��ˏ���
                // �e�𔭎˂���ꏊ���擾
                var bulletPosition = firingPoint.transform.position;
                // ��Ŏ擾�����ꏊ�ɁA"bullet"��Prefab���o��������
                GameObject sBullet = Instantiate(bullet, bulletPosition, arg_cameraRotation);
                // �o���������{�[����forward(z������)
                var direction = sBullet.transform.forward;
                // �e�̔��˕�����newBall��z����(���[�J�����W)�����A�e�I�u�W�F�N�g��rigidbody�ɏՌ��͂�������
                sBullet.GetComponent<Rigidbody>().AddForce(direction * bulletSpeed, ForceMode.Impulse);
                // �o���������{�[���̖��O��"bullet"�ɕύX
                sBullet.name = "SniperBullet";
                // �o���������{�[����0.8�b��ɏ���
                Destroy(sBullet, 0.8f);

                defScale.y = 0;
                sniperGauge.transform.localScale = defScale;
            }
        }
    }
}
