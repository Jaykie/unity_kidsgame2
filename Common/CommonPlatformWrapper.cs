using UnityEngine;
using System.Collections;
using System.IO;

internal class CommonPlatformWrapper
{
    public static CommonBasePlatformWrapper platform
    {
        get
        {
#if UNITY_ANDROID && !UNITY_EDITOR
				return new CommonAndroidWrapper();
#elif UNITY_IPHONE && !UNITY_EDITOR
				return new CommoniOSWrapper();
#else
            return new CommonBasePlatformWrapper();
#endif
        }
    }
}


internal class CommonBasePlatformWrapper
{

    public virtual string GetAppName()
    {
        return Application.productName;
    }
    public virtual string GetCachePath()
    {
        string ret = Application.dataPath + "/AppCache";
        //创建文件夹
        Directory.CreateDirectory(ret);

        return ret;
    }

    public virtual string GetAppVersion()
    {
        return Application.version;
    }
    public virtual string GetAppPackage()
    {
        return Application.identifier;
    }
    public virtual string GetChannelName()
    {
        if(FileUtil.FileIsExistAsset("channel/inmobi"))
        {
            return Source.INMOB;
        }
        return Source.APPSTORE;
    }

    public virtual void EnableAdSplash()
    {

    }

    public virtual void SetIpInChina(bool isin)
    {

    }


    public virtual void UnityStartUpFinish()
    {

    }

    public virtual void SetOrientation(int orientaion)
    {

    }

     public virtual bool isiPhoneX()
    {
        return false;
    }

     public virtual int getHeightSystemTopBar()
    {
        return 0;
    }
    public virtual int getHeightSystemHomeBar()
    {
        return 0;
    }
}
