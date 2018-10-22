using System.Collections;
using System.Collections.Generic;
using LitJson;
using UnityEngine;
using UnityEngine.UI;
public interface ICarMotion
{
    CarMotion.Move CarMotionGetMoveDirection(CarMotion motion);
    bool CarMotionIsNearCenter(CarMotion motion); 
}
public class CarMotion : MonoBehaviour
{
    public enum Move
    {
        NONE = 0,
        RIGHT,
        LEFT,
        UP,
        DOWN

    }

    public enum RunMode
    {
        NONE = 0,
        AUTO,//自动驾驶
        CMD//根据命令驾驶 

    }

    public enum RunStatus
    {
        NONE = 0,//
        ERROR,//方向错误
        NO_CMD,//当前barrun所在item cmdtype为none
        STOP//停止

    }

    public GameObject objCar;
    public ICarMotion iDelegate;
    // public Move directionFrom = Move.NONE;
    public Move direction = Move.NONE;
    public RunMode runMode = RunMode.NONE;
    public RunStatus runStatus = RunStatus.NONE;
    public float moveStep = 0.1f;
    bool isRun = false;
    float timerRun = 0.1f;
    Vector2 posRoadTileCenter = Vector2.zero;//车子所在位置对应的道路瓦片中心位置

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {

    }

    void Start()
    {
    }

    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {

    }

    void OnTimer()
    {
        if (isRun)
        {
            DoRun();
        }
    }

    void DoRun()
    {
        float x, y, z;
        Vector3 pos = objCar.transform.localPosition;
        x = pos.x;
        y = pos.y;
        z = pos.z;

        //在中心点附近check是否转向

        if (IsNearCenter())
        {
            if (iDelegate != null)
            {
                direction = iDelegate.CarMotionGetMoveDirection(this);
            }
        }


        switch (direction)
        {
            case Move.RIGHT:
                x += moveStep;
                //矫正位置 让沿着它在中心线运动
                y = posRoadTileCenter.y;
                break;
            case Move.LEFT:
                x -= moveStep;
                //矫正位置 让沿着它在中心线运动
                y = posRoadTileCenter.y;
                break;
            case Move.UP:
                y += moveStep;
                //矫正位置 让沿着它在中心线运动
                x = posRoadTileCenter.x;
                break;
            case Move.DOWN:
                y -= moveStep;
                //矫正位置 让沿着它在中心线运动
                x = posRoadTileCenter.x;
                break;
            case Move.NONE:
                //矫正位置 让它停在中心
                x = posRoadTileCenter.x;
                y = posRoadTileCenter.y;
                break;
        }
        objCar.transform.localPosition = new Vector3(x, y, z);
        UpdateCarAngle();
    }

    //
    bool IsNearCenter()
    {
        bool ret = false;
        if (iDelegate != null)
        {
            ret = iDelegate.CarMotionIsNearCenter(this);
        }
        return ret;
    }

    public void UpdateRoadTileCenter(Vector2 pos)
    {
        posRoadTileCenter = pos;
    }

    void UpdateCarAngle()
    {
        float anglez = 0;
        if (direction == Move.NONE)
        {
            return;
        }
        //Quaternion angle = objCar.transform.localRotation;
        SpriteRenderer rd = objCar.GetComponent<SpriteRenderer>();
        if (rd != null)
        {
            rd.flipX = false;
        }
        switch (direction)
        {
            case Move.LEFT:
                anglez = 0;
                //x轴反转
                if (rd != null)
                {
                    rd.flipX = true;
                }

                break;
            case Move.RIGHT:
                anglez = 0;
                break;
            case Move.UP:
                anglez = 90;
                break;
            case Move.DOWN:
                anglez = 270;
                break;
        }
        objCar.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, anglez));
    }
    Move GetPointDirection(Vector2 pt1, Vector2 pt2)
    {
        Move ret = Move.RIGHT;
        if (pt2.x > pt1.x)
        {
            ret = Move.RIGHT;
        }
        if (pt2.x < pt1.x)
        {
            ret = Move.LEFT;
        }
        if (pt2.y > pt1.y)
        {
            ret = Move.UP;
        }
        if (pt2.y < pt1.y)
        {
            ret = Move.DOWN;
        }

        return ret;
    }



    public void OnCarRun()
    {
        isRun = true;
        // tileX = GetTileXOfCar();
        //  tileY = GetTileYOfCar();
        InvokeRepeating("OnTimer", 0, timerRun);
    }
    public void OnCarStop()
    {
        isRun = false;
        CancelInvoke("OnTimer");
    }
}

