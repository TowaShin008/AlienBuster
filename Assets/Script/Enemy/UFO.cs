using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Util;

public class UFO : MonoBehaviour
{

    //現在のwaveをwaveManegerから取得
    [SerializeField] WaveManager waveManager;

    public MeshRenderer mesh;
    public MeshRenderer mesh_2;
    public MeshRenderer mesh_3;
    public MeshRenderer mesh_barrier;
    bool entryFlag = false;

    private float rotateY = 0;
    [SerializeField] private int hp = 30;

    //爆発エフェクト
    [SerializeField] GameObject explosion;
    [SerializeField] private Vector3 largeExplosionSize = new Vector3(30.0f, 30.0f, 30.0f);
    [SerializeField] private Vector3 smallExplosionSize = new Vector3(5.0f, 5.0f, 5.0f);

    private bool deadFlag;

    [SerializeField] GameObject barrier;
    //バリア発生しているか
    bool barrierFlag;
    //敵がすべて出てからバリアが発生するまでの時間
    [SerializeField] int barrierCreateMaxTime = 60;
    private int barrierCreateTime;

    public AudioClip clip;
    private AudioSource audioSource;

    //ダメージ時演出
    Color defaultColor1;
    Color defaultColor2;
    Color defaultColor3;
    [SerializeField] Color damageColor = Color.red;
    bool damageFlag = false;
    [SerializeField] int damageMaxCount = 5;
    private int damageCount;

    //弱点の位置テキスト表示
    public MeshRenderer textMesh;

    //[SerializeField]
    //public GameObject player;

    //死亡演出
    private bool deleteFlag;
    Rigidbody rigidbody;
    [SerializeField, Min(0)] int explosionDelayMaxTime = 20;
    int explosionDelayTime = 0;

    [SerializeField] private GameObject CautionText;

    private bool weakTextFlag = false;

    // Start is called before the first frame update
    void Start()
    {
        gameObject.SetActive(false);

        barrierFlag = true;
        barrierCreateTime = barrierCreateMaxTime;

        audioSource = GetComponent<AudioSource>();
        audioSource.clip = clip;
        audioSource.Play();

        defaultColor1 = mesh.material.color;
        defaultColor2 = mesh_2.material.color;
        defaultColor3 = mesh_3.material.color;

        damageCount = damageMaxCount;

        textMesh.enabled = false;
        weakTextFlag = false;

        rigidbody = GetComponent<Rigidbody>();

        explosionDelayTime = explosionDelayMaxTime;
    }

