using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectMiniMapWeponObject : MonoBehaviour
{
    public enum MMMODEL
    {
        model,
        box,
    }
    public MMMODEL mm_model = MMMODEL.box;
    // Start is called before the first frame update
    void Start()
    {
        switch (mm_model)
        {
            case MMMODEL.model:
                GameObject.Find("MMNormalGun_Model").SetActive(true);
                GameObject.Find("MMGrenadeLauncher_Model").SetActive(true);
                GameObject.Find("MMSniperRifle_Model").SetActive(true);
                GameObject.Find("MMShotGun_Model").SetActive(true);
                GameObject.Find("MMNormalGun_Box").SetActive(false);
                GameObject.Find("MMGrenadeLauncher_Box").SetActive(false);
                GameObject.Find("MMSniperRifle_Box").SetActive(false);
                GameObject.Find("MMShotGun_Box").SetActive(false);
                break;
            case MMMODEL.box:
                GameObject.Find("MMNormalGun_Model").SetActive(false);
                GameObject.Find("MMGrenadeLauncher_Model").SetActive(false);
                GameObject.Find("MMSniperRifle_Model").SetActive(false);
                GameObject.Find("MMShotGun_Model").SetActive(false);
                GameObject.Find("MMNormalGun_Box").SetActive(true);
                GameObject.Find("MMGrenadeLauncher_Box").SetActive(true);
                GameObject.Find("MMSniperRifle_Box").SetActive(true);
                GameObject.Find("MMShotGun_Box").SetActive(true);
                break;
            default:
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
