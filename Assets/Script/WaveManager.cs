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
    public GameObject enemySpawner4;
    public GameObject enemySpawner5;
    public GameObject enemySpawner6;
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

    public const float tutorialMaxTime = 2000.0f;
    public float tutorialTime = 0.0f;
    private bool startFlag = false;


    [SerializeField]
    GameObject tutorialTextObject;
    [SerializeField]
    GameObject skipTextObject;

    Text tutorialText;
    Text skipText;
    void Start()
    {
        //�t���[�����[�g�̌Œ�
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

        skipText = skipTextObject.GetComponent<Text>();
    }


    void Update()
    {
		if (startFlag == false)
        {//�`���[�g���A������
            TutorialProcessing();
            return;
        }

        enemyBox = GameObject.FindGameObjectsWithTag(Constants.enemyName.ToString());
        ufoBox = GameObject.FindGameObjectsWithTag(Constants.ufoName.ToString());

        numberchange = false;
        //�����̃g���K�[�Ŏ��̃E�F�[�u��
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
                    ufo.GetComponent<UFO>().Initialize(true, true);
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
                if (nowWave == 7)
                {
                    defaultAudioSource.Play();
                    enemySpawner4.GetComponent<EnemySpawner>().Initialize(false, Constants.stayEnemy);
                    enemySpawner5.GetComponent<EnemySpawner>().Initialize(false, Constants.stayEnemy);
                    enemySpawner6.GetComponent<EnemySpawner>().Initialize(false, Constants.stayEnemy);
                }
                if (nowWave >= 8)
				{
                    bossAudioSource.Play();
                    ufo.GetComponent<UFO>().Initialize();
                    ufo_2.GetComponent<UFO>().Initialize();
                    ufo_3.GetComponent<UFO>().Initialize();
                }
                nextWaveCheck = false;
            }
            //�E�F�[�u���̐؂�ւ��`�揈��
            NumberChange();
        }
    }
    /// <summary>
    /// �ǂ��܂Ői�񂾂��̃E�F�[�u��n���֐�
    /// </summary>
    /// <returns>���݂̃E�F�[�u</returns>
    public static int GetWave()
    {
        return nowWave;
    }
    /// <summary>
    /// �E�F�[�u�J�ڃt���O�̎擾
    /// </summary>
    public void WaveChangeFlagOn()
    {
        waveChangeFlag = true;
    }
    /// <summary>
    /// �`�F���W�E�F�[�u�t���O�̎擾
    /// </summary>
    /// <returns>�`�F���W�E�F�[�u�t���O</returns>
    public static bool GetChangeWaveFlag()
    {
        return numberchange;
    }
    /// <summary>
    /// �E�F�[�u���̐؂�ւ��`�揈��
    /// </summary>
    private void NumberChange()
    {
        foreach(var num in numberBox ?? Enumerable.Empty<GameObject>())
       {
            if (num.name == "number" + nowWave)
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
    /// �`���[���A������
    /// </summary>
    /// <returns>�I������t���O</returns>
    public bool TutorialProcessing()
	{
        tutorialTime++;

        skipText.enabled = true;

        if (tutorialTime / tutorialMaxTime < 0.2f)
		{
            tutorialText.text = "�ړ��FWASD or LStick\n";
        }
        else if (tutorialTime / tutorialMaxTime < 0.4f)
        {
            tutorialText.text = "�\����FRClick or LT\n�ˌ��FLClick or RT";
        }
        else if (tutorialTime / tutorialMaxTime < 0.6f)
        {
            tutorialText.text = "�㏸�FSPACE or RB\n�X�e�b�v�FLShift or LB";
        }
        else if (tutorialTime / tutorialMaxTime < 0.8f)
        {
            tutorialText.text = "�_�b�V���FLShift or LB������";
        }
        else if (tutorialTime / tutorialMaxTime < 1.0f)
        {
            tutorialText.text = "���肭��G��|��������I";
        }

        if (Input.GetKeyDown(KeyCode.R) || Input.GetKey(KeyCode.Joystick1Button3))
		{
            tutorialTime = tutorialMaxTime;
		}

        if (tutorialTime > tutorialMaxTime)
		{
            skipText.enabled = false;
            tutorialText.text = "";
			enemySpawner.GetComponent<EnemySpawner>().Initialize(false, Constants.normalEnemy);
			enemySpawner2.GetComponent<EnemySpawner>().Initialize(false, Constants.normalEnemy);
			enemySpawner3.GetComponent<EnemySpawner>().Initialize(false, Constants.normalEnemy);

			//ufo.GetComponent<UFO>().Initialize();

			startFlag = true;

            return true;
		}
        return false;
	}
}
