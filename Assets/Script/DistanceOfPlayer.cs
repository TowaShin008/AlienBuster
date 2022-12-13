using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class DistanceOfPlayer : MonoBehaviour
{
    [SerializeField] private GameObject playerMainCamera;
    GameObject[] enemys;
    public float rayDistance = 100;
    [SerializeField] Text marker;
    [SerializeField] SimpleUITransition[] transitions;
    bool lookOn ;
    bool savelookOn;
    float speed;
    float distance;
    // Start is called before the first frame update
    void Start()
    {
        rayDistance = 100;
        speed = 1.0f;
        lookOn = false;
        savelookOn = lookOn;
        distance = 0;
        enemys = GameObject.FindGameObjectsWithTag("Enemy");
    }

    // Update is called once per frame
    void Update()
    {
        //距離の計算
        for (int i = 0; i < enemys.Length; i++)
        {
            float distance = Vector3.Distance(playerMainCamera.transform.position,
                enemys[i].transform.position);
           /* if (distance<=rayDistance)
            {
                lookOn = true;
            }
            else
            {
                lookOn = false;
            }*/
        }
        //レイの召喚
        Ray ray = new Ray(playerMainCamera.transform.position, playerMainCamera.transform.forward);
        Debug.DrawRay(playerMainCamera.transform.position, playerMainCamera.transform.forward * rayDistance, Color.red);

        //レイキャスト
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, rayDistance))
        {
            if (hit.collider.gameObject.tag == "Enemy")
            {
                Debug.Log("HHHHHHHHIT");
                lookOn = true;
                marker.color = Color.red;
                foreach (var trans in transitions)
                {
                    trans.Show();
                }
            }
            else
            {
                if (savelookOn)
                {
                    marker.color = Color.gray;
                    foreach (var trans in transitions)
                    {
                        trans.Hide();
                    }
                }
            }
        }
        else
        {
            if (savelookOn)
            {
                marker.color = Color.gray;
                foreach (var trans in transitions)
                {
                    trans.Hide();
                }
            }
        }
        savelookOn = lookOn;
        lookOn = false;
    }

}
