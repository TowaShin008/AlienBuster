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
    void Start()
    {
        speed = 1f;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("space"))
        {
            speed += 10;
            animator.SetFloat("Speed", speed);
            FadeManager.Instance.LoadScene("GameScene", 1f);
        }
    }
}
