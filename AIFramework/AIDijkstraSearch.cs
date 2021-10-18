using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
   public override IEnumerator Search()
   {
       yield return 0;
   }

}
