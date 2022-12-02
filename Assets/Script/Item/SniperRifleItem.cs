using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SniperRifleItem : MonoBehaviour
{
    [SerializeField]
    private GameObject player;
    [SerializeField]
    private GameObject sniperRifle;
    // Start is called before the first frame update
    void Start()
    {
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (player.GetComponent<FPSController>().GetGunType() == 3)
        {
            sniperRifle.SetActive(false);
        }
        else
        {
            sniperRifle.SetActive(true);

            float yRot = 0.0f;

            yRot += 2.0f;
            if (yRot > 360.0f)
            {
                yRot = 0.0f;
            }
            sniperRifle.transform.rotation *= Quaternion.Euler(0, yRot, 0);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {

    }
}
