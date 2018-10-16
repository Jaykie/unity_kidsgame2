using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;
using LitJson;
using System.Text;
//瓦片地图
public class MapTileInfo : MapJsonItemInfo
{
    public const string ID_PREFIX_ROAD = "id_road_"; //前缀
    public const string ID_PREFIX_TREE = "id_tree_";
    public GameObject obj;
    public UIMapItem.ItemType type;


}

public class MapJsonItemInfo
{
    public string itemtype;
    //public string pos;//screen (x,y)
    public string id;
    public string idLeft;
    public string idRight;
    public string idTop;
    public string idBottom;

    public bool isCorner;//拐点

    //瓦片坐标
    public int tileX;
    public int tileY;
}

public class UIMapEditor : UIView, IUIMapItemDelegate
{
    public const int SIDE_TYPE_LEFT = 0;
    public const int SIDE_TYPE_RIGHT = 1;
    public const int SIDE_TYPE_TOP = 2;
    public const int SIDE_TYPE_BOTTOM = 3;
    public Vector2 sizeRect;
    public Image imageBg;
    public MeshRoad meshRoad;
    GameObject objMap;
    UIMapGrid uiGrid;
    int indextmp;
    List<object> listItem;
    List<object> listTile;//道路瓦片
                          //瓦片大小
    int mapSizeX;//
    int mapSizeY;//
    float rootPosZ = 0;

    MapTileInfo tileInfoNow;

    int indexRoadTile;
    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        listItem = new List<object>();
        listTile = new List<object>();
        InitValue();
        AppSceneBase.main.ClearMainWorld();
        AppSceneBase.main.UpdateWorldBg(AppRes.IMAGE_GAME_BG);
        indexRoadTile = 0;


        objMap = new GameObject("Map");
        AppSceneBase.main.AddObjToMainWorld(objMap);
        objMap.transform.localPosition = Vector3.zero;

        GameObject obj = new GameObject("BgGrid");
        AddObjToMap(obj);
        obj.transform.localPosition = Vector3.zero;
        uiGrid = obj.AddComponent<UIMapGrid>();


        AddObjToMap(meshRoad.gameObject);
        meshRoad.gameObject.transform.localPosition = Vector3.zero;


        mapSizeY = 4;
        mapSizeX = 5;
        ResizeMainRect();

