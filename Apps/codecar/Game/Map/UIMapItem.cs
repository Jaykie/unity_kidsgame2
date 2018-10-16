
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public interface IUIMapItemDelegate
{
    void OnTileItemTouchDown(UIMapItem item, PointerEventData eventData);
    void OnTileItemTouchMove(UIMapItem item, PointerEventData eventData);
    void OnTileItemTouchUp(UIMapItem item, PointerEventData eventData);
}

public class UIMapItem : UIView, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    public enum ItemType
    {
        ROAD_TILE = 0,
        FLAG,
        TREE,
        CAR

    }
    public IUIMapItemDelegate iDelegate;
    public int index;
    public ItemType type;
    public MapTileInfo info;
    public Vector2 posTouchDown;//　transform.position　canvase ui 的屏幕坐标 
    public bool enableMove;

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    public void Awake()
    {
        Init();

    }
    public void UpdateItem(float w, float h)
    {
        SpriteRenderer rd = gameObject.AddComponent<SpriteRenderer>();
        string pic = "";
        switch (type)
        {
            case ItemType.ROAD_TILE:
                pic = ResMap.IMAGE_TILE;
                break;
            case ItemType.FLAG:
                pic = ResMap.IMAGE_FLAG;
                break;
            case ItemType.TREE:
                pic = ResMap.IMAGE_TREE;
                break;
            case ItemType.CAR:
                pic = ResMap.IMAGE_CAR;
                break;
            default:
                pic = ResMap.IMAGE_TILE;
                break;

        }
        Texture2D tex = TextureCache.main.Load(pic);
        rd.sprite = LoadTexture.CreateSprieFromTex(tex);
        BoxCollider box = gameObject.AddComponent<BoxCollider>();
        box.size = new Vector2(tex.width / 100f, tex.height / 100f);

        if (type != ItemType.ROAD_TILE)
        {
            float scale = Common.GetBestFitScale(tex.width / 100f, tex.height / 100f, w, h);
            this.gameObject.transform.localScale = new Vector3(scale, scale, 1f);
        }
    }

    void Init()
    {
        enableMove = true;
    }

    static public ItemType String2Type(string str)
    {
        ItemType ty;
        if (str == ItemType.ROAD_TILE.ToString())
        {
            ty = ItemType.ROAD_TILE;
        }
        else if (str == ItemType.FLAG.ToString())
        {
            ty = ItemType.FLAG;
        }
        else if (str == ItemType.TREE.ToString())
        {
            ty = ItemType.TREE;
        }
        else
        {
            ty = ItemType.ROAD_TILE;
        }

        return ty;
    }

    //相当于touchDown
    public void OnPointerDown(PointerEventData eventData)
    {

        Debug.Log("OnPointerDown,cmditem");
        posTouchDown = this.gameObject.transform.position;
        //置顶显示
        // this.gameObject.transform.SetAsLastSibling();
        if (iDelegate != null)
        {
            iDelegate.OnTileItemTouchDown(this, eventData);
        }

    }
    //相当于touchUp
    public void OnPointerUp(PointerEventData eventData)
    {
        Debug.Log("UIMapItem::OnTileItemTouchUp ");
        if (iDelegate != null)
        {
            iDelegate.OnTileItemTouchUp(this, eventData);
        }
    }
    //相当于touchMove
    public void OnDrag(PointerEventData eventData)
    {
        //Debug.Log("OnDrag:eventData.position=" + eventData.position + "  eventData.worldPosition=" + eventData.pointerPressRaycast.worldNormal);
        if (enableMove)
        {
            Vector3 pos = mainCam.ScreenToWorldPoint(eventData.position);
            pos.z = this.gameObject.transform.localPosition.z;
            this.gameObject.transform.localPosition = pos;
        }

        if (iDelegate != null)
        {
            iDelegate.OnTileItemTouchMove(this, eventData);
        }
    }
}

