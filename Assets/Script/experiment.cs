using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class experiment : MonoBehaviour
{
    [SerializeField] float angle = 90f;
    [SerializeField] Vector3 axis = Vector3.up;
    [SerializeField] float interpolant = 0.8f;

    Quaternion recoil;//(targetRot)
    Quaternion recoilgun;//(startRot)
    float sec;

    // Start is called before the first frame update
    void Start()
    {
        recoilgun = transform.rotation;
        recoil = Quaternion.AngleAxis(angle, axis) * transform.rotation;
    }

    void Update()
    {
        sec += Time.deltaTime;
        transform.rotation = Quaternion.Lerp(recoilgun, recoil, sec * interpolant);
    }

}
