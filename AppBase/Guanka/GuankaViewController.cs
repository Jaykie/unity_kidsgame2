﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuankaViewController : UIViewController
{
    public int indexPlace = 0;
    UIGuankaBase uiGuankaPrefab;
    UIGuankaBase uiGuanka;


    static private GuankaViewController _main = null;
    public static GuankaViewController main
    {
        get
        {
            if (_main == null)
            {
                _main = new GuankaViewController();
                _main.Init();
            }
            return _main;
        }
    }

    void Init()
    {

        string strPrefab = "App/Prefab/Guanka/" + GetPrefabName();
        string strPrefabDefault = "Common/Prefab/Guanka/UIGuankaController";
        GameObject obj = PrefabCache.main.Load(strPrefab);
        if (obj == null)
        {
            obj = PrefabCache.main.Load(strPrefabDefault);
        }

        uiGuankaPrefab = obj.GetComponent<UIGuankaBase>();
    }

    public override void ViewDidLoad()
    {
        base.ViewDidLoad();
        CreateUI();
    }

    string GetPrefabName()
    {
        //Resources.Load 文件可以不区分大小写字母
        name = "UIGuanka" + Common.appType;
        return name;
    }
    public void CreateUI()
    {
        uiGuanka = (UIGuankaBase)GameObject.Instantiate(uiGuankaPrefab);
        uiGuanka.indexPlace = indexPlace;
        uiGuanka.SetController(this);
        ViewControllerManager.ClonePrefabRectTransform(uiGuankaPrefab.gameObject, uiGuanka.gameObject);
    }
}
