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


        //ゲージ
        Vector3 defScale = sniperGauge.transform.localScale;
        if (defScale.y <= 0.25f) defScale.y += 0.005f;


        if (defScale.y >= 0.25f)
        {
            if (Input.GetKey(KeyCode.Mouse0))
            {
                //弾の発射処理
                // 弾を発射する場所を取得
                var bulletPosition = defaultShotPos.position;
                // 上で取得した場所に、"bullet"のPrefabを出現させる
                GameObject sBullet = Instantiate(bullet, bulletPosition, Camera.main.transform.rotation);
                // 出現させたボールのforward(z軸方向)
                var direction = sBullet.transform.forward;
                // 弾の発射方向にnewBallのz方向(ローカル座標)を入れ、弾オブジェクトのrigidbodyに衝撃力を加える
                sBullet.GetComponent<Rigidbody>().AddForce(direction * bulletSpeed, ForceMode.Impulse);
                // 出現させたボールの名前を"bullet"に変更
                sBullet.name = "SniperBullet";
                // 出現させたボールを0.8秒後に消す
                Destroy(sBullet, 0.8f);
                


                defScale.y = 0;
            }
        }
        sniperGauge.transform.localScale = defScale;
    }
}
