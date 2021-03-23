using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System.IO;
using System.Linq;

public class SPUM_Manager : MonoBehaviour
{
    #if UNITY_EDITOR
    public float _version;
    public Texture2D _mainBody;
    public List<Sprite> _mainBodyList = new List<Sprite>();
    public int _maxNumber = 100;
    public string unitPath = "Assets/Resources/SPUM/SPUM_Units/";
    public SPUM_Prefabs _unitObjSet;
    public List<SPUM_TexutreList> _textureList = new List<SPUM_TexutreList>();
    public SPUM_SpriteList _spriteObj;
    public Text _unitCode;
    public Text _unitNumber;
    public GameObject _drawItemObj;
    public GameObject _childItem;
    public Transform _childPool;
    public Text _spumVersion;

    public List<string> _packageList = new List<string>();
    public List<SPUM_Button> _SPButtonList = new List<SPUM_Button>();

    // Start is called before the first frame update
    void Start()
    {
        if(_spumVersion!=null) _spumVersion.text = "VER " + _version.ToString(); 
        StartCoroutine(StartProcess());
    }


    public IEnumerator StartProcess()
    {
        bool dirChk = Directory.Exists("Assets/Resources/SPUM/SPUM_Sprites/Items");

        if(!dirChk)
        {
            OnNotice("[Empty body image source]\n\nYou need setup first\nPlease Sprite images locate to Resource Folder\nPlease Read Readme.txt file",1,1);
            yield return null;
        }
        else
        {
            SetSpriteList(0,"0_Hair"); //헤어 연결
            SetSpriteList(1,"1_FaceHair"); //수염 연결
            SetSpriteList(2,"2_Cloth"); //옷 연결
            SetSpriteList(3,"3_Pant"); //헤어 연결
            SetSpriteList(4,"4_Helmet"); //투구 연결
            SetSpriteList(5,"5_Armor"); //갑옷 연결
            SetSpriteList(6,"6_Weapons"); //오른쪽 무기 연결
            SetSpriteList(7,"6_Weapons"); //왼쪽 무기 연결
            SetSpriteList(8,"7_Back"); //뒤 아이템 연결


            //추가 패키지 연결
            AddPackageSprite();

            yield return StartCoroutine(GetPrefabList());
            //프리팹 연동
            ShowNowUnitNumber(); //프리팹 숫자 연동

            SetInit();
            //기본 색 연동
            //UI연동.
            _colorButton[0].color = _basicColor;
            _colorButton[1].color = _basicColor;
            _colorButton[2].color = _basicColor;

            _unitCode.text = GetFileName();

            //데이터 인증 부분
        }
    }

    public void SetInit()
    {
        //SetInitValue
        _spriteObj._eyeList[0].color = _basicColor;
        _spriteObj._eyeList[1].color = _basicColor;
        _spriteObj._hairList[3].color = _basicColor;
        _spriteObj._hairList[0].color = _basicColor;
        _spriteObj.Reset(); 
    }

    public void SetSpriteList(int num, string path)
    {
        _textureList[num]._textureList.Clear();

        Object[] tObj = Resources.LoadAll("SPUM/SPUM_Sprites/Items/"+path+"/",typeof(Texture2D));
        for(var i = 0 ; i < tObj.Length; i++)
        {
            if(tObj[i].GetType() == typeof(Texture2D))
            {
                string ttPath = "SPUM/SPUM_Sprites/Items/"+path+"/" + tObj[i].name;
                _textureList[num]._textureList.Add(ttPath);
            }
        }
    }

    //패키지 추가
    public void AddPackageSprite()
    {
        string packagePath = "Assets/Resources/SPUM/SPUM_Sprites/Packages";
        bool packageChk = Directory.Exists(packagePath);
        if(packageChk)
        {
            DirectoryInfo dir = new DirectoryInfo(packagePath);
            FileInfo[] info = dir.GetFiles("*.*");
            foreach (FileInfo f in info) 
            {
                string[] words = (f.Name).Split('.');
                _packageList.Add(words[0]);
            }
        }

        if(_packageList.Count > 0)
        {
            for(var i = 0 ; i < _packageList.Count;i++)
            {
                string pName = _packageList[i];
                SetSpritePackageList(0,"0_Hair",pName); //헤어 연결
                SetSpritePackageList(1,"1_FaceHair",pName); //수염 연결
                SetSpritePackageList(2,"2_Cloth",pName); //옷 연결
                SetSpritePackageList(3,"3_Pant",pName); //헤어 연결
                SetSpritePackageList(4,"4_Helmet",pName); //투구 연결
                SetSpritePackageList(5,"5_Armor",pName); //갑옷 연결
                SetSpritePackageList(6,"6_Weapons",pName); //오른쪽 무기 연결
                SetSpritePackageList(7,"6_Weapons",pName); //왼쪽 무기 연결
                SetSpritePackageList(8,"7_Back",pName); //뒤 아이템 연결
            }
        }
    }

    public void SetSpritePackageList(int num, string path, string package)
    {
        Object[] tObj = Resources.LoadAll("SPUM/SPUM_Sprites/Packages/"+package+"/"+path+"/",typeof(Texture2D));
        for(var i = 0 ; i < tObj.Length; i++)
        {
            if(tObj[i].GetType() == typeof(Texture2D))
            {
                string ttPath = "SPUM/SPUM_Sprites/Packages/"+package+"/"+path+"/" + tObj[i].name;
                _textureList[num]._textureList.Add(ttPath);
            }
        }
    }
    
    public void SetHair(int value){SetSpriteItem(0,value);}
    public void SetFaceHair(int value){SetSpriteItem(1,value);}
    public void SetClothSet(int value){SetSpriteItem(2,value);}
    public void SetPantSet(int value){SetSpriteItem(3,value);}
    public void SetHelmet(int value){SetSpriteItem(4,value);}
    public void SetArmorSet(int value){SetSpriteItem(5,value);}
    public void SetWeaponRight(int value){SetSpriteItem(6,value);}
    public void SetWeaponLeft(int value){SetSpriteItem(7,value);}
    public void SetBack(int value){SetSpriteItem(8,value);}

