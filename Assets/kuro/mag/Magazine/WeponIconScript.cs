using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeponIconScript : MonoBehaviour
{
    [SerializeField] List<GameObject> gameObjectItems;
    [SerializeField] List<Sprite> sprites;

    private Image image;
    private WeponTextScript textScript;
    // Start is called before the first frame update
    void Start()
    {
        image = gameObject.GetComponent("Image") as Image;
        textScript = GameObject.Find("WeponText").GetComponent<WeponTextScript>();
    }

    // Update is called once per frame
    void Update()
    {
        for(int i = 0; i < gameObjectItems.Count; i++)
        {
            if (gameObjectItems[i].activeSelf)
            {
                image.sprite = sprites[i];
                textScript.LoadText(i);
            }
        }
    }
}
