using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    //Œ»İ‚Ìwave‚ğwaveManeger‚©‚çæ“¾
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
        {

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
    /// ‰Šú‰»ˆ—
    /// </summary>
	public void Initialize()
    {
        gameObject.SetActive(true);
        gameObject.GetComponent<EnemySpawnManager>().SetMoveFlag(true);
    }
}
