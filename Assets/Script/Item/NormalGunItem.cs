using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalGunItem : MonoBehaviour
{
    [SerializeField]
    private GameObject player;
    [SerializeField]
    private GameObject normalGun;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(player.GetComponent<FPSController>().GetGunType()==1)
		{
            normalGun.SetActive(false);
		}
		else
		{
            normalGun.SetActive(true);
		}
    }

	private void OnCollisionEnter(Collision collision)
	{

    }
}
