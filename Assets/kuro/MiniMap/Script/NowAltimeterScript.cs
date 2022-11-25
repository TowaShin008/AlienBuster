using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NowAltimeterScript : MonoBehaviour
{
    Transform playerTransform;
    private float playerHeight;

    // Start is called before the first frame update
    void Start()
    {
        playerTransform = GameObject.Find("Player").GetComponent("Transform") as Transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (playerTransform.position.y >= 270.0f)
		{
            playerHeight = 270.0f;
		}
		else
		{
            playerHeight = playerTransform.position.y;

        }
        transform.position = new Vector3(296.0f, playerHeight + 69.0f, 0);
    }
}
