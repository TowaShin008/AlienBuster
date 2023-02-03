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
        if (time > 60)
        {
            active = !active;
            time = 0;
        }
        nextText.SetActive(active);
    }
}
