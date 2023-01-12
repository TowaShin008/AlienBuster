using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    //現在のwaveをwaveManegerから取得
    [SerializeField] WaveManager waveManager;
    private bool isDeadFlag = false;
    private int enemySpawnCount = 0;
    void Start()
    {
        isDeadFlag = false;
        enemySpawnCount = 0;
        gameObject.SetActive(false);
    }


    void Update()
    {
        if (gameObject.GetComponent<EnemySpawnManager>().StopProcessing())
        {//ストップした際の処理をここに記述する

        }

        if (gameObject.GetComponent<EnemySpawnManager>().LauncherProcessing())
        {
            enemySpawnCount++;
            if(enemySpawnCount >=gameObject.GetComponent<EnemySpawnManager>().GetMaxEnemyCount())
			{
                isDeadFlag = true;
                waveManager.WaveChangeFlagOn();
            }
        }
    }
    /// <summary>
    /// 初期化処理
    /// </summary>
	public void Initialize(bool arg_randomSpawnFlag = true, int arg_enemtType = 0)
    {
        isDeadFlag = false;
        enemySpawnCount = 0;
        gameObject.SetActive(true);
        gameObject.GetComponent<EnemySpawnManager>().SetMoveFlag(true);
        gameObject.GetComponent<EnemySpawnManager>().Initialize(arg_randomSpawnFlag, arg_enemtType);
    }
}
