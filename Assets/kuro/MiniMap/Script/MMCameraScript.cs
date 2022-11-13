using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MMCameraScript : MonoBehaviour
{
    public float distance = 50.0f;
    Transform playerTransform;

    // Start is called before the first frame update
    void Start()
    {
        playerTransform = GameObject.Find("Player").GetComponent("Transform") as Transform;
    }

    // Update is called once per frame
    void Update()
    {
        gameObject.transform.position = new Vector3(playerTransform.position.x,
                                                    playerTransform.position.y + distance,
                                                    playerTransform.position.z);
    }
}
