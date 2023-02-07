using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Util;
public class ShotGunBulllet : MonoBehaviour
{
    SphereCollider collider;
    public int collsionDelayTime = 2;
    // Start is called before the first frame update
    void Start()
    {
        collider = GetComponent<SphereCollider>();
        collider.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (collsionDelayTime > 0)
		{
            collsionDelayTime--;
		}
        else
		{
            collider.enabled = true;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == Constants.normalBulletName.ToString()) { return; }

        Destroy(gameObject);
    }
}
