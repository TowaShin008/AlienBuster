using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    Camera camera;

    bool focusFlag = false;
    // Start is called before the first frame update
    void Start()
    {
        camera = GameObject.Find("Main Camera").GetComponent<Camera>();

    }

    // Update is called once per frame
    void Update()
    {
        if (focusFlag)
        {
            camera.transform.LookAt(gameObject.transform);
            camera.fieldOfView = 30;
        }
        else
        {
            camera.fieldOfView = 60;

        }

        if (Input.GetKeyDown(KeyCode.O)) ChangeFocusFlag();
    }

    public void ChangeFocusFlag()
    {
        focusFlag = !focusFlag;
    }
}
