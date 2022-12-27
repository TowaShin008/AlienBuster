using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    Camera camera;

    bool focusFlag = false;
    [SerializeField] float focusDirection = 60.0f;
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
            //camera.fieldOfView = focusDirection;
        }
        else
        {
            //camera.fieldOfView = 60;
        }
    }

    public void ChangeFocusFlag()
    {
        focusFlag = !focusFlag;
    }

    public void SetFocusFlag(bool arg_focusFlag)
	{
        focusFlag = arg_focusFlag;
	}
}
