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
        cmdItem.callbackTouchMove = OnUICmdItemTouchMove;
        cmdItem.callbackTouchUp = OnUICmdItemTouchUp;
        cmdItem.UpdateItem();
        listItem.Add(cmdItem);

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
        bool ret = uiCmdBarRun.IsItemInTheBar(item);
        if (!ret)
        {
            //恢复位置 

        }
        Debug.Log("posScreen =" + posScreen + " item.posTouchDown=" + item.posTouchDown);
        float action_time = 1f;
        RectTransform rctran = item.GetComponent<RectTransform>();
        Vector2 pt = item.posTouchDown;
        // 
        rctran.DOMove(pt, action_time).SetEase(Ease.InOutSine).OnComplete(
            () =>
            {
                item.transform.parent = this.transform;
                LayOutItem();
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


