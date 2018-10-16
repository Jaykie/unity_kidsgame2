using System.Collections;
using System.Collections.Generic;
using LitJson;
using UnityEngine;
using UnityEngine.UI;
public class UIGameCodeCar : UIGameBase
{
    GameCodeCar gameCodeCarPrefab;
    GameCodeCar gameCodeCar;

    public UICmdBarSelect uiCmdBarSelect;
    public UICmdBarRun uiCmdBarRun;

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        LoadPrefab();
        uiCmdBarSelect.uiCmdBarRun = uiCmdBarRun;
        uiCmdBarRun.uiCmdBarSelect = uiCmdBarSelect;
        AppSceneBase.main.UpdateWorldBg(AppRes.IMAGE_GAME_BG);

        gameCodeCar = (GameCodeCar)GameObject.Instantiate(gameCodeCarPrefab);

        AppSceneBase.main.AddObjToMainWorld(gameCodeCar.gameObject);

        //必须在设置transform.parent之后重置offsetMin和offsetMax
        RectTransform rctran = gameCodeCar.GetComponent<RectTransform>();
        rctran.offsetMin = new Vector2(0, 0);
        rctran.offsetMax = new Vector2(0, 0);
        gameCodeCar.transform.localPosition = Vector3.zero;
        gameCodeCar.uiCmdBarRun = uiCmdBarRun;

    }

    void Start()
    {
        //在objMainWorld recttran设置完成后初始化
        gameCodeCar.InitValue();
        ParseGuanka();
        UpdateGuankaLevel(GameManager.gameLevel);
        LayOut();
    }

    public override void LayOut()
    {

        float x, y, w, h;

        LayoutChildBase();
        {
            RectTransform rctran = uiCmdBarSelect.GetComponent<RectTransform>();
            w = rctran.sizeDelta.x;
            h = this.frame.height;
            rctran.sizeDelta = new Vector2(w, h);
        }


    }

    void LoadPrefab()
    {
        {
            GameObject obj = PrefabCache.main.Load("App/Prefab/Game/GameCodeCar");
            gameCodeCarPrefab = obj.GetComponent<GameCodeCar>();
            Debug.Log("UIGameCodeCar LoadPrefab");
            if (gameCodeCarPrefab == null)
            {
                Debug.Log("UIGameCodeCar LoadPrefab gameCodeCarPrefab is null");
            }
        }
    }
    public override void UpdateGuankaLevel(int level)
    {
        CodeCarItemInfo info = GetGuankaItemInfo(GameManager.gameLevel) as CodeCarItemInfo;
        gameCodeCar.ParseMapInfo(info);
        InitCmdRun();
    }
    public override int GetGuankaTotal()
    {
        ParseGuanka();
        if (listGuanka != null)
        {
            return listGuanka.Count;
        }
        return 0;
    }

    public override void CleanGuankaList()
    {
        if (listGuanka != null)
        {
            listGuanka.Clear();
        }
    }

    public override int ParseGuanka()
    {
        int count = 0;

        if ((listGuanka != null) && (listGuanka.Count != 0))
        {
            return listGuanka.Count;
        }

        listGuanka = new List<object>();
        int idx = GameManager.placeLevel;
        string fileName = Common.GAME_RES_DIR + "/guanka/place" + idx + "/guanka_" + idx + ".json";
        //FILE_PATH
        string json = FileUtil.ReadStringAsset(fileName);//((TextAsset)Resources.Load(fileName, typeof(TextAsset))).text;
        // Debug.Log("json::"+json);
        JsonData root = JsonMapper.ToObject(json);
        string strPlace = (string)root["place"];
        JsonData items = root["item"];
        for (int i = 0; i < 100; i++)
        {
            CodeCarItemInfo info = new CodeCarItemInfo();

            string picdir = Common.GAME_RES_DIR + "/image/" + strPlace;
            info.pic = picdir + "/" + info.id + ".png";
            info.source = "png";
            if (!FileUtil.FileIsExistAsset(info.pic))
            {
                info.source = "jpg";
                info.pic = picdir + "/" + info.id + ".jpg";
            }

            listGuanka.Add(info);
        }

        count = listGuanka.Count;

        Debug.Log("ParseGame::count=" + count);
        return count;
    }

    void InitCmdRun()
    {
        int total = 5;
        for (int i = 0; i < total; i++)
        {
            uiCmdBarRun.AddItem(UICmdItem.CmdType.NONE);
        }

    }

}

