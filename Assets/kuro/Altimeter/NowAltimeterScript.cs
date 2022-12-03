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
        if (playerTransform.position.y > 100.0f) return;
        transform.localPosition = new Vector3(0.0f, playerTransform.position.y - 55.0f, 0);
    }
}
