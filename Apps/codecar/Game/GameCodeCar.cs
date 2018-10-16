using System.Collections;
using System.Collections.Generic;
using LitJson;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CodeCarItemInfo : ItemInfo
{
    public List<MapTileInfo> listTile;
}
public class GameCodeCar : UIView, IUIMapItemDelegate
{
    public MeshRoad meshRoad;
    public UICmdBarRun uiCmdBarRun;
    UICar uiCar;
    CodeCarItemInfo itmeInfoGuanka;//当前关卡

    float rootPosZ = 0;
    int mapSizeX;
    int mapSizeY;
    Vector2 sizeRect;
    float topBarHeightCanvas = 160;
    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {

    }

    void Start()
    {

        LayOut();


    }

    public override void LayOut()
    {


        float x, y, w, h;


        RectTransform rctran = this.transform.GetComponent<RectTransform>();
        float ofty = Common.CanvasToWorldHeight(mainCam, AppSceneBase.main.sizeCanvas, topBarHeightCanvas);
        rctran.offsetMin = new Vector2(0, 0);
        rctran.offsetMax = new Vector2(0, -ofty);
    }
    public void InitValue()
    {
        RectTransform rctran = this.transform.GetComponent<RectTransform>();
        float ofty = Common.CanvasToWorldHeight(mainCam, AppSceneBase.main.sizeCanvas, topBarHeightCanvas);
        sizeRect = new Vector2(rctran.rect.width, rctran.rect.height - ofty);
        Debug.Log("sizeRect=" + sizeRect + " ofty=" + ofty);
    }

    void ResizeMainRect()
    {
        float ratio = mapSizeX * 1.0f / mapSizeY;
        float w = sizeRect.x;
        float h = w / ratio;
        if (h > sizeRect.y)
        {
            h = sizeRect.y;
            w = h * ratio;
        }
        sizeRect.x = w;
        sizeRect.y = h;
        meshRoad.roadWidth = w / mapSizeX;

        Debug.Log("ResizeMainRect :sizeRect=" + sizeRect + " mapSizeX=" + mapSizeX + " mapSizeY=" + mapSizeY);
    }
    string GetImageOfItem(MapTileInfo info)
    {
        string pic = "";
        switch (info.type)
        {
            case UIMapItem.ItemType.FLAG:
                pic = ResMap.IMAGE_FLAG;
                break;
            case UIMapItem.ItemType.TREE:
                pic = ResMap.IMAGE_TREE;
                break;
            case UIMapItem.ItemType.CAR:
                pic = ResMap.IMAGE_CAR;
                break;
            case UIMapItem.ItemType.ROAD_TILE:
                pic = ResMap.IMAGE_TILE;
                break;
        }
        return pic;
    }


    void CreateMapItem(MapTileInfo info)
    {
        Vector3 posworld = MapUtils.GetPositionCenter(info.tileX, info.tileY, mapSizeX, mapSizeY, sizeRect);
        posworld.z = rootPosZ - 1;
        float w, h;
        w = meshRoad.roadWidth;
        h = w;
        // Vector2 posworld = mainCam.ScreenToWorldPoint(pos);

        if (info.type == UIMapItem.ItemType.ROAD_TILE)
        {
            meshRoad.AddPoint(posworld);
        }

        if ((info.type == UIMapItem.ItemType.FLAG) || (info.type == UIMapItem.ItemType.TREE) || (info.type == UIMapItem.ItemType.CAR))
        {
            GameObject obj = new GameObject("item_" + info.type.ToString());
            //AppSceneBase.main.AddObjToMainWorld(obj);
            obj.transform.parent = this.gameObject.transform;
            obj.transform.localScale = new Vector3(1f, 1f, 1f);
            obj.transform.localPosition = new Vector3(posworld.x, posworld.y, rootPosZ - 1);
            if (info.type == UIMapItem.ItemType.CAR)
            {
                uiCar = obj.AddComponent<UICar>();
                uiCar.type = info.type;
                uiCar.iDelegate = this;
                uiCar.sizeRect = sizeRect;
                uiCar.uiCmdBarRun = uiCmdBarRun;
                uiCar.SetMapSize(mapSizeX, mapSizeY);
                uiCar.UpdateGuankaItem(itmeInfoGuanka);
                uiCar.UpdateItem(w, h);
            }
            else
            {
                UIMapItem item = obj.AddComponent<UIMapItem>();
                item.type = info.type;
                item.enableMove = false;
                item.UpdateItem(w, h);
            }

            //
        }

    }


    string GetIdJson(JsonData data, string key)
    {
        string ret = "";
        JsonData idleft = data[key];
        if (idleft != null)
        {
            ret = (string)idleft;
        }
        return ret;
    }
    public void ParseMapInfo(CodeCarItemInfo info)
    {
        itmeInfoGuanka = info;
        string fileName = Common.GAME_RES_DIR + "/guanka/place" + GameManager.placeLevel + "/guanka_" + GameManager.gameLevel + ".json";
        //FILE_PATH
        string json = FileUtil.ReadStringAsset(fileName);//((TextAsset)Resources.Load(fileName, typeof(TextAsset))).text;
        // Debug.Log("json::"+json);
        JsonData root = JsonMapper.ToObject(json);
        string strPlace = (string)root["place"];
        mapSizeX = Common.String2Int((string)root["mapSizeX"]);
        mapSizeY = Common.String2Int((string)root["mapSizeY"]);
        ResizeMainRect();
        JsonData items = root["item"];

        info.listTile = new List<MapTileInfo>();
        bool iscreate_car = false;
        for (int i = 0; i < items.Count; i++)
        {
            JsonData item = items[i];
            MapTileInfo infotile = new MapTileInfo();
            string type = (string)item["itemtype"];//ROAD_TILE
                                                   // string strpos =//(573.0, 200.0)
            infotile.type = UIMapItem.String2Type(type);
            infotile.id = GetIdJson(item, "id");
            infotile.idLeft = GetIdJson(item, "idLeft");
            infotile.idRight = GetIdJson(item, "idRight");
            infotile.idTop = GetIdJson(item, "idTop");
            infotile.idBottom = GetIdJson(item, "idBottom");

            infotile.tileX = (int)item["tileX"];
            infotile.tileY = (int)item["tileY"];

            info.listTile.Add(infotile);
            CreateMapItem(infotile);

            if ((!iscreate_car) && (infotile.type == UIMapItem.ItemType.ROAD_TILE))
            {
                //   Vector2 posworld = mainCam.ScreenToWorldPoint(infotile.pos);
                //car
                MapTileInfo infocar = new MapTileInfo();
                infocar.type = UIMapItem.ItemType.CAR;
                infocar.tileX = infotile.tileX;
                infocar.tileY = infotile.tileY;
                // infocar.pos = infotile.pos;
                CreateMapItem(infocar);
                iscreate_car = true;

            }

        }


        meshRoad.Draw();
    }


    public void OnTileItemTouchDown(UIMapItem item, PointerEventData eventData)
    {

    }
    public void OnTileItemTouchMove(UIMapItem item, PointerEventData eventData)
    {

    }
    public void OnTileItemTouchUp(UIMapItem item, PointerEventData eventData)
    {
        Debug.Log("GameCodeCar::OnTileItemTouchUp ");
        if (item == uiCar)
        {
            uiCar.OnRun();
        }

    }


}