    public void SetSpriteItem (int listNum,int num, bool rand = false)
    {
        if( num != 0 )
        {
            int value = (rand) ? Random.Range(0,_textureList[listNum]._textureList.Count) : num; 
            bool textureChk = (listNum == 2||listNum == 3||listNum == 5) ? false : true;
            
            Sprite tSprite = null;
            Object[] tObj = null;
            if(textureChk) tSprite = Resources.Load<Sprite>(_textureList[listNum]._textureList[value]);
            else tObj = Resources.LoadAll<Sprite>(_textureList[listNum]._textureList[value]);
            
            
            switch(listNum)
            {
                case 0: 
                // 헤어
                _spriteObj._hairList[0].sprite = tSprite;
                _spriteObj._hairList[1].sprite = null;

                if(EmptyChk())
                {
                    _spriteObj._hairList[0].sprite = null;
                    _spriteObj._hairList[1].sprite = null;
                }
                
                break;

                case 1: 
                //수염
                _spriteObj._hairList[3].sprite = tSprite;

                if(EmptyChk())
                {
                    _spriteObj._hairList[3].sprite = null;
                }
                break;

                case 2: 
                
                // 옷
                _spriteObj._clothList[0].sprite = null;
                _spriteObj._clothList[1].sprite = null;
                _spriteObj._clothList[2].sprite = null;
                for(var i = 0; i < tObj.Length;i++)
                {
                    switch(tObj[i].name)
                    {
                        case "Body":
                        _spriteObj._clothList[0].sprite = tObj[i] as Sprite;
                        break;

                        case "Left":
                        _spriteObj._clothList[1].sprite = tObj[i] as Sprite;
                        break;

                        case "Right":
                        _spriteObj._clothList[2].sprite = tObj[i] as Sprite;
                        break;

                    }
                }

                if(EmptyChk())
                {
                    _spriteObj._clothList[0].sprite = null;
                    _spriteObj._clothList[1].sprite = null;
                    _spriteObj._clothList[2].sprite = null;
                }
                break;

                case 3: 
                //바지
                for(var i = 0; i < tObj.Length;i++)
                {
                    switch(tObj[i].name)
                    {
                        case "Left":
                        _spriteObj._pantList[0].sprite = tObj[i] as Sprite;
                        break;

                        case "Right":
                        _spriteObj._pantList[1].sprite = tObj[i] as Sprite;
                        break;
                    }
                }
                if(EmptyChk())
                {
                    _spriteObj._pantList[0].sprite = null;
                    _spriteObj._pantList[1].sprite = null;
                }
                break;

                case 4: 
                //헬멧
                _spriteObj._hairList[1].sprite = tSprite;
                _spriteObj._hairList[0].sprite = null;
                if(EmptyChk())
                {
                    _spriteObj._hairList[1].sprite = null;
                    _spriteObj._hairList[0].sprite = null;
                }
                break;

                case 5: 
                // 갑옷
                _spriteObj._armorList[0].sprite = null;
                _spriteObj._armorList[1].sprite = null;
                _spriteObj._armorList[2].sprite = null;

                for(var i = 0; i < tObj.Length;i++)
                {
                    switch(tObj[i].name)
                    {
                        case "Body":
                        _spriteObj._armorList[0].sprite = tObj[i] as Sprite;
                        break;

                        case "Left":
                        _spriteObj._armorList[1].sprite = tObj[i] as Sprite;
                        break;

                        case "Right":
                        _spriteObj._armorList[2].sprite = tObj[i] as Sprite;
                        break;

                    }
                }
                if(EmptyChk())
                {
                    _spriteObj._armorList[0].sprite = null;
                    _spriteObj._armorList[1].sprite = null;
                    _spriteObj._armorList[2].sprite = null;
                }
                break;

                case 6: 
                //오른손 무기
                string tRWName = tSprite.name;
                if(tRWName.Contains("Shield"))
                {
                    //방패
                    _spriteObj._weaponList[0].sprite = null;
                    _spriteObj._weaponList[1].sprite = tSprite;
                }
                else
                {
                    _spriteObj._weaponList[0].sprite = tSprite;
                    _spriteObj._weaponList[1].sprite = null;
                }
                if(EmptyChk())
                {
                    _spriteObj._weaponList[0].sprite = null;
                    _spriteObj._weaponList[1].sprite = null;
                }
                break;

                case 7: 
                //왼손 무기
                string tLWName = tSprite.name;
                if(tLWName.Contains("Shield"))
                {
                    //방패
                    _spriteObj._weaponList[2].sprite = null;
                    _spriteObj._weaponList[3].sprite = tSprite;
                }
                else
                {
                    _spriteObj._weaponList[2].sprite = tSprite;
                    _spriteObj._weaponList[3].sprite = null;
                }
                if(EmptyChk())
                {
                    _spriteObj._weaponList[2].sprite = null;
                    _spriteObj._weaponList[3].sprite = null;
                }
                break;

                case 8: 
                //뒤 아이템
                _spriteObj._backList[0].sprite = tSprite;
                if(EmptyChk())
                {
                    _spriteObj._backList[0].sprite = null;
                }
                break;

                
            }
        }
        else
        {
            // 없을때 초기화 구문
            switch(listNum)
            {
                case 0: 
                // 헤어
                _spriteObj._hairList[0].sprite = null;
                break;

                case 1: 
                //수염
                _spriteObj._hairList[3].sprite = null;
                break;

                case 2: 
                // 옷
                _spriteObj._clothList[0].sprite = null;
                _spriteObj._clothList[1].sprite = null;
                _spriteObj._clothList[2].sprite = null;
                break;

                case 3: 
                //바지
                _spriteObj._pantList[0].sprite = null;
                _spriteObj._pantList[1].sprite = null;
                break;

                case 4: 
                //헬멧
                _spriteObj._hairList[1].sprite = null;
                break;

                case 5: 
                // 갑옷
                _spriteObj._armorList[0].sprite = null;
                _spriteObj._armorList[1].sprite = null;
                _spriteObj._armorList[2].sprite = null;
                break;

                case 6: 
                //오른손 무기
                _spriteObj._weaponList[0].sprite = null;
                _spriteObj._weaponList[1].sprite = null;
                break;

                case 7: 
                //왼손 무기
                _spriteObj._weaponList[2].sprite = null;
                _spriteObj._weaponList[3].sprite = null;
                break;

                case 8: 
                //뒤 아이템
                _spriteObj._backList[0].sprite = null;
                break;
            }
        }

    }


