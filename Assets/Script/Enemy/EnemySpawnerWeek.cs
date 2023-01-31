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
        if (gameObjectName != Constants.normalBulletName.ToString() && gameObjectName != Constants.rocketBombName.ToString() && gameObjectName != Constants.sniperBulletName.ToString() && gameObjectName == Constants.enemyBulletName.ToString()) { return; }
        if (ufo.GetComponent<UFO>().GetDeadFlag() == true) { return; }

        if (gameObjectName == Constants.normalBulletName.ToString())
        {
            audioSource.PlayOneShot(soundDamege);
            ufo.GetComponent<UFO>().Damage(Constants.normalBulletDamage);
        }
        else if (gameObjectName == Constants.rocketBombName.ToString())
        {
            audioSource.PlayOneShot(soundDamege);
            ufo.GetComponent<UFO>().Damage(Constants.rocketBombDamage);
        }
        else if (gameObjectName == Constants.sniperBulletName.ToString())
        {
            audioSource.PlayOneShot(soundDamege);
            ufo.GetComponent<UFO>().Damage(Constants.sniperBulletDamage);
        }
    }
}
