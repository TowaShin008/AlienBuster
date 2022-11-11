using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class WaveManager : MonoBehaviour
{
    private GameObject[] enemyBox;
    public GameObject wave_object = null;
    public static int nowWave = 0;
    bool nextWaveChack = false;
    int saveWave = 0;
    // Start is called before the first frame update
    void Start()
    {
        nowWave = 0;
    }
  
    // Update is called once per frame
    void Update()
    {
        enemyBox = GameObject.FindGameObjectsWithTag("Enemy");
        //何かのトリガーで次のウェーブへ
        if (enemyBox.Length <= 0)
        {
            nextWaveChack = true;
        }
        else
        {
            saveWave = nowWave;
        }
        if (nextWaveChack)
        {
            if (nowWave == saveWave)
            {
                nowWave++;
                nextWaveChack = false;
            }
        }

        Text wave_text = wave_object.GetComponent<Text>();
        wave_text.text = "Wave : " + nowWave;
    }

    //どこまで進んだかのウェーブを渡す関数
    public static int GetWave()
    {
        return nowWave;
    }
}