    public void AllRandom()
    {
        
        RandomSelect(1);
        RandomSelect(2);
        RandomSelect(3);
        RandomSelect(5);
        RandomSelect(6);
        RandomSelect(7);
        RandomSelect(8);

        float tValue = Random.Range(0,0.7f);
        if(tValue <= 0.1f)
        {
            //대머리
        }
        else if(tValue <= 0.4f) //머리를 설치
        {
            RandomSelect(0); 
        }
        else
        {
            RandomSelect(4);
        }

        // _spriteObj._eyeList[0].color = _basicColor;
        // _spriteObj._eyeList[1].color = _basicColor;

        // if(!_SPButtonList[0]._toggle.isOn) RandomObjColor(0);
        // if(!_SPButtonList[2]._toggle.isOn) RandomObjColor(1);
        // if(!_SPButtonList[4]._toggle.isOn) RandomObjColor(2);
    }

    //랜덤 메이킹
    public void RandomSelect(int num)
    {
        switch(num)
        {
            case 0:
            //헤어 종류
            if(!_SPButtonList[1]._toggle.isOn) SetSpriteItem(0,-1,true);
            if(!_SPButtonList[2]._toggle.isOn) RandomObjColor(1);
            break;

            case 1:
            //수염
            if(!_SPButtonList[3]._toggle.isOn) SetSpriteItem(1,-1,true);
            if(!_SPButtonList[4]._toggle.isOn) RandomObjColor(2);
            break;

            case 2:
            //옷 종류
            if(!_SPButtonList[5]._toggle.isOn) SetSpriteItem(2,-1,true);
            break;

            case 3:
            //바지 종류
            if(!_SPButtonList[6]._toggle.isOn) SetSpriteItem(3,-1,true);
            break;

            case 4:
            //헬멧 종류
            if(!_SPButtonList[7]._toggle.isOn) SetSpriteItem(4,-1,true);
            break;

            case 5:
            //갑옷 종류
            if(!_SPButtonList[8]._toggle.isOn) SetSpriteItem(5,-1,true);
            break;

            case 6:
            //오른손 무기 종류
            if(!_SPButtonList[9]._toggle.isOn) SetSpriteItem(6,-1,true);
            break;

            case 7:
            //왼손 무기 종류
            if(!_SPButtonList[10]._toggle.isOn) SetSpriteItem(7,-1,true);
            break;

            case 8:
            //뒤 종류
            if(!_SPButtonList[11]._toggle.isOn) SetSpriteItem(8,-1,true);
            break;

            case 9:
            //눈 색
            if(!_SPButtonList[0]._toggle.isOn) RandomObjColor(0);
            break;

            case 10:
            //머리색
            if(!_SPButtonList[2]._toggle.isOn) RandomObjColor(1);
            break;

            case 11:
            //수염색
            if(!_SPButtonList[4]._toggle.isOn) RandomObjColor(2);
            break;
        }
    }

    
    public void DrawItem(int num)
    {
        //차일드 삭제
        if(_childPool.childCount > 0)
        {
            for(var i=0; i < _childPool.childCount;i++)
            {
                Destroy(_childPool.GetChild(i).gameObject);
            }
        }
        bool textureChk = (num == 2||num == 3||num == 5) ? true : false;

        GameObject ttObj2 = Instantiate(_childItem) as GameObject;
        ttObj2.transform.SetParent(_childPool);
        ttObj2.transform.localScale = new Vector3(1,1,1);
        SPUM_PreviewItem ttObjST2 = ttObj2.GetComponent<SPUM_PreviewItem>();
        ttObjST2._basicImage.sprite = null;
        ttObjST2.ShowObj(-1);
        ttObjST2._managerST = this;
        ttObjST2._itemType = num;
        ttObjST2._sprite = null;
        
        Debug.Log(num);
        Debug.Log(textureChk);
        if(!textureChk)
        {
            List<Sprite> tObj = new List<Sprite>();
            for(var i = 0 ; i < _textureList[num]._textureList.Count;i++ )
            {
                Sprite tSP = Resources.Load<Sprite>(_textureList[num]._textureList[i]);
                tObj.Add(tSP);
            }
            for(var i = 0 ; i < tObj.Count; i++)
            {
                GameObject ttObj = Instantiate(_childItem) as GameObject;
                ttObj.transform.SetParent(_childPool);
                ttObj.transform.localScale = new Vector3(1,1,1);

                SPUM_PreviewItem ttObjST = ttObj.GetComponent<SPUM_PreviewItem>();

                ttObjST._basicImage.sprite = tObj[i];
                ttObjST._basicImage.SetNativeSize();
                ttObjST._basicImage.rectTransform.pivot =  new Vector2(tObj[i].pivot.x/tObj[i].rect.width,tObj[i].pivot.y/tObj[i].rect.height);
                ttObjST._basicImage.rectTransform.localPosition = Vector2.zero;
                ttObjST.ShowObj(0);
                ttObjST._managerST = this;
                ttObjST._itemType = num;
                ttObjST._sprite = tObj[i];
            }
        }
        else
        {
            List<Texture2D> tObj = new List<Texture2D>();
            for(var i = 0 ; i < _textureList[num]._textureList.Count;i++ )
            {
                Texture2D tSP = Resources.Load<Texture2D>(_textureList[num]._textureList[i]);
                tObj.Add(tSP);
            }
            
            for(var i = 0 ; i < tObj.Count; i++)
            {
                if(tObj[i].GetType() == typeof(Texture2D))
                {
                    GameObject ttObj = Instantiate(_childItem) as GameObject;
                    ttObj.transform.SetParent(_childPool);
                    ttObj.transform.localScale = new Vector3(1,1,1);

                    SPUM_PreviewItem ttObjST = ttObj.GetComponent<SPUM_PreviewItem>();
                    switch(num)
                    {
                        case 2:
                        //옷 종류
                        ttObjST._clothList[0].gameObject.SetActive(false);
                        ttObjST._clothList[1].gameObject.SetActive(false);
                        ttObjST._clothList[2].gameObject.SetActive(false);

                        Sprite[] tSpriteCloth = Resources.LoadAll<Sprite>( _textureList[num]._textureList[i]);
                        for(var j = 0; j < tSpriteCloth.Length;j++)
                        {
                            switch(tSpriteCloth[j].name)
                            {
                                case "Body":
                                ttObjST._clothList[0].gameObject.SetActive(true);
                                ttObjST._clothList[0].sprite = tSpriteCloth[j];
                                break;

                                case "Left":
                                ttObjST._clothList[1].gameObject.SetActive(true);
                                ttObjST._clothList[1].sprite = tSpriteCloth[j];
                                break;

                                case "Right":
                                ttObjST._clothList[2].gameObject.SetActive(true);
                                ttObjST._clothList[2].sprite = tSpriteCloth[j];
                                break;

                            }
                        }
                        ttObjST.ShowObj(3);
                        break;

                        case 3:
                        //바지 종류
                        ttObjST._pantList[0].sprite=null;
                        ttObjST._pantList[1].sprite=null;
                        //바지
                        Sprite[] tSpritePant = Resources.LoadAll<Sprite>( _textureList[num]._textureList[i]);
                        for(var j = 0; j < tSpritePant.Length;j++)
                        {
                            switch(tSpritePant[j].name)
                            {
                                case "Left":
                                ttObjST._pantList[0].sprite = tSpritePant[j];
                                break;

                                case "Right":
                                ttObjST._pantList[1].sprite = tSpritePant[j];
                                break;
                            }
                        }
                        ttObjST.ShowObj(4);
                        break;

                        case 5:
                        //갑옷 종류
                        ttObjST._armorList[0].gameObject.SetActive(false);
                        ttObjST._armorList[1].gameObject.SetActive(false);
                        ttObjST._armorList[2].gameObject.SetActive(false);

                        Sprite[] tSpriteArmor = Resources.LoadAll<Sprite>( _textureList[num]._textureList[i]);

                        for(var j = 0; j < tSpriteArmor.Length;j++)
                        {
                            switch(tSpriteArmor[j].name)
                            {
                                case "Body":
                                ttObjST._armorList[0].gameObject.SetActive(true);
                                ttObjST._armorList[0].sprite = tSpriteArmor[j];
                                break;

                                case "Left":
                                ttObjST._armorList[1].gameObject.SetActive(true);
                                ttObjST._armorList[1].sprite = tSpriteArmor[j];
                                break;

                                case "Right":
                                ttObjST._armorList[2].gameObject.SetActive(true);
                                ttObjST._armorList[2].sprite = tSpriteArmor[j];
                                break;

                            }
                        }
                        ttObjST.ShowObj(2);
                        break;
                    }
                    
                    ttObjST._managerST = this;
                    ttObjST._itemType = num;
                    ttObjST._name = _textureList[num]._textureList[i];
                }
            }
        }
        
       
        _drawItemObj.SetActive(true);
    }

