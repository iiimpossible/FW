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

    public float blackRate{get;set;}

    public bool randomTarget{get;set;}

    /// <summary>
    /// 地图的大小
    /// </summary>
    /// <value></value>
    public Vector2Int size{get;private set;}

    /// <summary>
    /// 地图在世界坐标中的大小
    /// </summary>
    /// <value></value>
    public Vector2 worldSize{get;private set;}

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
    /// 砖块大小
    /// </summary>
    /// <value></value>
    public float brickSize{get;private set;}

    /// <summary>
    /// 地图的本地坐标在世界坐标系中的原点
    /// </summary>
    /// <value></value>
    public Vector3 mapZero{get;set;}
    public float obstacleRate{get;set;}  //生成障碍物的概率 
    private Dictionary<Vector2Int,T> dicMap = new Dictionary<Vector2Int, T>();//地图每个格子的状态
    private Vector3 toWorldPos = Vector3.zero;//转世界坐标临时变狼
    private Vector2Int toMapPos = Vector2Int.zero;//转地图坐标临时变量

    public MapBase(Vector2Int aSize)
    {
        Debug.Log("isize is--->" + aSize);
        this.size = aSize;
        offset = Vector2.one;       
        mapZero = Vector3.zero;
        gridSize = Vector2.one;
        obstacleRate = 0.4f;

        worldSize = new Vector2(size.x * offset.x * gridSize.x, size.y * offset.y * gridSize.y); //计算整个地图的实际长度      
        brickSize = gridSize.x;
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
        //初步随机
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
                //因为就算是0列/行，它也加了一个offset 得处理一下，
                worldPos.Set(this.gridSize.x * column +  offset.x, this.gridSize.y * row + offset.y, mapZero.z);
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
                InitBricks(newBrick,blackRate);           
            }
        }

        //噪声消除处理
        NoiseElimination();       

        //巢穴生成

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
        Vector2Int p  ;
        T tbrick = this.GetBrickState(pos,EBitMask.ACSSESS | EBitMask.FOUND | EBitMask.OBSTACLE);
        if(tbrick == null)
        {
            //Debug.Log("GetSpawnPos Invalid pos--->" + pos);
            return pos;
        }

        tbrick.Clear();
        for(int i= 0;i< 8;i++)
        {
            p = tbrick.GetNeighborsDiagnol(i);
            if(!IsValidInMap(p))continue;
            GetBrickState(p,EBitMask.ACSSESS | EBitMask.FOUND | EBitMask.OBSTACLE).Clear();           
        }  
       // Debug.Log("GetSpawnPOs:--->"+ pos);
        return pos;
    }


    /// <summary>
    /// 将2D地图上的坐标转为世界空间中的3D坐标（z = 0）
    /// </summary>
    /// <param name="mapPos"></param>
    /// <returns></returns>
    public Vector3 MapSpaceToWorldSpace(Vector2Int mapPos)
    {   
        //Debug.Log("[MapSpaceToWorldSpace]  map pos is----->" + mapPos);
        toWorldPos.Set(mapZero.x + (mapPos.x  * gridSize.x + offset.x), mapZero.y + (mapPos.y *   gridSize.y + offset.y)  ,0);
        //Debug.Log("[MapSpaceToWorldSpace]  world pos is----->" + toWorldPos);
        //Maps属性：size offset girdsize mapZero    
        return toWorldPos;
    }

    /// <summary>
    /// 将世界3D坐标转为地图上的坐标，并进行范围限制，将移动范围控制在这个方法
    /// </summary>
    /// <param name="worldPos"></param>
    /// <returns></returns>
    public Vector2Int WorldSpaceToMapSpace(Vector3 worldPos)
    {
        //Debug.Log("[WorldSpaceToMapSpace]  world pos is---->"+ worldPos);
        //计算地图的世界坐标范围   
        //1.求w到m的相对距离
        //判断是否在地图范围中
        if (IsValidInMap(worldPos.x - mapZero.x, worldPos.y - mapZero.y))
        {          
            //假设输入相对坐标（16.5，18.5）/（1，1） (16,18) 问题 需要将地图的索引定为1开始？
            toMapPos.Set( Mathf.RoundToInt((worldPos.x-mapZero.x / brickSize)) -1,  Mathf.RoundToInt(worldPos.y-mapZero.y / brickSize)-1);//这里有问题，如果是小数就有舍入误差了，避免的方法是，地图远点不应该有小数，CellSize也不应该为小数
            //Debug.Log("[WorldSpaceToMapSpace]  Map pos is----->" + toMapPos);
            return toMapPos;
        }
        Debug.Log("[WorldSpaceToMapSpace]  Worldpos is not valid.");
        //不在地图中，返回中心      
        return this.GetMapCenter();
    }

    /// <summary>
    /// 判断是否在地图范围内
    /// </summary>
    /// <param name="worldPos"></param>
    /// <returns></returns>
    public bool IsValidInMap(float x, float y)
    {
        //x不在范围中
        if (x > worldSize.x || x < 0)
        {
            return false;
        }
        else if (y > worldSize.y || y < 0)//y不再范围中
        {
            return false;
        }

        return true;
    }


    /// <summary>
    /// 判断一个地图空间的点是否合法
    /// </summary>
    /// <param name="mapPos"></param>
    /// <returns></returns>
    public bool IsValidInMap(Vector2Int mapPos)
    {
        if(mapPos.x >= size.x || mapPos.x < 0)
        {
            return false;
        }
        else if(mapPos.y >= size.y || mapPos.y < 0)
        {
            return false;   
        }
        return true;
    }

    /// <summary>
    /// 获取系统时间，并将时间字符串的哈希码作为随机种子传入（所以Rimworld的种子字符串就是这么来的吧）
    /// </summary>
    private void SetRandomSeed()
    {
        string seed;
        seed = System.DateTime.Now.ToString();
        System.Random pseudoRandom = new System.Random(seed.GetHashCode());
    }

    private int GetNeighborsObstacleNum(Vector2Int pos)
    {
        AIBrickState st = this.GetBrickState(pos,EBitMask.ACSSESS | EBitMask.FOUND | EBitMask.OBSTACLE);
        if(st == null) return 0;
        int counter = 0;
        for(int i = 0; i < 8; i++)
        {
            var v =  GetBrickState(st.GetNeighborsDiagnol(i),EBitMask.ACSSESS | EBitMask.FOUND | EBitMask.OBSTACLE); 
            if(v == null) continue;
            if(v.isObstacle == true)
            {
                counter++;
            }
        }
        if(st.isObstacle == true) counter ++ ;
        return counter;
    }

    /// <summary>
    /// 消除随机地图噪点（合并孤立点，其实就是重新填充，障碍越集中，那么要它全部填充为障碍，障碍越少的地方，让它没有障碍）
    /// 1.获取一个点周围的障碍数量
    /// 2.如果障碍数量大于 value 那么
    /// 3.应该是一遍合并障碍，
    /// </summary>
    public void NoiseElimination()
    {
        int count = 0;
        //第一遍，填充障碍聚集区域
        ForeachMap((brick) =>
        {
            if (brick == null) return;
            count = GetNeighborsObstacleNum(brick.pos);
            Debug.Log("Count is--->" + count);
            if (count > 4)
            {
                brick.SetObstacle();
            }
             if (brick.isObstacle == true && count < 4)
            {
                brick.Clear();                
            }
        });

        //第二遍，消除孤立的障碍
        ForeachMap((brick) =>
        {
             if (brick == null) return;
            count = GetNeighborsObstacleNum(brick.pos);
            Debug.Log("Count is--->" + count);
            if (count > 4)
            {
                brick.SetObstacle();
            }
             if (brick.isObstacle == true && count < 4)
            {
                brick.Clear();                
            }
           
        });


        // Vector2Int pos = Vector2Int.zero;
        // for (int k = 0; k < 3; k++)
        // {
        //     for (int i = 0; i < this.size.x; i++)
        //     {
        //         for (int j = 0; j < this.size.y; j++)
        //         {
        //             pos.Set(i, j);
        //             var brick = GetBrickState(pos, EBitMask.ACSSESS | EBitMask.FOUND | EBitMask.OBSTACLE);
        //             if (brick == null) continue;
        //             count = GetNeighborsObstacleNum(brick.pos);
        //             Debug.Log("Count is--->" + count);
        //             if (brick.isObstacle == false && count > 4)
        //             {
        //                 brick.SetObstacle();
        //                 yield return new WaitForSeconds(0.0001f);
        //             }
        //             if (brick.isObstacle == true && count < 3)
        //             {
        //                 brick.Clear();
        //                 yield return new WaitForSeconds(0.001f);
        //             }
        //         }
        //     }
        // }
        // yield return 0;
    }

    /// <summary>
    /// 遍历整个地图，提供一个回调函数控制砖块的状态
    /// </summary>
    /// <param name="action"></param>
    private void ForeachMap(System.Action<AIBrickState> action)
    {
        Vector2Int pos = Vector2Int.zero;
        for (int i = 0; i < this.size.x; i++)
        {
            for (int j = 0; j < this.size.y; j++)
            {
                pos.Set(i, j);
                action.Invoke(GetBrickState(pos, EBitMask.ACSSESS | EBitMask.FOUND | EBitMask.OBSTACLE));
            }
        }
    }


}
