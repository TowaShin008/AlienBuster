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

    //効果音
    public AudioClip shotSound;
    AudioSource audioSource;

    private float bulletSpeed = 120.0f;
    void Start()
    {
        //音のコンポーネント取得
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        float lTri = Input.GetAxis("L_Trigger");

        if (Input.GetMouseButton(1) || lTri > 0)
        {//銃を構える処理
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

        //ゲージ
        defScale = sniperGauge.transform.localScale;
        if (defScale.y <= 0.25f)
        {
            defScale.y += 0.003f;
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
        //float rTri = Input.GetAxis("R_Trigger");
        if (defScale.y >= 0.25f)
        {
            //if (Input.GetMouseButtonDown(0) || rTri > 0)
            {//弾の発射処理
                //銃の音
                audioSource.PlayOneShot(shotSound);
                // 弾を発射する場所を取得
                var bulletPosition = firingPoint.transform.position;
                // 上で取得した場所に、"bullet"のPrefabを出現させる
                GameObject sBullet = Instantiate(bullet, bulletPosition, arg_cameraRotation);
                // 出現させたボールのforward(z軸方向)
                var direction = sBullet.transform.forward;
                // 弾の発射方向にnewBallのz方向(ローカル座標)を入れ、弾オブジェクトのrigidbodyに衝撃力を加える
                sBullet.GetComponent<Rigidbody>().AddForce(direction * bulletSpeed, ForceMode.Impulse);
                // 出現させたボールの名前を"bullet"に変更
                sBullet.name = "SniperBullet";
                // 出現させたボールを0.8秒後に消す
                Destroy(sBullet, 1.0f);

                defScale.y = 0;
                sniperGauge.transform.localScale = defScale;
            }
        }
    }
}
