using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GraphyFW.Common;
public class AIDFSSerch : AISearchBase
{
    public AIDFSSerch(Vector2Int mapSize) : base(mapSize) { }


    private AIBrickState GetNextState(Vector2Int pos)
    {
         AIBrickState state = map.GetBrickState(pos,EBitMask.ACSSESS | EBitMask.FOUND);
        if (state == null) return null; 
        Vector2Int tpos = new Vector2Int(); 
        AIBrickState[] ss = new AIBrickState[4];
        //遍历四方，选择distance最小的那个,但是不能选择障碍物
        tpos.Set(state.pos.x + 1, state.pos.y);
        ss[0] = map.GetBrickState(tpos, EBitMask.FOUND);//可以访问已经被标为被访问的方块
        tpos.Set(state.pos.x - 1, state.pos.y);
        ss[1] = map.GetBrickState(tpos, EBitMask.FOUND);
        tpos.Set(state.pos.x, state.pos.y + 1);
        ss[2] = map.GetBrickState(tpos, EBitMask.FOUND);
        tpos.Set(state.pos.x, state.pos.y - 1);
        ss[3] = map.GetBrickState(tpos, EBitMask.FOUND);

        //  tpos.Set(state.pos.x + 1, state.pos.y + 1);//右上
        // ss[4] = GetBirckStateDic(tpos, EBitMask.ACSSESS | EBitMask.FOUND);
        //  tpos.Set(state.pos.x + 1, state.pos.y - 1);//右下
        // ss[5] = GetBirckStateDic(tpos, EBitMask.ACSSESS | EBitMask.FOUND);
        //  tpos.Set(state.pos.x - 1, state.pos.y - 1);//左下
        // ss[6] = GetBirckStateDic(tpos, EBitMask.ACSSESS | EBitMask.FOUND);
        //  tpos.Set(state.pos.x - 1, state.pos.y + 1);//左上
        // ss[7] = GetBirckStateDic(tpos, EBitMask.ACSSESS | EBitMask.FOUND);
        //四方物体有可能空，临时State是空的
        for (int i = 0; i < 4; i++)
        {
            if (ss[i] == null) continue;
            ss[i].SetFound();           
            return ss[i];
        } 
        return null;
         
    }


    //访问目标节点周围没有访问过的节点
    //问题：将周围节点都访问导致 接下来访问权限不足导致 搜索失败
    //可以先拿到下一个节点，再将周围节点访问
    public  AIBrickState AccessRoundBricks(AIBrickState state)
    {
        if(state == null) return null;

        AIBrickState up = map.GetBrickState(state.GetUp());
        if(up != null) up.SetFound().SetParentState(state);

        AIBrickState down = map.GetBrickState(state.GetDown());
        if(down != null) down.SetFound().SetParentState(state);

        AIBrickState left = map.GetBrickState(state.GetLeft());
        if(left != null) left.SetFound().SetParentState(state);

        AIBrickState right = map.GetBrickState(state.GetRight());
        if(right != null) right.SetFound().SetParentState(state);
        return null;
    }

    public void DrawDFSPath(AIBrickState state)
    {
        if (state == null){
            Debug.Log("State null.");
            return;
        } 
        
        AIBrickState ts = state;
        int c = 100;//至多进行100个循环
        while (ts != null && c != 0)
        {
            //Debug.Log("Object position:---------->" + ts.parentState?.pos);
            ts.SetColor(Color.green);
            ts = map.GetBrickStateNearestNeighbor(ts);
            
            c--;
        }


    }

    public override IEnumerator Search()
    {
        //心得
        //1.一定要确定迭代跳出条件，否则内存泄漏
        //2.外循环管出栈，内循环管入栈和跳出

        //得到一个子物体，（入栈）
            //遍历它的四方直到不能遍历，
            //回溯，（出栈）
        //下一个子物体。。。循环

        //循环不变式
        Stack<AIBrickState> stackStates = new Stack<AIBrickState>();

        AIBrickState lastState = map.GetBrickState(sourcePos);
        AIBrickState nextStaete;       

         stackStates.Push(lastState);
        Debug.Log("Search...");
        
        DebugTime.StartTimer(timeTotal);
        while (stackStates.Count > 0)
        {
            lastState = stackStates.Peek();
            //这个循环将访问过的节点都入栈
            while (lastState != null)
            {
                if (IsTarget(lastState.pos))
                {
                    DrawDFSPath(map.GetBrickStateNearestNeighbor(lastState));
                    Debug.Log("Search over.");
                    DebugTime.EndTimer(timeTotal);
                    yield break;
                }

                lastState.SetAccess();
                nextStaete = GetNextState(lastState.pos);
                AccessRoundBricks(lastState);
                if (nextStaete != null)
                {
                    nextStaete.SetParentState(lastState);
                    stackStates.Push(nextStaete);
                }
                //当nextstate为空的时候，意味着死胡同，该回溯
                lastState = nextStaete;
                Debug.Log("Inner: " + stackStates.Count);
                yield return new WaitForSeconds(levelDelayTime);
            }
            
            //遇到死胡同出栈，即回溯
            stackStates.Pop();
            Debug.Log("Outer: " + stackStates.Count);
        }


    }




}