    public void DrawItemOff()
    {
        _drawItemObj.SetActive(false);
    }

    public void SetSprite(int num, Sprite sprite, string name,int index)
    {
        switch(num)
        {
            case 0:
            //헤어 종류
            _spriteObj._hairList[0].sprite = sprite;
            _spriteObj._hairList[1].sprite = null;
            break;
            
            case 1:
            //수염
            _spriteObj._hairList[3].sprite = sprite;
            break;

            case 2:
            //옷 종류
            // 옷
            _spriteObj._clothList[0].sprite = null;
            _spriteObj._clothList[1].sprite = null;
            _spriteObj._clothList[2].sprite = null;

            if(name.Length > 0)
            {
                Sprite[] tSpriteCloth = Resources.LoadAll<Sprite>(name);
                for(var i = 0; i < tSpriteCloth.Length;i++)
                {
                    switch(tSpriteCloth[i].name)
                    {
                        case "Body":
                        _spriteObj._clothList[0].sprite = tSpriteCloth[i];
                        break;

                        case "Left":
                        _spriteObj._clothList[1].sprite = tSpriteCloth[i];
                        break;

                        case "Right":
                        _spriteObj._clothList[2].sprite = tSpriteCloth[i];
                        break;
                    }
                }
            }
            
            break;

            case 3:
            //바지 종류
            //바지
            _spriteObj._pantList[0].sprite = null;
            _spriteObj._pantList[1].sprite = null;
            if(name.Length > 0)
            {
                Sprite[] tSpritePant = Resources.LoadAll<Sprite>(name);
                for(var i = 0; i < tSpritePant.Length;i++)
                {
                    switch(tSpritePant[i].name)
                    {
                        case "Left":
                        _spriteObj._pantList[0].sprite = tSpritePant[i];
                        break;

                        case "Right":
                        _spriteObj._pantList[1].sprite = tSpritePant[i];
                        break;
                    }
                }
            }
            
            break;

            case 4:
            //헬멧 종류
            _spriteObj._hairList[1].sprite = sprite;
            _spriteObj._hairList[0].sprite = null;
            break;

            case 5:
            //갑옷 종류
            _spriteObj._armorList[0].sprite = null;
            _spriteObj._armorList[1].sprite = null;
            _spriteObj._armorList[2].sprite = null;
            if(name.Length > 0)
            {
                Sprite[] tSpriteCloth = Resources.LoadAll<Sprite>(name);
                for(var i = 0; i < tSpriteCloth.Length;i++)
                {
                    switch(tSpriteCloth[i].name)
                    {
                        case "Body":
                        _spriteObj._armorList[0].sprite = tSpriteCloth[i];
                        break;

                        case "Left":
                        _spriteObj._armorList[1].sprite = tSpriteCloth[i];
                        break;

                        case "Right":
                        _spriteObj._armorList[2].sprite = tSpriteCloth[i];
                        break;
                    }
                }
            }
            break;

            case 6:
            //오른손 무기 종류
            if(sprite==null)
            {
                _spriteObj._weaponList[0].sprite = null;
                _spriteObj._weaponList[1].sprite = null;
            }
            else
            {
                if((sprite.name).Contains("Shield"))
                {
                    //방패
                    _spriteObj._weaponList[0].sprite = null;
                    _spriteObj._weaponList[1].sprite = sprite;
                }
                else
                {
                    _spriteObj._weaponList[0].sprite = sprite;
                    _spriteObj._weaponList[1].sprite = null;
                }
            }
            
            break;

            case 7:
            //왼손 무기 종류
            if(sprite==null)
            {
                _spriteObj._weaponList[2].sprite = null;
                _spriteObj._weaponList[3].sprite = null;
            }
            else
            {
                if((sprite.name).Contains("Shield"))
                {
                    //방패
                    _spriteObj._weaponList[2].sprite = null;
                    _spriteObj._weaponList[3].sprite = sprite;
                }
                else
                {
                    _spriteObj._weaponList[2].sprite = sprite;
                    _spriteObj._weaponList[3].sprite = null;
                }
            }
            break;

            case 8:
            //뒷 아이템
            _spriteObj._backList[0].sprite = sprite;
            break;

            case 11:
            //풀셋
            editObjNum = index;
            LoadUnitDataName(index);
            LoadButtonSet(true);
            CloseLoadData();
            break;
        }

        DrawItemOff();
    }

