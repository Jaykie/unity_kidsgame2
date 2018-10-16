using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Moonma.AdKit.AdBanner;
using Moonma.AdKit.AdInsert;
using Moonma.AdKit.AdVideo;
using Moonma.AdKit.AdConfig;
public delegate void OnAdKitFinishDelegate(AdKitCommon.AdType type, AdKitCommon.AdStatus status, string str);
public class AdKitCommon : MonoBehaviour
{
    public enum AdType
    {
        BANNER = 0,
        INSERT,
        VIDEO
    }
    public enum AdStatus
    {
        FAIL = 0,
        SUCCESFULL,
        START,
        CLOSE
    }
    public static AdKitCommon main;

    public OnAdKitFinishDelegate callbackFinish { get; set; }
    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        if (main == null)
        {
            main = this;
        }

    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }


    public void InitAdBanner()
    {
        if (Common.noad)
        {
            return;
        }
        bool isShowAdBanner = true;
        if (Common.isiOS)
        {
            if (!AppVersion.appCheckHasFinished)
            {
                //ios app审核不显示banner
                isShowAdBanner = false;
            }
        }

        if (Common.isAndroid)
        {

            if (!AppVersion.appCheckHasFinished)
            {
                //xiaomi app审核不显示banner
                isShowAdBanner = false;
            }
        }

        if (isShowAdBanner)
        {
            AdBanner.SetScreenSize(Screen.width, Screen.height);
            AdBanner.SetScreenOffset(0, Device.heightSystemHomeBar);
            {
                int type = AdConfigParser.SOURCE_TYPE_BANNER;
                string source = AdConfig.main.GetAdSource(type);
                AdBanner.InitAd(source);
                AdBanner.ShowAd(true);
            }
        }




    }

    public void InitAdInsert()
    {
        if (Common.noad)
        {
            return;
        }

        bool isShowAdInsert = false;
        if (AppVersion.appCheckHasFinished)
        {
            isShowAdInsert = true;
        }
        if (isShowAdInsert)
        {
            AdInsert.SetObjectInfo(this.gameObject.name);
            int type = AdConfigParser.SOURCE_TYPE_INSERT;
            string source = AdConfig.main.GetAdSource(type);
            AdInsert.InitAd(source);
        }

    }

    public void InitAdVideo()
    {
        if (Common.noad)
        {
            return;
        }

        if (AppVersion.appCheckHasFinished)
        {
            AdVideo.SetType(AdVideo.ADVIDEO_TYPE_REWARD);
            int type = AdConfigParser.SOURCE_TYPE_VIDEO;
            string source = AdConfig.main.GetAdSource(type);
            AdVideo.InitAd(source);
        }
    }

    public void ShowAdBanner(bool isShow)
    {
        AdBanner.ShowAd(isShow);
    }
    public void   ShowAdVideo()
    {
        //show 之前重新设置广告
        InitAdVideo();
        AdVideo.ShowAd();
    }

    public void ShowAdInsert(int rate)
    {

        if (!AppVersion.appCheckHasFinished)
        {
            return;
        }

        if (Common.noad)
        {
            return;
        }

        if (Common.isAndroid)
        {
            if (Common.GetDayIndexOfUse() <= Config.main.NO_AD_DAY)
            {
                return;
            }
        }


        int randvalue = Random.Range(0, 100);
        if (randvalue > rate)
        {
            return;
        }
         //show 之前重新设置广告
        //InitAdInsert();
        AdInsert.ShowAd();
    }


    public void AdBannerDidReceiveAd(string str)
    {

        int w = 0;
        int h = 0;
        int idx = str.IndexOf(":");
        string strW = str.Substring(0, idx);
        int.TryParse(strW, out w);
        string strH = str.Substring(idx + 1);
        int.TryParse(strH, out h);
        Debug.Log("AdBannerDidReceiveAd::w=" + w + " h=" + h);
        // if (gameBaseRun != null)
        // {
        //     gameBaseRun.AdBannerDidReceiveAd(w, h);
        // }
        if (callbackFinish != null)
        {
            callbackFinish(AdType.BANNER, AdStatus.SUCCESFULL, str);
        }

    }
    public void AdBannerDidReceiveAdFail(string adsource)
    {


        int type = AdConfigParser.SOURCE_TYPE_BANNER;
        AdInfo info = AdConfig.main.GetNextPriority(type);
        if (info != null)
        {
            AdBanner.InitAd(info.source);
            AdBanner.ShowAd(true);
        }
        else
        {
            if (callbackFinish != null)
            {
                callbackFinish(AdType.BANNER, AdStatus.FAIL, null);
            }
        }
    }



    public void AdInsertWillShow(string str)
    {
        //Debug.Log(str);
        // PauseGame(true);

        if (callbackFinish != null)
        {
            callbackFinish(AdType.INSERT, AdStatus.START, null);
        }
    }
    public void AdInsertDidClose(string str)
    {
        //s Debug.Log(str);
        // PauseGame(false);
        if (callbackFinish != null)
        {
            callbackFinish(AdType.INSERT, AdStatus.CLOSE, null);
        }
    }

    public void AdInsertDidFail(string adsource)
    {

        int type = AdConfigParser.SOURCE_TYPE_INSERT;
        AdInfo info = AdConfig.main.GetNextPriority(type);
        if (info != null)
        {
            AdInsert.InitAd(info.source);
            AdInsert.ShowAd();
        }
        else
        {
            if (callbackFinish != null)
            {
                callbackFinish(AdType.INSERT, AdStatus.FAIL, null);
            }
        }

    }

    public void AdVideoDidFail(string str)
    {
        int type = AdConfigParser.SOURCE_TYPE_VIDEO;
        AdInfo info = AdConfig.main.GetNextPriority(type);
        if (info != null)
        {
            AdVideo.InitAd(info.source); 
            AdVideo.ShowAd();
        }
        else
        {
            if (callbackFinish != null)
            {
                callbackFinish(AdType.VIDEO, AdStatus.FAIL, null);
            }
        }
    }

    public void AdVideoDidStart(string str)
    {
        AudioPlay.main.Pause();
        if (callbackFinish != null)
        {
            callbackFinish(AdType.VIDEO, AdStatus.START, null);
        }

    }
    public void AdVideoDidFinish(string str)
    {
        bool ret = Common.GetBool(AppString.STR_KEY_BACKGROUND_MUSIC);
        if (ret)
        {
            AudioPlay.main.Play();
        }

        if (callbackFinish != null)
        {
            callbackFinish(AdType.VIDEO, AdStatus.SUCCESFULL, null);
        }

    }

}
