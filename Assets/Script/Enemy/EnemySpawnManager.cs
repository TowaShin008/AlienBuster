using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnManager : MonoBehaviour
{

    //出現させる敵のオブジェクト
    [SerializeField] GameObject[] enemys;
    //waveごとの次に敵が出現するまでの時間
    [SerializeField] float[] spawnNextTime = { 10.0f,5.0f };
    //現在の次に敵が出現するまでの時間
    private float nowSpawnNextTime;
    //出現するwave毎の敵の最大数
    [SerializeField] int[] maxEnemyCount = {5,10};
    //出現する現在の敵の最大数
    private int nowMaxEnemyCount;
    //出現する位置
    [SerializeField] GameObject spawnPoint;
    //現在のwaveで敵を出現させた総数
    private int enemyCount;
    //敵を出現させた総数
    private int enemyAllCount;
    //次の敵を出現させるまでの待ち時間
    private float elapsedTime;

    //現在のwaveをwaveManegerから取得
    [SerializeField] WaveManager waveManager;
    //現在のwaveを保存するための変数
    int nowWave;

    [SerializeField] GameObject barrier;
    //バリア発生しているか
    bool barrierFlag;
    //敵がすべて出てからバリアが発生するまでの時間
    [SerializeField] int barrierCreateMaxTime = 60;
    private int barrierCreateTime;

    // Start is called before the first frame update
    void Start()
    {
        enemyCount = 0;
        elapsedTime = 0.0f;
        this.nowWave = WaveManager.nowWave;

        if (nowWave <= spawnNextTime.Length)
        {
            nowSpawnNextTime = spawnNextTime[nowWave - 1];
        }
        else
        {
            float index = spawnNextTime[spawnNextTime.Length - 1];
            nowSpawnNextTime = index;
        }

        if (nowWave <= maxEnemyCount.Length)
        {
            nowMaxEnemyCount = maxEnemyCount[nowWave - 1];
        }
        else
        {
            nowMaxEnemyCount = maxEnemyCount[maxEnemyCount.Length - 1];
        }

        barrierFlag = true;
        barrierCreateTime = barrierCreateMaxTime;
    }
   

    // Update is called once per frame
    void Update()
    {

        //wave変更時の処理
        if (nowWave != WaveManager.nowWave)
        {
            nowWave = WaveManager.nowWave;

            if (nowWave <= spawnNextTime.Length)
            {
                nowSpawnNextTime = spawnNextTime[nowWave - 1];
            }
            else
            {
                nowSpawnNextTime = spawnNextTime[spawnNextTime.Length - 1];
            }

            if (nowWave <= maxEnemyCount.Length)
            {
                nowMaxEnemyCount = maxEnemyCount[nowWave - 1];
            }
            else
            {
                nowMaxEnemyCount = maxEnemyCount[maxEnemyCount.Length - 1];
            }

            enemyCount = 0;
        }

        if (barrierFlag == true)
        {
            barrier.SetActive(true);
        }
        else
        {
            barrier.SetActive(false);
        }

        //　この場所から出現する最大数を超えてたら何もしない
        if (enemyCount >= nowMaxEnemyCount)
        {
            waveManager.WaveChangeFlagOn();

            if (barrierFlag == false)
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
         
            return;
        }
        //　経過時間を足す
        elapsedTime += Time.deltaTime;

        //　経過時間が経ったら
        if (elapsedTime > nowSpawnNextTime)
        {
            elapsedTime = 0.0f;

            barrierFlag = false;
            SpawnEnemy();
        }
       
    }

    //　敵出現
    void SpawnEnemy()
    {
        //出現させる敵をランダムに選ぶ
        var randomValue = Random.Range(0, enemys.Length);
        //敵の向きをランダムに決定
        var randomRotationY = Random.value * 360f;

        GameObject.Instantiate(enemys[randomValue], spawnPoint.transform.position, Quaternion.Euler(0f, randomRotationY, 0f));

        enemyCount++;
        enemyAllCount++;
        elapsedTime = 0.0f;
    }
}
