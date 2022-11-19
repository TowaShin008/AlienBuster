using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndScene : MonoBehaviour
{
    // Start is called before the first frame update
    int timer;
    void Start()
    {
        timer = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (timer > 120)
        {
            if (Input.GetKeyDown("space"))
            {
                FadeManager.Instance.LoadScene("TitleScene", 0.5f);
            }
        }
        else
        {
            timer++;
        }
    }
}
