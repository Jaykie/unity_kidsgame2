using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIHomeAppCenter : UIView
{
    public const string AD_JSON_FILE_KIDS = "applist_home_kids.json";
    public const string AD_JSON_FILE_SMALL_GAME = "applist_home_minigame.json";

    //http://www.mooncore.cn/moonma/app_center/applist_home_smallgame.json
    public const string APPCENTER_HTTP_URL_HOME_SMALL_GAME = "http://www.mooncore.cn/moonma/app_center/" + AD_JSON_FILE_SMALL_GAME;

    //http://www.mooncore.cn/moonma/app_center/applist_home_kids.json
    public const string APPCENTER_HTTP_URL_HOME_KIDS_GAME = "http://www.mooncore.cn/moonma/app_center/" + AD_JSON_FILE_KIDS;
    public Button btnAppIcon0;
    public Button btnAppIcon1;
    public Button btnAppIcon2;

    List<ItemInfo> listApp;

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        //先不显示
        if (btnAppIcon0)
        {
            btnAppIcon0.GetComponent<Image>().color = Color.clear;
        }
        if (btnAppIcon1)
        {
            btnAppIcon1.GetComponent<Image>().color = Color.clear;
        }
        if (btnAppIcon2)
        {
            btnAppIcon2.GetComponent<Image>().color = Color.clear;
        }

        this.gameObject.SetActive(false);


        StartParserAppList();
    }
    // Use this for initialization
    void Start()
    {
        LayoutChild();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void LayoutChild()
    {
        float x = 0, y = 0;
        //layoutappicon 

        Rect rectParent = this.frameParent;
        {
            GridLayoutGroup gridLayout = this.GetComponent<GridLayoutGroup>();
            Vector2 cellSize = gridLayout.cellSize;
            RectTransform rctran = this.transform as RectTransform;
            float oft = 16f;
            if (Device.isLandscape)
            {
                rctran.sizeDelta = new Vector2(cellSize.x, rctran.sizeDelta.y);
                x = rectParent.width / 2 - rctran.sizeDelta.x / 2 - oft;
                y = 0;
            }
            else
            {
                rctran.sizeDelta = new Vector2(rctran.sizeDelta.x, cellSize.y);
                x = 0;
                y = -rectParent.height / 2 + rctran.sizeDelta.y / 2 + oft;
            }
            rctran.anchoredPosition = new Vector2(x, y);
        }


    }

    void StartParserAppList()
    {

        bool enable = false;
        if (Common.isiOS)
        {
            enable = true;
        }
        else
        {
            if (AppVersion.appCheckHasFinished)
            {
                enable = true;
            }
        }

        if (Common.noad)
        {
            enable = false;
        }


        this.gameObject.SetActive(false);
        if (enable)
        {
            this.gameObject.SetActive(true);
        }


        if (!enable)
        {
            return;
        }

        listApp = new List<ItemInfo>();
        string url = APPCENTER_HTTP_URL_HOME_KIDS_GAME;
        if (!Config.main.APP_FOR_KIDS)
        {
            url = APPCENTER_HTTP_URL_HOME_SMALL_GAME;
        }
        HttpRequest http = new HttpRequest(OnHttpRequestFinished);
        http.Get(url);

    }
    void parserAppList(byte[] data)
    {
        MoreAppParser.parserJson(data, listApp);
        //display
        int idx = 0;
        int num_display = 3;
        foreach (ItemInfo infoapp in listApp)
        {
            if (idx < num_display)
            {
                startParserImage(infoapp.pic, idx);
            }

            idx++;
        }
    }

    public void startParserImage(string url, int idx)
    {
        HttpRequest http = new HttpRequest(OnHttpRequestFinishedImage);
        http.index = idx;
        http.Get(url);
    }
    void OnHttpRequestFinishedImage(HttpRequest req, bool isSuccess, byte[] data)
    {
        Button btn = null;
        switch (req.index)
        {
            case 0:
                {
                    btn = btnAppIcon0;
                }
                break;
            case 1:
                {
                    btn = btnAppIcon1;
                }
                break;
            case 2:
                {
                    btn = btnAppIcon2;
                }
                break;

        }
        if (btn == null)
        {
            return;
        }
        btn.GetComponent<Image>().color = Color.white;
        Texture2D tex = new Texture2D(0, 0, TextureFormat.ARGB32, false);
        tex.LoadImage(data);
        btn.GetComponent<Image>().sprite = LoadTexture.CreateSprieFromTex(RoundRectTexture(tex));

    }
    void OnHttpRequestFinished(HttpRequest req, bool isSuccess, byte[] data)
    {
        // Debug.Log("MoreAppParser OnHttpRequestFinished"); 
        if (isSuccess)
        {
            parserAppList(data);

        }
        else
        {
            string filepath = Common.rootDirAppCenter + "/" + AD_JSON_FILE_SMALL_GAME;
            if (Config.main.APP_FOR_KIDS)
            {
                filepath = Common.rootDirAppCenter + "/" + AD_JSON_FILE_KIDS;
            }

            byte[] dataApp = FileUtil.ReadDataAsset(filepath);
            if (dataApp != null)
            {
                parserAppList(dataApp);
            }
        }
    }

    //圆角
    Texture2D RoundRectTexture(Texture2D tex)
    {
        int w = tex.width;
        int h = tex.height;
        RenderTexture rt = new RenderTexture(w, h, 0);
        string strshader = "Custom/RoundRect";
        //string str = FileUtil.ReadStringAsset(ShotBase.STR_DIR_ROOT_SHADER+"/ShaderRoundRect.shader");
        Material mat = new Material(Shader.Find(strshader));//
        float value = (156 * 1f / 1024);
        //Debug.Log("RoundRectTexture:value=" + value);
        //value = 0.1f;
        //设置半径 最大0.5f
        mat.SetFloat("_RADIUSBUCE", value);
        Graphics.Blit(tex, rt, mat);
        Texture2D texRet = TextureUtil.RenderTexture2Texture2D(rt);
        return texRet;
    }

    void GotoAppUrl(int idx)
    {
        if (listApp == null)
        {
            return;
        }
        ItemInfo info = listApp[idx];
        if (Common.BlankString(info.url))
        {
            return;
        }
        Application.OpenURL(info.url);
    }


    public void OnUIParentGateDidCloseAppCenter(UIParentGate ui, bool isLongPress)
    {
        if (isLongPress)
        {
            Debug.Log("OnUIParentGateDidCloseAppCenter");
            GotoAppUrl(ParentGateViewController.main.index);
        }
    }
    public void ShowParentGate(int idx)
    {
        Debug.Log("ShowParentGate idx=" + idx);

        ParentGateViewController.main.index = idx;
        ParentGateViewController.main.Show(null, null);
        ParentGateViewController.main.ui.callbackClose = OnUIParentGateDidCloseAppCenter;

    }

    public void DoClickBtnAppIcon(int idx)
    {
        if (Config.main.APP_FOR_KIDS && (!AppVersion.appCheckHasFinished))
        {
            ShowParentGate(idx);
        }
        else
        {
            GotoAppUrl(idx);
        }
    }

    public void OnClickBtnAppIcon0()
    {
        DoClickBtnAppIcon(0);
    }
    public void OnClickBtnAppIcon1()
    {
        DoClickBtnAppIcon(1);
    }
    public void OnClickBtnAppIcon2()
    {
        DoClickBtnAppIcon(2);
    }

}
