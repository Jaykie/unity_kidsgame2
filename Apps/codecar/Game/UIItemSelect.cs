using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;

//命令选择条  显示在右边
public class UIItemSelect : UIView
{
    public Image imageBg;
    public UICmdBarRun uiCmdBarRun;
    UICmdItem uiCmdItemPrefab;

    UICmdItem uiCmdItemBg;
    UICmdItem uiCmdItemFt;
    public Text textCount;
    public int index;
    public UICmdItem.CmdType cmdType;
    public int maxCount;
    int count;

    List<object> listItem;
    bool isFirstTouchMove;

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        listItem = new List<object>();
        GameObject obj = PrefabCache.main.Load(AppRes.PREFAB_CmdItem);
        uiCmdItemPrefab = obj.GetComponent<UICmdItem>();
        count = 0;

    }
    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {

    }
    public override void LayOut()
    {

    }

    UICmdItem CreateItem(UICmdItem.CmdType type)
    {
        UICmdItem cmdItem = (UICmdItem)GameObject.Instantiate(uiCmdItemPrefab);
        cmdItem.transform.parent = this.transform;
        cmdItem.transform.localScale = new Vector3(1, 1, 1);
        cmdItem.transform.localPosition = new Vector3(0, 0, 0);
        cmdItem.index = 0;
        cmdItem.cmdType = type;
        cmdItem.callBackTouch = OnUITouchEvent;
        cmdItem.UpdateItem();
        cmdItem.localPosNormal = cmdItem.transform.localPosition;
        uiCmdItemBg = cmdItem;

        return cmdItem;
    }

    public void AddItem(UICmdItem.CmdType type)
    {
        count = maxCount;
        uiCmdItemBg = CreateItem(type);
        uiCmdItemBg.enableTouch = false;
        for (int i = 0; i < maxCount; i++)
        {
            uiCmdItemFt = CreateItem(type);
        }

        SetTextToTopMost();

        UpdateCount();
    }

    void UpdateCount()
    {
        textCount.text = count.ToString();
    }
    void SetTextToTopMost()
    {
        textCount.transform.SetAsLastSibling();
    }

    //从bar select 脱离
    public void OnDragStart()
    {
        SetTextToTopMost();

        {
            count--;
            if (count < 0)
            {
                count = 0;
            }
            UpdateCount();
        }

    }
    public void OnDragAnimateFinish()
    {
        SetTextToTopMost();
        count++;
        if (count > maxCount)
        {
            count = maxCount;
        }
        UpdateCount();
    }
    public void OnUITouchEvent(UICmdItem item, PointerEventData eventData, int status)
    {
        switch (status)
        {
            case UITouchEvent.STATUS_TOUCH_DOWN:
                OnUICmdItemTouchDown(item, eventData);
                break;

            case UITouchEvent.STATUS_TOUCH_MOVE:
                OnUICmdItemTouchMove(item, eventData);
                break;

            case UITouchEvent.STATUS_TOUCH_UP:
                OnUICmdItemTouchUp(item, eventData);
                break;
        }
    }
    public void OnUICmdItemTouchDown(UICmdItem item, PointerEventData eventData)
    {
        isFirstTouchMove = true;

    }
    public void OnUICmdItemTouchMove(UICmdItem item, PointerEventData eventData)
    {
        Vector2 posScreen = eventData.position;
        //position 当gameObject为canvas元素时为屏幕坐标而非世界坐标
        item.gameObject.transform.position = posScreen;

        if (isFirstTouchMove)
        {
            if (item.transform.parent == this.transform)
            {
                OnDragStart();
            }

            isFirstTouchMove = false;
        }


        item.transform.parent = AppSceneBase.main.canvasMain.transform;
        // item.ShowTextCount(false);


    }

    public void OnUICmdItemTouchUp(UICmdItem item, PointerEventData eventData)
    {
        Vector2 posScreen = eventData.position;
        Transform parent = uiCmdBarRun.GetItemParent(item);
        if (parent != null)
        {
            //显示在run bar上面
            item.transform.parent = parent;
            RectTransform rt = item.GetComponent<RectTransform>();
            rt.anchoredPosition = Vector2.zero;
            return;
        }
        Debug.Log("posScreen =" + posScreen + " item.posTouchDown=" + item.posTouchDown);
        float action_time = 1f;
        RectTransform rctran = item.GetComponent<RectTransform>();
        Vector2 pt = item.localPosNormal;
        item.transform.parent = this.transform;
        rctran.DOLocalMove(pt, action_time).SetEase(Ease.InOutSine).OnComplete(
            () =>
            {
                Debug.Log("rctran.localPosition=" + rctran.localPosition);
                //  item.ShowTextCount(true);
                OnDragAnimateFinish();
                // LayOutItem();
            }
        );
    }

}


