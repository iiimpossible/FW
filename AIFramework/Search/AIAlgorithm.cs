using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GraphyFW.Common;

namespace GraphyFW.AI
{
    public class AIAlgorithm
    {
        static GraphyFW.Common.PriorityQueue<AIBrickState> q = new Common.PriorityQueue<AIBrickState>();

        static List<AIBrickState> accessed = new List<AIBrickState>();
        static Timer timer = new Timer("Search time:");
        /// <summary>
        /// 静态A*算法，根据提供的地图，输出路径
        /// 搜索u节点周围的f(v) = g(v) + h(v)最小的节点 g(v)是v（下一个要搜索的点）点到当前点的距离耗费，
        /// h(v)是v点到目标点的实际距离的估算（有障碍物，无障碍就是精确值）
        /// </summary>
        /// <param name="map"></param>
        /// <param name="path"></param>
        public static void AstarSearch(MapBase<AIBrickState> map, Vector2Int sourcePos, Vector2Int targetPos, List<Vector2> path)
        {           
            Debug.Log("AStarsearch sourcepos-->" + sourcePos + "targetpos--->" + targetPos);
            DebugTime.StartTimer(timer);
            
            q.Clear();   
            AIBrickState b = map.GetBrickState(sourcePos,EBitMask.ACSSESS | EBitMask.FOUND);
            if(b == null) 
            {
                Debug.LogWarning("Path find faild, perhaps SOURCE obstacle.");
                return ;
            }

            //检测目的地是否可访问
            if(map.GetBrickState(targetPos,EBitMask.ACSSESS | EBitMask.FOUND) == null)
            {
                Debug.LogWarning("Path find faild, perhaps TARGET obstacle.");
                return ;
            }


            q.EnQueueBh(b);//源节点入队获取状态
            Vector2Int vpos = new Vector2Int();
            int max = 20000; //被这个强制退出了。
            while (q.bhCout > 0 && max != 0)
            {
                AIBrickState u = q.DeQueueBh();
                u.SetAccess();
                //accessed.Add(u);
                AIBrickState v = null;
                for (int i = 0; i < 8; i++)
                {
                    vpos = u.GetNeighborsDiagnol(i);
                    // Debug.Log("v pos is---->" + vpos);
                    // Debug.Log("Choose neighbor-->" + i);
                    // if(i >3)
                    // {
                    //     if(IsObstacle(map,u.pos,vpos)) continue ;
                    // }
                    v = map.GetBrickState(vpos);//邻居节点状态
                    if (v != null)
                    {
                        v.distance = DiagonalDistance(vpos, u.pos, targetPos);
                        
                        q.EnQueueBh(v);
                        //v.SetFound();
                        v.SetParent(u);
                        v.SetText(v.distance.ToString());
                    }
                   
                }
                // Debug.Log("Cur priority queue count is--->" + q.Count);
                if (u.pos == targetPos)
                {
                    while(u.parentState != null)
                    {
                        path.Add(map.MapSpaceToWorldSpace(u.pos));
                        var t = u;
                        u = u.parentState;
                        t.Clear();
                    }
                    u?.Clear();
                    q.Foreach((brick) => {brick.Clear();});
                     DebugTime.EndTimer(timer);
                    return;
                }
                max--;
            }
        }


        /// <summary>
        /// 曼哈顿距离d(i,j)=|X1-X2|+|Y1-Y2|. 即在只能水平和竖直移动的区域，两点的距离是东西方向的距离加上南北方向的距离。
        /// 直线距离即欧氏（几何）距离
        /// </summary>
        /// <param name="v">当前节点</param>
        /// <param name="u">v的父节点</param>
        /// <param name="s">目标节点</param>
        /// <returns></returns>
        public static float ManhattanDistance(Vector2Int v, Vector2Int u, Vector2Int s)
        {
            return Mathf.Abs(v.x - u.x) + Mathf.Abs(v.y - u.y) + Mathf.Abs(s.x - v.x) + Mathf.Abs(s.y - v.y);
        }
 
        /// <summary>
        /// 求两点对角距离
        /// d = sqrt((x-x)^2 + (y-y)^2)
        /// </summary>
        /// <returns></returns>
        public static float DiagonalDistance(Vector2Int v, Vector2Int u, Vector2Int s)
        {
            return Mathf.Round(Mathf.Sqrt(Mathf.Pow((v.x - s.x), 2) + Mathf.Pow(v.y - s.y, 2)));
        }

        /// <summary>
        /// 判断这两个位置是否是对角阻塞的
        /// </summary>
        /// <param name="u"></param>
        /// <param name="v"></param>
        /// <returns></returns>
        public static bool IsObstacle(MapBase<AIBrickState> map,Vector2Int u,Vector2Int v)
        {
            //两个点的1号方块
            Vector2Int a  = Vector2Int.zero;
            Vector2Int b = Vector2Int.zero;
            a.Set(u.x, v.y);
            b.Set(v.x, u.y);
            if(map.GetBrickState(a) != null || map.GetBrickState(b) != null)
            {
                Debug.Log("Is not obstable");
                return false;
            }
            return true;
        }



        /// <summary>
        /// 检测物体是否已经到达特定位置
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool Arrive(Vector2 a, Vector2 b)
        {
              if ((a - b).sqrMagnitude < 0.01f)
                return true;
            return false;
        }



        /// <summary>
        /// 物体沿路径移动
        /// </summary>
        /// <param name="trans"></param>
        /// <param name="tgPos"></param>
        public static bool MoveTo(Transform trans, Vector3 tgPos, List<Vector2> path, float speed = 10f)
        {
            Vector2 dir = Vector2.zero;
            Vector3 deltaPos = Vector3.zero;
             if (path.Count > 0)
            {
                dir = path[path.Count-1] - (Vector2)trans.position;
                deltaPos.Set(dir.x * Time.deltaTime * speed, dir.y * Time.deltaTime * speed, 0);
                trans.position +=deltaPos;
                if (Arrive(path[path.Count-1],trans.position))
                {
                    path.RemoveAt(path.Count-1);
                }
                return true;
            }
            return false;
        }

    }

}
