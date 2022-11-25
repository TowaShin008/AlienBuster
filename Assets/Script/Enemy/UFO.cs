using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UFO : MonoBehaviour
{
    public MeshRenderer mesh;
    public MeshRenderer mesh_2;
    bool entryFlag = false;
    public GameObject enemySpawnManager;
    private bool isDeadFlag = false;
    private int hp = 100;
    private float rotateY = 0;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(entryFlag)
		{//出現演出
            EntryProcessing();
		}
        RotationProcessing();
    }
    /// <summary>
    /// 出現演出
    /// </summary>
    private void EntryProcessing()
    {
        mesh.material.color = mesh.material.color + new Color(0, 0, 0, 0.005f);
        mesh_2.material.color = mesh_2.material.color + new Color(0, 0, 0, 0.005f);
        if (mesh.material.color.a>=1.0f)
		{
            entryFlag = false;
            enemySpawnManager.GetComponent<EnemySpawnManager>().SetMoveFlag(true);
		}
    }
    /// <summary>
    /// UFOの回転演出
    /// </summary>
    private void RotationProcessing()
	{
        if(rotateY>360)
		{
            rotateY = 0;
		}
        else
		{
            rotateY++;
		}
        mesh.SetRotateY(rotateY);
        mesh_2.SetRotateY(rotateY);
    }
	public void Initialize()
	{
        entryFlag = true;
        mesh.material.color = new Color(mesh.material.color.r, mesh.material.color.g, mesh.material.color.b, 0);
        mesh_2.material.color = new Color(mesh_2.material.color.r, mesh_2.material.color.g, mesh_2.material.color.b, 0);
        enemySpawnManager.GetComponent<EnemySpawnManager>().SetMoveFlag(false);
    }
	public void SetEntryFlag(bool arg_entryFlag)
	{
        entryFlag = arg_entryFlag;
	}
}
