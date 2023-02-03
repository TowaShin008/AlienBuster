using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Util;

public class RocketLauncher : MonoBehaviour
{
    public MeshRenderer mesh;
    [SerializeField]
    private GameObject rocketBomb;
    private float bulletSpeed = 30.0f;
    //大体10秒に1発
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

    [SerializeField]
    const int remainingMaxBullet = 10;
    int remainingBullets = remainingMaxBullet;

    MagazineScript magazineScript = null;

    // Start is called before the first frame update
    void Start()
    {
        //音のコンポーネント取得
        audioSource = GetComponent<AudioSource>();
        recoil = Quaternion.AngleAxis(10.0f, new Vector3(0.0f, 0.0f, 1.0f));
        recoilback = Quaternion.AngleAxis(10.0f, new Vector3(0.0f, 0.0f, 1.0f));
        shotAgainFlag = true;
        recoilback = gunModel.transform.localRotation;
        remainingBullets = remainingMaxBullet;
        MagazineInitialize();
        magazineScript.ReloadEnable(true);
        OpaqueRenderingMode();
    }

    // Update is called once per frame
    void Update()
    {
        if (lerp)
        {
            //リコイル処理（イージング処理付き）
            lerp = false;
            lerpback = true;
            gunModel.transform.DOLocalRotateQuaternion(recoil, interpolant)
                              .SetEase(Ease.InOutQuart).OnComplete(Recoilback);
        }

        //Recoilback();

        if (shotDelayTime > 0)
        {
            shotDelayTime--;
        }
    }
    private void Recoilback()
    {
        if (lerpback == true)
        {
            gunModel.transform.DOLocalRotateQuaternion(recoilback, interpolant)
                          .SetEase(Ease.InOutQuart);
            lerp = false;
            lerpback = false;
            shotAgainFlag = true;
        }
    }
    /// <summary>
    /// 銃を構える処理
    /// </summary>
    public void HoldGun(Vector3 arg_holdGunPosition)
    {
        FadeRenderingMode();
        Color color = mesh.material.color;
        color.a = 0.2f;
        mesh.material.color = color;

        this.transform.position = arg_holdGunPosition;
    }
    /// <summary>
    /// 弾の発射処理
    /// </summary>
    /// <param name="arg_cameraRotation">カメラの回転量</param>
    public bool Shot(Quaternion arg_cameraRotation)
    {
        if (!magazineScript.CheckBullets())
		{
            magazineScript.SetRemainingBulletsSize(remainingBullets);
            return true;
        }

        if (shotDelayTime <= 0 && lerpback == false && shotAgainFlag)
        {// 弾の発射処理
            magazineScript.DecrementMagazine();
            if (remainingBullets>0)
			{
                remainingBullets--;
            }
            recoilgun = gunModel.transform.localRotation;
            recoil = Quaternion.AngleAxis(angle, axis) * gunModel.transform.localRotation;
            //recoilback = recoilgun;
            //銃の音
            audioSource.PlayOneShot(shotSound);
			// 弾を発射する場所を取得
			var bulletPosition = firingPoint.transform.position;
            // 上で取得した場所に、"grenade"のPrefabを出現させる
            GameObject newBall = Instantiate(rocketBomb, bulletPosition, arg_cameraRotation);
            // 出現させたボールのforward(z軸方向)
            var direction = newBall.transform.forward;
            // 弾の発射方向にnewBallのz方向(ローカル座標)を入れ、弾オブジェクトのrigidbodyに衝撃力を加える
            newBall.GetComponent<Rigidbody>().AddForce(direction * bulletSpeed, ForceMode.Impulse);
            Invoke("Explode", 2.0f); // グレネードを発射してから1.5秒後に爆発させる
            // 出現させたボールの名前を"bullet"に変更
            newBall.name = rocketBomb.name;
            // 出現させたボールを2秒後に消す
            //Destroy(newBall, 1.5f);
            shotAgainFlag = false;

            shotDelayTime = shotDelayMaxTime;

            lerp = true;
        }
        if(remainingBullets<=0)
		{
            return false;
		}

        return true;
    }
    /// <summary>
    /// 爆破演出
    /// </summary>
    void Explode()
    {
        GameObject[] cubes = GameObject.FindGameObjectsWithTag(Constants.enemyName.ToString()); //「Enemy」タグのついたオブジェクトを全て検索して配列にいれる

        if (cubes.Length == 0) return; // 「Enemy」タグがついたオブジェクトがなければ何もしない。

        foreach (GameObject cube in cubes) // 配列に入れた一つひとつのオブジェクト
        {
            if (cube.GetComponent<Rigidbody>()) // Rigidbodyがあれば、グレネードを中心とした爆発の力を加える
            {
                cube.GetComponent<Rigidbody>().AddExplosionForce(30f, transform.position, 30f, 5f, ForceMode.Impulse);
            }
        }
    }
    /// <summary>
    /// 残弾数のリセット
    /// </summary>
    public void ResetRemainigBullet()
	{
        remainingBullets = remainingMaxBullet;
    }
    /// <summary>
    /// マガジンの初期化
    /// </summary>
    void MagazineInitialize()
    {
        this.gameObject.AddComponent<MagazineScript>();
        magazineScript = this.gameObject.GetComponent<MagazineScript>();

        magazineScript.ReloadEnable(false);
        magazineScript.SetMagazineSize(Constants.rocketLauncherMagazineSize);
        magazineScript.SetReloadTime(Constants.rocketLauncherReloadTime);
    }
    /// <summary>
    /// 初期化処理
    /// </summary>
    public void Initialize()
	{
        OpaqueRenderingMode();
        ResetRemainigBullet();
        magazineScript.SetRemainingBulletsSize(remainingMaxBullet);
        magazineScript.SetMagazineSize(Constants.rocketLauncherMagazineSize);
        magazineScript.SetReloadTime(Constants.rocketLauncherReloadTime);
    }
    /// <summary>
    /// 通常描画処理
    /// </summary>
    public void OpaqueRenderingMode()
    {
        mesh.material.SetOverrideTag("RenderType", "");
        mesh.material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
        mesh.material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
        mesh.material.SetInt("_ZWrite", 1);
        mesh.material.DisableKeyword("_ALPHATEST_ON");
        mesh.material.DisableKeyword("_ALPHABLEND_ON");
        mesh.material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
        mesh.material.renderQueue = -1;
    }
    /// <summary>
    /// 透過描画処理
    /// </summary>
    public void FadeRenderingMode()
    {
        mesh.material.SetOverrideTag("RenderType", "Transparent");
        mesh.material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
        mesh.material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
        mesh.material.SetInt("_ZWrite", 0);
        mesh.material.DisableKeyword("_ALPHATEST_ON");
        mesh.material.EnableKeyword("_ALPHABLEND_ON");
        mesh.material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
        mesh.material.renderQueue = 3000;
    }
}
