using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnManager : MonoBehaviour
{

    //�o��������G�̃I�u�W�F�N�g
    [SerializeField] GameObject[] enemys;
    //wave���Ƃ̎��ɓG���o������܂ł̎���
    [SerializeField] float[] spawnNextTime = { 10.0f,5.0f };
    //���݂̎��ɓG���o������܂ł̎���
    private float nowSpawnNextTime;
    //�o������wave���̓G�̍ő吔
    [SerializeField] int[] maxEnemyCount = {5,10};
    //�o�����錻�݂̓G�̍ő吔
    private int nowMaxEnemyCount;
    //�o������ʒu
    [SerializeField] GameObject spawnPoint;
    //���݂�wave�œG���o������������
    private int enemyCount;
    //�G���o������������
    private int enemyAllCount;
    //���̓G���o��������܂ł̑҂�����
    private float elapsedTime;

    //���݂�wave��waveManeger����擾
    [SerializeField] WaveManager waveManager;
    //���݂�wave��ۑ����邽�߂̕ϐ�
    int nowWave;

    [SerializeField] GameObject barrier;
    //�o���A�������Ă��邩
    bool barrierFlag;
    //�G�����ׂďo�Ă���o���A����������܂ł̎���
    [SerializeField] int barrierCreateMaxTime = 60;
    private int barrierCreateTime;

    // Start is called before the first frame update
    void Start()
    {
        enemyCount = 0;
        elapsedTime = 0.0f;
        this.nowWave = WaveManager.nowWave;

        if (nowWave <= spawnNextTime.Length)
        {
            nowSpawnNextTime = spawnNextTime[nowWave - 1];
        }
        else
        {
            float index = spawnNextTime[spawnNextTime.Length - 1];
            nowSpawnNextTime = index;
        }

        if (nowWave <= maxEnemyCount.Length)
        {
            nowMaxEnemyCount = maxEnemyCount[nowWave - 1];
        }
        else
        {
            nowMaxEnemyCount = maxEnemyCount[maxEnemyCount.Length - 1];
        }

        barrierFlag = true;
        barrierCreateTime = barrierCreateMaxTime;
    }
   

    // Update is called once per frame
    void Update()
    {

        //wave�ύX���̏���
        if (nowWave != WaveManager.nowWave)
        {
            nowWave = WaveManager.nowWave;

            if (nowWave <= spawnNextTime.Length)
            {
                nowSpawnNextTime = spawnNextTime[nowWave - 1];
            }
            else
            {
                nowSpawnNextTime = spawnNextTime[spawnNextTime.Length - 1];
            }

            if (nowWave <= maxEnemyCount.Length)
            {
                nowMaxEnemyCount = maxEnemyCount[nowWave - 1];
            }
            else
            {
                nowMaxEnemyCount = maxEnemyCount[maxEnemyCount.Length - 1];
            }

            enemyCount = 0;
        }

        if (barrierFlag == true)
        {
            barrier.SetActive(true);
        }
        else
        {
            barrier.SetActive(false);
        }

        //�@���̏ꏊ����o������ő吔�𒴂��Ă��牽�����Ȃ�
        if (enemyCount >= nowMaxEnemyCount)
        {
            waveManager.WaveChangeFlagOn();

            if (barrierFlag == false)
            {
                if (barrierCreateTime > 0)
                {
                    barrierCreateTime--;
                }
                else
                {
                    barrierCreateTime = barrierCreateMaxTime;
                    barrierFlag = true;
                }
            }               
         
            return;
        }
        //�@�o�ߎ��Ԃ𑫂�
        elapsedTime += Time.deltaTime;

        //�@�o�ߎ��Ԃ��o������
        if (elapsedTime > nowSpawnNextTime)
        {
            elapsedTime = 0.0f;

            barrierFlag = false;
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

        GameObject.Instantiate(enemys[randomValue], spawnPoint.transform.position, Quaternion.Euler(0f, randomRotationY, 0f));

        enemyCount++;
        enemyAllCount++;
        elapsedTime = 0.0f;
    }
}
