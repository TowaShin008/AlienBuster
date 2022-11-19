using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleSceneScript : MonoBehaviour
{
    // Start is called before the first frame update

    //タイトルのclickアニメーション
    [SerializeField]
    Animator animator;
    float speed = 1f;
    bool click = false;
    void Start()
    {
        click = false;
        speed = 1f;
    }

    // Update is called once per frame
    void Update()
    {
<<<<<<< HEAD:Assets/Script/TitleSceneScript.cs
        //if (Input.GetKeyDown("space"))
        //{
        //    FadeManager.Instance.LoadScene("GameScene", 0.5f);
        //}
=======
        if (Input.GetKeyDown("space"))
        {
            speed += 10;
            animator.SetFloat("Speed", speed);
            FadeManager.Instance.LoadScene("GameScene", 1f);
        }
>>>>>>> origin/yamaguchi:Assets/TitleSceneScript.cs
    }
}
