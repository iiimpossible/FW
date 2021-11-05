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
public class MapBase<T> where T: AIBrickState,new()
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

    /// <summary>
    /// 每个格子与其他格子的偏移
    /// </summary>
    /// <value></value>
    public Vector2 offset{get;set;}

    /// <summary>
    /// 每个格子的大小（不是砖块的大小）
    /// </summary>
    /// <value></value>
    public Vector2 gridSize{get;set;}

    /// <summary>
    /// 地图的本地坐标在世界坐标系中的原点
    /// </summary>
    /// <value></value>
    public Vector3 mapZero{get;set;}


    public float obstacleRate{get;set;}
    private Dictionary<Vector2Int,T> dicMap = new Dictionary<Vector2Int, T>();

    public MapBase(Vector2Int size)
    {
        this.size = size;
        offset = Vector2.zero;
        size = Vector2Int.zero;
        mapZero = Vector3.zero;
        gridSize = Vector2.one;
        obstacleRate = 0.4f;
    }

    
    /// <summary>
    /// 地图网格访问索引器
    /// </summary>
    /// <value></value>
    public T this[Vector2Int i]
    {
        get{return dicMap[i];}
    } 

     //生成地图
    public void GenMap(GameObject prefab, GameObject container)
    {
        if (container == null || prefab == null)
        {
            Debug.LogError("GenMap error, floor is null.");
            return;
        }
        Vector2Int pos = new Vector2Int();
        Vector3 worldPos = Vector3.zero;
        for (int column = 0; column < this.size.x; column++)
        {            
            //listBrickStates.Add(new List<AIBrickState>());
            for (int row = 0; row < this.size.y; row++)
            {
                worldPos.Set(this.gridSize.x * column + offset.x, this.gridSize.y * row + offset.y, mapZero.z);
                GameObject newGo = GameObject.Instantiate<GameObject>(prefab, worldPos, Quaternion.identity);
                if (!newGo)
                {
                    Debug.Log("Invalid prefab.");
                    return;
                }             
                newGo.transform.SetParent(container.transform);               
                pos.Set(column,row);

                T newBrick = new T();
                newBrick.InitBrick(pos,newGo);
                dicMap.Add(pos,newBrick);   
                InitBricks(newBrick);           
            }
        }
       
    }

    /// <summary>
    /// 初始化砖块，随机是否是障碍物
    /// 设置状态,必须在字典初始化这个key之后调用此方法
    /// </summary>
    /// <param name="brick"></param>
    /// <param name="blackRate"></param>
    private void InitBricks(T brick, float blackRate = 0.4f)
    {
       
        int ran = Random.Range(0, 100);
        if (ran > 100 * (1 - blackRate))
            brick.SetObstacle();
    }


    /// <summary>
    /// 获取整个的地图的四个角的格子
    /// 顺时针，0 右上
    /// </summary>
    /// <param name="i"></param>
    /// <returns></returns>
    public Vector2Int GetMapCorner(int i)
    {
        switch (i)
        {
            case 0://右上 顺时针            
                return new Vector2Int(size.x-1,size.y-1); 
            case 1://右下
                return new Vector2Int(size.x-1, 0);
            case 2://左下
                return new Vector2Int(0,0);
            case 3://左上
                return new Vector2Int(0,size.y-1);
        }
        return default(Vector2Int);
    }


    /// <summary>
    /// 获取整个地图的中心
    /// </summary>
    /// <returns></returns>
    public Vector2Int GetMapCenter()
    {
        return new Vector2Int(size.x/2 , size.y/2);
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


    /// <summary>
    /// 根据索引获取对应位置的砖块状态
    /// </summary>
    /// <param name="pos"></param>
    /// <returns></returns>
    public T GetBrickState(Vector2Int pos, EBitMask mask = EBitMask.NONE)
    { 
        if(dicMap.ContainsKey(pos))
        {
            T state =  dicMap[pos];
            if(PermissionMask.ISAllow((int)mask,state.accsessFlag))
                return state;
        } 
        return default(T);
    }
     

    /// <summary>
    /// 在地图上获取一个点用于生成AI 对象
    /// 这个在未来应该是从一个点搜索，搜索出一片空白地区，生成一个蚁巢
    /// </summary>
    /// <returns></returns>
    public Vector2Int GetSpawnPos(Vector2Int pos)
    {         
        T tbrick = this.GetBrickState(pos,EBitMask.ACSSESS | EBitMask.FOUND | EBitMask.OBSTACLE);
        for(int i= 0;i< 8;i++)
        {
            GetBrickState(tbrick.GetNeighbors(i),EBitMask.ACSSESS | EBitMask.FOUND | EBitMask.OBSTACLE).Clear();
        }
        return pos;
    }


    /// <summary>
    /// 将2D地图上的坐标转为世界空间中的3D坐标（z = 0）
    /// </summary>
    /// <param name="mapPos"></param>
    /// <returns></returns>
    public Vector3 MapSpaceToWorldSpace(Vector2Int mapPos)
    {   
        Debug.Log("Center pos ?0------>" + mapPos);
        //Maps属性：size offset girdsize mapZero
        // worldPos.x = mapZero.x + (size.x * (offset.x +gridsize.x)) 
        Debug.Log("offset.x + gridSize.x------>" + (offset.x + gridSize.x));
        return new Vector3(mapZero.x + (mapPos.x * offset.x * gridSize.x), mapZero.y + (mapPos.y * offset.y * gridSize.y)  ,0);
    }

    
}
