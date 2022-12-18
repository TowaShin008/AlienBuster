using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class WaveManager : MonoBehaviour
{
    private GameObject[] enemyBox;
    private GameObject[] ufoBox;
    private GameObject[] numberBox;
    public GameObject wave_object = null;
    public GameObject enemySpawner;
    public GameObject enemySpawner2;
    public GameObject enemySpawner3;
    public GameObject ufo;
    public GameObject ufo_2;
    public GameObject ufo_3;
    public static int nowWave = 1;
    public static bool nextWaveCheck = false;
    int saveWave = 0;

    public  bool waveChangeFlag = false;

    [SerializeField] private AudioSource defaultAudioSource;
    [SerializeField] private AudioSource bossAudioSource;
    [SerializeField] private GameObject CautionText;

    public static bool numberchange = false;
    void Start()
    {
        //フレームレートの固定
        Application.targetFrameRate = 60;

        if (Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.OSXPlayer || Application.platform == RuntimePlatform.LinuxPlayer)
        {
            Screen.SetResolution(1920, 1080, false);
        }

        enemySpawner.GetComponent<EnemySpawner>().Initialize();
        enemySpawner2.SetActive(false);
        enemySpawner3.SetActive(false);
        ufo.SetActive(false);
        ufo_2.SetActive(false);
        ufo_3.SetActive(false);

        nowWave = 1;

        defaultAudioSource.Play();
    }
 

    void Update()
    {
        enemyBox = GameObject.FindGameObjectsWithTag("Enemy");
        ufoBox = GameObject.FindGameObjectsWithTag("UFO");
        numberBox = GameObject.FindGameObjectsWithTag("waveNumber");

        numberchange = false;
        //何かのトリガーで次のウェーブへ
        if (waveChangeFlag == true && enemyBox.Length <= 0 && ufoBox.Length <= 0)
        {
            waveChangeFlag = false;
            nextWaveCheck = true;
            numberchange = true;
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

                if (nowWave == 2)
                {
                    CautionText.GetComponent<Controll_Var>().ChangeStartFlag();
                    bossAudioSource.Play();
                    ufo.GetComponent<UFO>().Initialize();
                    ufo.GetComponent<CameraMove>().ChangeFocusFlag();
                }
                if (nowWave == 3)
				{
                    defaultAudioSource.Play();
                    enemySpawner2.GetComponent<EnemySpawner>().Initialize();
                }
                if (nowWave == 4)
				{
                    CautionText.GetComponent<Controll_Var>().ChangeStartFlag();
                    bossAudioSource.Play();
                    ufo_2.GetComponent<UFO>().Initialize();
                    ufo_2.GetComponent<CameraMove>().ChangeFocusFlag();
                }
                if (nowWave == 5)
                {
                    defaultAudioSource.Play();
                    enemySpawner3.GetComponent<EnemySpawner>().Initialize();
                }
                if (nowWave == 6)
                {
                    CautionText.GetComponent<Controll_Var>().ChangeStartFlag();
                    bossAudioSource.Play();
                    ufo_3.GetComponent<UFO>().Initialize();
                    ufo_3.GetComponent<CameraMove>().ChangeFocusFlag();
                }
                if (nowWave >= 7)
				{
                    bossAudioSource.Play();
                    ufo.GetComponent<UFO>().Initialize();
                    ufo_2.GetComponent<UFO>().Initialize(1);
                    ufo_3.GetComponent<UFO>().Initialize(3);
                    ufo.GetComponent<CameraMove>().ChangeFocusFlag();
                }
                nextWaveCheck = false;
            }
        }
        NumberChange();


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
    public static bool GetChangeWaveFlag()
    {
        return numberchange;
    }
    
    private void NumberChange()
    {
        for(int i = 0; i < numberBox.Length; i++)
        {
            if (i == nowWave-1)
            {
                numberBox[i].transform.SetScaleXY(5, 5);
                numberBox[i].GetComponent<ColorChangeText>().ChangeRed();
            }
            else
            {
                numberBox[i].transform.SetScaleXY(2, 2);
                numberBox[i].GetComponent<ColorChangeText>().ChageNormal();

            }
        }
    }
}