    public GameObject _colorPicker;
    public int _nowColorNum;
    public List<Image> _colorButton = new List<Image>();
    public Color _basicColor;
    public Color _nowColor;
    public void OpenColorPick(int num)
    {
        _colorPicker.SetActive(true);
        _nowColorNum = num;
    }

    public void CloseColorPick()
    {
        _colorPicker.SetActive(false);
    }
    Texture2D tex;
    public void PickColor()
    {
        tex = new Texture2D(1, 1);
        //get the color printed by calling:
        StartCoroutine(CaptureTempArea());
    }

    IEnumerator CaptureTempArea() {
        yield return new WaitForEndOfFrame();
        Vector2 pos = EventSystem.current.currentInputModule.input.mousePosition;
        tex.ReadPixels(new Rect(pos.x, pos.y, 1, 1), 0, 0);
        tex.Apply();
        _nowColor = tex.GetPixel(0, 0);
        SetObjColor();
    }

    public void SetObjColor()
    {
        switch(_nowColorNum)
        {
            case 0: //눈의 경우
            _colorButton[0].color = _nowColor;
            _spriteObj._eyeList[0].color = _nowColor;
            _spriteObj._eyeList[1].color = _nowColor;
            break;

            case 1: //수염의 경우
            _colorButton[1].color = _nowColor;
            _spriteObj._hairList[3].color = _nowColor;
            break;

            case 2: //머리의 경우
            _colorButton[2].color = _nowColor;
            _spriteObj._hairList[0].color = _nowColor;
            break;
        }
        CloseColorPick();
    }

    public void RandomObjColor(int num)
    {
        Color tColor = new Color(Random.Range(0,1f),Random.Range(0,1f),Random.Range(0,1f),1f);
        switch(num)
        {
            case 0: //눈의 경우
            _colorButton[0].color = tColor;
            _spriteObj._eyeList[0].color = tColor;
            _spriteObj._eyeList[1].color = tColor;
            break;

            case 1: //머리의 경우
            _colorButton[1].color = tColor;
            _spriteObj._hairList[0].color = tColor;
            break;

            case 2: //수염의 경우  
            _colorButton[2].color = tColor;
            _spriteObj._hairList[3].color = tColor;
            break;
        }
    }

    public void CheckPrefabVersionData()
    {
        if(Directory.Exists(unitPath))
        {
            Debug.Log("Now sync version data..");
            DirectoryInfo dirInfo = new DirectoryInfo(unitPath);
            FileInfo[] fileInf = dirInfo.GetFiles("*.prefab");
            foreach (FileInfo fileInfo in fileInf)
            {
                string path = unitPath + fileInfo.Name;
                GameObject prefab = AssetDatabase.LoadAssetAtPath(path, typeof(GameObject)) as GameObject;
                //데이터 싱크 부분
                SPUM_Prefabs tST = prefab.GetComponent<SPUM_Prefabs>();
                SPUM_SpriteList tObjST = tST._spriteOBj;
                _spriteObj.LoadSprite(tObjST);

                //UI연동.
                _colorButton[0].color = tObjST._eyeList[0].color;
                _colorButton[1].color = tObjST._hairList[3].color;
                _colorButton[2].color = tObjST._hairList[0].color;
                
                _spriteObj.ResyncData();
                _unitObjSet._version = _version;
                _unitObjSet._code = tST._code;

                bool _bodyDataCheck = false;
                for(var i = 0 ; i <tST._spriteOBj._bodyList.Count; i++)
                {
                    if(tST._spriteOBj._bodyList[i]==null)
                    {
                        _bodyDataCheck = true;
                    }
                }

                if(_bodyDataCheck || tST._spriteOBj._bodyString.Length < 1)
                {
                    _unitObjSet._spriteOBj._bodyList[0].sprite = _mainBodyList[5];
                    _unitObjSet._spriteOBj._bodyList[1].sprite = _mainBodyList[2];
                    _unitObjSet._spriteOBj._bodyList[2].sprite = _mainBodyList[0];
                    _unitObjSet._spriteOBj._bodyList[3].sprite = _mainBodyList[1];
                    _unitObjSet._spriteOBj._bodyList[4].sprite = _mainBodyList[3];
                    _unitObjSet._spriteOBj._bodyList[5].sprite = _mainBodyList[4];
                    _spriteObj._bodyTexture = _mainBody;
                    _spriteObj._bodyString = AssetDatabase.GetAssetPath(_mainBody);
                }

                GameObject tObj = PrefabUtility.SaveAsPrefabAsset(_unitObjSet.gameObject,unitPath+prefab.name+".prefab");
            }
        }

        Debug.Log("Now sync data process done...");
        SetInit();
    }

