using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barrier : MonoBehaviour
{
    //’e‚­‰¹
    public AudioClip barrierSound;
    AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        string gameObjectName = collision.gameObject.tag;
        if (gameObjectName != "Bullet" && gameObjectName != "RocketBumb" && gameObjectName != "SniperBullet" && gameObjectName == "EnemyBullet") { return; }

        if (gameObjectName == "Bullet"|| gameObjectName == "RocketBumb"|| gameObjectName == "SniperBullet")
        {
            audioSource.PlayOneShot(barrierSound);
        }
    }
}
