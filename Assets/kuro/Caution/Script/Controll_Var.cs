using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �g����
/// ���̎g�������^�C�~���O������X�N���v�g�ŉ��̂����s
/// Controll_Var cautionScript = GameObject.Find("Caurion_Transform").GetComponent<Controll_Var>();
/// cautionScript.ChangeStartFlag();
/// �I���̃^�C�~���O�ŉ��̂����s
/// Controll_Var cautionScript = GameObject.Find("Caurion_Transform").GetComponent<Controll_Var>();
/// cautionScript.ChangeEndFlag();
/// </summary>
public class Controll_Var : MonoBehaviour
{
    [Header("�����̐ݒ�")]
    public float speed = 5.0f;
    public float left_Pos = -1920;
    public float right_Pos = 3840;

    [Header("�o�ꎞ�̍��W")]
    public Vector2 defaultPosition = new Vector2(0, 700);
    public Vector2 stay_Position = new Vector2(0, 240);
    public float velocity = 5.0f;

    [Header("���̑�")]
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
