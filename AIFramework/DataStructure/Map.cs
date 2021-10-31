using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//地图数据结构
//存储坐标，提供遍历方法，
//用一个数据结构（字典或者矩阵）存储砖块位置
//生成随机地图？
public class Map<T>
{
    
    //地图中心点
    //地图四个角的点 用于和相机比较，视口是否包括了整个地图
    //地图大小

    public bool randomTarget;

    private Dictionary<Vector2Int,T> dicMap = new Dictionary<Vector2Int, T>();




    public void GenMap()
    {

    }

    public void Strategy(AISearchBase st)
    {

    }

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


    public T this[Vector2Int i]
    {
        get{return dicMap[i];}
    }


     

    
}
