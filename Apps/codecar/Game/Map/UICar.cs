using System.Collections;
using System.Collections.Generic;
using LitJson;
using UnityEngine;
using UnityEngine.UI;

public interface ICar
{
    void CarUpdateStatus(UICar car, CarMotion.RunStatus status);
}

public class UICar : UIMapItem, ICarMotion
{
    CodeCarItemInfo itmeInfoGuanka;//当前关卡
    CarMotion carMotion;

    public int mapSizeX;
    public int mapSizeY;
    //瓦片坐标
    public int tileX;
    public int tileY;

    public int tileXPre;//上一个位置
    public int tileYPre;
    public Vector2 sizeRect;
    public Vector3 localPositionInit;
    public UICmdBarRun uiCmdBarRun;
    public ICar iCarDelegate;

    int indexCmd = 0;
    CarMotion.Move[] listMoveNow = new CarMotion.Move[4];

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        base.Awake();

        enableMove = false;

        carMotion = this.gameObject.AddComponent<CarMotion>();
        carMotion.objCar = this.gameObject;
        carMotion.iDelegate = this;
    }

    void Start()
    {

    }

    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {

    }
    public void Reset()
    {
        carMotion.OnCarStop();
        indexCmd = 0;
        this.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 0));
        this.transform.localPosition = localPositionInit;
    }
    public void SetMapSize(int x, int y)
    {
        mapSizeX = x;
        mapSizeY = y;
        //carMotion.mapSizeX = x;
        //carMotion.mapSizeY = y;
    }
    public void UpdateGuankaItem(CodeCarItemInfo info)
    {
        itmeInfoGuanka = info;
    }

    bool UpdateRoadTilePostion()
    {
        bool ret = false;
        int x = GetRoadTileXOfCar();
        int y = GetRoadTileYOfCar();
        if (x != tileX)
        {
            ret = true;

            //x,y需要同时更新
            tileXPre = tileX;
            tileYPre = tileY;

            tileX = x;
        }
        if (y != tileY)
        {
            ret = true;
            //x,y需要同时更新
            tileXPre = tileX;
            tileYPre = tileY;

            tileY = y;
        }
        return ret;
    }
    public void OnRun()
    {
        indexCmd = 0;
        tileX = GetRoadTileXOfCar();
        tileY = GetRoadTileYOfCar();
        tileXPre = tileX;
        tileYPre = tileY;
        carMotion.runMode = CarMotion.RunMode.CMD;//AUTO CMD

        UpdateRoadTilePostion();
        //初始化move方向
        carMotion.direction = CarMotionGetMoveDirection(carMotion);
        //carMotion.direction = CarMotion.Move.RIGHT;

        carMotion.OnCarRun();
    }
    CarMotion.Move GetCmdDirection(int idx)
    {
        CarMotion.Move ret = CarMotion.Move.NONE;
        if (idx < uiCmdBarRun.listItem.Count)
        {
            UICmdItem item = uiCmdBarRun.listItem[idx] as UICmdItem;
            switch (item.cmdType)
            {
                case UICmdItem.CmdType.START:
                    ret = CarMotion.Move.RIGHT;
                    break;
                case UICmdItem.CmdType.LEFT:
                    ret = CarMotion.Move.LEFT;
                    break;
                case UICmdItem.CmdType.RIGHT:
                    ret = CarMotion.Move.RIGHT;
                    break;
                case UICmdItem.CmdType.UP:
                    ret = CarMotion.Move.UP;
                    break;
                case UICmdItem.CmdType.DOWN:
                    ret = CarMotion.Move.DOWN;
                    break;
                case UICmdItem.CmdType.NONE:
                    ret = CarMotion.Move.NONE;
                    carMotion.runStatus = CarMotion.RunStatus.NO_CMD;
                    if (iCarDelegate != null)
                    {
                        iCarDelegate.CarUpdateStatus(this, carMotion.runStatus);
                    }
                    break;
                default:

                    break;

            }
        }

        return ret;
    }

    //实时获取瓦片位置
    int GetRoadTileXOfCar()
    {
        int x = MapUtils.GetXOfPositionCenter(this.transform.localPosition, mapSizeX, mapSizeY, sizeRect);

        return x;
    }
    int GetRoadTileYOfCar()
    {
        int y = MapUtils.GetYOfPositionCenter(this.transform.localPosition, mapSizeX, mapSizeY, sizeRect);
        return y;
    }
    MapTileInfo GetRoadTileInfo(int x, int y)
    {
        MapTileInfo inforet = null;
        if ((x < 0) || (x >= mapSizeX) || (y < 0) || (y >= mapSizeY))
        {
            return null;
        }
        foreach (MapTileInfo info in itmeInfoGuanka.listTile)
        {
            if (info.type == UIMapItem.ItemType.ROAD_TILE)
            {
                if ((x == info.tileX) && (y == info.tileY))
                {
                    inforet = info;
                    break;
                }
            }
        }
        return inforet;
    }


    //获取当前位置左右上下的道路tile，如果不存在返回null
    MapTileInfo GetRoadTileLeft()
    {
        MapTileInfo info = GetRoadTileInfo(tileX - 1, tileY);
        return info;
    }
    MapTileInfo GetRoadTileRight()
    {
        MapTileInfo info = GetRoadTileInfo(tileX + 1, tileY);
        return info;
    }
    MapTileInfo GetRoadTileTop()
    {
        MapTileInfo info = GetRoadTileInfo(tileX, tileY + 1);
        return info;
    }
    MapTileInfo GetRoadTileBottom()
    {
        MapTileInfo info = GetRoadTileInfo(tileX, tileY - 1);
        return info;
    }

    //上一个tile的方向
    CarMotion.Move GePretRoadTileDirection()
    {
        CarMotion.Move ret = CarMotion.Move.NONE;
        if ((tileXPre < tileX) && (tileYPre == tileY))
        {
            ret = CarMotion.Move.LEFT;
        }
        if ((tileXPre > tileX) && (tileYPre == tileY))
        {
            ret = CarMotion.Move.RIGHT;
        }

        if ((tileXPre == tileX) && (tileYPre > tileY))
        {
            ret = CarMotion.Move.UP;
        }
        if ((tileXPre == tileX) && (tileYPre < tileY))
        {
            ret = CarMotion.Move.DOWN;
        }

        return ret;
    }

    //统计可走的“方向”总数
    int GetAllMoveDirection(CarMotion.Move[] listMove)
    {
        MapTileInfo infoLeft = GetRoadTileLeft();
        MapTileInfo infoRight = GetRoadTileRight();
        MapTileInfo infoTop = GetRoadTileTop();
        MapTileInfo infoBottom = GetRoadTileBottom();
        //统计可走的“方向”总数
        int total = 0;

        if ((infoLeft != null) && (GePretRoadTileDirection() != CarMotion.Move.LEFT))
        {
            listMove[total] = CarMotion.Move.LEFT;
            total++;
        }
        if ((infoRight != null) && (GePretRoadTileDirection() != CarMotion.Move.RIGHT))
        {
            listMove[total] = CarMotion.Move.RIGHT;
            total++;
        }

        if ((infoTop != null) && (GePretRoadTileDirection() != CarMotion.Move.UP))
        {
            listMove[total] = CarMotion.Move.UP;
            total++;
        }

        if ((infoBottom != null) && (GePretRoadTileDirection() != CarMotion.Move.DOWN))
        {
            listMove[total] = CarMotion.Move.DOWN;
            total++;
        }
        return total;
    }
    //判断命令是否错误
    public bool IsCmdError(CarMotion motion, CarMotion.Move cmdDirection, int total)
    {
        bool ret = true;
        for (int i = 0; i < total; i++)
        {
            CarMotion.Move direction = listMoveNow[i];
            if (cmdDirection == direction)
            {
                ret = false;
            }
        }

        return ret;
    }
    //判断是否是拐弯点
    public bool IsCornerItem(CarMotion motion)
    {
        bool ret = false;
        CarMotion.Move[] listMove = new CarMotion.Move[4];
        int total = GetAllMoveDirection(listMove);
        if (total > 1)
        {
            ret = true;
        }
        else if (total == 1)
        {
            if (listMove[0] != motion.direction)
            {
                ret = true;
            }
        }
        return ret;
    }
    CarMotion.Move GetOneMoveDirection(CarMotion.Move[] listMove, int total)
    {
        CarMotion.Move ret = carMotion.direction;
        int idx = Random.Range(0, total);
        //随机选一个方向走
        ret = listMove[idx];
        return ret;
    }
    #region ICarMotion 
    public bool CarMotionIsNearCenter(CarMotion motion)
    {
        bool ret = false;
        Vector3 posCenter = MapUtils.GetPositionCenter(tileX, tileY, mapSizeX, mapSizeY, sizeRect);
        motion.UpdateRoadTileCenter(posCenter);
        bool isNeedUpdate = UpdateRoadTilePostion();


        float x, y, w, h;
        int vaule = 4;
        w = motion.moveStep * vaule;
        h = motion.moveStep * vaule;
        x = posCenter.x - w / 2;
        y = posCenter.y - h / 2;
        Rect rc = new Rect(x, y, w, h);
        posCenter.z = this.transform.localPosition.z;
        Vector2 pos = this.transform.localPosition;
        // Debug.Log("CarMotionIsNearCenter:tileX="+tileX+" rc="+rc+" posworld="+posworld+" pos="+pos+" sizeRect="+sizeRect);
        if (rc.Contains(pos))
        {
            // Debug.Log("CarMotionIsNearCenter:ret = true");
            //强制设置成中心位置
            //this.transform.localPosition = posworld;
            ret = true;
            if (motion.runMode == CarMotion.RunMode.CMD)
            {
                bool isCorner = IsCornerItem(motion);
                if (isCorner)
                {
                    //经过转弯角
                    if (carMotion.direction != CarMotion.Move.NONE)
                    {
                        //切换到下个命令
                        indexCmd++;
                    }

                }
            }

        }

        return ret;
    }
    public CarMotion.Move CarMotionGetMoveDirection(CarMotion motion)
    {
        CarMotion.Move ret = motion.direction;

        // bool isNeedUpdate = UpdateRoadTilePostion();
        // if (!isNeedUpdate)
        // {
        //  return ret;
        // }


        int total = GetAllMoveDirection(listMoveNow);
        if (carMotion.runMode == CarMotion.RunMode.AUTO)
        {
            if (total > 1)
            {
                Debug.Log("CarMotionGetMoveDirection:total=" + total + " carMotion.direction=" + carMotion.direction + " now=(" + tileX + " ," + tileY + ") pre=(" + tileXPre + "," + tileYPre + ")");
            }
            if (total > 0)
            {
                ret = GetOneMoveDirection(listMoveNow, total);

            }

        }
        else if (carMotion.runMode == CarMotion.RunMode.CMD)
        {
            ret = GetCmdDirection(indexCmd);
            Debug.Log("GetCmdDirection::ret=" + ret + " indexCmd=" + indexCmd);
            if (IsCmdError(motion, ret, total))
            {
                //命令错误 停止运动
                Debug.Log("cmd is wrong,stop...");
                ret = CarMotion.Move.NONE;
                carMotion.runStatus = CarMotion.RunStatus.ERROR;
                if (iCarDelegate != null)
                {
                    iCarDelegate.CarUpdateStatus(this, carMotion.runStatus);
                }
            }
        }

        //运动停止
        if (total == 0)
        {
            //无路可走
            Debug.Log("no road to go...");
            ret = CarMotion.Move.NONE;
            carMotion.runStatus = CarMotion.RunStatus.STOP;
            if (iCarDelegate != null)
            {
                iCarDelegate.CarUpdateStatus(this, carMotion.runStatus);
            }

        }

        return ret;
    }
    #endregion
}

