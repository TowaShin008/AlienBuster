using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatusBarScript : MonoBehaviour
{
    public enum TYPE
    {
        TYPE1,
        TYPE2,
    }
    public TYPE type;

    private GameObject hpBar;
    private GameObject staminaBar;

    private Vector3 hpBarPos;
    private Vector3 staminaBarPos;


    private int hp;
    private int stamina;
    private int maxhp;
    private int maxstamina;

    private Vector3 hpdef;
    private Vector3 staminadef;

    RectTransform hpMask;
    RectTransform staminaMask;

    private FPSController playerObj;

    //êÿÇËéÊÇÈç¿ïW(hp,stamina)
    private float[] rectHP = { 95.0f, 3.0f };
    private float[] rectStamina = { 64.0f, 24.0f };
    // Start is called before the first frame update
    void Start()
    {
        switch (type)
        {
            case TYPE.TYPE1:
                GameObject.Find("StatusBarFrame").SetActive(true);
                GameObject.Find("StatusBarFrame2").SetActive(false);
                rectHP[0] = 95.0f;
                rectHP[1] = 3.0f;
                rectStamina[0] = 64.0f;
                rectStamina[1] = 24.0f;
                break;
            case TYPE.TYPE2:
                GameObject.Find("StatusBarFrame").SetActive(false);
                GameObject.Find("StatusBarFrame2").SetActive(true);
                rectHP[0] = 57.0f;
                rectHP[1] = 2.0f;
                rectStamina[0] = 50.0f;
                rectStamina[1] = 8.0f;
                break;
        }
        hpBar = GameObject.Find("HPBar") as GameObject;
        staminaBar = GameObject.Find("StaminaBar") as GameObject;

        hpBarPos = hpBar.transform.localPosition;
        staminaBarPos = staminaBar.transform.localPosition;

        playerObj = GameObject.Find("Player").GetComponent("FPSController") as FPSController;

        hp = playerObj.GetHP();
        stamina = playerObj.GetStamina();

        maxhp = playerObj.GetMaxHP();
        maxstamina = playerObj.GetMaxStamina();

        hpMask = GameObject.Find("HPMask").GetComponent("RectTransform") as RectTransform;
        staminaMask = GameObject.Find("StaminaMask").GetComponent("RectTransform") as RectTransform;

        hpdef = hpMask.localPosition;
        staminadef = staminaMask.localPosition;
    }

    // Update is called once per frame  

    void Update()
    {
        hp = playerObj.GetHP();
        stamina = playerObj.GetStamina();

        float mask = ((float)hp / (float)maxhp) * rectHP[0];
        mask = (mask - rectHP[0]) * -1.0f + rectHP[1];
        hpMask.localPosition = new Vector3(mask, hpdef.y, hpdef.z);

        mask = ((float)stamina / (float)maxstamina) * rectStamina[0];
        mask = (mask - rectStamina[0]) * -1.0f + rectStamina[1];
        staminaMask.localPosition = new Vector3(mask, staminadef.y, staminadef.z);



        hpBar.transform.localPosition = hpBarPos - hpMask.localPosition;
        staminaBar.transform.localPosition = staminaBarPos - staminaMask.localPosition;


        //Debug.Log("Stamina" + (float)stamina / (float)maxstamina);
    }
}