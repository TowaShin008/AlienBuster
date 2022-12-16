using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MiniMap2D : MonoBehaviour
{
    [System.Serializable]
    struct MMTYPE
    {
        public string _TagName;
        public Sprite _Sprite;

        public float _IconSize;
    }
    [SerializeField] List<MMTYPE> _MMType = new List<MMTYPE>();//サンプル用データ
    public Sprite _MMFrameSprite;
    public Sprite _MMBackGroundSprite;
    public Sprite _PlayerSprite;
    public bool rotate_Direction = false;
    public float _MMDistance = 1.0f;


    int[] itemNum;
    GameObject sampleObject;
    public struct ITEM
    {
        public string tagName;
        public GameObject _Object;
        public GameObject _MMObject;

        public float _IconSize;
    }
    private List<ITEM> _MMItems = new List<ITEM>();//各オブジェクト

    GameObject player;
    GameObject _MMPlayer;
    GameObject _MMFrame;
    GameObject _MMBackGround;

    public float _BaseIconSize = 0.1f;

    public float edge = 50.0f;
    // Start is called before the first frame update
    void Start()
    {
        sampleObject = GameObject.Find("MMSampleImage") as GameObject;
        sampleObject.SetActive(false);

        MMInitialize();
        CreateObjects();
    }

    // Update is called once per frame
    void Update()
    {
        if (GetAllObjectCount() != _MMItems.Count) CreateObjects();

        for(int i = 0; i < _MMType.Count; i++)
        {
            for (int n = 0; n < _MMItems.Count; n++)
            {
                if(_MMType[i]._TagName == _MMItems[n].tagName)
                {
                    ITEM item = _MMItems[n];
                    item._IconSize = _MMType[i]._IconSize;
                    _MMItems[n] = item;
                }
            }
        }

        Rotate_MM();
        SetPlayerRotation();
        _MMPlayer.transform.localScale = new Vector3(_BaseIconSize, _BaseIconSize, 1.0f);

        CalculationUpdate();
    }

    void SetSprite()
    {
        for(int i = 0; i < _MMItems.Count; i++)//ミニマップに描画されるオブジェクト
        {
            ITEM mmItem = _MMItems[i];
            for (int t = 0; t < _MMType.Count; t++) //Tagの数
            {
                if (mmItem.tagName == _MMType[t]._TagName)
                {
                    Image _Image = mmItem._MMObject.GetComponent("Image") as Image;
                    _Image.sprite = _MMType[t]._Sprite;
                }
            }
            _MMItems[i] = mmItem;
        }
    }

    void CreateObjects()
    {
        //とりあえず全部作り直す
        if(_MMItems.Count != 0)
        {
            for(int i = 0; i < _MMItems.Count; i++)
            {
                //Destroy(_Objects[i]._object);
                Destroy(_MMItems[i]._MMObject);
            }

            _MMItems.Clear();
        }

        for (int i = 0; i < _MMType.Count; i++)
        {
            GameObject[] items = GameObject.FindGameObjectsWithTag(_MMType[i]._TagName);//特定のTagのオブジェクトをセット
            for (int c = 0; c < items.Length; c++)
            {
                //ミニマップ用のオブジェクトも生成しておく
                ITEM item = new ITEM();
                item._Object = items[c];
                item.tagName = _MMType[i]._TagName;
                item._MMObject = Instantiate(sampleObject);
                item._MMObject.SetActive(true);
                item._MMObject.name = "MM" + _MMType[i]._TagName + i;
                item._MMObject.transform.parent = GameObject.Find("MM2DMask").transform;
                //item._object.transform.localPosition = new Vector3(0, 0, 0);
                item._MMObject.transform.localPosition = CalculationPositionPlayer(item._Object.transform.localPosition);
                _MMItems.Add(item);

                item._IconSize = _MMType[i]._IconSize;
                item._MMObject.transform.localScale = new Vector3(_BaseIconSize + item._IconSize, _BaseIconSize + item._IconSize, 1.0f);

            }
        }

        SetSprite();
    }
    int GetAllObjectCount()
    {
        int counter = 0;
        for (int i = 0; i < _MMType.Count; i++)
        {
            GameObject[] items = GameObject.FindGameObjectsWithTag(_MMType[i]._TagName);//特定のTagのオブジェクトをセット
            counter += items.Length;
        }
        return counter;
    }

    void MMInitialize()
    {
        player = GameObject.Find("Player") as GameObject;

        _MMBackGround = Instantiate(sampleObject);
        _MMFrame = Instantiate(sampleObject);
        _MMPlayer = Instantiate(sampleObject);

        _MMBackGround.SetActive(true);
        _MMFrame.SetActive(true);
        _MMPlayer.SetActive(true);

        _MMBackGround.name = "MMBackGround";
        _MMFrame.name = "MMFrame";
        _MMPlayer.name = "MMPlayer";

        _MMBackGround.transform.parent = GameObject.Find("MM2DMask").transform;
        _MMFrame.transform.parent = GameObject.Find("MMSubCanvas").transform;
        _MMPlayer.transform.parent = GameObject.Find("MMSubCanvas").transform;

        Image image = _MMBackGround.GetComponent("Image") as Image;
        image.sprite = _MMBackGroundSprite;
        image = _MMFrame.GetComponent("Image") as Image;
        image.sprite = _MMFrameSprite;
        image = _MMPlayer.GetComponent("Image") as Image;
        image.sprite = _PlayerSprite;

        //_MMBackGround.transform.position = new Vector3(0, 0, 0);
        _MMBackGround.transform.position = sampleObject.transform.position;
        _MMFrame.transform.position = sampleObject.transform.position; ;
        _MMPlayer.transform.position = sampleObject.transform.position; ;


        _MMPlayer.transform.localScale = new Vector3(_BaseIconSize, _BaseIconSize, 1.0f);
    }

    float CalculationDistancePlayer(Vector3 pos)//プレイヤーと指定の座標のYを無視した距離を計算
    {
        return Vector3.Distance(pos,_MMPlayer.transform.localPosition);
    }

    Vector2 CalculationPositionPlayer(Vector3 pos)//Y mushi
    {
        Vector3 pPos = player.transform.position;
        Vector3 calPos = pPos - pos;
        return new Vector2(-calPos.x, -calPos.z);
    }

    void CalculationUpdate()
    {
        if (_MMItems.Count == 0) return;
        for(int i = 0; i < _MMItems.Count; i++)
        {
            if(_MMItems[i]._Object == null) { continue; }
            ITEM item = _MMItems[i];
            Vector2 pos = CalculationPositionPlayer(_MMItems[i]._Object.transform.position);
            pos = pos * _MMDistance;

            if (item.tagName == "Enemy"|| item.tagName == "WeaponItem")
            {
                pos = IconInMap(pos);
            }

            Vector3 pos3 = new Vector3(pos.x, pos.y, 0.0f);
            //pos3 += sampleObject.transform.position;
            item._MMObject.transform.localPosition = pos3;
            item._MMObject.transform.localScale = new Vector3(_BaseIconSize + item._IconSize, _BaseIconSize + item._IconSize, 1.0f);

            _MMItems[i] = item;
        }

    }

    Vector3 IconInMap(Vector3 _Pos)
    {
        Vector3 pos = _Pos;
            while (true) 
            {
                float distance = CalculationDistancePlayer(pos);
                if (distance < edge) break;

                pos = Vector3.MoveTowards(pos, _MMPlayer.transform.localPosition, 1.0f);
            }
        return pos;
    }

    void Rotate_MM()
    {
        if (!rotate_Direction) return;

        GameObject _MM2DMask = GameObject.Find("MM2DMask") as GameObject;
        Quaternion rot = _MM2DMask.transform.rotation;
        rot.z = player.transform.rotation.y;
        rot.w = player.transform.rotation.w;
        _MM2DMask.transform.rotation = rot;
    }

    void SetPlayerRotation()
    {
        Quaternion rot = _MMPlayer.transform.rotation;
        if (rotate_Direction)
        {
            GameObject _MM2DMask = GameObject.Find("MM2DMask") as GameObject;
            rot.z = -_MM2DMask.transform.rotation.y;
            rot.w = -_MM2DMask.transform.rotation.w;
        }
        else
        {
            rot.z = -player.transform.rotation.y;
            rot.w = player.transform.rotation.w;
        }
        _MMPlayer.transform.rotation = rot;
    }
}
