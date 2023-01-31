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

    bool infiniteFlag = false;
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

        if (infiniteFlag == false)
		{
            text.text = magazine.ToString() + "/" + otherMagazineBullets.ToString();
        }
        else
		{
            text.text = magazine.ToString() + "/" + "��";
        }

        if(magazine <= 0)
        {
            if (reloadEnable)
            {
                text.text += "\n" +
                    "Reloading...";
            }
        }
    }
    /// <summary>
    /// �}�K�W���̎c�i���̃`�F�b�N
    /// </summary>
    /// <returns></returns>
    public bool CheckBullets()
    {
        if (magazine <= 0)
        {
            return false;
        }

        return true;
    }
    /// <summary>
    /// �}�K�W���̎c�i���̏�����
    /// </summary>
    public void ResetMagazine()
    {
        magazine = maxMagazine;
    }
    /// <summary>
    /// �}�K�W���̍ő吔�̃Z�b�g
    /// </summary>
    /// <param name="mag"></param>
    public void SetMagazineSize(int mag)
    {
        maxMagazine = magazine = mag;
        UpdateOtherMagazineBullets();
    }
    /// <summary>
    /// �c�e���̃Z�b�g
    /// </summary>
    /// <param name="arg_remainingBullet"></param>
    public void SetRemainingBulletsSize(int arg_remainingBullet)
    {
        remainingBullet = arg_remainingBullet;

        if(arg_remainingBullet==0)
		{
            infiniteFlag = true;
		}
		else
		{
            infiniteFlag = false;
		}
    }
    /// <summary>
    /// �}�K�W���̎c�i����1���炷����
    /// </summary>
    /// <param name="num"></param>
    public void DecrementMagazine(int num = 1)
    {
        if (magazine - num <= -1) return;
        magazine -= num;
    }
    /// <summary>
    /// �����[�h���Ԃ̃Z�b�g
    /// </summary>
    /// <param name="time"></param>
    public void SetReloadTime(int time)
    {
        reloadTime = time;
    }
    /// <summary>
    /// ���݂̃}�K�W�����̎擾
    /// </summary>
    /// <returns></returns>
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