using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    // Start is called before the first frame update
    int timer;
    void Start()
    {
        timer = 0;
    }

    // Update is called once per frame
    void Update()
    {
<<<<<<< HEAD:Assets/Script/Enemy/EnemyBullet.cs
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        Destroy(gameObject);
=======
        if (timer > 120)
        {
            if (Input.GetKeyDown("space"))
            {
                FadeManager.Instance.LoadScene("TitleScene", 0.5f);
            }
        }
        else
        {
            timer++;
        }
      
>>>>>>> origin/yamaguchi:Assets/EndScene.cs
    }
}
