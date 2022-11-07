using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnManager : MonoBehaviour
{

    //出現させる敵のオブジェクト
    [SerializeField] GameObject[] enemys;
    //次に敵が出現するまでの時間
    [SerializeField] float spawnNextTime = 10.0f;
    //この場所から出現する敵の数
    [SerializeField] int maxEnemyCount = 5;
    //出現させた総数
    private int enemyCount;
    //次の敵を出現させるまでの待ち時間
    private float elapsedTime;

    // Start is called before the first frame update
    void Start()
    {
        enemyCount = 0;
        elapsedTime = 0.0f;
    }
   

    // Update is called once per frame
    void Update()
    {
        //　この場所から出現する最大数を超えてたら何もしない
        if (enemyCount >= maxEnemyCount)
        {
            return;
        }
        //　経過時間を足す
        elapsedTime += Time.deltaTime;

        //　経過時間が経ったら
        if (elapsedTime > spawnNextTime)
        {
            elapsedTime = 0.0f;

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

        GameObject.Instantiate(enemys[randomValue], transform.position, Quaternion.Euler(0f, randomRotationY, 0f));

        enemyCount++;
        elapsedTime = 0.0f;
    }
}
