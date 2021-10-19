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
                foreach vertex v in G.Adj[u]    对所有从节点u出发的边做松弛操作
                    Relax(u,v,w)
        }
    
    */

   public override IEnumerator Search()
   {
       //最小优先队列
       PriorityQueue<AIBrickState> pque = new PriorityQueue<AIBrickState>();
       List<AIBrickState> list = new List<AIBrickState>();

        var s = GetBirckStateDic(sourcePos,EBitMask.OBSTACLE | EBitMask.ACSSESS | EBitMask.FOUND);
        pque.EnQUeue(s,s.distance);

        Vector2Int pos = new Vector2Int();

        //将所有的节点都入队 目前权重都是1，要随机显示一个权重到颜色上
        for(int i = 0 ; i < mapSize.x; i ++ )
        {
            for(int j = 0 ; j < mapSize.y; j++)
            {
                pos.Set(i,j);
                AIBrickState tstate = dicBrickStates[pos];
                tstate.distance = 1e10f;
                pque.EnQUeue(dicBrickStates[pos],dicBrickStates[pos].weight);
            }
        }

        AIBrickState u;
        
        while( pque.Count > 0)
        {
            u = pque.DeQueue();
            list.Add(u);
            
        }
       yield return 0;
   }

}
