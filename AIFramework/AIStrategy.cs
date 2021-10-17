using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GraphyFW.Common;

namespace GraphyFW
{

    namespace AI
    {

        //AI寻路策略，包括广度、深度、Dj、A*等
        //游戏中点击寻路默认是知道目的地的，只是在路径中有障碍物，寻路是为了避开障碍
        public class AIStrategy
        {
            //一个地图存储节点数据（地图块的二维坐标）
            //整个查询和GameObject没什么关系，它被存储在BrickState中，作为self对其进行一些操作
            //主要是要根据某个类查询其状态
            //需要一个内存池避免频繁销毁对象

            private Timer runTotal = new Timer("Total timer");
            private Timer runFourChild = new Timer("Four children");
            private Timer runList = new Timer("List");
            private Timer runGetNear = new Timer("GetNear");

            public Dictionary<GameObject, AIBrickState> dicBrickStates = new Dictionary<GameObject, AIBrickState>();
            public List<List<GameObject>> matrixObjs;
            public Vector2Int matrixSize;

            public Vector2Int sourcePos{get; private set;}
            
            public Vector2Int targetPos{get; private set;}

            public float levelDelayTime = 0.05f;

            public AIStrategy()
            {
                matrixObjs = new List<List<GameObject>>();
                matrixSize = new Vector2Int(30,30);
            }

            //设置源节点
            public void SetSourcePos(Vector2Int pos)
            {
                this.sourcePos = pos;
            }

            //设置目标节点
            public void SetTargetPos(Vector2Int pos)
            {
                this.targetPos = pos;   
            }

            //返回当前地图大小
            public Vector2Int GetMapSize()
            {
                Vector2Int size = new Vector2Int();
                size.x = matrixObjs.Count;
                if(matrixObjs.Count >0)
                    size.y = matrixObjs[0].Count;
                else
                    size.y = 0;        
                return size;        
            }

            /// <summary>
            /// 广度优先搜索
            /// 必要参数：1.地图，由一个二维数组储存
            /// 2.节点处理回调函数
            /// 3.字节点搜寻回调函数
            /// </summary>    
            public IEnumerator BFSSearch(Vector2Int pos)
            {
                Queue<GameObject> que_objs = new Queue<GameObject>();
                que_objs.Enqueue(GetObjInSquar(sourcePos.x,sourcePos.y,matrixObjs,EBitMask.OBSTACLE));//搜索起点入队    
                int objNumOfLevel = 1;
                int level = 0;
                //开始计时
                DebugTime.StartTimer(runTotal);
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
                        GameObject cur = que_objs.Dequeue();
                        AIBrickState curState = dicBrickStates[cur];
                        if (curState.isObstacle) continue;
                        curState.SetAccess();
                        Vector2Int cur_pos = curState.pos;

                        if (IsTarget(cur))
                        {
                            Debug.Log("Search...");
                            GameObject tgo = GetObjInSquar(pos.x, pos.y, matrixObjs, EBitMask.ACSSESS);
                            AIBrickState tgt = GetNearestObject(GetBirckStateDic(tgo), matrixObjs);
                            DrawPath(tgt);
                            Debug.Log("Search over");
                            DebugTime.EndTimer(runTotal);
                            yield break;
                        }
                        //发现游戏物体                
                        {
                            DebugTime.StartTimer(runFourChild);
                            //上
                            GameObject up = GetObjInSquar(cur_pos.x, cur_pos.y + 1, matrixObjs);
                            if (up)
                            {
                                dicBrickStates[up].SetParentState(curState);
                                que_objs.Enqueue(up);
                            }
                            //下
                            GameObject down = GetObjInSquar(cur_pos.x, cur_pos.y - 1, matrixObjs);
                            if (down)
                            {
                                dicBrickStates[down].SetParentState(curState);
                                que_objs.Enqueue(down);
                            }
                            //左
                            GameObject left = GetObjInSquar(cur_pos.x - 1, cur_pos.y, matrixObjs);
                            if (left)
                            {
                                dicBrickStates[left].SetParentState(curState);
                                que_objs.Enqueue(left);
                            }
                            //右
                            GameObject right = GetObjInSquar(cur_pos.x + 1, cur_pos.y, matrixObjs);
                            if (right)
                            {
                                dicBrickStates[right].SetParentState(curState);
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
                        Debug.Log("Search...");
                        GameObject tgo = GetObjInSquar(pos.x, pos.y, matrixObjs, EBitMask.ACSSESS);
                        AIBrickState tgt = GetNearestObject(GetBirckStateDic(tgo), matrixObjs);
                        DrawPath(tgt);
                        DebugTime.EndTimer(runTotal);
                        Debug.Log("Search over.");
                    }
                }
            }

