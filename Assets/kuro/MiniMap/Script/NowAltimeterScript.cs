using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NowAltimeterScript : MonoBehaviour
{
    Transform playerTransform;

    // Start is called before the first frame update
    void Start()
    {
        playerTransform = GameObject.Find("Player").GetComponent("Transform") as Transform;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(296.0f, playerTransform.position.y+69.0f, 0);
    }
}
