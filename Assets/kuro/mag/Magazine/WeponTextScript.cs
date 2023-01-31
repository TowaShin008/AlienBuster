using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class WeponTextScript : MonoBehaviour
{
    [SerializeField] List<string> texts;
    private Text text;

    private bool loadflag;
    private float a_color;
    private int a_counter;
    private int saveNumber;
    // Start is called before the first frame update
    void Start()
    {
        text = gameObject.GetComponent<Text>();
        loadflag = true;
        saveNumber = -1;
    }

    // Update is called once per frame
    void Update()
    {
        if (loadflag)
        {
            if (a_counter <= 60 * 0.5)
            {
                a_color += 1.0f / 30.0f;
            }
            else if (a_counter > 60 * 4.5 && a_counter <= 60 * 5)
            {
                a_color -= 1.0f / 30.0f;
            }
            else if (a_counter >= 60 * 5) loadflag = false;
            text.color = new Color(255, 255, 255, a_color);
            a_counter++;
        }
    }

    public void LoadText(int num)
    {
        if (saveNumber == num) return;

        loadflag = true;
        text.text = texts[num];
        a_counter = 0;
        saveNumber = num;
    }
}