            //绘制路径
            public void DrawPath(AIBrickState state)
            {
                if (state == null) return;
                if (state.parentState == null) return;
                DebugTime.StartTimer(runList);
                AIBrickState ts = state;
                int c = 200;
                while (ts.parentState != null && c != 0)
                {
                    Debug.Log("Object position:---------->"+ts.parentState?.pos);
                    ts.SetColor(Color.green);
                    ts = ts.parentState;
                    c--;
                }
                DebugTime.EndTimer(runList);
            }

            //从当前节点的上下左右四个方向上选取一个distance值最小的节点
            private AIBrickState GetNearestObject(AIBrickState state, List<List<GameObject>> objects)
            {
                if (state == null) return null;
                DebugTime.StartTimer(runGetNear);
                Debug.Log("Target pos--------->" + state.pos);
                AIBrickState res = null;
                AIBrickState[] ss = new AIBrickState[4];
                //遍历四方，选择distance最小的那个
                ss[0] = GetBirckStateDic(GetObjInSquar(state.pos.x + 1, state.pos.y, objects, EBitMask.ACSSESS));//可以访问已经被标为被访问的方块
                ss[1] = GetBirckStateDic(GetObjInSquar(state.pos.x - 1, state.pos.y, objects, EBitMask.ACSSESS));
                ss[2] = GetBirckStateDic(GetObjInSquar(state.pos.x, state.pos.y + 1, objects, EBitMask.ACSSESS));
                ss[3] = GetBirckStateDic(GetObjInSquar(state.pos.x, state.pos.y - 1, objects, EBitMask.ACSSESS));
                //四方物体有可能空，临时State是空的
                for (int i = 0; i < 4; i++)
                {
                    if (ss[i] == null) continue;
                    if (ss[i].parentState == null) continue;
                    if (res == null) res = ss[i];
                    else if (res.distance > ss[i].distance) res = ss[i];
                }
                //if (res != null) Debug.Log($"Get nearest object:----->{res.pos} distance:----->{res.distance}");
                DebugTime.EndTimer(runGetNear);
                return res;
            }
          
            //根据访问权限访问不同状态下的节点
            private GameObject GetObjInSquar(int x, int y, List<List<GameObject>> objects, EBitMask mask = EBitMask.NONE)
            {
                if (!IndexValid(x, y))
                    return null;
                GameObject ogo = objects[x][y];
                AIBrickState stata = dicBrickStates[ogo];

                switch (mask)
                {
                    //无访问权限
                    case EBitMask.NONE:
                        {
                            if (stata.isFound || stata.isObstacle)
                                return null;
                            else
                                stata.SetFound();
                            return ogo;
                        }
                    //可访问被访问过的物体
                    case EBitMask.ACSSESS:
                        {
                            if (stata.isObstacle) return null;
                            return ogo;
                            break;
                        }
                    //可访问被发现的物体
                    case EBitMask.FOUND:
                        {
                            break;
                        }
                    //可访问障碍物
                    case EBitMask.OBSTACLE:
                        {
                            return ogo;
                        }
                    //拥有所有访问权限
                    case EBitMask.ACSSESS | EBitMask.FOUND | EBitMask.OBSTACLE:
                        {
                            return ogo;
                        }
                }
                return null;
            }

            //判断输入坐标是否是合法的索引
            private bool IndexValid(int x, int y)
            {
                if (x >= 0 && x < matrixSize.x)
                {
                    if (y >= 0 && y < matrixSize.y)  
                        return true;
                }
                return false;
            }

            private AIBrickState GetBirckStateDic(GameObject key)
            {
                if (key == null)   
                    return null;             
                return dicBrickStates[key];
            }

            //初始化源节点和目标节点
            public void InitOriginTargetPos(Vector2Int s, Vector2Int t)
            {
                GameObject tgtGO = GetObjInSquar(t.x, t.y, matrixObjs, EBitMask.OBSTACLE);
                GetBirckStateDic(tgtGO).SetColor(Color.red).SetColorVariable(false).SetObstacle(false);
                GameObject oriGO = GetObjInSquar(s.x, s.y, matrixObjs, EBitMask.OBSTACLE);
                GetBirckStateDic(oriGO).SetColor(Color.blue).SetColorVariable(false).SetObstacle(false);                            
            }
            
            //判断当前访问的节点是否是目标节点
            private bool IsTarget(GameObject cur)
            {
                if (GetBirckStateDic(cur) == GetBirckStateDic(GetObjInSquar(targetPos.x, targetPos.y, matrixObjs, EBitMask.OBSTACLE)))
                    return true;
                return false;
            }


            private Vector2 normalize(Vector2 origin)
            {
                origin.Set(Mathf.Sqrt(Mathf.Pow(origin.x, 2f)), Mathf.Sqrt(Mathf.Pow(origin.y, 2f)));
                return origin;
            }

        }

    }

}



