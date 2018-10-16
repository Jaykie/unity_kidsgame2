
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Vectrosity;
public class MapUtils
{
    static public Camera mainCam
    {
        get
        {
            return AppSceneBase.main.mainCamera;
        }
    }


    static public Vector2 GetPositionCenter(int x, int y, int sizeX, int sizeY, Vector2 sizeRect)
    {
        Vector2 worldsize = sizeRect;
        float posx, posy;
        float stepy = worldsize.y / sizeY;
        float stepx = worldsize.x / sizeX;
        posx = -worldsize.x / 2 + stepx * x + stepx / 2;
        posy = -worldsize.y / 2 + stepy * y + stepy / 2;
        return new Vector2(posx, posy);
    }


    static public int GetYOfPositionCenter(Vector2 pos, int sizeX, int sizeY, Vector2 sizeRect)
    {
        int y = 0;
        Vector2 worldsize = sizeRect;
        float stepy = worldsize.y / sizeY;
        float stepx = worldsize.x / sizeX;
        for (int i = 0; i < sizeY; i++)
        {
            Vector2 center = GetPositionCenter(0, i, sizeX, sizeY, sizeRect);
            if (Mathf.Abs(center.y - pos.y) <= stepy / 2)
            {
                y = i;
                break;
            }
        }
        return y;
    }

    static public int GetXOfPositionCenter(Vector2 pos, int sizeX, int sizeY, Vector2 sizeRect)
    {
        int x = 0;
        Vector2 worldsize = sizeRect;
        float stepy = worldsize.y / sizeY;
        float stepx = worldsize.x / sizeX;
        for (int i = 0; i < sizeX; i++)
        {
            Vector2 center = GetPositionCenter(i, 0, sizeX, sizeY, sizeRect);
            if (Mathf.Abs(center.x - pos.x) <= stepx / 2)
            {
                x = i;
                break;
            }
        }
        return x;
    }

}

