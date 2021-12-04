using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 存储区绘制
/// 1.材质
/// 2.绘制方法
/// 3.重绘
///
/// 2021.11.25
/// 1.在OnRnederObject里边绘制图形
/// 2.输入存储区center，和size，然后计算处原点（0，0）即右下角，
/// 3.根据顶点绘制出方形
/// </summary>
public class ScptStorageArea : MonoBehaviour
{

    [SerializeField]
    [Tooltip("Whether or not draw storage area")]
    private bool drawArea = true;

    [SerializeField]
    private Material material = null;

    [SerializeField]
    [Tooltip("Size in world space")]
    private Vector2 size = Vector2.zero;

    [SerializeField]
    [Tooltip("Center in world space")]
    private Vector2 center = Vector2.zero;

    [SerializeField]
    [Tooltip("Storage area color")]
    private Color color = Color.green;

    [SerializeField]
    [Tooltip("Center in map space")]
    private Vector2Int _mapCenter = Vector2Int.zero;

    [SerializeField]
    [Tooltip("Center in map space")]
    private Vector2Int _mapSize = Vector2Int.zero;

    [SerializeField]
    [Tooltip("区域左上角")]
    /// <summary>
    /// 区域左上角（遍历起始点）
    /// </summary>
    private Vector2Int areaStart = Vector2Int.zero;

    [SerializeField]
    [Tooltip("区域右下角")]
    /// <summary>
    /// 区域右下角（遍历终点）
    /// </summary>
    private Vector2Int areaEnd = Vector2Int.zero;

    [SerializeField]
    [Tooltip("区域的大小")]
    /// <summary>
    /// 区域大小
    /// </summary>
    private Vector2Int areaSize = Vector2Int.zero;

    private Vector3 worldStart;

    private Vector3 wroldEnd;

    //0表示没有，其他数字表示数量
    private Dictionary<Vector2Int, int> _dicState = new Dictionary<Vector2Int, int>();

    [SerializeField]
    private int totalCount = 0;

    private bool isFull = false;

    private bool isInited = false;
    private void OnRenderObject()
    {
        if (!isInited) return;
        DrawRunTimeShape.DrawQuad(worldStart, wroldEnd, color, material);
    }

    /// <summary>
    /// 通过给定的center和size计算区域左下角位置
    /// </summary>
    /// <param name="center"></param>
    /// <param name="size"></param>
    public void InitArea(Vector2Int center, Vector2Int size)
    {
        //Debug.Log($"Cener: {center} size: {size}");
        color = new Color(0, 1, 0, 0.1f);
        // _mapCenter.Set(Mathf.RoundToInt(center.x), Mathf.RoundToInt(center.y));
        // _mapSize.Set(Mathf.RoundToInt(size.x), Mathf.RoundToInt(size.y));

        // _start = new Vector2Int(_mapCenter.x - _mapSize.x / 2, _mapCenter.y - _mapSize.y / 2);//这个是左下角

        //需要获得 从地图坐标转为世界坐标的 左上角（start）坐标
        //需要获得 从地图坐标转为世界坐标的 右下角（end）坐标

        areaStart = new Vector2Int(center.x - size.x / 2, center.y + size.y / 2);
        areaEnd = new Vector2Int(center.x + size.x / 2, center.y - size.y / 2);
        areaSize.Set(Mathf.Abs(areaEnd.x - areaStart.x + 1), Mathf.Abs(areaStart.y - areaEnd.y + 1));//是封闭区间，应该加1   

        worldStart = GameMode.instance.GetMap().MapSpaceToWorldSpace(areaStart) + new Vector3(-0.5f, 0.5f, 0);
        wroldEnd = GameMode.instance.GetMap().MapSpaceToWorldSpace(areaEnd) + new Vector3(0.5f, -0.5f, 0);
        Debug.LogWarning($"mapStart: {areaStart}, mapEnd: {areaEnd} ------ worldStart: {worldStart}, worldEnd: {wroldEnd}");

         for (int i = areaStart.x; i <= areaEnd.x; i++)
        {
            for (int j = areaStart.y; j >= areaEnd.y; j--)
            {
                _dicState.Add(new Vector2Int(i, j), 0);
            }
        }
        isInited = true;
    }

    /// <summary>
    /// 从该区域获取一个空余位置
    /// </summary>
    /// <returns></returns>
    public Vector2Int GetEmptyPos()
    {
        Debug.Log($"Start: {areaStart} End: {areaEnd}");
        Vector2Int pos = Vector2Int.zero;
        for (int i = areaStart.x; i <= areaEnd.x; i++)
        {
            for (int j = areaStart.y; j >= areaEnd.y; j--)
            {
                pos.Set(i, j);
                //Debug.Log(_dicState[pos]);
                if (_dicState[pos] == 0)
                {
                    totalCount++;
                    _dicState[pos]++;
                    return pos;
                }
            }
        }
        Debug.LogError($"Get area pos Faild: area size: {_mapSize} ");
        //isFull = true;
        return Vector2Int.zero;
    }

    /// <summary>
    /// 当从区域搬走东西的时候，应该将该位置设置为空
    /// </summary>
    /// <param name="pos"></param>
    public void SetPosEmpty(Vector2Int pos)
    {
        if (!Valid(pos)) return;
        _dicState[pos] = 0;
    }

    /// <summary>
    /// 检测该区域是否被填满
    /// TODO：不应该直接遍历，应该使用一个Int 值记录该区域是否被填满
    /// </summary>
    /// <returns></returns>
    public bool Full()
    {
        Vector2Int pos = Vector2Int.zero;
       for (int i = areaStart.x; i <= areaEnd.x; i++)
        {
            for (int j = areaStart.y; j >= areaEnd.y; j--)
            {
                pos.Set(i, j);
                if (_dicState[pos] == 0)
                {
                    return false;
                }
            }
        }
        Debug.Log("Storage area is full.");
        return true;

    }

    private bool Valid(Vector2Int pos)
    {
        if (pos.x < _mapSize.x && pos.x >= 0)
        {
            if (pos.y < _mapSize.y && pos.y >= 0)
            {
                return true;
            }
        }
        return false;
    }


}