    // Update is called once per frame
    void Update()
    {       
        if (deleteFlag)
        {
            waveManager.WaveChangeFlagOn();
            GameObject newExplosion = Instantiate(explosion, this.gameObject.transform.position, Quaternion.Euler(0, 0, 0));
            newExplosion.transform.localScale = largeExplosionSize;
            Destroy(newExplosion, 1.0f);
            //Destroy(gameObject);
            gameObject.SetActive(false);
        }

        if (deadFlag)
        {
            rigidbody.isKinematic = false;

            explosionDelayTime--;

            if(explosionDelayTime <= 0)
            {
                // ランダムの位置
                Vector3 pos;
                pos = this.gameObject.transform.position + Random.insideUnitSphere * 50;
                pos.y = this.gameObject.transform.position.y + 20.0f;

                GameObject newExplosion = Instantiate(explosion, pos, Quaternion.Euler(0, 0, 0));
                newExplosion.transform.localScale = smallExplosionSize;
                Destroy(newExplosion, 0.3f);

                explosionDelayTime = explosionDelayMaxTime;
            }

        }
        else
        {
            if (entryFlag)
            {//出現演出
                EntryProcessing();
            }
            //回転演出
            RotationProcessing();

            if (hp <= 0)
            {
                deadFlag = true;
            }

            //バリアの切り替え
            if (barrierFlag == true)
            {
                barrier.SetActive(true);
            }
            else
            {
                barrier.SetActive(false);
            }

            if (gameObject.GetComponent<EnemySpawnManager>().ChangeNextWave())
            {
                barrierFlag = true;
                barrierCreateTime = barrierCreateMaxTime;
            }

            if (gameObject.GetComponent<EnemySpawnManager>().StopProcessing())
            {
                if (barrierFlag == false && entryFlag == false)
                {
                    if (barrierCreateTime > 0)
                    {
                        barrierCreateTime--;
                    }
                    else
                    {
                        barrierCreateTime = barrierCreateMaxTime;
                        barrierFlag = true;
                    }
                }
            }

            if (gameObject.GetComponent<EnemySpawnManager>().GetEnemyCount() < gameObject.GetComponent<EnemySpawnManager>().GetMaxEnemyCount())
            {
                if (entryFlag == false)
                {
                    gameObject.GetComponent<EnemySpawnManager>().SetMoveFlag(true);
                }
            }

            if (gameObject.GetComponent<EnemySpawnManager>().LauncherProcessing())
            {
                barrierFlag = false;
            }

            if (damageFlag)
            {
                mesh.material.color = damageColor;
                mesh_2.material.color = damageColor;
                mesh_3.material.color = damageColor;

                damageCount--;

                if (damageCount <= 0)
                {
                    damageFlag = false;
                    mesh.material.color = defaultColor1;
                    mesh_2.material.color = defaultColor2;
                    mesh_3.material.color = defaultColor3;
                }
            }
        }
    }
    /// <summary>
    /// 出現演出
    /// </summary>
    private void EntryProcessing()
    {
        mesh.material.color = mesh.material.color + new Color(0, 0, 0, 0.005f);
        mesh_2.material.color = mesh_2.material.color + new Color(0, 0, 0, 0.005f);
        mesh_3.material.color = mesh_3.material.color + new Color(0, 0, 0, 0.005f);
        mesh_barrier.material.SetFloat("_MyAlpha", mesh.material.color.a);

        if (mesh.material.color.a >= 1.0f)
		{
            //通常描画設定
            OpaqueRenderingMode();
            CautionText.GetComponent<Controll_Var>().ChangeEndFlag();
            entryFlag = false;
            gameObject.GetComponent<EnemySpawnManager>().SetMoveFlag(true);
            gameObject.GetComponent<CameraMove>().SetFocusFlag(false);
            if(weakTextFlag)
			{
                textMesh.enabled = true;
            }
        }
    }
    /// <summary>
    /// UFOの回転演出
    /// </summary>
    private void RotationProcessing()
	{
        if (rotateY > 360)
		{
            rotateY = 0;
		}
        else
		{
            rotateY++;
		}
        mesh.SetRotateY(rotateY);
        mesh_2.SetRotateY(rotateY);
        mesh_3.SetRotateY(rotateY);
    }
    /// <summary>
    /// 初期化処理
    /// </summary>
    /// <param name="arg_weakTextFlag">UFOのテキストを表示するかどうか</param>
	public void Initialize(bool arg_weakTextFlag = false,bool arg_focusFlag = false)
	{
        //var playerPosition = player.transform.position;
        gameObject.SetActive(true);
  //      if (positionNum == 1)
		//{
  //          gameObject.transform.position = new Vector3(player.transform.position.x + 160, gameObject.transform.position.y, player.transform.position.z);
  //      }
  //      else if (positionNum == 2)
		//{
  //          gameObject.transform.position = new Vector3(player.transform.position.x, gameObject.transform.position.y, player.transform.position.z);
  //      }
  //      else
		//{
  //          gameObject.transform.position = new Vector3(player.transform.position.x - 160, gameObject.transform.position.y, player.transform.position.z);
  //      }
        
        weakTextFlag = arg_weakTextFlag;

        gameObject.GetComponent<CameraMove>().SetFocusFlag(arg_focusFlag);
        entryFlag = true;
        hp = 30;
        deadFlag = false;
        mesh.material.color = new Color(mesh.material.color.r, mesh.material.color.g, mesh.material.color.b, 0);
        mesh_2.material.color = new Color(mesh_2.material.color.r, mesh_2.material.color.g, mesh_2.material.color.b, 0);
        mesh_3.material.color = new Color(mesh_3.material.color.r, mesh_3.material.color.g, mesh_3.material.color.b, 0);
        mesh_barrier.material.SetFloat("_MyAlpha", 0);
        gameObject.GetComponent<EnemySpawnManager>().SetMoveFlag(false);
        deleteFlag = false;
        rigidbody.isKinematic = true;
        //透過描画設定
        FadeRenderingMode();
    }
    public bool GetDeadFlag()
    {
        return deadFlag;
    }
    public void SetEntryFlag(bool arg_entryFlag)
	{
        entryFlag = arg_entryFlag;
	}
    public bool GetEntryFlag()
	{
        return entryFlag;
	}

