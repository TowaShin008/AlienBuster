using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Flash_Caution_Script : MonoBehaviour
{

    public float speed = 0.1f;
    private float alpha = 1.0f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        alpha += speed;
        if(alpha < 0.0f || alpha > 2.0f)
        {
            speed *= -1;
        }


        Image image = GetComponent<Image>();

        Color color = image.material.color;
        color.a = alpha;
        image.material.color = color;
    }
}
