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
    bool entryFlag = false;
    //public GameObject enemySpawnManager;
    private float rotateY = 0;
    [SerializeField] private int hp = 30;

    //爆発エフェクト
    [SerializeField] GameObject explosion;
    [SerializeField] private Vector3 explosionSize = new Vector3(10.0f, 10.0f, 10.0f);

    private bool deadFlag;
    // Start is called before the first frame update
    void Start()
    {
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
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

        if (deadFlag)
        {
            waveManager.WaveChangeFlagOn();
            GameObject newExplosion = Instantiate(explosion, this.gameObject.transform.position, Quaternion.Euler(0, 0, 0));
            newExplosion.transform.localScale = explosionSize;
            Destroy(newExplosion, 1.0f);
            //Destroy(gameObject);
            gameObject.SetActive(false);
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
        if (mesh.material.color.a >= 1.0f)
        {
            entryFlag = false;
            gameObject.GetComponent<EnemySpawnManager>().SetMoveFlag(true);
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
	public void Initialize()
    {
        gameObject.SetActive(true);
        entryFlag = true;
        hp = 30;
        deadFlag = false;
        mesh.material.color = new Color(mesh.material.color.r, mesh.material.color.g, mesh.material.color.b, 0);
        mesh_2.material.color = new Color(mesh_2.material.color.r, mesh_2.material.color.g, mesh_2.material.color.b, 0);
        mesh_3.material.color = new Color(mesh_3.material.color.r, mesh_3.material.color.g, mesh_3.material.color.b, 0);
        gameObject.GetComponent<EnemySpawnManager>().SetMoveFlag(false);
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
    }
}
