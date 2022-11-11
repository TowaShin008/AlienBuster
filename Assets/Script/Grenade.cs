using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Grenade : MonoBehaviour
{
    [SerializeField]
    private GameObject Explosion;

    private static bool isQuitting = false;
    void OnApplicationQuit()
    {
        isQuitting = true;
    }

    void OnDestroy()
    {
        if (!isQuitting)
        {
            // �����G�t�F�N�g�𔭐�������ꏊ(�e�̏ꏊ)���擾
            var ExplodePosition = this.transform.position;
            // ��Ŏ擾�����ꏊ�ɁA"�����G�t�F�N�g"��Prefab���o��������
            GameObject newExplode = Instantiate(Explosion, ExplodePosition, Quaternion.Euler(0, 0, 0));
            newExplode.name = Explosion.name;
            Explode();
            Destroy(newExplode, 2.1f);
            Debug.Log("OnDisable");
        }
    }
    void Explode()
    {
        GameObject[] cubes = GameObject.FindGameObjectsWithTag("Enemy"); //�uEnemy�v�^�O�̂����I�u�W�F�N�g��S�Č������Ĕz��ɂ����

        if (cubes.Length == 0) return; // �uEnemy�v�^�O�������I�u�W�F�N�g���Ȃ���Ή������Ȃ��B

        foreach (GameObject cube in cubes) // �z��ɓ��ꂽ��ЂƂ̃I�u�W�F�N�g
        {
            if (cube.GetComponent<Rigidbody>()) // Rigidbody������΁A�O���l�[�h�𒆐S�Ƃ��������̗͂�������
            {
                cube.GetComponent<Rigidbody>().drag = 0;
                cube.GetComponent<Rigidbody>().AddExplosionForce(30f,/*������΂�����*/ this.transform.position, 30f/*���m�͈�*/, 5f/*�Ώۂ��猟�m���Ȃ��͈�*/, ForceMode.Impulse);
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
