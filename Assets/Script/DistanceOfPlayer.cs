using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Util;
public class DistanceOfPlayer : MonoBehaviour
{
    [SerializeField] private GameObject playerMainCamera;
    GameObject[] enemys;
    public float rayDistance = 100;
    [SerializeField] Text marker;
    [SerializeField] SimpleUITransition[] transitions;
    bool lookOn ;
    bool savelookOn;
    int playerGunType;
    //float distance;
    // Start is called before the first frame update
    void Start()
    {
        rayDistance = 100;
        lookOn = false;
        savelookOn = lookOn;
        //distance = 0;
        enemys = GameObject.FindGameObjectsWithTag(Constants.enemyName.ToString());
    }

    // Update is called once per frame
    void Update()
    {
        //距離の計算
        //for (int i = 0; i < enemys.Length; i++)
        //{
        //    float distance = Vector3.Distance(playerMainCamera.transform.position,
        //        enemys[i].transform.position);
        /* if (distance<=rayDistance)
         {
             lookOn = true;
         }
         else
         {
             lookOn = false;
         }*/
        //}
        //レイの召喚
        Ray ray = new Ray(playerMainCamera.transform.position, playerMainCamera.transform.forward);
        Debug.DrawRay(playerMainCamera.transform.position, playerMainCamera.transform.forward * rayDistance, Color.red);

        //レイキャスト
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, rayDistance))
        {
            if (hit.collider.gameObject.tag == Constants.enemyName.ToString()|| hit.collider.gameObject.tag == Constants.ufoName.ToString())
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

    public void SetPlayerGunType(int arg_playerGunType)
	{
        playerGunType = arg_playerGunType;

        if (playerGunType == 1)
        {
            rayDistance = Constants.normalGunBulletRange;
        }
        else if (playerGunType == 2)
        {
            rayDistance = Constants.rocketBombRange;
        }
        else if (playerGunType == 3)
        {
            rayDistance = Constants.sniperBulletRange;
        }
        else if (playerGunType == 4)
        {
            rayDistance = Constants.shotGunBulletRange;
        }
    }
}
