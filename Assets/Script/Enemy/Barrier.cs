using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Util;
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
        if (gameObjectName != Constants.normalBulletName.ToString() && gameObjectName != Constants.rocketBombName.ToString() && gameObjectName != Constants.sniperBulletName.ToString() && gameObjectName == Constants.enemyBulletName.ToString()) { return; }

        if (gameObjectName == Constants.normalBulletName.ToString()|| gameObjectName == Constants.rocketBombName.ToString()|| gameObjectName == Constants.sniperBulletName.ToString())
        {
            audioSource.PlayOneShot(barrierSound);
        }
    }
}
