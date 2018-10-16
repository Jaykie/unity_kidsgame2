using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UIHomeCodeCar : UIHomeBase, IPopViewControllerDelegate
{
    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    //f88816
    public GameObject objLayoutBtn;

    static public void SetBtnText(Button btn, string str)
    {
        Transform tr = btn.transform.Find("Text");
        Text btnText = tr.GetComponent<Text>();
        btnText.text = str;
        float str_w = Common.GetStringLength(str, AppString.STR_FONT_NAME, btnText.fontSize);
        RectTransform rctran = btnText.transform as RectTransform;
        Vector2 sizeDelta = rctran.sizeDelta;
        sizeDelta.x = str_w;
        rctran.sizeDelta = sizeDelta;
    }
    void Awake()
    {
        TextureUtil.UpdateImageTexture(imageBgName, AppRes.IMAGE_LOGO, true);
        TextureUtil.UpdateImageTexture(imageBg, AppRes.IMAGE_GAME_BG, true);

    }
    // Use this for initialization
    void Start()
    {
        LayOutChild();

        Vector2 pt = new Vector2(0, 20);
        // pointStart= CCPointMake(0, step);
        //   CCEaseSineInOut - 由慢至快再由快至慢
        // CCMoveBy *action = CCMoveBy::create(2,  pointStart);

        // viewLogo->runAction(CCRepeatForever::create( CCSequence::create(CCEaseSineInOut::create(action),
        //                                                                   CCEaseSineInOut::create(action->reverse()),

        //iTween.MoveBy(imageBgName.gameObject, iTween.Hash("islocal", true, "position", pt, "time", 2f, "easeType", iTween.EaseType.easeInSine, "loopType", "pingPong"));
        RectTransform rctran = imageBgName.GetComponent<RectTransform>();
        Tweener action = rctran.DOMove(pt, 2f).SetRelative().SetEase(Ease.InSine);
        action.SetLoops(-1, LoopType.Yoyo);

        UpdateTitle();


    }

    // Update is called once per frame
    void Update()
    {

    }
    void UpdateTitle()
    {
        string title = Language.main.GetString("STR_PLAY") + ":" + Language.game.GetString("GUANKA_TITLE" + GameManager.gameLevel);
        // SetBtnText(btnPlay, title);
        // SetBtnText(btnSetting, Language.main.GetString("STR_SETTING"));
        // SetBtnText(btnMore, Language.main.GetString("STR_MORE"));
        // SetBtnText(btnShare, Language.main.GetString("SHARE_TITLE"));
        // SetBtnText(btnNoAd, Language.main.GetString("STR_BTN_NOAD"));

    }
    public void OnPopViewControllerDidClose(PopViewController controller)
    {
        UpdateTitle();
    }

    public void OnClickBtnMapEditor()
    {
        //MapEditorViewController.main.Show(null, this);
        if (this.controller != null)
        {
            NaviViewController navi = this.controller.naviController;
            navi.Push(MapEditorViewController.main);
        }
    }

    public void OnClickBtnPlay()
    {
        if (this.controller != null)
        {
            NaviViewController navi = this.controller.naviController;
            navi.Push(GuankaViewController.main);
        }
    }
    void LayOutChild()
    {
        Vector2 sizeCanvas = AppSceneBase.main.sizeCanvas;
        float x = 0, y = 0;
        {
            RectTransform rctran = imageBg.GetComponent<RectTransform>();
            float w = imageBg.sprite.texture.width;//rectTransform.rect.width;
            float h = imageBg.sprite.texture.height;//rectTransform.rect.height;
            if (w != 0)
            {
                float scalex = sizeCanvas.x / w;
                float scaley = sizeCanvas.y / h;
                float scale = Mathf.Max(scalex, scaley);
                imageBg.transform.localScale = new Vector3(scale, scale, 1.0f);
                //屏幕坐标 现在在屏幕中央
                imageBg.transform.position = new Vector2(Screen.width / 2, Screen.height / 2);
            }

        }

        //image name
        {
            RectTransform rctran = imageBgName.GetComponent<RectTransform>();
            x = 0;
            y = (this.frame.size.y - topBarHeight * 2) / 4;
            rctran.anchoredPosition = new Vector2(x, y);
        }

        {
            RectTransform rctran = objLayoutBtn.GetComponent<RectTransform>();
            x = 0;
            y = -rctran.rect.height * 2;
            rctran.anchoredPosition = new Vector2(x, y);
        }


        //LayoutChildBase();
    }
}
