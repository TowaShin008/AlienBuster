using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashingText : MonoBehaviour
{
    // Start is called before the first frame update

    int time;
    bool active;
    [SerializeField]
    GameObject nextText;
    float startPosition;
    [SerializeField]
    int maxTime;
    void Start()
    {
        startPosition = nextText.transform.position.x;
        active = true;
        time = 0;
    }
    // Update is called once per frame
    void Update()
    {
        if (nextText.transform.position.x != startPosition)
        {
            time++;
        }
        if (time > maxTime)
        {
            active = !active;
            time = maxTime/2;
        }
        nextText.SetActive(active);
    }
}
