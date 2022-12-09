using System.Collections;
using System.Collections.Generic;
using UnityEngine;

<<<<<<<< HEAD:Assets/Script/Enemy/Explosion.cs
public class Explosion : MonoBehaviour
========
public class Bullet : MonoBehaviour
>>>>>>>> origin/yamaguchi:Assets/Script/Weapon/Bullet.cs
{
    //”š”­‰¹
    public AudioClip explosionSound;
    AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.PlayOneShot(explosionSound);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        Destroy(gameObject);
    }
}
