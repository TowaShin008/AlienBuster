using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaveScore : MonoBehaviour
{
    // Start is called before the first frame update
    int waveScore;
    [SerializeField]
    SimpleUITransition[] transitions;

    public GameObject wave_object = null;
    public GameObject enemy_object = null;
    public GameObject ufo_object = null;

    int enemyScore;
    int ufoScore;

    void Start()
    {
        waveScore = WaveManager.GetWave();
        enemyScore = DestroyEnemyUfoCounter.EnemyreturnCounter();
        ufoScore = DestroyEnemyUfoCounter.UforeturnCounter();
        foreach(var trans in transitions)
        {
            trans.Show();
        }
    }

    // Update is called once per frame
    void Update()
    {
        Text wave_text = wave_object.GetComponent<Text>();
        wave_text.text = ""+waveScore;
        Text enemy_text = enemy_object.GetComponent<Text>();
        enemy_text.text = "" + enemyScore;
        Text ufo_text = ufo_object.GetComponent<Text>();
        ufo_text.text = "" + ufoScore;
    }
}
