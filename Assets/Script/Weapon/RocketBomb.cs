using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketBomb : MonoBehaviour
{
    //爆発エフェクト
    [SerializeField] GameObject explosion;

	public AudioClip explosionSound;
    private AudioSource audioSource;

    private bool isDeadFlag = false;
	// Start is called before the first frame update
	void Start()
    {
        isDeadFlag = false;
        audioSource = GetComponent<AudioSource>();
        Invoke("ExplodeProcessing", 2.0f); // グレネードを発射してから1.5秒後に爆発させる
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
        //Instantiate(explosion, this.gameObject.transform.position, Quaternion.Euler(0, 0, 0)); // ★追加
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag != "Enemy" || isDeadFlag) { return; }

        ExplodeProcessing();
    }

    private void ExplodeProcessing()
	{
		if (isDeadFlag) { return; }
        //爆破演出
        GameObject newExplosion = Instantiate(explosion, this.gameObject.transform.position, Quaternion.Euler(0, 0, 0));
        Destroy(newExplosion, 1.0f);

        audioSource.PlayOneShot(explosionSound);

        isDeadFlag = true;
    }
}
