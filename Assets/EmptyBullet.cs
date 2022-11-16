using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmptyBullet : MonoBehaviour
{
    //���䰂̔��ł������o
    bool hit = false;
    public AudioClip exitGunSound;
    AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        //���̃R���|�[�l���g�擾
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
            //��䰂̉�
            audioSource.PlayOneShot(exitGunSound);
            hit = true;
        }
    }
}