    public List<GameObject> _prefabUnitList = new List<GameObject>();
    public IEnumerator GetPrefabList()
    {
        if(Directory.Exists(unitPath))
        {

            DirectoryInfo dirInfo = new DirectoryInfo(unitPath);
            FileInfo[] fileInf = dirInfo.GetFiles("*.prefab");

            //loop through directory loading the game object and checking if it has the component you want
            _prefabUnitList.Clear();
            foreach (FileInfo fileInfo in fileInf)
            {
                string path = unitPath + fileInfo.Name;
                GameObject prefab = AssetDatabase.LoadAssetAtPath(path, typeof(GameObject)) as GameObject;
                //데이터 싱크 부분
                SPUM_Prefabs tST = prefab.GetComponent<SPUM_Prefabs>();
                if(tST._version == 0 || tST._version < _version  || tST._spriteOBj._bodyString.Length < 1)
                {
                    Debug.Log("Old Version data found.. Now sync version data..");
                    //이 경우는 데이터를 싱크해줘야한다.
                    SPUM_SpriteList tObjST = tST._spriteOBj;
                    _spriteObj.LoadSprite(tObjST);

                    //UI연동.
                    _colorButton[0].color = tObjST._eyeList[0].color;
                    _colorButton[1].color = tObjST._hairList[3].color;
                    _colorButton[2].color = tObjST._hairList[0].color;
                    
                    _spriteObj.ResyncData();
                    _unitObjSet._version = _version;
                    _unitObjSet._code = tST._code;

                    bool _bodyDataCheck = false;
                    for(var i = 0 ; i <tST._spriteOBj._bodyList.Count; i++)
                    {
                        if(tST._spriteOBj._bodyList[i]==null)
                        {
                            _bodyDataCheck = true;
                        }
                    }

                    if(_bodyDataCheck || tST._spriteOBj._bodyString.Length < 1)
                    {
                        _unitObjSet._spriteOBj._bodyList[0].sprite = _mainBodyList[5];
                        _unitObjSet._spriteOBj._bodyList[1].sprite = _mainBodyList[2];
                        _unitObjSet._spriteOBj._bodyList[2].sprite = _mainBodyList[0];
                        _unitObjSet._spriteOBj._bodyList[3].sprite = _mainBodyList[1];
                        _unitObjSet._spriteOBj._bodyList[4].sprite = _mainBodyList[3];
                        _unitObjSet._spriteOBj._bodyList[5].sprite = _mainBodyList[4];
                        _spriteObj._bodyTexture = _mainBody;
                        _spriteObj._bodyString = AssetDatabase.GetAssetPath(_mainBody);
                    }

                    GameObject tObj = PrefabUtility.SaveAsPrefabAsset(_unitObjSet.gameObject,unitPath+prefab.name+".prefab");
                    _prefabUnitList.Add(tObj);
                    yield return null;
                }
                else
                {
                    _prefabUnitList.Add(prefab);
                }
            }
        }
        else
        {
            yield return null;
        }

        
    }
    //프리팹 저장 부분
    public void SavePrefabs()
    {
        if(_prefabUnitList.Count < _maxNumber)
        {
            string prefabName = _unitCode.text;

            SPUM_Prefabs ttObjST = _unitObjSet.GetComponent<SPUM_Prefabs>();
            ttObjST._code = prefabName;
            ttObjST.EditChk = false;

            SPUM_SpriteList tSpST = ttObjST._spriteOBj;
            SyncPath(tSpST._hairList,tSpST._hairListString);
            SyncPath(tSpST._clothList,tSpST._clothListString);
            SyncPath(tSpST._armorList,tSpST._armorListString);
            SyncPath(tSpST._pantList,tSpST._pantListString);
            SyncPath(tSpST._weaponList,tSpST._weaponListString);
            SyncPath(tSpST._backList,tSpST._backListString);

            GameObject tObj = PrefabUtility.SaveAsPrefabAsset(_unitObjSet.gameObject,unitPath+prefabName+".prefab");

            _prefabUnitList.Add(tObj);

            ttObjST._code = "";
            ttObjST._version = _version;
            ttObjST.EditChk = true;

            ToastOn("Saved Unit Object " + prefabName);
            _unitCode.text = GetFileName();

            ShowNowUnitNumber();
            editObjNum = -1;
        }
    }

    public void ShowNowUnitNumber()
    {
        bool dirUnitChk = Directory.Exists("Assets/Resources/SPUM/SPUM_Units");
        if(dirUnitChk)
        {
            DirectoryInfo dirInfo = new DirectoryInfo(unitPath);
            FileInfo[] fileInfo = dirInfo.GetFiles("*.prefab");

            _unitNumber.text = fileInfo.Length + " / 100";
        }
    }

    public void SyncPath(List<SpriteRenderer> _objList, List<string> _pathList)
    {
        _pathList.Clear();
        for(var i = 0 ; i < _objList.Count ; i++)
        {
            if(_objList[i].sprite !=null) 
            {
                string gPath = AssetDatabase.GetAssetPath(_objList[i].sprite);
                _pathList.Add(gPath);
            }
            else
            {
                _pathList.Add("");
            }
        }
    }

    public int editObjNum;
    public void EditPrefabs()
    {
        if(editObjNum!=-1)
        {
            string prefabName = _unitCode.text;

            SPUM_Prefabs ttObjST = _unitObjSet.GetComponent<SPUM_Prefabs>();
            ttObjST._code = prefabName;
            ttObjST._version = _version;
            ttObjST.EditChk = false;

            GameObject tObj = PrefabUtility.SaveAsPrefabAsset(_unitObjSet.gameObject,unitPath+prefabName+".prefab");

            _prefabUnitList[editObjNum] = tObj;

            ttObjST._code = "";
            ttObjST.EditChk = true;

            ToastOn("Edited Unit Object " + prefabName);
            NewMake();
        }
    }

    public string GetFileName()
    {
        string tName ="Unit";
        int tNameNum = 0;
        List<string> _prefabNameList = new List<string>();
        for(var i = 0 ; i < _prefabUnitList.Count;i++)
        {
            _prefabNameList.Add(_prefabUnitList[i].name);
        }

        for(var i = 0; i < 10000; i++)
        {
            if(_prefabNameList.Contains(tName+i) == false)
            {
                tNameNum = i;
                break;
            }
        }
        
        tName = tName+tNameNum;
        return tName;
    }


    public void NewMake()
    {
        _unitCode.text = GetFileName();
        LoadButtonSet(false);
        SetInit();
    }

    public GameObject _loadObjCanvas;
    public Transform _loadPool;
    public List<GameObject> _buttonList = new List<GameObject>();

