using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResultStartScript : MonoBehaviour
{
    // Start is called before the first frame update
    private RawImage rawImage;
    public Texture changeImage;
    void Start()
    {
    }
    // Update is called once per frame
    void Update()
    {
     
    }
    public void ChangeImage()
    {
        this.rawImage = gameObject.GetComponent<RawImage>();
        this.rawImage.texture = changeImage;
    }
}