    public void Damage(int damegeValue)
    {
        hp -= damegeValue;
        if (damageFlag == false)
        {
            defaultColor1 = mesh.material.color;
            defaultColor2 = mesh_2.material.color;
            defaultColor3 = mesh_3.material.color;
        }

        damageFlag = true;
        damageCount = damageMaxCount;
        textMesh.enabled = false;
    }

    public void SetBarrierFlag(bool arg_barrierFlag)
	{
        barrierFlag = arg_barrierFlag;
	}

    public bool GetBarrierFlag()
	{
        return barrierFlag;
	}

    private void OnTriggerEnter(Collider other)
    {
        string gameObjectName = other.gameObject.tag;
        if (gameObjectName != Constants.fieldName.ToString() || deadFlag == false) { return; }
        deleteFlag = true;
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

        mesh_2.material.SetOverrideTag("RenderType", "");
        mesh_2.material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
        mesh_2.material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
        mesh_2.material.SetInt("_ZWrite", 1);
        mesh_2.material.DisableKeyword("_ALPHATEST_ON");
        mesh_2.material.DisableKeyword("_ALPHABLEND_ON");
        mesh_2.material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
        mesh_2.material.renderQueue = -1;

        mesh_3.material.SetOverrideTag("RenderType", "");
        mesh_3.material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
        mesh_3.material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
        mesh_3.material.SetInt("_ZWrite", 1);
        mesh_3.material.DisableKeyword("_ALPHATEST_ON");
        mesh_3.material.DisableKeyword("_ALPHABLEND_ON");
        mesh_3.material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
        mesh_3.material.renderQueue = -1;

        //mesh_barrier.material.SetOverrideTag("RenderType", "");
        //mesh_barrier.material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
        //mesh_barrier.material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
        //mesh_barrier.material.SetInt("_ZWrite", 1);
        //mesh_barrier.material.DisableKeyword("_ALPHATEST_ON");
        //mesh_barrier.material.DisableKeyword("_ALPHABLEND_ON");
        //mesh_barrier.material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
        //mesh_barrier.material.renderQueue = -1;
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

        mesh_2.material.SetOverrideTag("RenderType", "Transparent");
        mesh_2.material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
        mesh_2.material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
        mesh_2.material.SetInt("_ZWrite", 0);
        mesh_2.material.DisableKeyword("_ALPHATEST_ON");
        mesh_2.material.EnableKeyword("_ALPHABLEND_ON");
        mesh_2.material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
        mesh_2.material.renderQueue = 3000;

        mesh_3.material.SetOverrideTag("RenderType", "Transparent");
        mesh_3.material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
        mesh_3.material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
        mesh_3.material.SetInt("_ZWrite", 0);
        mesh_3.material.DisableKeyword("_ALPHATEST_ON");
        mesh_3.material.EnableKeyword("_ALPHABLEND_ON");
        mesh_3.material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
        mesh_3.material.renderQueue = 3000;

        //mesh_barrier.material.SetOverrideTag("RenderType", "Transparent");
        //mesh_barrier.material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
        //mesh_barrier.material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
        //mesh_barrier.material.SetInt("_ZWrite", 0);
        //mesh_barrier.material.DisableKeyword("_ALPHATEST_ON");
        //mesh_barrier.material.EnableKeyword("_ALPHABLEND_ON");
        //mesh_barrier.material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
        //mesh_barrier.material.renderQueue = 3000;
    }
}
