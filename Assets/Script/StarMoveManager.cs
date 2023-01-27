using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StarMoveManager : MonoBehaviour
{
    [SerializeField]
    SimpleUITransition[] transitions;
    [SerializeField] ParticleSystem[] particle;
    [SerializeField]GameObject[] baseStar;
    [SerializeField]GameObject[] changeStar;
    int score;
    int rank;
    bool[] particlePlay = new bool[3];
    // Start is called before the first frame update
    void Start()
    {
        score = WaveManager.GetWave();
        rank = 0;
        for (int i = 0; i < 3; i++)
        {
            particlePlay[i] = false;
        }
        foreach (var trans in transitions)
        {
            trans.Show();
        } 
        foreach(var par in particle)
        {
            par.Stop();
        }
        /*foreach(var change in changeStar)
        {
            change.SetActive(false);
        }*/
    }

    // Update is called once per frame
    void Update()
    {
        //進んだウェーブ数でランクが変動
        switch (score)
        {
            //0~1
            case int i when i <=1:
                rank = 1;
                break;
            //2~3
            case int i when i <= 3:
                rank = 2;
                break;
            //4~5
            case int i when i <= 5:
                rank = 3;
                break;
        }
        for(int i = 2; i >=rank ; --i)
        {
            transitions[i].Hide();
        }

        for (int i = 0; i < rank; i++)
        {
            if (baseStar[i].transform.position.y ==
                changeStar[i].transform.position.y)
            {
                if (!particlePlay[i])
                {
                    particle[i].Play();
                    
                    particlePlay[i] = true;
                    
                }
            }

        }
        
    }
}
