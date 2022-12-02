using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UFO : MonoBehaviour
{

    //���݂�wave��waveManeger����擾
    [SerializeField] WaveManager waveManager;

    public MeshRenderer mesh;
    public MeshRenderer mesh_2;
    public MeshRenderer mesh_3;
    bool entryFlag = false;

    private float rotateY = 0;
    [SerializeField] private int hp = 30;

    //�����G�t�F�N�g
    [SerializeField] GameObject explosion;
    [SerializeField] private Vector3 explosionSize = new Vector3(10.0f, 10.0f, 10.0f);

    private bool deadFlag;

    [SerializeField] GameObject barrier;
    //�o���A�������Ă��邩
    bool barrierFlag;
    //�G�����ׂďo�Ă���o���A����������܂ł̎���
    [SerializeField] int barrierCreateMaxTime = 60;
    private int barrierCreateTime;
    // Start is called before the first frame update
    void Start()
    {
        gameObject.SetActive(false);

        barrierFlag = true;
        barrierCreateTime = barrierCreateMaxTime;
    }

    // Update is called once per frame
    void Update()
    {
        if(entryFlag)
		{//�o�����o
            EntryProcessing();
		}
        //��]���o
        RotationProcessing();

        if (hp <= 0)
        {
            deadFlag = true;
        }

        //�o���A�̐؂�ւ�
        if (barrierFlag == true)
        {
            barrier.SetActive(true);
        }
        else
        {
            barrier.SetActive(false);
        }

        if(gameObject.GetComponent<EnemySpawnManager>().ChangeNextWave())
		{
            barrierFlag = true;
            barrierCreateTime = barrierCreateMaxTime;
        }

        if(gameObject.GetComponent<EnemySpawnManager>().StopProcessing())
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
            if(entryFlag == false)
			{
                gameObject.GetComponent<EnemySpawnManager>().SetMoveFlag(true);
            }
        }

        if (gameObject.GetComponent<EnemySpawnManager>().LauncherProcessing())
		{
            barrierFlag = false;
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
    /// �o�����o
    /// </summary>
    private void EntryProcessing()
    {
        mesh.material.color = mesh.material.color + new Color(0, 0, 0, 0.005f);
        mesh_2.material.color = mesh_2.material.color + new Color(0, 0, 0, 0.005f);
        mesh_3.material.color = mesh_2.material.color + new Color(0, 0, 0, 0.005f);
        if (mesh.material.color.a >= 1.0f)
		{
            entryFlag = false;
            gameObject.GetComponent<EnemySpawnManager>().SetMoveFlag(true);
		}
    }
    /// <summary>
    /// UFO�̉�]���o
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
    /// ����������
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

    public void SetBarrierFlag(bool arg_barrierFlag)
	{
        barrierFlag = arg_barrierFlag;
	}

    public bool GetBarrierFlag()
	{
        return barrierFlag;
	}
}
