using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GraphyFW.Common;

//Dijkstra 算法解决的是带权重的有向图中的单源最短路径问题。该算法要求所有变边的权重都为非负值
//松弛操作：对于每个节点v来说，维持一个属性v.d 用来记录从源节点s到节点v的最短路径权重的上界
//称v.d为s到v的最短路径估计
//对一条边的松弛操作为：首先测试一下是否可以对从s到v的最短路径进行改善，
public class AIDijkstraSearch : AISearchBase
{
    public AIDijkstraSearch (Vector2Int mapSize):base(mapSize)
    {
        
    }
   
    
    //测试从s到v的最短路径进行改善，
    /*
        Relax(u,v,w)
        {
            if(v.d > u.d + w(u,v))
            {
                v.d = u.d + w(u,v)
                v.pai = u
            }
        }

    */


    /*
        伪码：
        DIJSKTRA(G,w,s)
        {
            Initialize(G,s)                     值初始化，值为0
            S = null                            S集合为空
            Q = G.v                             优先队列将所有无向图节点入队
            while Q != null                     循环，注意一开始u = s
                u = Extract-Min(Q)              从优先队列Q中抽取节点u
                S = S.Add(u)                    将u加入到S集合
                foreach vertex v in G.Adj[u]    对所有从节点u出发的边做松弛操作 v点是u点周围的能访问到的节点？ 计算v点 到u点的代价？
                    Relax(u,v,w)
        }
    
    */
    /*在Dijkstra算法中，需要计算每一个节点距离起点的总移动代价。同时，还需要一个优先队列结构。对于所有待遍历的节点，放入优先队列中会按照代价进行排序。*/

    private void Relax(AIBrickState u, AIBrickState v)
    {
        if( (u == null) || (v == null) )return ;
        float pathL = u.distance + v.weight;
        if(v.distance > pathL)
        {
            v.distance = pathL;
            v.SetParentState(u);
            v.SetFound(); 
            Debug.Log("relax v.distance------->" + v.distance);           
        }       
    }

   public override IEnumerator Search()
   {
       //最小优先队列
       PriorityQueue<AIBrickState> pque = new PriorityQueue<AIBrickState>();
       List<AIBrickState> list = new List<AIBrickState>();

        var s = GetBirckStateDic(sourcePos,EBitMask.OBSTACLE | EBitMask.ACSSESS | EBitMask.FOUND);
        pque.EnQUeue(s);

        Vector2Int pos = new Vector2Int();

        //将所有的节点都入队 目前权重都是1，要随机显示一个权重到颜色上
        //这个操作不需要每次搜索都执行，实际上是一次性预处理的，有改动可以直接更新
        for(int i = 0 ; i < mapSize.x; i ++ )
        {
            for(int j = 0 ; j < mapSize.y; j++)
            {
                pos.Set(i,j);
                AIBrickState tstate = dicBrickStates[pos];
                tstate.distance = 1e10f;
                //if(pos == sourcePos)tstate.distance = 0;
                pque.EnQUeue(dicBrickStates[pos]);
            }
        }
        //pque.Watch();

        AIBrickState u;
        
        while( pque.Count > 0)
        {
            Debug.Log("The first priority is----------->"+ pque.First().distance);
            u = pque.DeQueue();            
            u.SetAccess();           
         
            for(int i = 0;i<4;i++)
            {
                AIBrickState v = GetBirckStateDic(u.GetNeighbors(i)) ;
                Relax(u,v);
                pque.Refresh();
            }



            if(u.pos == targetPos)
            {
                DrawPath(u);
            }
            list.Add(u);
            yield return new WaitForSeconds(levelDelayTime);
            
        }
        
       
   }

}
