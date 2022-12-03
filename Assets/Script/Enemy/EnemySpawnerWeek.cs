using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        if (gameObjectName != "Bullet" && gameObjectName != "RocketBumb" && gameObjectName == "EnemyBullet") { return; }

        if (gameObjectName == "Bullet")
        {
            audioSource.PlayOneShot(soundDamege);
            ufo.GetComponent<UFO>().Damage(1);
        }
        else if (gameObjectName == "RocketBumb")
        {
            audioSource.PlayOneShot(soundDamege);
            ufo.GetComponent<UFO>().Damage(10);
        }
    }
}
