using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Util;

public class HomingMissile : MonoBehaviour
{
    //�v���C���[�̃|�W�V����
    [SerializeField] private GameObject playerObject;
    //�����G�t�F�N�g
    [SerializeField] GameObject explosion;

    public AudioClip explosionSound;
    private AudioSource audioSource;

    private bool isDeadFlag = false;
    Rigidbody rigidbody;

    private const int riseMaxCounter = 50;
    private int riseCounter = riseMaxCounter;
    private Transform target;
    // Start is called before the first frame update
    void Start()
    {
        isDeadFlag = false;
        rigidbody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
        Invoke("ExplodeProcessing", Constants.missileLife); // �O���l�[�h�𔭎˂��Ă���1.5�b��ɔ���������
        riseCounter = riseMaxCounter;
    }
    // Update is called once per frame
    void Update()
    {
        if (RiseProcessing())
        {
            if (isDeadFlag == false)
			{
                rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
                transform.position += transform.forward;
            }
        }

        if (isDeadFlag)
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
        if (collision.gameObject.tag == Constants.enemyName.ToString() || isDeadFlag || collision.gameObject.name == "UFO_weak" || collision.gameObject.tag == Constants.enemyBulletName.ToString()) { return; }
        //���j���o
        ExplodeProcessing();
    }
    /// <summary>
    /// ���j���o
    /// </summary>
    private void ExplodeProcessing()
    {
        if (isDeadFlag) { return; }
        //���j���o
        GameObject newExplosion = Instantiate(explosion, this.gameObject.transform.position, Quaternion.Euler(0, 0, 0));
        Destroy(newExplosion, 1.0f);

        audioSource.PlayOneShot(explosionSound);

        rigidbody.velocity = Vector3.zero;
        rigidbody.angularVelocity = Vector3.zero;

        var renderer = gameObject.GetComponent<Renderer>();
        // ��\��
        renderer.enabled = false;

        isDeadFlag = true;
    }
    /// <summary>
    /// �㏸����
    /// </summary>
    /// <returns>�����������������ǂ���</returns>
    private bool RiseProcessing()
	{
        if (riseCounter > 0)
		{
            riseCounter--;
            transform.position += transform.up;
            target = playerObject.transform;
            transform.LookAt(target);
        }
        else
		{
            return true;
		}
        return false;
	}
}
