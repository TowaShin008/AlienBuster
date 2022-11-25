using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnerWeek : MonoBehaviour
{
    [SerializeField] EnemySpawnManager Spawner;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        string gameObjectName = collision.gameObject.tag;
        if (gameObjectName != "Bullet" && gameObjectName != "RocketBumb" && gameObjectName == "EnemyBullet") { return; }

        if (gameObjectName == "Bullet")
        {
            Spawner.Damage(1);
        }
        else if (gameObjectName == "RocketBumb")
        {
            Spawner.Damage(10);
        }
    }
}
