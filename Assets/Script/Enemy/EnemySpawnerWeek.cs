using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Util;

public class EnemySpawnerWeek : MonoBehaviour
{
    [SerializeField] public GameObject ufo;

    public AudioClip soundDamege;
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
        if (ufo.GetComponent<UFO>().GetDeadFlag() == true) { return; }

        if (gameObjectName == "Bullet")
        {
            audioSource.PlayOneShot(soundDamege);
            ufo.GetComponent<UFO>().Damage(Constants.normalBulletDamage);
        }
        else if (gameObjectName == "RocketBumb")
        {
            audioSource.PlayOneShot(soundDamege);
            ufo.GetComponent<UFO>().Damage(Constants.rocketBombDamage);
        }
        else if (gameObjectName == "SniperBullet")
        {
            audioSource.PlayOneShot(soundDamege);
            ufo.GetComponent<UFO>().Damage(Constants.sniperBulletDamage);
        }
    }
}
