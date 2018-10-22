using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;

//将要运行的命令  现在屏幕顶部
public class UICmdBarRun : UIView
{
    public GameObject objScrollView;
    public GameObject objScrollViewContent;
    public Image imageBg;
    public UICmdBarSelect uiCmdBarSelect;

    UICmdItem uiCmdItemPrefab;
    public List<UICmdItem> listItem;
    public ScrollRect scrollRect;

    public List<UICmdItem.CmdType> listCmd;
    float widthItem;
    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        listItem = new List<UICmdItem>();
        listCmd = new List<UICmdItem.CmdType>();
        GameObject obj = PrefabCache.main.Load(AppRes.PREFAB_CmdItem);
        uiCmdItemPrefab = obj.GetComponent<UICmdItem>();
        scrollRect = objScrollView.GetComponent<ScrollRect>();
        // AddItem(UICmdItem.CmdType.RIGHT);
        // AddItem(UICmdItem.CmdType.UP);
        // AddItem(UICmdItem.CmdType.RIGHT);//RIGHT
        // AddItem(UICmdItem.CmdType.DOWN);
        // AddItem(UICmdItem.CmdType.LEFT);
    }

    void Start()
    {
        LayOutItem();
    }
    public override void LayOut()
    {

    }
    public void Reset()
    {
        foreach (UICmdItem item in listItem)
        {
            item.cmdType = UICmdItem.CmdType.NONE;
        }
    }
    void LayOutItem()
    {
        // float x, y, w, h;
        // float step = 4;
        // int i = 0;
        // foreach (UICmdItem item in listItem)
        // {
        //     RectTransform rctran = item.GetComponent<RectTransform>();
        //     w = (this.frame.width - step * (listItem.Count + 1)) / listItem.Count;
        //     h = this.frame.height - step * 2;
        //     y = 0;
        //     x = w * i + step * (i + 1) + w / 2;
        //     x += -this.frame.width / 2;
        //     rctran.anchoredPosition = new Vector2(x, y);
        //     i++;
        // }
    }

    public void AddItem(UICmdItem.CmdType type)
    {
        int idx = listItem.Count;
        UICmdItem cmdItem = (UICmdItem)GameObject.Instantiate(uiCmdItemPrefab);
        cmdItem.transform.parent = objScrollViewContent.transform;
        //this.transform;
        cmdItem.transform.localScale = new Vector3(1, 1, 1);
        cmdItem.transform.localPosition = new Vector3(0, 0, 0);
        cmdItem.index = idx;
        cmdItem.cmdType = type;
        cmdItem.callBackTouch = OnUITouchEvent;
        cmdItem.UpdateItem();

        //更新scrollview 内容的长度
        RectTransform rctranItem = cmdItem.GetComponent<RectTransform>();
        RectTransform rctran = objScrollViewContent.GetComponent<RectTransform>();
        Vector2 size = rctran.sizeDelta;
        widthItem = rctranItem.rect.width;
        Debug.Log("widthItem=" + widthItem);
        size.x = widthItem * (idx + 1);
        rctran.sizeDelta = size;

        listItem.Add(cmdItem);

    }
    public void AddCmd(UICmdItem.CmdType type)
    {
        listCmd.Add(type);
    }

    // 判断命令位置放置正确
    public bool IsItemInTheBar(UICmdItem item)
    {
        bool ret = false;

        return ret;
    }
    public Transform GetItemParent(UICmdItem item)
    {
        Transform t = null;
        foreach (UICmdItem item_bar in listItem)
        {
            Vector2 posNow = item.gameObject.transform.position;
            RectTransform rctran_bar = item_bar.GetComponent<RectTransform>();
            Vector2 posBar = item_bar.gameObject.transform.position;
            float w_screen = Common.CanvasToScreenWidth(AppSceneBase.main.sizeCanvas, rctran_bar.rect.width);
            float h_screen = Common.CanvasToScreenWidth(AppSceneBase.main.sizeCanvas, rctran_bar.rect.height);
            Rect rc = new Rect(posBar.x - w_screen / 2, posBar.y - h_screen / 2, w_screen, h_screen);
            if (rc.Contains(posNow))
            {
                t = item_bar.transform;
                item_bar.cmdType = item.cmdType;
                break;
            }

        }
        return t;
    }


    public void OnUITouchEvent(UICmdItem item, PointerEventData eventData, int status)
    {
        switch (status)
        {
            case UITouchEvent.STATUS_TOUCH_DOWN:

                break;

            case UITouchEvent.STATUS_TOUCH_MOVE:
                OnUICmdItemTouchMove(item, eventData);
                break;

            case UITouchEvent.STATUS_TOUCH_UP:
                OnUICmdItemTouchUp(item, eventData);
                break;
        }
    }
    public void OnUICmdItemTouchMove(UICmdItem item, PointerEventData eventData)
    {
        Vector2 posScreen = eventData.position;
        //position 当gameObject为canvas元素时为屏幕坐标而非世界坐标
        item.gameObject.transform.position = posScreen;
        item.transform.parent = AppSceneBase.main.canvasMain.transform;

    }

    public void OnUICmdItemTouchUp(UICmdItem item, PointerEventData eventData)
    {
        Vector2 posScreen = eventData.position;
        //恢复位置动画 
        // iTween.MoveTo(item.gameObject, iTween.Hash("position", item.posTouchDown, "time", 1f, "easeType", iTween.EaseType.easeInQuint));
        float action_time = 1f;
        RectTransform rctran = item.GetComponent<RectTransform>();
        Vector2 pt = item.posTouchDown;
        // 
        rctran.DOMove(pt, action_time).SetEase(Ease.InOutSine).OnComplete(
            () =>
            {
                item.transform.parent = objScrollViewContent.transform;
                LayOutItem();
            }
        );

    }
    public void OnClickBtnPre()
    {
        Vector2 pos = scrollRect.content.anchoredPosition;
        pos.x += widthItem;
        scrollRect.content.anchoredPosition = pos;
    }

    public void OnClickBtnNext()
    {
        Vector2 pos = scrollRect.content.anchoredPosition;
        pos.x -= widthItem;
        scrollRect.content.anchoredPosition = pos;
    }

    public void OnClickBtnReset()
    {
        UIGameCodeCar game = GameViewController.main.gameBase as UIGameCodeCar;
        game.Reset();
    }

}


