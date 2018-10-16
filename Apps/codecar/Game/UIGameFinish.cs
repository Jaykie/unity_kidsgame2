
using System.Collections;
using System.Collections.Generic;
using Moonma.Share;
using UnityEngine;
using UnityEngine.UI;
public class UIGameFinish : UIView
{
    public GameObject objContent;
    public Image imageBg;
    public Image imageBoard;
    public Image imageLogo;
    public Text textTitle;
    public Button btnShare;
    public Button btnContinue;

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        textTitle.text = Language.main.GetString("STRING_GAME_FINISH_TITLE_NORMAL");
        Common.SetButtonText(btnShare, Language.main.GetString("STRING_GAME_FINISH_BTN_SHARE"), -1);
        Common.SetButtonText(btnContinue, Language.main.GetString("STRING_GAME_FINISH_BTN_CONTINUE"), -1);
        TextureUtil.UpdateImageTexture(imageLogo, AppRes.IMAGE_LOGO, true);
        LayOutChild();
    }


    void LayOutChild()
    {
        float x, y, w, h;
        Vector2 sizeCanvas = AppSceneBase.main.sizeCanvas;
        {
            RectTransform rectTransform = imageBg.GetComponent<RectTransform>();
            float w_image = rectTransform.rect.width;
            float h_image = rectTransform.rect.height;
            float scalex = sizeCanvas.x / w_image;
            float scaley = sizeCanvas.y / h_image;
            float scale = Mathf.Max(scalex, scaley);
            imageBg.transform.localScale = new Vector3(scale, scale, 1.0f);
            //屏幕坐标 现在在屏幕中央
            imageBg.transform.position = new Vector2(Screen.width / 2, Screen.height / 2);
        }
        {
            RectTransform rctran = objContent.GetComponent<RectTransform>();
            w = Mathf.Min(this.frame.width, this.frame.height) * 0.8f;
            h = w;
            rctran.sizeDelta = new Vector2(w, h);

        }
        RectTransform rctranContent = objContent.GetComponent<RectTransform>();
        {

            RectTransform rctran = imageLogo.GetComponent<RectTransform>();
            w = imageLogo.sprite.texture.width;//rectTransform.rect.width;
            h = imageLogo.sprite.texture.height;//rectTransform.rect.height;
            if (w != 0)
            {
                float scalex = rctranContent.rect.width / w;
                float scaley = rctranContent.rect.height / h;
                float scale = Mathf.Min(scalex, scaley) * 0.9f;
                imageLogo.transform.localScale = new Vector3(scale, scale, 1.0f);
            }

        }

    }

    void ShowShare()
    {
        // ShareManager.main.callBackClick = OnUIShareDidClick;
        // ShareManager.main.Show();
    }



    public void OnUIShareDidClick(ItemInfo item)
    {
        string title = Language.main.GetString("UIMAIN_SHARE_TITLE");
        string detail = Language.main.GetString("UIMAIN_SHARE_DETAIL");
        string url = Config.main.shareAppUrl;
        Share.main.ShareWeb(item.source, title, detail, url);
    }
    public void OnClickBtnShare()
    {
        ShowShare();
    }
    public void OnClickBtnContinue()
    {
        this.gameObject.SetActive(false);
        GameManager.GotoNextLevel();
    }


}
