using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GraphyFW.Common;
using UnityEngine.Events;
//寻路算法的基类
public class AISearchBase
{
      
    public Vector2Int mapSize { get; private set; }
    public Vector2Int sourcePos { get; private set; }
    public Vector2Int targetPos { get; private set; } 

    public Vector2 girdSize{get;set;}
    public float levelDelayTime { get; set; }

    public float blackRate{get;set;}

    public MapBase<AIBrickState> map{get;set;}
    protected Timer timeTotal = new Timer("Total run time: ");

    protected List<AIBrickState> accessedBricksList = new List<AIBrickState>();
    public AISearchBase(Vector2Int mapSize)
    {
        this.mapSize = mapSize;  
 
        this.levelDelayTime = 0.05f;
        blackRate = 0.2f;
    }

    //设置源节点
    public AISearchBase SetSourcePos(Vector2Int pos)
    {
        map.GetBrickState(sourcePos,EBitMask.OBSTACLE)?.Clear();
        map.GetBrickState(pos,EBitMask.OBSTACLE)?.SetColorVariable(true).SetColor(Color.red).SetColorVariable(false);
        sourcePos = pos;
        return this;
    }

    //设置目标节点
    public AISearchBase SetTargetPos(Vector2Int pos)
    {
        map.GetBrickState(targetPos,EBitMask.OBSTACLE)?.Clear();
        map.GetBrickState(pos,EBitMask.OBSTACLE)?.SetColorVariable(true).SetColor(Color.blue).SetColorVariable(false);
        this.targetPos = pos;
        return this;
    }

    //设置网格大小
    public AISearchBase SetGridSize(Vector2 size)
    {
        this.girdSize = size;
        return this;
    }

    public void Clear()
    {
        foreach (var item in accessedBricksList)
        {
            if(item.isObstacle) continue;
            item.Clear();
        }
    }


    //生成地图
    public void GenMap(GameObject prefab, GameObject container, Vector2 initOffset = new Vector2())
    {       
        map.GenMap(prefab,container);
    }

 

    //虚函数寻路算法
    public  virtual IEnumerator Search()
    {
        yield return new WaitForSeconds(0.1f) ;
    }


 
    //绘制路径
    protected void DrawPath(AIBrickState state)
    {
        if (state == null){
            Debug.Log("State null.");
            return;
        } 
        if (state.parentState == null){
            Debug.Log("Draw Path: state parent null.");
             return;
        }
        AIBrickState ts = state;
        int c = 100;//至多进行100个循环
        while (ts.parentState != null && c != 0)
        {
            //Debug.Log("Object position:---------->" + ts.parentState?.pos);
            ts.SetColor(Color.green);
            ts = ts.parentState;
            c--;
        }
    }
 

    //判断当前访问的节点是否是目标节点
    protected bool IsTarget(Vector2Int pos)
    {   
        if(pos == targetPos)
            return true;
        return false;
    }



}