    public void OpenLoadData()
    {
        if(_prefabUnitList.Count == 0)
        {
            ToastOn("There is no any saved Unit");
            return;
        }

        if(_loadPool.childCount > 0)
        {
            for(var i=0; i < _loadPool.childCount;i++)
            {
                Destroy(_loadPool.GetChild(i).gameObject);
            }
        }
        

        for ( var j = 0 ; j < _prefabUnitList.Count ; j++)
        {
            GameObject tObj = _prefabUnitList[j];
            SPUM_SpriteList tObjST = tObj.GetComponent<SPUM_Prefabs>()._spriteOBj;

            GameObject ttObj = Instantiate(_childItem) as GameObject;
            ttObj.transform.SetParent(_loadPool);
            ttObj.transform.localScale = new Vector3(1,1,1);

            SPUM_PreviewItem ttObjST = ttObj.GetComponent<SPUM_PreviewItem>();
            ttObjST.ShowObj(5);
            //아이템 연동 부분
            ttObjST._fullSetList[0].sprite = tObjST._bodyList[0].sprite;
            ttObjST._fullSetList[1].sprite = tObjST._bodyList[1].sprite;
            ttObjST._fullSetList[2].sprite = tObjST._bodyList[2].sprite;
            ttObjST._fullSetList[3].sprite = tObjST._bodyList[3].sprite;
            ttObjST._fullSetList[4].sprite = tObjST._bodyList[4].sprite;
            ttObjST._fullSetList[5].sprite = tObjST._bodyList[5].sprite;

            ttObjST._fullSetList[6].sprite = tObjST._eyeList[0].sprite;
            ttObjST._fullSetList[7].sprite = tObjST._eyeList[0].sprite;

            ttObjST._fullSetList[8].sprite = tObjST._hairList[3].sprite;
            ttObjST._fullSetList[9].sprite = tObjST._hairList[0].sprite;
            ttObjST._fullSetList[10].sprite = tObjST._hairList[1].sprite;

            ttObjST._fullSetList[11].sprite = tObjST._clothList[0].sprite;
            ttObjST._fullSetList[12].sprite = tObjST._clothList[1].sprite;
            ttObjST._fullSetList[13].sprite = tObjST._clothList[2].sprite;

            ttObjST._fullSetList[14].sprite = tObjST._armorList[0].sprite;
            ttObjST._fullSetList[15].sprite = tObjST._armorList[1].sprite;
            ttObjST._fullSetList[16].sprite = tObjST._armorList[2].sprite;

            ttObjST._fullSetList[17].sprite = tObjST._pantList[0].sprite;
            ttObjST._fullSetList[18].sprite = tObjST._pantList[1].sprite;

            ttObjST._fullSetList[19].sprite = tObjST._weaponList[0].sprite;
            ttObjST._fullSetList[20].sprite = tObjST._weaponList[1].sprite;

            ttObjST._fullSetList[21].sprite = tObjST._weaponList[2].sprite;
            ttObjST._fullSetList[22].sprite = tObjST._weaponList[3].sprite;

            ttObjST._fullSetList[23].sprite = tObjST._backList[0].sprite;

            //string 연동

            
            //색연동
            if(!tObjST._eyeList[0].gameObject.activeInHierarchy) 
            {
                ttObjST._fullSetList[6].gameObject.SetActive(true);
                ttObjST._fullSetList[7].gameObject.SetActive(true);

                ttObjST._fullSetList[6].color = tObjST._eyeList[0].color;
                ttObjST._fullSetList[7].color = tObjST._eyeList[0].color;
            }
            else
            {
                ttObjST._fullSetList[6].gameObject.SetActive(false);
                ttObjST._fullSetList[7].gameObject.SetActive(false);
            }

            if(!tObjST._hairList[3].gameObject.activeInHierarchy)
            {
                ttObjST._fullSetList[8].gameObject.SetActive(true);
                ttObjST._fullSetList[8].color = tObjST._hairList[3].color;
            }
            else
            {
                ttObjST._fullSetList[8].gameObject.SetActive(false);
            }

            if(!tObjST._hairList[0].gameObject.activeInHierarchy)
            {
                ttObjST._fullSetList[9].gameObject.SetActive(true);
                ttObjST._fullSetList[9].color = tObjST._hairList[0].color;
            }
            else
            {
                ttObjST._fullSetList[9].gameObject.SetActive(true);
            }

            for(var i = 0 ; i < ttObjST._fullSetList.Count; i++)
            {
                ttObjST._fullSetList[i].SetNativeSize();
                if(ttObjST._fullSetList[i].sprite != null)
                {
                    ttObjST._fullSetList[i].gameObject.SetActive(true);
                    ttObjST._fullSetList[i].rectTransform.pivot =  new Vector2(ttObjST._fullSetList[i].sprite.pivot.x/ttObjST._fullSetList[i].sprite.rect.width,ttObjST._fullSetList[i].sprite.pivot.y/ttObjST._fullSetList[i].sprite.rect.height);
                }
                else ttObjST._fullSetList[i].gameObject.SetActive(false);
            }
            
            ttObjST._name = _prefabUnitList[j].name;
            ttObjST._managerST = this;
            ttObjST._itemType = 11;
            ttObjST._index = j;
            ttObjST.DeleteButton.SetActive(true);
        }
        _loadObjCanvas.SetActive(true);
    }

    public void CloseLoadData()
    {
        _loadObjCanvas.SetActive(false);
    }

    public void LoadButtonSet(bool value)
    {
        _buttonList[0].SetActive(!value);
        _buttonList[1].SetActive(value);
    }

    public void LoadUnitDataName(int index)
    {
        SPUM_Prefabs tPrefabST = _prefabUnitList[index].GetComponent<SPUM_Prefabs>();
        SPUM_SpriteList tObjST = tPrefabST._spriteOBj;
        _spriteObj.LoadSprite(tObjST);

        //UI연동.
        _colorButton[0].color = tObjST._eyeList[0].color;
        _colorButton[1].color = tObjST._hairList[3].color;
        _colorButton[2].color = tObjST._hairList[0].color;
        
        _unitCode.text = (_prefabUnitList[index].name).Split('.')[0];

        //데이터 유효성 체크
        if(tPrefabST._version == 0 || tPrefabST._version < _version)
        {
            //버젼이 존재하지 않거나 없으면 Resync 실행
            tObjST.ResyncData();
            tPrefabST._version = _version;
            Debug.Log(unitPath+_prefabUnitList[index].name+".prefab");
            // GameObject tObj = PrefabUtility.SaveAsPrefabAsset(_unitObjSet.gameObject,unitPath+_prefabUnitList[index].name+".prefab");
            
        }
    }

