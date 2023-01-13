using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MagazineScript : MonoBehaviour
{
    int magazine = 0;
    int maxMagazine = 0;
    int remainingBullet = 0;

    int reloadTime = 0;
    int nowReloadTime = 0;

    bool reloadEnable = true; //�����[�h�̐؂�ւ��͂�������E�ʂɐݒ肵�����ꍇ�͐�p�̊֐��ł��ꂼ��Z�b�g����B

    TMPro.TextMeshProUGUI text = null;
    int otherMagazineBullets;
    // Start is called before the first frame update
    void Start()
    {
        text = GameObject.Find("MagazineText").GetComponent<TMPro.TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        if(magazine <= 0)
        {
            if (reloadEnable)
            {
                if (nowReloadTime >= reloadTime)
                {
                    nowReloadTime = 0;
                    magazine = maxMagazine;
                    UpdateOtherMagazineBullets();
                }
                nowReloadTime++;
            }
        }


        text.text = magazine.ToString() + "/" + otherMagazineBullets.ToString();
        if(magazine <= 0)
        {
            if (reloadEnable)
            {
                text.text += "\n" +
                    "Reloding...";
            }
        }
    }

    public bool CheckBullets()
    {
        if (magazine <= 0)
        {
            return false;
        }

        return true;
    }

    public void ResetMagazine()
    {
        magazine = maxMagazine;
    }

    public void SetMagazineSize(int mag)
    {
        maxMagazine = magazine = mag;
        UpdateOtherMagazineBullets();
    }

    public void SetRemainingBulletsSize(int arg_remainingBullet)
    {
        remainingBullet = arg_remainingBullet;
    }

    public void DecrementMagazine(int num = 1)
    {
        if (magazine - num <= -1) return;
        magazine -= num;
    }

    public void SetReloadTime(int time)
    {
        reloadTime = time;
    }

    public int GetNowMagazine()
    {
        return magazine;
    }

    public void ReloadEnable(bool flag)
    {
        reloadEnable = flag;
    }
    /// <summary>
    /// �}�K�W���O�̒e���̍X�V����
    /// </summary>
    public void UpdateOtherMagazineBullets()
	{
        otherMagazineBullets = remainingBullet - maxMagazine;
        if (otherMagazineBullets < 0)
        {
            otherMagazineBullets = 0;
        }
    }
}