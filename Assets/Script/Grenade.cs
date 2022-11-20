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
            // �����G�t�F�N�g�𔭐�������ꏊ(�e�̏ꏊ)���擾
            var ExplodePosition = this.transform.position;
            // ��Ŏ擾�����ꏊ�ɁA"�����G�t�F�N�g"��Prefab���o��������
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
        GameObject[] cubes = GameObject.FindGameObjectsWithTag("Enemy"); //�uEnemy�v�^�O�̂����I�u�W�F�N�g��S�Č������Ĕz��ɂ����

        if (cubes.Length == 0) return; // �uEnemy�v�^�O�������I�u�W�F�N�g���Ȃ���Ή������Ȃ��B

        foreach (GameObject cube in cubes) // �z��ɓ��ꂽ��ЂƂ̃I�u�W�F�N�g
        {
            if (cube.GetComponent<Rigidbody>()) // Rigidbody������΁A�O���l�[�h�𒆐S�Ƃ��������̗͂�������
            {
                cube.GetComponent<Rigidbody>().drag = 0;
                cube.GetComponent<Rigidbody>().AddExplosionForce(PowertoBlowAway,/*������΂�����*/ this.transform.position, DetectionRange/*���m�͈�*/, RangeNotDetected/*�Ώۂ��猟�m���Ȃ��͈�*/, ForceMode.Impulse);
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
