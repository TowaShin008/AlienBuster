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

    //���݂�wave��ۑ����邽�߂̕ϐ�
    int nowWave;

    bool moveFlag = false;



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

        moveFlag = false;
    }
   

    // Update is called once per frame
    void Update()
    {
		if (moveFlag == false) { return; }

        if(gameObject.GetComponent<UFO>().GetEntryFlag()==false)
        {//�@�o�ߎ��Ԃ𑫂�
            elapsedTime += Time.deltaTime;
        }
    }

    /// <summary>
    /// �G�̏o������
    /// </summary>
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

    public bool ChangeNextWave()
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

            //deadFlag = false;

            enemyCount = 0;

            return true;
        }

        return false;
    }
    /// <summary>
    /// ��~����
    /// </summary>
    /// <returns></returns>
    public bool StopProcessing()
	{
        //�@���̏ꏊ����o������ő吔�𒴂��Ă��牽�����Ȃ�
        if (enemyCount >= nowMaxEnemyCount)
        {
            //waveManager.WaveChangeFlagOn();
            moveFlag = false;

            return true;
        }
        return false;
    }
    /// <summary>
    /// �G�̓�������
    /// </summary>
    /// <returns></returns>
    public bool LauncherProcessing()
	{
        //�@�o�ߎ��Ԃ��o������
        if (elapsedTime > nowSpawnNextTime)
        {
            elapsedTime = 0.0f;
            //�G�̏o������
            SpawnEnemy();

            return true;
        }
        return false;
	}

    public void SetMoveFlag(bool arg_moveFlag)
	{
        moveFlag = arg_moveFlag;
	}

    public void DecrimentEnemyCount()
	{
        enemyCount--;
	}
}
