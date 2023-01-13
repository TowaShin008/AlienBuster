using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Util;

public class ShotGunItem : MonoBehaviour
{
    [SerializeField]
    private GameObject player;
    [SerializeField]
    private GameObject shotGunItem;
    // Start is called before the first frame update
    void Start()
    {
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
		if (player.GetComponent<FPSController>().GetGunType() == 4)
		{
			shotGunItem.SetActive(false);
		}

		float yRot = 0.0f;

		yRot += 2.0f;
		if (yRot > 360.0f)
		{
		    yRot = 0.0f;
		}
        shotGunItem.transform.rotation *= Quaternion.Euler(0, yRot, 0);

        var currentPosition = gameObject.transform.position;

        if (currentPosition.y < Constants.stageMinPositionY)
        {
            currentPosition.y = Constants.stageMinPositionY;

            gameObject.transform.position = currentPosition;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {

    }
}
