using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scroll_Script : MonoBehaviour
{
    private float speed = 5.0f;
    private float right_Pos = 1920.0f;
    private float left_Pos = -1920.0f;
    Controll_Var controller;

    private Vector2 resetPos;

    // Start is called before the first frame update
    void Start()
    {
        resetPos = transform.position;

        controller = GameObject.Find("Caution_Transform").GetComponent<Controll_Var>();
        speed = controller.speed;
        right_Pos = controller.right_Pos;
        left_Pos = controller.left_Pos;
    }

    // Update is called once per frame
    void Update()
    {
        speed = controller.speed;
        right_Pos = controller.right_Pos;
        left_Pos = controller.left_Pos;

        transform.Translate(-speed, 0, 0);
        if (transform.localPosition.x < left_Pos)
        {
            Vector3 pos = transform.localPosition;
            pos.x = right_Pos;
            transform.localPosition = pos;
        }
    }

    private void Reset()
    {
        transform.position = resetPos;
    }
}
