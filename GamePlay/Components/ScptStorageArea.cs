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

    Vector2Int _start;//是图形左下角

    //放框右下角
    Vector2Int _end;

    Vector3 worldStart;

    Vector3 wroldEnd;

    //0表示没有，其他数字表示数量
    private Dictionary<Vector2Int, int> _dicState = new Dictionary<Vector2Int, int>();

    [SerializeField]
    private int totalCount = 0;

    private bool isFull = false;

    private bool isInited = false;
    private void OnRenderObject()
    {
        if(!isInited) return;
        DrawRunTimeShape.DrawQuad(worldStart,wroldEnd,color,material);
    }

    /// <summary>
    /// 通过给定的center和size计算区域左下角位置
    /// </summary>
    /// <param name="center"></param>
    /// <param name="size"></param>
    public void InitArea(Vector2Int center, Vector2Int size)
    {
        Debug.Log($"Cener: {center} size: {size}");
        color = new Color(0,1,0,0.1f);
        _mapCenter.Set(Mathf.RoundToInt(center.x), Mathf.RoundToInt(center.y));
        _mapSize.Set(Mathf.RoundToInt(size.x), Mathf.RoundToInt(size.y));

        _start = new Vector2Int(_mapCenter.x - _mapSize.x / 2, _mapCenter.y - _mapSize.y / 2);//这个是左下角

        //需要获得 从地图坐标转为世界坐标的 左上角（start）坐标
        //需要获得 从地图坐标转为世界坐标的 右下角（end）坐标

        Vector2Int mapStart = new Vector2Int(center.x - size.x /2, center.y + size.y/2);
        Vector2Int mapEnd = new Vector2Int(center.x + size.x/2, center.y - size.y/2);

        worldStart = new Vector3(_mapCenter.x - _mapSize.x / 2, _mapCenter.y + _mapSize.y / 2,0);//这个是左上角
        wroldEnd = new Vector2();//这个是右下角

        worldStart = GameMode.instance.GetMap().MapSpaceToWorldSpace(mapStart);
        wroldEnd = GameMode.instance.GetMap().MapSpaceToWorldSpace(mapEnd);

        for (int i = _start.x; i <= (_start.x + _mapSize.x); i++)
        {
            for (int j = _start.y; j <= (_start.y + _mapSize.y); j++)
            {
                _dicState.Add(new Vector2Int(i, j), 0);
            }
        }
        isInited = true;
    }

    public Vector2Int GetEmptyPos()
    {
        Debug.Log($"Start: {_start} End: {_end}");
        Vector2Int pos = Vector2Int.zero;
        for (int i = _start.x; i <= (_start.x + _mapSize.x); i++)
        {
            for (int j = _start.y; j <= (_start.y + _mapSize.y); j++)
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

    public void SetPosEmpty(Vector2Int pos)
    {
        if (!Valid(pos)) return;
        _dicState[pos] = 0;
    }

    public bool Full()
    {
        return totalCount == _dicState.Count ? true : false;
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
