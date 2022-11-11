using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class WaveManager : MonoBehaviour
{
    private GameObject[] enemyBox;
    public GameObject wave_object = null;
    public int nowWave = 1;
    bool nextWaveCheck = false;
    int saveWave = 0;

    public bool waveChangeFlag = false;

    void Start()
    {
        
    }

    void Update()
    {
        enemyBox = GameObject.FindGameObjectsWithTag("Enemy");
        ////�����̃g���K�[�Ŏ��̃E�F�[�u��
        //if (enemyBox.Length <= 0)
        //{
        //    nextWaveCheck = true;
        //}
        //else
        //{
        //    saveWave = nowWave;
        //}

        //�����̃g���K�[�Ŏ��̃E�F�[�u��
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
                nowWave++;
                nextWaveCheck = false;
            }
        }

        Text wave_text = wave_object.GetComponent<Text>();
        wave_text.text = "Wave : " + nowWave;
    }

    //wave�ύX�p�֐�
    public void WaveChangeFlagOn()
    {
        waveChangeFlag = true;
    }
}