        uiGrid.mapSizeX = mapSizeX;
        uiGrid.mapSizeY = mapSizeY;
        uiGrid.sizeRect = sizeRect;


    }
    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {
        LayOut();
    }

    void OnDestroy()
    {
        AppSceneBase.main.ClearMainWorld();
    }
    public override void LayOut()
    {
        RectTransform rctran = AppSceneBase.main.objMainWorld.transform.GetComponent<RectTransform>();
        float x, y, w, h;

        w = rctran.rect.width;
        h = rctran.rect.height;
        x = 0;
        y = -(h - sizeRect.y) / 2;
        objMap.transform.localPosition = new Vector3(x, y, 0);

    }
    public void InitValue()
    {

        RectTransform rctran = AppSceneBase.main.objMainWorld.transform.GetComponent<RectTransform>();
        float ofty = Common.CanvasToWorldHeight(mainCam, AppSceneBase.main.sizeCanvas, 160);
        sizeRect = new Vector2(rctran.rect.width, rctran.rect.height - ofty);//Common.GetWorldSize(mainCam);
        Debug.Log("sizeRect=" + sizeRect);
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

    public void AddObjToMap(GameObject obj)
    {
        obj.transform.parent = objMap.transform;
        obj.transform.localScale = new Vector3(1f, 1f, 1f);
    }
    MapTileInfo CreateMapItem(UIMapItem.ItemType type)
    {
        MapTileInfo info = new MapTileInfo();
        GameObject obj = new GameObject("item_" + type.ToString());
        UIMapItem item = null;
        item = obj.AddComponent<UIMapItem>();

        AddObjToMap(obj);
        info.obj = obj;
        info.type = type;


        item.type = type;
        item.iDelegate = this;
        item.info = info;

        obj.transform.localPosition = new Vector3(0, 0, rootPosZ - 1);
        float w,h;
        w = meshRoad.roadWidth;
        h = meshRoad.roadWidth;
        item.UpdateItem(w,h);
        return info;
    }


    public void OnTileItemTouchDown(UIMapItem item, PointerEventData eventData)
    {

    }
    public void OnTileItemTouchMove(UIMapItem item, PointerEventData eventData)
    {

    }
    public void OnTileItemTouchUp(UIMapItem item, PointerEventData eventData)
    {
        if (tileInfoNow == null)
        {
            // return;
        }
        int x = MapUtils.GetXOfPositionCenter(item.transform.localPosition, mapSizeX, mapSizeY, sizeRect);
        int y = MapUtils.GetYOfPositionCenter(item.transform.localPosition, mapSizeX, mapSizeY, sizeRect);
        item.info.tileX = x;
        item.info.tileY = y;
        Vector3 pos = MapUtils.GetPositionCenter(x, y, mapSizeX, mapSizeY, sizeRect);
        pos.z = rootPosZ - 1;
        item.gameObject.transform.localPosition = pos;
        Vector2 pos_screen = mainCam.WorldToScreenPoint(pos);
        //item.info.pos = pos_screen;//(-2.4, -1.0) 
        Debug.Log("OnTileItemTouchUp x=" + x + " y=" + y + " pos=" + pos);
        if (item.type == UIMapItem.ItemType.ROAD_TILE)
        {
            item.info.id = MapTileInfo.ID_PREFIX_ROAD + indexRoadTile.ToString();
            indexRoadTile++;
            meshRoad.AddPoint(pos);
            meshRoad.Draw();
            GameObject obj = item.gameObject;
            DestroyImmediate(obj);
            obj = null;
        }

    }


    MapTileInfo GetItemById(string id)
    {
        MapTileInfo ret = null;
        foreach (MapTileInfo infotmp in listTile)
        {
            if (infotmp.id == id)
            {
                ret = infotmp;
                break;
            }
        }
        return ret;
    }
    //相邻
    MapTileInfo GetItemSide(MapTileInfo info, int type)
    {
        MapTileInfo ret = null;
        int stepx = 0;
        int stepy = 0;
        foreach (MapTileInfo infotmp in listTile)
        {
            stepx = infotmp.tileX - info.tileX;
            stepy = infotmp.tileY - info.tileY;
            switch (type)
            {
                case SIDE_TYPE_LEFT:
                    {
                        if ((stepx == -1) && (stepy == 0))
                        {
                            ret = infotmp;
                            return ret;
                        }
                    }
                    break;

                case SIDE_TYPE_RIGHT:
                    {
                        if ((stepx == 1) && (stepy == 0))
                        {
                            ret = infotmp;
                            return ret;
                        }
                    }
                    break;

                case SIDE_TYPE_TOP:
                    {
                        if ((stepx == 0) && (stepy == 1))
                        {
                            ret = infotmp;
                            return ret;
                        }
                    }
                    break;

                case SIDE_TYPE_BOTTOM:
                    {
                        if ((stepx == 0) && (stepy == -1))
                        {
                            ret = infotmp;
                            return ret;
                        }
                    }
                    break;

            }
        }
        return ret;
    }
    //计算地图拐点等
    void FormatMapData()
    {

        foreach (MapTileInfo info in listTile)
        {
            //left
            {
                MapTileInfo infoside = GetItemSide(info, SIDE_TYPE_LEFT);
                if (infoside != null)
                {
                    info.idLeft = infoside.id;
                }
            }

            //right
            {
                MapTileInfo infoside = GetItemSide(info, SIDE_TYPE_RIGHT);
                if (infoside != null)
                {
                    info.idRight = infoside.id;
                }
            }

            //top
            {
                MapTileInfo infoside = GetItemSide(info, SIDE_TYPE_TOP);
                if (infoside != null)
                {
                    info.idTop = infoside.id;
                }
            }
            //bottom
            {
                MapTileInfo infoside = GetItemSide(info, SIDE_TYPE_BOTTOM);
                if (infoside != null)
                {
                    info.idBottom = infoside.id;
                }
            }

        }
    }
    void SaveGuankaJsonFile()
    {
        FormatMapData();
        string strPlace = "place0";
        string filepath = Application.streamingAssetsPath + "/" + ResMap.JSON_MAP;
        Debug.Log(filepath);
        //创建文件夹
        FileUtil.CreateDir(FileUtil.GetFileDir(filepath));

        List<MapJsonItemInfo> listGuankaJson = new List<MapJsonItemInfo>();

        foreach (MapTileInfo mapinfo in listTile)
        {
            MapJsonItemInfo info = new MapJsonItemInfo();
            info.itemtype = mapinfo.type.ToString();

            info.tileX = mapinfo.tileX;
            info.tileY = mapinfo.tileY;
            info.isCorner = mapinfo.isCorner;
            info.id = mapinfo.id;
            info.idLeft = mapinfo.idLeft;
            info.idRight = mapinfo.idRight;
            info.idTop = mapinfo.idTop;
            info.idBottom = mapinfo.idBottom;

            //info.pos = pos.ToString();//(-2.4, -1.0)
            listGuankaJson.Add(info);
        }


        //save guanka json
        {

            Hashtable data = new Hashtable();
            data["place"] = strPlace;
            data["mapSizeX"] = mapSizeX.ToString();
            data["mapSizeY"] = mapSizeY.ToString();
            data["item"] = listGuankaJson;
            string strJson = JsonMapper.ToJson(data);
            byte[] bytes = Encoding.UTF8.GetBytes(strJson);
            System.IO.File.WriteAllBytes(filepath, bytes);
        }

        Debug.Log("CreateGuankaJsonFile Finished");

    }
    public void OnClickBtnBack()
    {
        // PopViewController pop = (PopViewController)this.controller;
        // if (pop != null)
        // {
        //     pop.Close();
        // }

        NaviViewController navi = this.controller.naviController;
        if (navi != null)
        {
            navi.Pop();
        }
    }

    public void OnClickBtnTile()
    {
        tileInfoNow = CreateMapItem(UIMapItem.ItemType.ROAD_TILE);
        listTile.Add(tileInfoNow);
    }

    public void OnClickBtnEndFlag()
    {
        MapTileInfo info = CreateMapItem(UIMapItem.ItemType.FLAG);
        listTile.Add(info);
    }

    public void OnClickBtnTree()
    {
        MapTileInfo info = CreateMapItem(UIMapItem.ItemType.TREE);
        listTile.Add(info);
    }
    public void OnClickBtnSave()
    {
        SaveGuankaJsonFile();
    }
    public void OnClickBtnRun()
    {
        if (this.controller != null)
        {
            NaviViewController navi = this.controller.naviController;
            navi.Push(GameViewController.main);
        }
    }
}


