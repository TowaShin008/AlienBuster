using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using Util;

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
    private int saveWave = 0;

    public  bool waveChangeFlag = false;

    [SerializeField] private AudioSource defaultAudioSource;
    [SerializeField] private AudioSource bossAudioSource;

    public static bool numberchange = false;

    public const float tutorialMaxTime = 1000.0f;
    public float tutorialTime = 0.0f;
    private bool startFlag = false;


    [SerializeField]
    GameObject tutorialTextObject;

    Text tutorialText;
    void Start()
    {
        //フレームレートの固定
        Application.targetFrameRate = 60;

        if (Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.OSXPlayer || Application.platform == RuntimePlatform.LinuxPlayer)
        {
            Screen.SetResolution(Constants.screen_width, Constants.screen_height, false);
        }

        ufo.SetActive(false);
        ufo_2.SetActive(false);
        ufo_3.SetActive(false);

        nowWave = 1;

        defaultAudioSource.Play();
        numberBox = GameObject.FindGameObjectsWithTag("waveNumber");
        NumberChange();

        startFlag = false;

        tutorialText = tutorialTextObject.GetComponent<Text>();

        tutorialText.text = "";
    }


    void Update()
    {
		if (startFlag == false)
        {
            TutorialProcessing();
            return;
        }

        enemyBox = GameObject.FindGameObjectsWithTag(Constants.enemyName.ToString());
        ufoBox = GameObject.FindGameObjectsWithTag(Constants.ufoName.ToString());

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
                    bossAudioSource.Play();
                    ufo.GetComponent<UFO>().Initialize(true,true);
                }
                if (nowWave == 3)
				{
                    defaultAudioSource.Play();
                    enemySpawner.GetComponent<EnemySpawner>().Initialize(false, Constants.jumpEnemy);
                    enemySpawner2.GetComponent<EnemySpawner>().Initialize(false, Constants.jumpEnemy);
                    enemySpawner3.GetComponent<EnemySpawner>().Initialize(false, Constants.jumpEnemy);
                }
                if (nowWave == 4)
				{
                    bossAudioSource.Play();
                    ufo_2.GetComponent<UFO>().Initialize();
				}
                if (nowWave == 5)
                {
                    defaultAudioSource.Play();
                    enemySpawner.GetComponent<EnemySpawner>().Initialize(false, Constants.stepEnemy);
                    enemySpawner2.GetComponent<EnemySpawner>().Initialize(false, Constants.stepEnemy);
                    enemySpawner3.GetComponent<EnemySpawner>().Initialize(false, Constants.stepEnemy);
                }
                if (nowWave == 6)
                {
                    bossAudioSource.Play();
                    ufo_3.GetComponent<UFO>().Initialize();
                }
                if (nowWave >= 7)
				{
                    bossAudioSource.Play();
                    ufo.GetComponent<UFO>().Initialize();
                    ufo_2.GetComponent<UFO>().Initialize();
                    ufo_3.GetComponent<UFO>().Initialize();
                }
                nextWaveCheck = false;
            }
            NumberChange();

        }

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
        foreach(var num in numberBox ?? Enumerable.Empty<GameObject>())
       {
            if (num.name== "number" + nowWave)
            {
                num.transform.SetScaleXY(5, 5);
                num.GetComponent<ColorChangeText>().ChangeRed();
            }
            else
            {
                num.transform.SetScaleXY(2, 2);
                num.GetComponent<ColorChangeText>().ChageNormal();
            }
        }
    }
    /// <summary>
    /// チューリアル処理
    /// </summary>
    /// <returns>終了判定フラグ</returns>
    public bool TutorialProcessing()
	{
        tutorialTime++;

        if(tutorialTime/tutorialMaxTime<0.25f)
		{
            tutorialText.text = "WASD or LStick：移動\n";
        }
        else if (tutorialTime / tutorialMaxTime < 0.5f)
        {
            tutorialText.text = "RClick or LT：構える\nLClick or RT：射撃";
        }
        else if (tutorialTime / tutorialMaxTime < 0.75f)
        {
            tutorialText.text = "SPACE or RB：上昇\nLShift or LB：ステップ";
        }
        else if (tutorialTime / tutorialMaxTime < 1.0f)
        {
            tutorialText.text = "LShift or LB長押し：ダッシュ";
        }

        if (tutorialTime > tutorialMaxTime)
		{
            tutorialText.text = "";
            enemySpawner.GetComponent<EnemySpawner>().Initialize(false, Constants.normalEnemy);
            enemySpawner2.GetComponent<EnemySpawner>().Initialize(false, Constants.normalEnemy);
            enemySpawner3.GetComponent<EnemySpawner>().Initialize(false, Constants.normalEnemy);

            startFlag = true;

            return true;
		}
        return false;
	}
}
