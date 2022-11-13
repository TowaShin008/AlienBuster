using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamageEffect : MonoBehaviour
{
    // ��ʂ�Ԃɂ��邽�߂̃C���[�W
    public Image img;
    [SerializeField]
    public GameObject player;

    public void Start()
    {
        // �����ɂ��Č����Ȃ����Ă����܂��B
        //img.color = Color.clear;
    }

    void Update()
    {
        // ���Ԃ��o�߂���ɂ�ď��X�ɓ����ɂ��܂��B
        //this.img.color = Color.Lerp(this.img.color, Color.clear, Time.deltaTime);

        this.img.color = new Color(this.img.color.r, this.img.color.g, this.img.color.b, (1-(float)player.GetComponent<FPSController>().GetHP() / (float)player.GetComponent<FPSController>().GetMaxHP()));
    }

    public virtual void Damage(int damage)
    {
        // *��ʂ�ԓh��ɂ���
        this.img.color = new Color(0.5f, 0f, 0f, 0.5f);
    }
}
