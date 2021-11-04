using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//地图数据结构
//存储坐标，提供遍历方法，
//用一个数据结构（字典或者矩阵）存储砖块位置
//生成随机地图？
/// <summary>
/// 地图类是一个定义了网格式地图的数据结构（容器）。
/// 1.提供某个格子的周围八个格子的坐标，格子状态由Map字典中存储的信息类实例提供
/// 2.Map提供操作格子颜色的API，格子的显示与其他类无关（是否每个格子对应一个游戏物体，还是整个地图的网格化由散列完成，或者说由一个专用着色器（可以吗）绘制整个地图）
/// 3.Map提供绘制折线的API，用于绘制路径
/// 4.关于着色器，如果使用许多的Sprite绘制地图，至少需要n*4的定点数目，如果使用一个网格，每个格子使用一个顶点，向周围的像素操作，使得颜色改变
/// </summary>
/// <typeparam name="T"></typeparam>
public class MapBase<T> where T:new()
{
    
    //地图中心点
    //地图四个角的点 用于和相机比较，视口是否包括了整个地图
    //地图大小

    public bool randomTarget;

    /// <summary>
    /// 地图的大小
    /// </summary>
    /// <value></value>
    public Vector2Int size{get;private set;}


    public Vector2 offset{get;set;}

    /// <summary>
    /// 地图的本地坐标在世界坐标系中的原点
    /// </summary>
    /// <value></value>
    public Vector3 mapZero{get;set;}

    private Dictionary<Vector2Int,T> dicMap = new Dictionary<Vector2Int, T>();

    public MapBase(Vector2Int size)
    {
        this.size = size;
        offset = Vector2.zero;
        size = Vector2Int.zero;
        mapZero = Vector3.zero;
    }

    
    /// <summary>
    /// 地图网格访问索引器
    /// </summary>
    /// <value></value>
    public T this[Vector2Int i]
    {
        get{return dicMap[i];}
    }



    /// <summary>
    /// 地图生成方法，使用该方法生成网格---砖块一一对应的地图，性能很低
    /// </summary>
    /// <param name="brick"></param>
    public void GenMap(GameObject brick)
    {

    }



     //生成地图
    public void GenMap(GameObject prefab, GameObject container, Vector2 initOffset = new Vector2())
    {
        if (container == null || prefab == null)
        {
            Debug.LogError("GenMap error, floor is null.");
            return;
        }
        Vector2Int pos = new Vector2Int();
        for (int column = 0; column < this.size.x; column++)
        {            
            //listBrickStates.Add(new List<AIBrickState>());
            for (int row = 0; row < this.size.y; row++)
            {
                GameObject newGo = GameObject.Instantiate<GameObject>(prefab, new Vector3(this.size.x * column + offset.x, this.size.y * row + offset.y, 0), Quaternion.identity);
                if (!newGo)
                {
                    Debug.Log("Invalid prefab.");
                    return;
                }             
                newGo.transform.SetParent(container.transform);               
                pos.Set(column,row);

                T newBrick = new T();
                dicMap.Add(pos,newBrick);              
            }
        }
       
    }
 

    /// <summary>
    /// 获取整个的地图的四个角的格子
    /// </summary>
    /// <param name="i"></param>
    /// <returns></returns>
    public Vector2 GetMapCorner(int i)
    {
        switch (i)
        {
            case 0://右上 逆时针
            break;
            case 1://右下
            break;
            case 2://左下
            break;
            case 3://左上
            break;
        }
        return default(Vector2);
    }


    /// <summary>
    /// 获取整个地图的中心
    /// </summary>
    /// <returns></returns>
    public Vector2Int GetMapCenter()
    {
        return Vector2Int.zero;
    }


    /// <summary>
    /// 获取一个格子的周围其他8个格子
    /// </summary>
    /// <param name="origin"></param>
    /// <param name="i"></param>
    /// <returns></returns>
    public Vector2Int GetAdjacencys(Vector2Int origin, int i)
    {
        return Vector2Int.zero;
    }



     

    
}
