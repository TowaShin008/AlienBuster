using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamageEffectScript : MonoBehaviour
{

    public enum TypeEffect
    {
        slash,
        hexagonal,
    }
    [SerializeField] TypeEffect type = TypeEffect.slash;
    Coffee.UISoftMask.SoftMask softMask;
    

    private int playerHP = 0;
    int checkHP = 0;
    private FPSController playerScript;
    // Start is called before the first frame update
    void Start()
    {
        playerScript = GameObject.Find("Player").GetComponent("FPSController") as FPSController;
        softMask = GameObject.Find("DamgeEffectMask").GetComponent("Coffee.UISoftMask.SoftMask") as Coffee.UISoftMask.SoftMask;


        Image imageObject = GameObject.Find("DamageImage").GetComponent("Image") as Image;
        Image sampleImage;

        switch (type)
        {
            case TypeEffect.slash:
                sampleImage = GameObject.Find("DamageEffect/Sample/Image1").GetComponent("Image") as Image;
                break;
            case TypeEffect.hexagonal:
                sampleImage = GameObject.Find("DamageEffect/Sample/Image2").GetComponent("Image") as Image;
                
                break;
            default:
                sampleImage = GameObject.Find("DamageEffect/Sample/Image1").GetComponent("Image") as Image;
                break;
        }

        imageObject.sprite = sampleImage.sprite;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Backslash))
        {
            Damage();
        }
        if (Input.GetKeyDown(KeyCode.Minus))
        {
            Heal();
        }

        //���������ŏ��ɓǂݍ��܂ꂽ�ꍇHP�̏����l���قȂ邽�ߎ��Ԃ�u��
        if (20 > checkHP++)
        {
            playerHP = playerScript.GetHP();
            return;
        }

        if (playerHP != playerScript.GetHP())
        {
            Damage();
        }
        playerHP = playerScript.GetHP();

        //�v���C���[�̗̑͂�ύX����ꍇ�͋����āi����j
    }
    void Damage()
    {
        softMask.alpha += 0.3f;
    }
    void Heal()
    {
        softMask.alpha -= 0.3f;
    }
}
