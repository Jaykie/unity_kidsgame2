﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceViewController : UIViewController
{

    UIPlaceBase uiPrefab;
    UIPlaceBase ui;


    static private PlaceViewController _main = null;
    public static PlaceViewController main
    {
        get
        {
            if (_main == null)
            {
                _main = new PlaceViewController();
                _main.Init();
            }
            return _main;
        }
    }

    void Init()
    {

        string strPrefab = "App/Prefab/Place/" + GetPrefabName();
        string strPrefabDefault = "Common/Prefab/Place/UIPlaceController";
        GameObject obj = PrefabCache.main.Load(strPrefab);
        if (obj == null)
        {
            obj = PrefabCache.main.Load(strPrefabDefault);
        }

        uiPrefab = obj.GetComponent<UIPlaceBase>();
    }

    public override void ViewDidLoad()
    {
        base.ViewDidLoad();
        CreateUI();
    }

    string GetPrefabName()
    {
        //Resources.Load 文件可以不区分大小写字母
        name = "UIPlace" + Common.appType;
        return name;
    }
    public void CreateUI()
    {
        ui = (UIPlaceBase)GameObject.Instantiate(uiPrefab);
        ui.SetController(this);
        ViewControllerManager.ClonePrefabRectTransform(uiPrefab.gameObject, ui.gameObject);
    }
}
