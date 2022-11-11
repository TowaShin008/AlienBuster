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
        }
    }

    private void OnCollisionEnter(Collision collision)
    {

    }
}
