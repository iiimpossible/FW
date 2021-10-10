using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 在2D平面上的寻路算法（A*,Dj,BFS,DFS）
/// </summary>
public class AIPathFiding
{
    /// <summary>
    /// 广度优先搜索路径
    /// </summary>
    /// <param name="a">物体当前的位置</param>
    /// <param name="b">目标位置</param>
    /// <param name="step">搜索步长，影响输出路径的位置数目，并影响性能</param>
    /// <param name="stopRange">搜索范围，每步搜索多大的范围能找到合适的位置，超过范围不能找到放弃搜索</param>
    /// <returns></returns>
    public static List<Vector3> BFS(Vector3 a,Vector3 b, float step, float stopRange,bool drawLine)
    {
        //对于目标位置是否合适，需要一个方法查询

        //获取从a 到 b 的向量

        /*
         思路：直接以步长为半径计算一个坐标，
              检测该坐标是否合法
                是：返回该坐标
                否：以二分法计算一个合法坐标并返回         
         */

        float distance = Vector3.Distance(a, b);
        Vector3 pos = Vector3.Normalize( a - b) * stopRange;//获取坐标点

        //检测当前坐标点是否合适
        //TODO：
        //while(CheckPosValid(pos))
        //{

        //}




        return new List<Vector3>();
    }

    public static void DFS(Vector3 targetPos)
    {

    }

    public static void AStar(Vector3 targetPos)
    {

    }

    public static void Dj(Vector3 targetPos)
    {

    }


}
