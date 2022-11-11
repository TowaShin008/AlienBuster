using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmptyBullet : MonoBehaviour
{
    //空薬莢の飛んでいく演出
    bool hit = false;
    public AudioClip exitGunSound;
    AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        //音のコンポーネント取得
        audioSource = GetComponent<AudioSource>();

        hit = false;
    }

    // Update is called once per frame
    void Update()
    {
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (!hit)
        {
            //薬莢の音
            audioSource.PlayOneShot(exitGunSound);
            hit = true;
        }
    }
}
