using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    Camera camera;
    GameObject focusObjects;
    bool focusFlag = false;

    public float speed = 1.0f;
    //[SerializeField] float focusDirection = 60.0f;
    // Start is called before the first frame update
    void Start()
    {
        camera = GameObject.Find("Main Camera").GetComponent<Camera>();

        focusObjects = new GameObject();

        focusObjects.transform.position = camera.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (focusFlag)
        {
            focusObjects.transform.position = Vector3.MoveTowards(focusObjects.transform.position, gameObject.transform.position, speed * Time.time);
            camera.transform.LookAt(focusObjects.transform);
            //camera.fieldOfView = focusDirection;
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