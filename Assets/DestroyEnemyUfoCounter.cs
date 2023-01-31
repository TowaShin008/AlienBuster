using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyEnemyUfoCounter : MonoBehaviour
{
    // Start is called before the first frame update
    public static int enemyCounter = 0;
    public static int UfoCounter = 0;
    void Start()
    {
        enemyCounter = 0;
        UfoCounter = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static void EnemyCounterPlus()
    {
        enemyCounter++;
    }
    public static void UfoCounterPlus()
    {
        UfoCounter++;
    }
    public static int EnemyreturnCounter()
    {
        return enemyCounter;
    }
    public static int UforeturnCounter()
    {
        return UfoCounter;
    }
}
