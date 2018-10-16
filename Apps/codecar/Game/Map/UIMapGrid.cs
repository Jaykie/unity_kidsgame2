
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Vectrosity;
public class UIMapGrid : UIView
{
    public Vector2 sizeRect;
    public int mapSizeY;//行
    public int mapSizeX;//列
    float lineWidth;

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {

        lineWidth = 2f;
        VectorLine.SetCamera3D(mainCam);
    }


    void Start()
    {
        DrawGrid(mapSizeX, mapSizeY);
    }

    //画网格
    public void DrawGrid(int sizex, int sizey)
    {
        mapSizeX = sizex;
        mapSizeY = sizey;
        float x, y, z, step;
        z = 0;
        Vector2 worldsize = sizeRect;//Common.GetWorldSize(mainCam);
        //横线
        for (int i = 0; i < (mapSizeY + 1); i++)
        {
            step = worldsize.y / mapSizeY;
            x = -worldsize.x / 2;
            y = -worldsize.y / 2 + step * i;
            Vector3 posstart = new Vector3(x, y, z);
            x = worldsize.x / 2;
            Vector3 posend = new Vector3(x, y, z);
            string str = "line_row_" + i;
            DrawGridLine(str, posstart, posend);
        }

        //竖线
        for (int j = 0; j < mapSizeX + 1; j++)
        {
            step = worldsize.x / mapSizeX;
            y = -worldsize.y / 2;
            x = -worldsize.x / 2 + step * j;
            Vector3 posstart = new Vector3(x, y, z);
            y = worldsize.y / 2;
            Vector3 posend = new Vector3(x, y, z);
            string str = "line_col_" + j;
            DrawGridLine(str, posstart, posend);
        }
    }
    //画网格线
    void DrawGridLine(string name, Vector3 posStart, Vector3 posEnd)
    {
        // Make Vector2 array; in this case we just use 2 elements...
        List<Vector3> linePoints = new List<Vector3>();
        linePoints.Add(posStart);
        linePoints.Add(posEnd);
        // Make a VectorLine object using the above points and the default material, with a width of 2 pixels
        VectorLine line = new VectorLine(name, linePoints, lineWidth);
        line.Draw3D();
        GameObject objLine = line.GetObj();
        objLine.transform.parent = this.gameObject.transform;
        objLine.transform.localPosition = Vector3.zero;
    }

}

