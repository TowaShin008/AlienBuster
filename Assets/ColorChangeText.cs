using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ColorChangeText : MonoBehaviour
{
    Color color;
    bool change;
    Text text;
    // Start is called before the first frame update
    void Start()
    {
        text = this.GetComponent<Text>();
        color = new Color(255, 212, 93);
        change = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void ChangeRed()
    {
        if(!change)
        text.color = Color.red;
        change = true;
    }
    public void ChageNormal()
    {
        change = false;
        text.color = color;
    }
}
