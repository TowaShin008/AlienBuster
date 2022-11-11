using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaveScore : MonoBehaviour
{
    // Start is called before the first frame update
    int waveScore;
    public GameObject wave_object = null;
    void Start()
    {
        waveScore = WaveManager.GetWave();
    }

    // Update is called once per frame
    void Update()
    {
        Text wave_text = wave_object.GetComponent<Text>();
        wave_text.text = "Wave : " + waveScore;
    }
}
