using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamageEffect : MonoBehaviour
{
    // 画面を赤にするためのイメージ
    public Image img;
    [SerializeField]
    public GameObject player;

    public void Start()
    {
        // 透明にして見えなくしておきます。
        //img.color = Color.clear;
    }

    void Update()
    {
        // 時間が経過するにつれて徐々に透明にします。
        //this.img.color = Color.Lerp(this.img.color, Color.clear, Time.deltaTime);

        this.img.color = new Color(this.img.color.r, this.img.color.g, this.img.color.b, (1-(float)player.GetComponent<FPSController>().GetHP() / (float)player.GetComponent<FPSController>().GetMaxHP()));
    }

    public virtual void Damage(int damage)
    {
        // *画面を赤塗りにする
        this.img.color = new Color(0.5f, 0f, 0f, 0.5f);
    }
}
