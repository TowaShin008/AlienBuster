using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreezeRotation : MonoBehaviour
{

    Vector3 def;
    // Start is called before the first frame update
    void Start()
    {
        def = transform.localRotation.eulerAngles;
    }

    // Update is called once per frame
    void Update()
    {

        Vector3 _parent = transform.parent.transform.localRotation.eulerAngles;

        
        transform.localRotation = Quaternion.Euler(def - _parent);        
    }
}
