using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;

//命令选择条  显示在右边
public class UICmdBarSelect : UIView
{
    public Image imageBg;
    public UICmdBarRun uiCmdBarRun;

    UICmdItem uiCmdItemPrefab;

    int indextmp;
    List<object> listItem;

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        listItem = new List<object>();
        GameObject obj = PrefabCache.main.Load(AppRes.PREFAB_CmdItem);
        uiCmdItemPrefab = obj.GetComponent<UICmdItem>();
        indextmp = 0;
        AddItem(UICmdItem.CmdType.START);
        AddItem(UICmdItem.CmdType.LEFT);
        AddItem(UICmdItem.CmdType.RIGHT);
        AddItem(UICmdItem.CmdType.UP);
        AddItem(UICmdItem.CmdType.DOWN);
    }
    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {
        LayOutItem();
    }
    public override void LayOut()
    {

    }

    void LayOutItem()
    {
        float x, y, w, h;
        float step = 4;
        int i = 0;
        foreach (UICmdItem item in listItem)
        {
            RectTransform rctran = item.GetComponent<RectTransform>();
            h = (this.frame.height - step * (listItem.Count + 1)) / listItem.Count;
            w = this.frame.width - step * 2;
            x = 0;
            y = h * i + step * (i + 1) + h / 2;
            y += -this.frame.height / 2;
            rctran.anchoredPosition = new Vector2(x, y);
            item.localPosNormal = item.transform.localPosition;
            i++;
        }
    }
    public void AddItem(UICmdItem.CmdType type)
    {
        int idx = indextmp++;
        UICmdItem cmdItem = (UICmdItem)GameObject.Instantiate(uiCmdItemPrefab);
        cmdItem.transform.parent = this.transform;
        cmdItem.transform.localScale = new Vector3(1, 1, 1);
        cmdItem.transform.localPosition = new Vector3(0, 0, 0);
        cmdItem.index = idx;
        cmdItem.cmdType = type;
        cmdItem.callBackTouch = OnUITouchEvent;
        cmdItem.UpdateItem();
        cmdItem.localPosNormal = cmdItem.transform.localPosition;
        listItem.Add(cmdItem);

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
                // LayOutItem();
            }
        );
    }

    public void OnClickBtnPre()
    {

    }

    public void OnClickBtnNext()
    {

    }

    public void OnClickBtnReset()
    {

    }

}


