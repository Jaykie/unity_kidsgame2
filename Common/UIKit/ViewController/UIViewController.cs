﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//ui显示完成
public delegate void OnUIViewDidFinishDelegate();
public class UIViewController
{

    public enum Type
    {
        NONE = 0,
        NAVIBAR

    }
    public GameObject objController;//ui层 必须放在canvas下
    public UIView view;
    public string name;
    public string title;
    public Vector2 sizeCanvas;
    public bool isActive = false;
    public Type type = Type.NONE;

    public NaviViewController naviController;

    public OnUIViewDidFinishDelegate callbackUIFinish { get; set; }

    public Camera mainCam
    {
        get
        {
            return AppSceneBase.main.mainCamera;
        }
    }

    public UIViewController()
    {
        Debug.Log("new UIViewController");
        //CreateView();
    }

    public virtual void ViewDidLoad()
    {
        Debug.Log("UIViewController:ViewDidLoad");
        isActive = true;
    }
    public virtual void ViewDidUnLoad()
    {
        isActive = false;
    }

    public virtual void LayOutView()
    {
        Debug.Log("UIViewController LayOutView ");
        if ((objController != null) && (objController.transform.parent != null))
        {
            RectTransform rctranParent = objController.transform.parent.GetComponent<RectTransform>();
            RectTransform rctran = objController.GetComponent<RectTransform>();
            rctran.sizeDelta = rctranParent.sizeDelta;
        }
    }

    void CreateObjController()
    {
        if (objController == null)
        {
            string classname = this.GetType().ToString();
            objController = new GameObject(classname);
            RectTransform rctran = objController.AddComponent<RectTransform>();
            rctran.anchorMin = new Vector2(0, 0);
            rctran.anchorMax = new Vector2(1, 1);

            rctran.offsetMin = new Vector2(0, 0);
            rctran.offsetMax = new Vector2(0, 0);

            UpdateCanvasSize(AppSceneBase.main.sizeCanvas);
            ViewDidLoad();
        }

    }

    public void DestroyObjController()
    {
        Debug.Log("UIViewController.DestroyObjController");
        ViewDidUnLoad();
        GameObject.DestroyImmediate(objController);
        objController = null;
    }
    public void SetViewParent(GameObject obj)
    {
        if (objController == null)
        {
            CreateObjController();
        }
        //objController.transform.parent = obj.transform;
        objController.transform.SetParent(obj.transform);
        objController.transform.localScale = new Vector3(1f, 1f, 1f);
        objController.transform.localPosition = new Vector3(0f, 0f, 0f);

        RectTransform rctran = objController.GetComponent<RectTransform>();
        rctran.offsetMin = new Vector2(0, 0);
        rctran.offsetMax = new Vector2(0, 0);
    }

    public void UpdateName(string str)
    {
        name = str;
        if (objController != null)
        {
            objController.name = name;
        }
    }

    public void UpdateCanvasSize(Vector2 size)
    {
        float x, y;
        sizeCanvas = size;
        ViewControllerManager.sizeCanvas = size;
        if (objController != null)
        {
            // float oft_top = Common.ScreenToCanvasHeigt(sizeCanvas, Device.offsetTop);
            // float oft_bottom = Common.ScreenToCanvasHeigt(sizeCanvas, Device.offsetBottom);
            // float oft_left = Common.ScreenToCanvasWidth(sizeCanvas, Device.offsetLeft);
            // float oft_right = Common.ScreenToCanvasWidth(sizeCanvas, Device.offsetRight);
            // RectTransform rctran = objController.GetComponent<RectTransform>();
            // rctran.sizeDelta = new Vector2(sizeCanvas.x - oft_left - oft_right, sizeCanvas.y - oft_top - oft_bottom);

        }
        LayOutView();
    }


}
