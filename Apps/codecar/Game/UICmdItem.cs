
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public delegate void OnUICmdItemTouchEventDelegate(UICmdItem item, PointerEventData eventData, int status);
// public class CmdItemInfo
// {
//     UICmdItem.CmdType type;
// }
//UICmdItem 有IDragHandler  的时候 scrollview滑动会失效
public class UICmdItem : MonoBehaviour
{
    public enum CmdType
    {
        NONE = 0,
        START,
        LEFT,
        RIGHT,
        UP,
        DOWN,

    }
    public Image imageBg;
    public Image imageCmd;
    public Text textCount;
    public Image imageAlpha;
    public Image imageBoard;
    public int index;
    public CmdType cmdType;
    public Vector2 posTouchDown;//　transform.position　canvase ui 的屏幕坐标
    public Vector2 localPosNormal;
    UITouchEvent uiTouchEvent;
    public OnUICmdItemTouchEventDelegate callBackTouch { get; set; }

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        uiTouchEvent = this.gameObject.AddComponent<UITouchEvent>();
        uiTouchEvent.callBackTouch = OnUITouchEvent;

        Texture2D tex = TextureCache.main.Load(AppRes.IMAGE_CMDITEM_BG);
        if (tex != null)
        {
            imageBg.sprite = LoadTexture.CreateSprieFromTex(tex);
        }
    }
    public void ShowBoard(bool isShow)
    {
        imageBoard.gameObject.SetActive(isShow);
        imageCmd.gameObject.SetActive(!isShow);
        //  textCount.gameObject.SetActive(!isShow);
        imageBg.gameObject.SetActive(!isShow);
    }
    public void UpdateItem()
    {
        textCount.text = index.ToString();

        string filename = "";
        switch (cmdType)
        {
            case CmdType.START:
                filename = "CmdStart";
                break;
            case CmdType.LEFT:
                filename = "CmdLeft";
                break;
            case CmdType.RIGHT:
                filename = "CmdRight";
                break;
            case CmdType.UP:
                filename = "CmdUp";
                break;
            case CmdType.DOWN:
                filename = "CmdDown";
                break;
            default:
                filename = "CmdStart";
                break;

        }

        Texture2D tex = TextureCache.main.Load("App/UI/Game/CmdItem/" + filename);
        if (tex != null)
        {
            imageCmd.sprite = LoadTexture.CreateSprieFromTex(tex);
        }

        if (cmdType == CmdType.NONE)
        {
            ShowBoard(true);
        }
        else
        {
            ShowBoard(false);
        }
    }

    public void OnUITouchEvent(UITouchEvent ev, PointerEventData eventData, int status)
    {
        if (cmdType == CmdType.NONE)
        {
            return;
        }
        switch (status)
        {
            case UITouchEvent.STATUS_TOUCH_DOWN:
                OnPointerDown(eventData);
                break;

            case UITouchEvent.STATUS_TOUCH_MOVE:
                OnDrag(eventData);
                break;

            case UITouchEvent.STATUS_TOUCH_UP:
                OnPointerUp(eventData);
                break;
        }
        if (callBackTouch != null)
        {
            callBackTouch(this, eventData, status);
        }
    }

    //相当于touchDown
    public void OnPointerDown(PointerEventData eventData)
    {

        Debug.Log("OnPointerDown,cmditem");
        posTouchDown = this.gameObject.transform.position;
        //置顶显示
        this.gameObject.transform.SetAsLastSibling();

        Vector2 posScreen = eventData.position;
        //position 当gameObject为canvas元素时为屏幕坐标而非世界坐标
        this.gameObject.transform.position = posScreen;
    }
    //相当于touchUp
    public void OnPointerUp(PointerEventData eventData)
    {


    }
    //相当于touchMove
    public void OnDrag(PointerEventData eventData)
    {
        // this.gameObject.transform.position = eventData.pointerPressRaycast.worldPosition;


    }
}

