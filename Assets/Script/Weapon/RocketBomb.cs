using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketBomb : MonoBehaviour
{
    //�����G�t�F�N�g
    [SerializeField] GameObject explosion;
    // Start is called before the first frame update
    void Start()
    {

    }
    // Update is called once per frame
    void Update()
    {

    }
    private void OnDestroy()
    {
        Instantiate(explosion, this.gameObject.transform.position, Quaternion.Euler(0, 0, 0)); // ���ǉ�
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag != "Enemy") { return; }

        Destroy(this);
    }
}
