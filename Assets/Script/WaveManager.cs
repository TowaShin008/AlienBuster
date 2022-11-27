using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class WaveManager : MonoBehaviour
{
    private GameObject[] enemyBox;
    public GameObject wave_object = null;
    public GameObject ufo;
    public GameObject ufo_2;
    public GameObject ufo_3;
    public static int nowWave = 1;
    bool nextWaveCheck = false;
    int saveWave = 0;

    public bool waveChangeFlag = false;

    void Start()
    {
        //フレームレートの固定
        Application.targetFrameRate = 60;

        if (Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.OSXPlayer || Application.platform == RuntimePlatform.LinuxPlayer)
        {
            Screen.SetResolution(1920, 1080, false);
        }

        ufo.GetComponent<UFO>().Initialize();
    }
 

    void Update()
    {
        enemyBox = GameObject.FindGameObjectsWithTag("Enemy");
        //何かのトリガーで次のウェーブへ
        if (waveChangeFlag == true && enemyBox.Length <= 0)
        {
            waveChangeFlag = false;
            nextWaveCheck = true;
        }
        else
        {
            saveWave = nowWave;
        }

        if (nextWaveCheck)
        {
            if (nowWave == saveWave)
            {
                ufo.GetComponent<UFO>().Initialize();
                nowWave++;

                if(nowWave>=2)
				{
                    ufo_2.GetComponent<UFO>().Initialize();
				}
                if (nowWave >= 3)
                {
                    ufo_3.GetComponent<UFO>().Initialize();
                }
                nextWaveCheck = false;
            }
        }

        Text wave_text = wave_object.GetComponent<Text>();
        wave_text.text = "Wave : " + nowWave;
    }
    /// <summary>
    /// どこまで進んだかのウェーブを渡す関数
    /// </summary>
    /// <returns>現在のウェーブ</returns>
    public static int GetWave()
    {
        return nowWave;
    }
    /// <summary>
    /// ウェーブ遷移フラグの取得
    /// </summary>
    public void WaveChangeFlagOn()
    {
        waveChangeFlag = true;
    }
}
