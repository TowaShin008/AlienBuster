using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Invoke("Explode", 0.3f); // �O���l�[�h������Ă���1.5�b��ɔ���������
    }

    void Explode()
    {
        GameObject[] enemys = GameObject.FindGameObjectsWithTag("Enemy"); //�uCube�v�^�O�̂����I�u�W�F�N�g��S�Č������Ĕz��ɂ����

        if (enemys.Length == 0) return; // �uCube�v�^�O�������I�u�W�F�N�g���Ȃ���Ή������Ȃ��B

        foreach (GameObject cube in enemys) // �z��ɓ��ꂽ��ЂƂ̃I�u�W�F�N�g
        {
            if (cube.GetComponent<Rigidbody>()) // Rigidbody������΁A�O���l�[�h�𒆐S�Ƃ��������̗͂�������
            {
                cube.GetComponent<Rigidbody>().AddExplosionForce(100f, transform.position, 30f, 5f, ForceMode.Impulse);
            }
        }
    }
}
