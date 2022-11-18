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

            float xRot = 0.0f;

            xRot += 2.0f;
            if (xRot > 360.0f)
            {
                xRot = 0.0f;
            }
            grenadeLauncher.transform.rotation *= Quaternion.Euler(xRot, 0, 0);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {

    }
}
