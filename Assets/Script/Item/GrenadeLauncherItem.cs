using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeLauncherItem : MonoBehaviour
{
    [SerializeField]
    private GameObject player;
    [SerializeField]
    private GameObject grenadeLauncher;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (player.GetComponent<FPSController>().GetGunType() == 2)
        {
            grenadeLauncher.SetActive(false);
        }
        else
        {
            grenadeLauncher.SetActive(true);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {

    }
}