    //Unit Delete
    public void DeleteUnit(int index)
    {
        GameObject tObj = _prefabUnitList[index];
        string pathToDelete = AssetDatabase.GetAssetPath(_prefabUnitList[index]);      
        _prefabUnitList.Remove(tObj);
        AssetDatabase.DeleteAsset(pathToDelete);

        ShowNowUnitNumber();
        CloseLoadData();
        OpenLoadData();
    }
    

    bool EmptyChk()
    {
        bool value = false;
        if(Random.Range(0,1.0f) < 0.2f) 
        {
            value = true;
        }
        return value;
    }

    public CanvasGroup _toastObj;
    public Text _toastMSG;
    bool toastChk = false;
    float toastTimer = 0;
    public void ToastOn(string Text)
    {
        _toastObj.gameObject.SetActive(true);
        _toastObj.alpha = 1.0f;
        _toastMSG.text = Text;
        toastChk = true;
        toastTimer = 0;
    }

    void Update()
    {
        if(toastChk)
        {
            toastTimer += Time.deltaTime;
            if(toastTimer > 2.0f) _toastObj.alpha = 1.0f - (toastTimer-2f);
            if(toastTimer > 3.0f) CloseToast();
        }
    }

    void CloseToast()
    {
        toastChk = false;
        toastTimer = 0;
        _toastObj.gameObject.SetActive(false);
    }

    public GameObject _noticeObj;
    public Text _noticeText;
    public List<GameObject> _buttonSet = new List<GameObject>();
    public int callbackNum = 0;

    public void OnNotice(string text,int type = 0, int callback = -1)
    {
        _noticeObj.SetActive(true);
        _noticeText.text = text;
        callbackNum = callback;

        if(type == 0 ) //버튼 사용 선택
        {
            _buttonSet[0].SetActive(true);
            _buttonSet[1].SetActive(false);
        }
        else
        {
            _buttonSet[0].SetActive(false);
            _buttonSet[1].SetActive(true);
        }
    }

    public void CloseNotice()
    {
        if(callbackNum!=1)CloseOnlyNotice();
        switch(callbackNum)
        {
            case 0:
            break;

            case 1:
            Debug.Log("Please Check Error Message");
            break;
        }
    }

    public void CloseOnlyNotice()
    {
        _noticeObj.SetActive(false);
    }

    //인스톨 관련
    
    public void InstallSpriteData()
    {
        bool Chk = false;
        //기본 폴더 제작
        if(Directory.Exists("Assets/Resources/SPUM/SPUM_Sprites/Items"))
        {
            Debug.Log("Found Resources Folder Success!!");
            if(Directory.Exists("Assets/Resources/SPUM/SPUM_Sprites/Items"))
            {
                Debug.Log("Found SPUM_Sprite Folder, will delete it");
                FileUtil.DeleteFileOrDirectory("Assets/Resources/SPUM/SPUM_Sprites/Items");
            }
        }
        else
        {
            Debug.Log("Project doesn't have Resources Folder Yet, Will make Resource Project Automatically");
            Directory.CreateDirectory("Assets/Resources/SPUM/SPUM_Sprites/");
        }

        //패키지 데이터 제작
        if(Directory.Exists("Assets/Resources/SPUM/SPUM_Sprites/Packages"))
        {
            Debug.Log("Found Package Folder Success!!");
            if(Directory.Exists("Assets/Resources/SPUM/SPUM_Sprites/Packages"))
            {
                Debug.Log("Found SPUM_Sprite Folder, will delete it");
                FileUtil.DeleteFileOrDirectory("Assets/Resources/SPUM/SPUM_Sprites/Packages");
            }
        }

        //어셋 카피

        if(AssetDatabase.CopyAsset("Assets/SPUM/SPUM_Sprites/Items","Assets/Resources/SPUM/SPUM_Sprites/Items"))
        {
            Debug.Log("Install SPUM Sprtie Data Success in Resources Folder");

            if(!Directory.Exists("Assets/Resources/SPUM/SPUM_UNITS"))
            {
                Directory.CreateDirectory("Assets/Resources/SPUM/SPUM_Units");
            }
        }
        else
        {
            Debug.Log("Copy Failed");
        }

        if(AssetDatabase.CopyAsset("Assets/SPUM/SPUM_Sprites/Packages","Assets/Resources/SPUM/SPUM_Sprites/Packages"))
        {
            Debug.Log("Install SPUM Sprtie Packages Data Success in Resources Folder");
        }
        else
        {
            Debug.Log("Copy Failed");
        }
    }

    public void SetBodySprite()
    {
        string spritePath = AssetDatabase.GetAssetPath( _mainBody );
        Object[] sprites = AssetDatabase.LoadAllAssetsAtPath(spritePath);
        var sortedList = sprites.OrderBy(go=>go.name).ToList();
        List<Sprite> tSP = new List<Sprite>();
        for(var i = 0 ; i < sortedList.Count;i++)
        {
            if(sortedList[i].GetType() == typeof(Sprite))
            {
                tSP.Add((Sprite)sortedList[i]);
            }
        }

        // for(var i = 0 ; i < tSP.Count;i++) Debug.Log(tSP[i]);
        // Debug.Log(tSP.Count);
        _unitObjSet._spriteOBj._bodyList[0].sprite = tSP[5];
        _unitObjSet._spriteOBj._bodyList[1].sprite = tSP[2];
        _unitObjSet._spriteOBj._bodyList[2].sprite = tSP[0];
        _unitObjSet._spriteOBj._bodyList[3].sprite = tSP[1];
        _unitObjSet._spriteOBj._bodyList[4].sprite = tSP[3];
        _unitObjSet._spriteOBj._bodyList[5].sprite = tSP[4];
    }
    #endif
}
