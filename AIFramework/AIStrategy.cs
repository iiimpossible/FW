using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GraphyFW.Common;

namespace GraphyFW
{

    namespace AI
    {

        //AIѰ·���ԣ�������ȡ���ȡ�Dj��A*��
        //��Ϸ�е��Ѱ·Ĭ����֪��Ŀ�ĵصģ�ֻ����·�������ϰ��Ѱ·��Ϊ�˱ܿ��ϰ�
        public class AIStrategy
        {
            //һ����ͼ�洢�ڵ����ݣ���ͼ��Ķ�ά���꣩
            //������ѯ��GameObjectûʲô��ϵ�������洢��BrickState�У���Ϊself�������һЩ����
            //��Ҫ��Ҫ����ĳ�����ѯ��״̬
            //��Ҫһ���ڴ�ر���Ƶ�����ٶ���

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

            //����Դ�ڵ�
            public void SetSourcePos(Vector2Int pos)
            {
                this.sourcePos = pos;
            }

            //����Ŀ��ڵ�
            public void SetTargetPos(Vector2Int pos)
            {
                this.targetPos = pos;   
            }

            //���ص�ǰ��ͼ��С
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
            /// �����������
            /// ��Ҫ������1.��ͼ����һ����ά���鴢��
            /// 2.�ڵ㴦��ص�����
            /// 3.�ֽڵ���Ѱ�ص�����
            /// </summary>    
            public IEnumerator BFSSearch(Vector2Int pos)
            {
                Queue<GameObject> que_objs = new Queue<GameObject>();
                que_objs.Enqueue(GetObjInSquar(sourcePos.x,sourcePos.y,matrixObjs,EBitMask.OBSTACLE));//����������    
                int objNumOfLevel = 1;
                int level = 0;
                //��ʼ��ʱ
                DebugTime.StartTimer(runTotal);
                while (que_objs.Count > 0)
                {
                    ++level;
                    //������ǰ���е���Ϸ������ĸ����� 
                    while (objNumOfLevel > 0)
                    {
                        if (que_objs.Count <= 0)
                        {
                            Debug.Log("Error:---------->Obstacle impassable.");
                            yield break;
                        }
                        //�Ե�ǰ�ķ��ʵ���Ϸ������д���                  
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
                        //������Ϸ����                
                        {
                            DebugTime.StartTimer(runFourChild);
                            //��
                            GameObject up = GetObjInSquar(cur_pos.x, cur_pos.y + 1, matrixObjs);
                            if (up)
                            {
                                dicBrickStates[up].SetParentState(curState);
                                que_objs.Enqueue(up);
                            }
                            //��
                            GameObject down = GetObjInSquar(cur_pos.x, cur_pos.y - 1, matrixObjs);
                            if (down)
                            {
                                dicBrickStates[down].SetParentState(curState);
                                que_objs.Enqueue(down);
                            }
                            //��
                            GameObject left = GetObjInSquar(cur_pos.x - 1, cur_pos.y, matrixObjs);
                            if (left)
                            {
                                dicBrickStates[left].SetParentState(curState);
                                que_objs.Enqueue(left);
                            }
                            //��
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

            //����·��
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

            //�ӵ�ǰ�ڵ�����������ĸ�������ѡȡһ��distanceֵ��С�Ľڵ�
            private AIBrickState GetNearestObject(AIBrickState state, List<List<GameObject>> objects)
            {
                if (state == null) return null;
                DebugTime.StartTimer(runGetNear);
                Debug.Log("Target pos--------->" + state.pos);
                AIBrickState res = null;
                AIBrickState[] ss = new AIBrickState[4];
                //�����ķ���ѡ��distance��С���Ǹ�
                ss[0] = GetBirckStateDic(GetObjInSquar(state.pos.x + 1, state.pos.y, objects, EBitMask.ACSSESS));//���Է����Ѿ�����Ϊ�����ʵķ���
                ss[1] = GetBirckStateDic(GetObjInSquar(state.pos.x - 1, state.pos.y, objects, EBitMask.ACSSESS));
                ss[2] = GetBirckStateDic(GetObjInSquar(state.pos.x, state.pos.y + 1, objects, EBitMask.ACSSESS));
                ss[3] = GetBirckStateDic(GetObjInSquar(state.pos.x, state.pos.y - 1, objects, EBitMask.ACSSESS));
                //�ķ������п��ܿգ���ʱState�ǿյ�
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
          
            //���ݷ���Ȩ�޷��ʲ�ͬ״̬�µĽڵ�
            private GameObject GetObjInSquar(int x, int y, List<List<GameObject>> objects, EBitMask mask = EBitMask.NONE)
            {
                if (!IndexValid(x, y))
                    return null;
                GameObject ogo = objects[x][y];
                AIBrickState stata = dicBrickStates[ogo];

                switch (mask)
                {
                    //�޷���Ȩ��
                    case EBitMask.NONE:
                        {
                            if (stata.isFound || stata.isObstacle)
                                return null;
                            else
                                stata.SetFound();
                            return ogo;
                        }
                    //�ɷ��ʱ����ʹ�������
                    case EBitMask.ACSSESS:
                        {
                            if (stata.isObstacle) return null;
                            return ogo;
                            break;
                        }
                    //�ɷ��ʱ����ֵ�����
                    case EBitMask.FOUND:
                        {
                            break;
                        }
                    //�ɷ����ϰ���
                    case EBitMask.OBSTACLE:
                        {
                            return ogo;
                        }
                    //ӵ�����з���Ȩ��
                    case EBitMask.ACSSESS | EBitMask.FOUND | EBitMask.OBSTACLE:
                        {
                            return ogo;
                        }
                }
                return null;
            }

            //�ж����������Ƿ��ǺϷ�������
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

            //��ʼ��Դ�ڵ��Ŀ��ڵ�
            public void InitOriginTargetPos(Vector2Int s, Vector2Int t)
            {
                GameObject tgtGO = GetObjInSquar(t.x, t.y, matrixObjs, EBitMask.OBSTACLE);
                GetBirckStateDic(tgtGO).SetColor(Color.red).SetColorVariable(false).SetObstacle(false);
                GameObject oriGO = GetObjInSquar(s.x, s.y, matrixObjs, EBitMask.OBSTACLE);
                GetBirckStateDic(oriGO).SetColor(Color.blue).SetColorVariable(false).SetObstacle(false);                            
            }
            
            //�жϵ�ǰ���ʵĽڵ��Ƿ���Ŀ��ڵ�
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



