using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    //���݂�wave��waveManeger����擾
    [SerializeField] WaveManager waveManager;
    private bool isDeadlag = false;
    private int enemySpawnCount = 0;
    void Start()
    {
        isDeadlag = false;
        enemySpawnCount = 0;
        gameObject.SetActive(false);
    }


    void Update()
    {
        if (gameObject.GetComponent<EnemySpawnManager>().StopProcessing())
        {//�X�g�b�v�����ۂ̏����������ɋL�q����

        }

        if (gameObject.GetComponent<EnemySpawnManager>().LauncherProcessing())
        {
            enemySpawnCount++;
            if(enemySpawnCount >=gameObject.GetComponent<EnemySpawnManager>().GetMaxEnemyCount())
			{
                isDeadlag = true;
                waveManager.WaveChangeFlagOn();
            }
        }
    }
    /// <summary>
    /// ����������
    /// </summary>
	public void Initialize()
    {
        gameObject.SetActive(true);
        gameObject.GetComponent<EnemySpawnManager>().SetMoveFlag(true);
    }
}
