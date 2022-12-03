using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class WaveManager : MonoBehaviour
{
    private GameObject[] enemyBox;
    private GameObject[] ufoBox;
    public GameObject wave_object = null;
    public GameObject enemySpawner;
    public GameObject ufo;
    public GameObject ufo_2;
    public GameObject ufo_3;
    public static int nowWave = 1;
    bool nextWaveCheck = false;
    int saveWave = 0;

    public bool waveChangeFlag = false;

    [SerializeField] private AudioSource defaultAudioSource;
    [SerializeField] private AudioSource bossAudioSource;

    void Start()
    {
        //フレームレートの固定
        Application.targetFrameRate = 60;

        if (Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.OSXPlayer || Application.platform == RuntimePlatform.LinuxPlayer)
        {
            Screen.SetResolution(1920, 1080, false);
        }

        enemySpawner.GetComponent<EnemySpawner>().Initialize();
        ufo.SetActive(false);
        ufo_2.SetActive(false);
        ufo_3.SetActive(false);

        defaultAudioSource.Play();
    }
 

    void Update()
    {
        enemyBox = GameObject.FindGameObjectsWithTag("Enemy");
        ufoBox = GameObject.FindGameObjectsWithTag("UFO");

        //何かのトリガーで次のウェーブへ
        if (waveChangeFlag == true && enemyBox.Length <= 0 && ufoBox.Length <= 0)
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
                nowWave++;

                defaultAudioSource.Stop();
                bossAudioSource.Stop();

                if (nowWave >= 2)
                {
                    bossAudioSource.Play();
                    ufo.GetComponent<UFO>().Initialize();
                }
                if (nowWave >= 3)
				{
                    ufo_2.GetComponent<UFO>().Initialize();
				}
                if (nowWave >= 4)
                {
                    ufo_3.GetComponent<UFO>().Initialize();
                }
                else
				{
                    enemySpawner.SetActive(false);
                    bossAudioSource.Play();
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
