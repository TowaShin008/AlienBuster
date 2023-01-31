using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreezePosition : MonoBehaviour
{
    Vector3 def;
    // Start is called before the first frame update
    void Start()
    {
        def = transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {

        Vector3 _parent = transform.parent.transform.localPosition;


        transform.localPosition = def - _parent;
    }
}
