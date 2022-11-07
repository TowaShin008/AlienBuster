using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnManager : MonoBehaviour
{

    //�o��������G�̃I�u�W�F�N�g
    [SerializeField] GameObject[] enemys;
    //���ɓG���o������܂ł̎���
    [SerializeField] float spawnNextTime = 10.0f;
    //���̏ꏊ����o������G�̐�
    [SerializeField] int maxEnemyCount = 5;
    //�o������������
    private int enemyCount;
    //���̓G���o��������܂ł̑҂�����
    private float elapsedTime;

    // Start is called before the first frame update
    void Start()
    {
        enemyCount = 0;
        elapsedTime = 0.0f;
    }
   

    // Update is called once per frame
    void Update()
    {
        //�@���̏ꏊ����o������ő吔�𒴂��Ă��牽�����Ȃ�
        if (enemyCount >= maxEnemyCount)
        {
            return;
        }
        //�@�o�ߎ��Ԃ𑫂�
        elapsedTime += Time.deltaTime;

        //�@�o�ߎ��Ԃ��o������
        if (elapsedTime > spawnNextTime)
        {
            elapsedTime = 0.0f;

            SpawnEnemy();
        }
    }

    //�@�G�o��
    void SpawnEnemy()
    {
        //�o��������G�������_���ɑI��
        var randomValue = Random.Range(0, enemys.Length);
        //�G�̌����������_���Ɍ���
        var randomRotationY = Random.value * 360f;

        GameObject.Instantiate(enemys[randomValue], transform.position, Quaternion.Euler(0f, randomRotationY, 0f));

        enemyCount++;
        elapsedTime = 0.0f;
    }
}
