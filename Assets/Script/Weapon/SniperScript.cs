using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Util;

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

    [SerializeField]
    Vector3 muzzleFlashScale;
    [SerializeField]
    GameObject muzzleFlashPrefab;

    GameObject muzzleFlash;

    [SerializeField] float dispersion = 0.02f; // ばらつき具合
    [SerializeField] float verticalToHorizontalRatio = 1.5f; // ばらつきの縦横比

    [SerializeField]
    const int remainingMaxBullet = 20;
    int remainingBullets = remainingMaxBullet;

    MagazineScript magazineScript = null;

    void Start()
    {
        //音のコンポーネント取得
        audioSource = GetComponent<AudioSource>();
        remainingBullets = remainingMaxBullet;
        MagazineInitialize();
        magazineScript.ReloadEnable(true);
    }

    // Update is called once per frame
    void Update()
    {
        float lTri = Input.GetAxis(Constants.lTriggerName.ToString());

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
                sniperMesh[i].material.color = Color.white;
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
    /// <summary>
    /// 射撃処理
    /// </summary>
    /// <param name="arg_cameraRotation">カメラの回転量</param>
    public bool Shot(Quaternion arg_cameraRotation,bool arg_holdFlag)
    {
        if (!magazineScript.CheckBullets())
        {
            magazineScript.SetRemainingBulletsSize(remainingBullets);
            return true;
        }
        //float rTri = Input.GetAxis("R_Trigger");
        if (defScale.y >= 0.25f)
        {
            magazineScript.DecrementMagazine();
            //if (Input.GetMouseButtonDown(0) || rTri > 0)
            {//弾の発射処理
                if (remainingBullets > 0)
                {
                    remainingBullets--;
                }
                //銃の音
                audioSource.PlayOneShot(shotSound);
                // 弾を発射する場所を取得
                var bulletPosition = firingPoint.transform.position;
                // 上で取得した場所に、"bullet"のPrefabを出現させる
                GameObject newBullet = Instantiate(bullet, bulletPosition, arg_cameraRotation);
                // 縦のばらつき
                float v = Random.Range(-dispersion * verticalToHorizontalRatio, dispersion * verticalToHorizontalRatio);
                Vector3 direction;

                if (arg_holdFlag)
                {
                    direction = newBullet.transform.forward;
                }
                else
                {
                    if (v >= 0)
                    {
                        direction = Vector3.Slerp(newBullet.transform.forward, newBullet.transform.up, v);
                    }
                    else
                    {
                        direction = Vector3.Slerp(newBullet.transform.forward, -newBullet.transform.up, -v);
                    }
                    // 横のばらつき
                    float h = Random.Range(-dispersion, dispersion);
                    if (h >= 0)
                    {
                        direction = Vector3.Slerp(direction, newBullet.transform.right, h);
                    }
                    else
                    {
                        direction = Vector3.Slerp(direction, -newBullet.transform.right, -h);
                    }
                }
                // 弾の発射方向にnewBallのz方向(ローカル座標)を入れ、弾オブジェクトのrigidbodyに衝撃力を加える
                newBullet.GetComponent<Rigidbody>().AddForce(direction * bulletSpeed, ForceMode.Impulse);
                // 出現させたボールの名前を"bullet"に変更
                newBullet.name = "SniperBullet";
                // 出現させたボールを0.8秒後に消す
                Destroy(newBullet, 1.0f);

                defScale.y = 0;
                sniperGauge.transform.localScale = defScale;

                MuzzleFashProcessing();
            }
        }

        if (remainingBullets <= 0)
        {
            Initialize();
            return false;
        }

        return true;
    }
    /// <summary>
    /// マズルフラッシュ演出
    /// </summary>
    private void MuzzleFashProcessing()
    {
        //マズルフラッシュON
        if (muzzleFlashPrefab != null)
        {
            if (muzzleFlash != null)
            {
                muzzleFlash.SetActive(true);
            }
            else
            {
                muzzleFlash = Instantiate(muzzleFlashPrefab, firingPoint.transform.position, firingPoint.transform.rotation);
                muzzleFlash.transform.SetParent(firingPoint.gameObject.transform);
                muzzleFlash.transform.localScale = muzzleFlashScale;
            }
        }

        //マズルフラッシュ終了演出
        StartCoroutine(MuzzleFlashEndProcessing());
    }
    /// <summary>
    /// マズルフラッシュ終了演出
    /// </summary>
    /// <returns>インターフェイス</returns>
    IEnumerator MuzzleFlashEndProcessing()
    {
        yield return new WaitForSeconds(0.15f);
        //マズルフラッシュOFF
        if (muzzleFlash != null)
        {
            muzzleFlash.SetActive(false);
        }
    }
    /// <summary>
    /// 残弾数のリセット
    /// </summary>
    public void ResetRemainigBullet()
    {
        remainingBullets = remainingMaxBullet;
    }

    public void Initialize()
	{
        ResetRemainigBullet();
        magazineScript.SetRemainingBulletsSize(remainingMaxBullet);
        magazineScript.SetMagazineSize(2);
        magazineScript.SetReloadTime(120);
        transform.position = normalGunPosition.transform.position;
        //transform.rotation = Quaternion.RotateTowards(transform.rotation, defaultPos.rotation, speed);
        sniperEdge.transform.localScale = new Vector2(5.0f, 5.0f);
        for (int i = 0; i < sniperMesh.Count; i++)
        {
            sniperMesh[i].material.color = Color.white;
        }

        Color32 color = sniperGauge.color;
        Color32 color2 = sniperGaugeEdge.color;
        color.a = 0;
        color2.a = 0;
        sniperGauge.color = color;
        sniperGaugeEdge.color = color2;
    }
    /// <summary>
    /// マガジンの初期化
    /// </summary>
    void MagazineInitialize()
    {
        this.gameObject.AddComponent<MagazineScript>();
        magazineScript = this.gameObject.GetComponent<MagazineScript>();

        magazineScript.ReloadEnable(false);
        magazineScript.SetMagazineSize(10);
        magazineScript.SetReloadTime(120);
    }
}
