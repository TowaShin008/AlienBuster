using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Util;

public class RocketLauncherItem : MonoBehaviour
{
    [SerializeField]
    private GameObject player;
    [SerializeField]
    private GameObject rocketLauncher;
    // Start is called before the first frame update
    void Start()
    {
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (player.GetComponent<FPSController>().GetGunType() == 2)
        {
            rocketLauncher.SetActive(false);
        }
        else
        {
            rocketLauncher.SetActive(true);

            float yRot = 0.0f;

            yRot += 2.0f;
            if (yRot > 360.0f)
            {
                yRot = 0.0f;
            }
            rocketLauncher.transform.rotation *= Quaternion.Euler(0, yRot, 0);
        }

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
