using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenShotViewController : NaviViewController
{ 
    public UIScreenShotController uiPrefab;
    public UIScreenShotController ui;
    static private ScreenShotViewController _main = null;
    public static ScreenShotViewController main
    {
        get
        {
            if (_main == null)
            {
                _main = new ScreenShotViewController();
                _main.Init();
            }
            return _main;
        }
    }

    void Init()
    {
        string strPrefab = "Common/Prefab/ScreenShot/UIScreenShotController";
        GameObject obj = PrefabCache.main.Load(strPrefab);
        uiPrefab = obj.GetComponent<UIScreenShotController>();
    }

    public override void ViewDidLoad()
    {
        base.ViewDidLoad();
        CreateUI(); 
        HideNavibar(true);
    } 
     public override void LayOutView()
    {
        base.LayOutView();
       
    }
    public void CreateUI()
    {
        ui = (UIScreenShotController)GameObject.Instantiate(uiPrefab);
        ui.SetController(this);
        ViewControllerManager.ClonePrefabRectTransform(uiPrefab.gameObject, ui.gameObject);
    }
}
