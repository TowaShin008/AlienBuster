using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 使い方
/// この使いたいタイミングがあるスクリプトで下のを実行
/// Controll_Var cautionScript = GameObject.Find("Caurion_Transform").GetComponent<Controll_Var>();
/// cautionScript.ChangeStartFlag();
/// 終了のタイミングで下のを実行
/// Controll_Var cautionScript = GameObject.Find("Caurion_Transform").GetComponent<Controll_Var>();
/// cautionScript.ChangeEndFlag();
/// </summary>
public class Controll_Var : MonoBehaviour
{
    [Header("動きの設定")]
    public float speed = 5.0f;
    public float left_Pos = -1920;
    public float right_Pos = 3840;

    [Header("登場時の座標")]
    public Vector2 defaultPosition = new Vector2(0, 700);
    public Vector2 stay_Position = new Vector2(0, 240);
    public float velocity = 5.0f;

    [Header("その他")]
    bool startflag = false;
    private bool endflag = true;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (startflag)
        {
            transform.localPosition = Vector3.MoveTowards(transform.localPosition, stay_Position, velocity);
        }
        if (endflag)
        {
            transform.localPosition = Vector3.MoveTowards(transform.localPosition, defaultPosition, velocity);

        }

		//if (Input.GetKeyDown(KeyCode.O)) ChangeStartFlag();
		//if (Input.GetKeyDown(KeyCode.I)) ChangeEndFlag();
	}

    public void ChangeStartFlag()
    {
        startflag = true;
        endflag = false;
    }
    public void ChangeEndFlag()
    {
        startflag = false;
        endflag = true;
    }

    public bool GetEndFlag()
    {
        return endflag;
    }
}
