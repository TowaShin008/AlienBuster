using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StaminaScript : MonoBehaviour
{
    public bool useType2 = false;
    public float shitY1=0;
    public float shitY2 =0;

    private GameObject gaugeObject;
    private GameObject staminaObject;


    private FPSController fpsController;
    private int stamina;
    private int maxStamina;
    // Start is called before the first frame update
    void Start()
    {
        gaugeObject = GameObject.Find("GaugeObject") as GameObject;
        staminaObject = GameObject.Find("StaminaObject") as GameObject;
        if (!useType2)
        {
            RectTransform trans = GameObject.Find("DefaultStaminaTransform").GetComponent("RectTransform") as RectTransform;
            RectTransform gtrans = gaugeObject.GetComponent("RectTransform") as RectTransform;
            gtrans.position = trans.position;
            gtrans.localScale = trans.localScale;


            RectTransform mask = GameObject.Find("Stamina/Mask").GetComponent("RectTransform") as RectTransform;
            Image gaugeImage = gaugeObject.GetComponent("Image") as Image;
            Image gaugetype1 = GameObject.Find("Stamina/Image/GaugeImage_Type1").GetComponent("Image") as Image;
            gaugeImage.sprite = gaugetype1.sprite;
            //gaugeImage.sprite = gaugetexImage;

            RectTransform gtrans2 = staminaObject.GetComponent("RectTransform") as RectTransform;
            Vector3 pos = new Vector3(trans.position.x, trans.position.y - shitY1, trans.position.z);
            mask.position = pos;
            mask.localScale = trans.localScale;


            Image staminaTexImage = staminaObject.GetComponent("Image") as Image;
            Image staminatype1 = GameObject.Find("Stamina/Image/StaminaImage_Type1").GetComponent("Image") as Image;
            staminaTexImage.sprite = staminatype1.sprite;
        }
        else
        {
            RectTransform trans = GameObject.Find("DefaultStaminaTransform2").GetComponent("RectTransform") as RectTransform;
            RectTransform gtrans = gaugeObject.GetComponent("RectTransform") as RectTransform;
            gtrans.position = trans.position;
            gtrans.localScale = trans.localScale;


            RectTransform mask = GameObject.Find("Stamina/Mask").GetComponent("RectTransform") as RectTransform;
            Image gaugeImage = gaugeObject.GetComponent("Image") as Image;
            Image gaugetype2 = GameObject.Find("Stamina/Image/GaugeImage_Type2").GetComponent("Image") as Image;
            gaugeImage.sprite = gaugetype2.sprite;

            RectTransform gtrans2 = staminaObject.GetComponent("RectTransform") as RectTransform;
            Vector3 pos = new Vector3(trans.position.x, trans.position.y - shitY2, trans.position.z);
            mask.position = pos;
            mask.localScale = trans.localScale;


            Image staminaTexImage = staminaObject.GetComponent("Image") as Image;
            Image staminatype2 = GameObject.Find("Stamina/Image/StaminaImage_Type2").GetComponent("Image") as Image;
            staminaTexImage.sprite = staminatype2.sprite;
        }
        fpsController = GameObject.Find("Player").GetComponent("FPSController") as FPSController;
        maxStamina = fpsController.GetMaxStamina();
    }

    // Update is called once per frame
    void Update()
    {
        stamina = fpsController.GetStamina();

        RectMask2D staminaMask = GameObject.Find("Stamina/Mask").GetComponent("RectMask2D") as RectMask2D;

        if (!useType2)
        {
            float onestamina = (100.0f * 3.0f) / (float)maxStamina;
            float transStamina = (100.0f * 3.0f) - (onestamina * (float)stamina);
            staminaMask.padding = new Vector4(0, 0, 0, transStamina);
        }
        else
        {
            float onestamina = (180.0f * 5.27f) / (float)maxStamina;
            float transStamina = (180.0f * 5.27f) - (onestamina * (float)stamina);
            staminaMask.padding = new Vector4(0, 0, transStamina, 0);
        }


        //Debug.Log(stamina);
    }
}
