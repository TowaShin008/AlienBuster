using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SniperScript : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject bullet;

    public float speed = 0.1f;

    public Transform defaultPos;
    public Transform aimPos;
    public Transform defaultShotPos;

    public List<MeshRenderer> sniperMesh;

    public Image sniperEdge;
    public Image sniperGaugeEdge;
    public Image sniperGauge;


    private float bulletSpeed = 60.0f;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKey(KeyCode.Mouse1))
        {
            transform.position = Vector3.MoveTowards(transform.position, aimPos.position, speed);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, aimPos.rotation, speed);
            sniperEdge.transform.localScale = Vector2.MoveTowards(sniperEdge.transform.localScale, new Vector2(1.0f, 1.0f), speed + 5.0f);
            if (sniperEdge.transform.localScale.x == 1.0f)
            {
                for(int i = 0; i < sniperMesh.Count; i++)
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
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, defaultPos.position, speed);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, defaultPos.rotation, speed);
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
        Vector3 defScale = sniperGauge.transform.localScale;
        if (defScale.y <= 0.25f) defScale.y += 0.005f;


        if (defScale.y >= 0.25f)
        {
            if (Input.GetKey(KeyCode.Mouse0))
            {
                //�e�̔��ˏ���
                // �e�𔭎˂���ꏊ���擾
                var bulletPosition = defaultShotPos.position;
                // ��Ŏ擾�����ꏊ�ɁA"bullet"��Prefab���o��������
                GameObject sBullet = Instantiate(bullet, bulletPosition, Camera.main.transform.rotation);
                // �o���������{�[����forward(z������)
                var direction = sBullet.transform.forward;
                // �e�̔��˕�����newBall��z����(���[�J�����W)�����A�e�I�u�W�F�N�g��rigidbody�ɏՌ��͂�������
                sBullet.GetComponent<Rigidbody>().AddForce(direction * bulletSpeed, ForceMode.Impulse);
                // �o���������{�[���̖��O��"bullet"�ɕύX
                sBullet.name = "SniperBullet";
                // �o���������{�[����0.8�b��ɏ���
                Destroy(sBullet, 0.8f);
                


                defScale.y = 0;
            }
        }
        sniperGauge.transform.localScale = defScale;
    }
}
