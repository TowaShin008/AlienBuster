using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeakText : MonoBehaviour
{
    [SerializeField] private GameObject playerObject;
    private Vector3 direction;
    Quaternion lookRotation;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        direction = playerObject.transform.position - transform.position;
        //direction.y = 0;        

        lookRotation = Quaternion.LookRotation(-direction, Vector3.up);
        transform.rotation = lookRotation;
    }
}

