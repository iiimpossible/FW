using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*
    启发函数
        f(n) = g(n) + h(n)
        f 是综合优先级
        g 是节点n距离起点的代价
        h 是节点n距离终点的预计代价
    A*算法在运算过程中，每次选择f(n)最小的节点作为下一个待遍历的节点
    A*算法使用两个集合来表示待遍历的节点与已经遍历过的节点，称之为open_set close_set

    伪代码
    * 初始化open_set和close_set；
    * 将起点加入open_set中，并设置优先级为0（优先级最高）；
    * 如果open_set不为空，则从open_set中选取优先级最高的节点n：
        * 如果节点n为终点，则：
            * 从终点开始逐步追踪parent节点，一直达到起点；
            * 返回找到的结果路径，算法结束；
        * 如果节点n不是终点，则：
            * 将节点n从open_set中删除，并加入close_set中；
            * 遍历节点n所有的邻近节点：
                * 如果邻近节点m在close_set中，则：
                    * 跳过，选取下一个邻近节点
                * 如果邻近节点m也不在open_set中，则：
                    * 设置节点m的parent为节点n
                    * 计算节点m的优先级
                    * 将节点m加入open_set中

*/
public class AIAStarSearch : AISearchBase
{
    public AIAStarSearch(Vector2Int mapSize):base(mapSize)
    {

    }

    public override IEnumerator Search()
    {
        //最小优先队列
        GraphyFW.Common.PriorityQueue<AIBrickState> q = new GraphyFW.Common.PriorityQueue<AIBrickState>();

        //源节点入队
        q.EnQueueBh(GetBirckStateDic(sourcePos));

        Vector2Int vpos = new Vector2Int();
        int max = 2000;
        GraphyFW.Common.DebugTime.StartTimer(timeTotal);
        while (q.bhCout > 0 && max != 0)
        {
             //AIBrickState u = q.DeQueue();
             AIBrickState u = q.DeQueueBh();
             u.SetAccess();

            //搜索u节点周围的f(v) = g(v) + h(v)最小的节点 g(v)是v（下一个要搜索的点）点到当前点的距离耗费，
            //h(v)是v点到目标点的实际距离的估算（有障碍物，无障碍就是精确值）
            AIBrickState v = null;
            
            for(int i = 0;i < 4; i++)
            {                
                vpos.Set(u.GetNeighbors(i).x,u.GetNeighbors(i).y);
                //这个访问周边节点要入队吗？ 优先级是动态计算还是预计算？总是可以预计算吗？
                //如果周边访问就入队，和Dijkstra就没啥分别了，就是要用h(v)来防止耗费大的入队
                 
                v = GetBirckStateDic(u.GetNeighbors(i)) ;
                if(v != null)
                {
                    v.distance =  this.ManhattanDistance(vpos,u.pos,targetPos);
                    q.EnQueueBh(v);//入队会排序 这里每次都排序，效率太低，考虑小根堆
                    v.SetFound();          
                    v.SetParent(u);         
                    //TODO:计算距离优先级并选择
                }                 
            }

             if(u.pos == targetPos)
            {
                DrawPath(u);
                 GraphyFW.Common.DebugTime.EndTimer(timeTotal);
                yield break;
            }
            max --;
             yield return new WaitForSeconds(levelDelayTime);
             
        }       
    }

 
 
    //曼哈顿距离d(i,j)=|X1-X2|+|Y1-Y2|. 即在只能水平和竖直移动的区域，两点的距离是东西方向的距离加上南北方向的距离。
    //直线距离即欧氏（几何）距离
    //对角距离

    public float ManhattanDistance(Vector2Int v, Vector2Int u, Vector2Int s)
    {
        return Mathf.Abs(v.x - u.x) + Mathf.Abs(v.y - u.y) + Mathf.Abs(s.x - v.x) + Mathf.Abs(s.y - v.y);    
    }


    public float DiagonalDistance()
    {
        return 0;
    }

    

}
