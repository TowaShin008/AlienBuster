using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Grenade : MonoBehaviour
{
    [SerializeField]private GameObject Explosion;
    [SerializeField] private float DestroyTime = 2.0f;
    [SerializeField] private float PowertoBlowAway = 30.0f;
    [SerializeField] private float DetectionRange = 30.0f;
    [SerializeField] private float RangeNotDetected = 5.0f;
    [SerializeField] private Vector3 ExplosionScale  = new Vector3(0.2f,0.2f,0.2f);
    private static bool isQuitting = false;
    void OnApplicationQuit()
    {
        isQuitting = true;
    }

    void OnDestroy()
    {
        if (!isQuitting)
        {
            // 爆発エフェクトを発生させる場所(弾の場所)を取得
            var ExplodePosition = this.transform.position;
            // 上で取得した場所に、"爆発エフェクト"のPrefabを出現させる
            GameObject newExplode = Instantiate(Explosion, ExplodePosition, Quaternion.Euler(0, 0, 0));
            newExplode.transform.localScale = ExplosionScale;
            newExplode.name = Explosion.name;
            Explode();
            Destroy(newExplode, DestroyTime);
            Debug.Log("OnDisable");
        }
    }
    void Explode()
    {
        GameObject[] cubes = GameObject.FindGameObjectsWithTag("Enemy"); //「Enemy」タグのついたオブジェクトを全て検索して配列にいれる

        if (cubes.Length == 0) return; // 「Enemy」タグがついたオブジェクトがなければ何もしない。

        foreach (GameObject cube in cubes) // 配列に入れた一つひとつのオブジェクト
        {
            if (cube.GetComponent<Rigidbody>()) // Rigidbodyがあれば、グレネードを中心とした爆発の力を加える
            {
                cube.GetComponent<Rigidbody>().drag = 0;
                cube.GetComponent<Rigidbody>().AddExplosionForce(PowertoBlowAway,/*吹き飛ばす強さ*/ this.transform.position, DetectionRange/*検知範囲*/, RangeNotDetected/*対象から検知しない範囲*/, ForceMode.Impulse);
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }

   
}
