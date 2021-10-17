using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Dijkstra 算法解决的是带权重的有向图中的单源最短路径问题。该算法要求所有变边的权重都为非负值
//
public class AIDijkstraSearch : AISearchBase
{
    public AIDijkstraSearch (Vector2Int mapSize):base(mapSize)
    {

    }
   

   public override IEnumerator Search()
   {
       yield return 0;
   }

}
