using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketBomb : MonoBehaviour
{
    //�����G�t�F�N�g
    [SerializeField] GameObject explosion;

	public AudioClip explosionSound;
    private AudioSource audioSource;

    private bool isDeadFlag = false;
	// Start is called before the first frame update
	void Start()
    {
        isDeadFlag = false;
        audioSource = GetComponent<AudioSource>();
        Invoke("ExplodeProcessing", 2.0f); // �O���l�[�h�𔭎˂��Ă���1.5�b��ɔ���������
    }
    // Update is called once per frame
    void Update()
    {
        if(isDeadFlag)
		{
            if (audioSource.isPlaying == false)
			{
                Destroy(gameObject);
			}
		}
    }
    private void OnDestroy()
    {
        //Instantiate(explosion, this.gameObject.transform.position, Quaternion.Euler(0, 0, 0)); // ���ǉ�
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag != "Enemy" || isDeadFlag) { return; }

        ExplodeProcessing();
    }

    private void ExplodeProcessing()
	{
		if (isDeadFlag) { return; }
        //���j���o
        GameObject newExplosion = Instantiate(explosion, this.gameObject.transform.position, Quaternion.Euler(0, 0, 0));
        Destroy(newExplosion, 1.0f);

        audioSource.PlayOneShot(explosionSound);

        isDeadFlag = true;
    }
}
