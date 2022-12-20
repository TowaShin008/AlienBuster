using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    [SerializeField] private Vector3 explosionSize = new Vector3(10.0f, 10.0f, 10.0f);

    private bool deadFlag;
    public bool GetDeadFlag() { return deadFlag; }

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
    }

    // Update is called once per frame
    void Update()
    {       
        if (deleteFlag)
        {
            waveManager.WaveChangeFlagOn();
            GameObject newExplosion = Instantiate(explosion, this.gameObject.transform.position, Quaternion.Euler(0, 0, 0));
            newExplosion.transform.localScale = explosionSize;
            Destroy(newExplosion, 1.0f);
            //Destroy(gameObject);
            gameObject.SetActive(false);
        }

        if (deadFlag)
        {
            rigidbody.isKinematic = false;
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
    /// <param name="arg_weakTextFlag">UFOが三体同時に出た際の出現位置の調整1~3で2がプレイヤーの真上</param>
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
        if (gameObjectName != "Field"||deadFlag == false) { return; }
        deleteFlag = true;
    }
}
