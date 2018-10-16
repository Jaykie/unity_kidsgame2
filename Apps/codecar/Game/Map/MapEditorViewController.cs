using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapEditorViewController : PopViewController
{
    UIView uiPrefab;
    public UIView ui;

    public int index;
    static private MapEditorViewController _main = null;
    public static MapEditorViewController main
    {
        get
        {
            if (_main == null)
            {
                _main = new MapEditorViewController();
                _main.Init();
            }
            return _main;
        }
    }

    void Init()
    {

        string strPrefab = "App/Prefab/Game/Map/UIMapEditor";
        GameObject obj = PrefabCache.main.Load(strPrefab);
        uiPrefab = obj.GetComponent<UIView>();
    }

    public override void ViewDidLoad()
    {
        base.ViewDidLoad();
        CreateUI();
    }

   public override void ViewDidUnLoad()
    {
        base.ViewDidUnLoad();
 
    }

    public void CreateUI()
    {
        ui = (UIView)GameObject.Instantiate(uiPrefab);
        ui.SetController(this);
        ViewControllerManager.ClonePrefabRectTransform(uiPrefab.gameObject, ui.gameObject);
    }


}
