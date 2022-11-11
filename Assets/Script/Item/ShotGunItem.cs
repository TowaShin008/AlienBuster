using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotGunItem : MonoBehaviour
{
    [SerializeField]
    private GameObject player;
    [SerializeField]
    private GameObject shotGun;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (player.GetComponent<FPSController>().GetGunType() == 4)
        {
            shotGun.SetActive(false);
        }
        else
        {
            shotGun.SetActive(true);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {

    }
}
