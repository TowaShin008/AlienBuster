using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixPositionScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //�����I�u�W�F�N�g���X�e�[�W�̉��ɗ����Ă�����X�e�[�W�̏�Ɏ����Ă���
        if (this.transform.position.y < -10.0f)
        {
            Vector3 pos = this.transform.position;
            pos.y = 10.0f;
            this.transform.position = pos;
        }
    }
}
