using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GraphyFW.Common;
public class AIBFSSearch : AISearchBase
{
    public AIBFSSearch(Vector2Int mapSize) : base(mapSize) { }
    public override IEnumerator Search()
    {        
        Queue<AIBrickState> que_objs = new Queue<AIBrickState>();
        que_objs.Enqueue(map.GetBrickState(sourcePos, EBitMask.OBSTACLE | EBitMask.ACSSESS | EBitMask.FOUND));//搜索起点入队    
        int objNumOfLevel = 1;
        int level = 0;
        Vector2Int tpos = new Vector2Int();
        //开始计时
        DebugTime.StartTimer(timeTotal);
        while (que_objs.Count > 0)
        {
            ++level;
            //遍历当前队中的游戏物体的四个方向 
            while (objNumOfLevel > 0)
            {
                if (que_objs.Count <= 0)
                {
                    Debug.Log("Error:---------->Obstacle impassable.");
                    yield break;
                }
                //对当前的访问的游戏物体进行处理                  
                AIBrickState curState = que_objs.Dequeue();

                if (curState.isObstacle) continue;
                curState.SetAccess();
                Vector2Int cur_pos = curState.pos;

                if (IsTarget(cur_pos))
                {
                    Debug.Log("Search...");
                    AIBrickState tstate = map.GetBrickState(targetPos, EBitMask.ACSSESS | EBitMask.FOUND | EBitMask.OBSTACLE);
                    DrawPath(tstate);
                    Debug.Log("Search over");
                    DebugTime.EndTimer(timeTotal);
                    yield break;
                }
                //发现游戏物体                
                {
                    //上
                    tpos.Set(cur_pos.x, cur_pos.y + 1);
                    AIBrickState up = map.GetBrickState(tpos);
                    if (up != null)
                    {
                        up.SetParentState(curState);
                        que_objs.Enqueue(up);
                    }
                    //下
                    tpos.Set(cur_pos.x, cur_pos.y - 1);
                    AIBrickState down = map.GetBrickState(tpos);
                    if (down != null)
                    {
                        down.SetParentState(curState);
                        que_objs.Enqueue(down);
                    }
                    //左     
                    tpos.Set(cur_pos.x - 1, cur_pos.y);
                    AIBrickState left = map.GetBrickState(tpos);
                    if (left != null)
                    {
                        left.SetParentState(curState);
                        que_objs.Enqueue(left);
                    }
                    //右 
                    tpos.Set(cur_pos.x + 1, cur_pos.y);
                    AIBrickState right = map.GetBrickState(tpos);
                    if (right != null)
                    {
                        right.SetParentState(curState);
                        que_objs.Enqueue(right);
                    }
                }
                //yield return new WaitForSeconds(0.001f);  
                objNumOfLevel--;
            }
            yield return new WaitForSeconds(levelDelayTime);
            objNumOfLevel = que_objs.Count;
            if (que_objs.Count <= 0)
            {
                Debug.Log("Search...");//目标因为已经被访问所以不能设置父节点？
                AIBrickState tstate = map.GetBrickStateNearestNeighbor(map.GetBrickState(targetPos));
                DrawPath(tstate);
                DebugTime.EndTimer(timeTotal);
                Debug.Log("Search over.");
            }
        }
        yield return 0;
    }

}
