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

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        listItem = new List<UICmdItem>();
        GameObject obj = PrefabCache.main.Load(AppRes.PREFAB_CmdItem);
        uiCmdItemPrefab = obj.GetComponent<UICmdItem>();
 
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

    void LayOutItem()
    {
        float x, y, w, h;
        float step = 4;
        int i = 0;
        foreach (UICmdItem item in listItem)
        {
            RectTransform rctran = item.GetComponent<RectTransform>();
            w = (this.frame.width - step * (listItem.Count + 1)) / listItem.Count;
            h = this.frame.height - step * 2;
            y = 0;
            x = w * i + step * (i + 1) + w / 2;
            x += -this.frame.width / 2;
            rctran.anchoredPosition = new Vector2(x, y);
            i++;
        }
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
        cmdItem.callbackTouchMove = OnUICmdItemTouchMove;
        cmdItem.callbackTouchUp = OnUICmdItemTouchUp;
        cmdItem.UpdateItem();

        //更新scrollview 内容的长度
        RectTransform rctranItem = cmdItem.GetComponent<RectTransform>();
        RectTransform rctran = objScrollViewContent.GetComponent<RectTransform>();
        Vector2 size = rctran.sizeDelta;
        size.x = rctranItem.rect.width * (idx + 1);
        rctran.sizeDelta = size;

        listItem.Add(cmdItem);

    }

    // 判断命令位置放置正确
    public bool IsItemInTheBar(UICmdItem item)
    {
        bool ret = false;
        foreach (UICmdItem item_bar in listItem)
        {
            RectTransform rctran = item.GetComponent<RectTransform>();
            Vector2 pt = rctran.rect.center;
            RectTransform rctran_bar = item_bar.GetComponent<RectTransform>();
            if (rctran_bar.rect.Contains(pt))
            {
                ret = true;
                break;
            }

        }
        return ret;
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

    }

    public void OnClickBtnNext()
    {

    }

    public void OnClickBtnReset()
    {

    }

}


