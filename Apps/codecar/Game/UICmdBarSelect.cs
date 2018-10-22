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

    UIItemSelect uiItemSelectPrefab;

    int indextmp;
    List<object> listItem;

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        listItem = new List<object>();
        GameObject obj = PrefabCache.main.Load(AppRes.PREFAB_Item_Select);
        uiItemSelectPrefab = obj.GetComponent<UIItemSelect>();
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
        foreach (UIItemSelect item in listItem)
        {
            RectTransform rctran = item.GetComponent<RectTransform>();
            h = (this.frame.height - step * (listItem.Count + 1)) / listItem.Count;
            w = this.frame.width - step * 2;
            x = 0;
            y = h * i + step * (i + 1) + h / 2;
            y += -this.frame.height / 2;
            rctran.anchoredPosition = new Vector2(x, y);
            // item.localPosNormal = item.transform.localPosition;
            i++;
        }
    }
    public void AddItem(UICmdItem.CmdType type)
    {
        int idx = indextmp++;
        UIItemSelect item = (UIItemSelect)GameObject.Instantiate(uiItemSelectPrefab);
        item.transform.parent = this.transform;
        item.transform.localScale = new Vector3(1, 1, 1);
        item.transform.localPosition = new Vector3(0, 0, 0);
        item.index = idx;
        item.cmdType = type;
        item.uiCmdBarRun = uiCmdBarRun;
        item.maxCount = 4;
        // cmdItem.callBackTouch = OnUITouchEvent;
        //  cmdItem.UpdateItem();
        //  cmdItem.localPosNormal = cmdItem.transform.localPosition;
        item.AddItem(type);
        listItem.Add(item);


    }
    public void Reset()
    {
        foreach (UIItemSelect item in listItem)
        {
            item.Reset();
        }
    }
}


