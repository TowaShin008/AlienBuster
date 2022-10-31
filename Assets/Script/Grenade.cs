using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Invoke("Explode", 0.3f); // グレネードが作られてから1.5秒後に爆発させる
    }

    void Explode()
    {
        GameObject[] enemys = GameObject.FindGameObjectsWithTag("Enemy"); //「Cube」タグのついたオブジェクトを全て検索して配列にいれる

        if (enemys.Length == 0) return; // 「Cube」タグがついたオブジェクトがなければ何もしない。

        foreach (GameObject cube in enemys) // 配列に入れた一つひとつのオブジェクト
        {
            if (cube.GetComponent<Rigidbody>()) // Rigidbodyがあれば、グレネードを中心とした爆発の力を加える
            {
                cube.GetComponent<Rigidbody>().AddExplosionForce(100f, transform.position, 30f, 5f, ForceMode.Impulse);
            }
        }
    }
}
