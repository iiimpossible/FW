using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GraphyFW.Common;
//寻路算法的基类
public class AISearchBase
{

    public Dictionary<Vector2Int, AIBrickState> dicBrickStates;
    private List<List<AIBrickState>> listBrickStates;
    public Vector2Int mapSize { get; private set; }
    public Vector2Int sourcePos { get; private set; }
    public Vector2Int targetPos { get; private set; }
    public Vector2 girdSize { get; private set; }
    public float levelDelayTime { get; set; }

    public float blackRate{get;set;}
    protected Timer timeTotal = new Timer("Total run time: ");
    public AISearchBase(Vector2Int mapSize)
    {
        this.mapSize = mapSize;
        dicBrickStates = new Dictionary<Vector2Int, AIBrickState>();
        listBrickStates = new List<List<AIBrickState>>();
        this.levelDelayTime = 0.05f;
        blackRate = 0.2f;
    }

    //设置源节点
    public AISearchBase SetSourcePos(Vector2Int pos)
    {
        this.sourcePos = pos;
        return this;
    }

    //设置目标节点
    public AISearchBase SetTargetPos(Vector2Int pos)
    {
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
        Vector2Int coord = new Vector2Int();
        for(int i = 0 ; i < mapSize.x ;i++)
        {
            for(int j = 0; j< mapSize.y;j++)
            {
                coord.Set(i,j);
                dicBrickStates[coord].Clear();

                RandomIsObstacle(dicBrickStates[coord]);//设置状态,必须在字典初始化这个key之后调用此方法
            }
        }
         InitOriginTargetPos(sourcePos,targetPos);
    }
 
    
    //生成地图
    public void GenMap(GameObject prefab, GameObject container, Vector2 initOffset = new Vector2())
    {
        if (container == null || prefab == null)
        {
            Debug.LogError("GenMap error, floor is null.");
            return;
        }
        Vector2Int pos = new Vector2Int();
        for (int column = 0; column < mapSize.x; column++)
        {            
            //listBrickStates.Add(new List<AIBrickState>());
            for (int row = 0; row < mapSize.y; row++)
            {
                GameObject newGo = GameObject.Instantiate<GameObject>(prefab, new Vector3(girdSize.x * column + initOffset.x, girdSize.y * row + initOffset.y, 0), Quaternion.identity);
                if (!newGo)
                {
                    Debug.Log("Invalid prefab.");
                    return;
                }             
                newGo.transform.SetParent(container.transform);               
                pos.Set(column,row);

                AIBrickState newBrick = new AIBrickState(pos,newGo);
                dicBrickStates.Add(pos,newBrick);
                RandomIsObstacle(newBrick);//设置状态,必须在字典初始化这个key之后调用此方法
                RandomColorForWeight(newBrick);
            }
        }
        InitOriginTargetPos(sourcePos,targetPos);
    }


    //虚函数寻路算法
    public  virtual IEnumerator Search()
    {
        yield return new WaitForSeconds(0.1f) ;
    }


     //随机生成障碍物（黑方块）
    protected virtual void RandomIsObstacle(AIBrickState state)
    {
        int ran = Random.Range(0, 100);         
        if (ran > 100 * (1 - blackRate))
            state.SetObstacle();
    }

    protected virtual void RandomColorForWeight(AIBrickState state)
    {
        if(state.isObstacle) return;
        float res = Random.Range(0.5f,1.0f);
        state.SetColor(new Color(res,res,res,1));
        state.weight = res * 10;
    }
   
    //从当前节点的上下左右四个方向上选取一个distance值最小的节点
    //TODO：计算源节点和目标节点方向与源节点到周边节点的方向的夹角从而判断是否略过
    protected AIBrickState GetNearestObject(Vector2Int pos, bool diagonal = false,int arraySize = 4)
    {
        AIBrickState state = GetBirckStateDic(pos,EBitMask.ACSSESS | EBitMask.FOUND | EBitMask.OBSTACLE);
        if (state == null) return null;       
        Debug.Log("Target pos--------->" + state.pos);
        Vector2Int tpos = new Vector2Int();    
        AIBrickState res = null;
        List<AIBrickState> tlist = new List<AIBrickState>(10);
        AIBrickState[] ss = new AIBrickState[arraySize];
        //遍历四方，选择distance最小的那个,但是不能选择障碍物
        tpos.Set(state.pos.x + 1, state.pos.y);
        ss[0] = GetBirckStateDic(tpos, EBitMask.ACSSESS | EBitMask.FOUND);//可以访问已经被标为被访问的方块
        tpos.Set(state.pos.x - 1, state.pos.y);
        ss[1] = GetBirckStateDic(tpos, EBitMask.ACSSESS | EBitMask.FOUND);
        tpos.Set(state.pos.x, state.pos.y + 1);
        ss[2] = GetBirckStateDic(tpos, EBitMask.ACSSESS | EBitMask.FOUND);
        tpos.Set(state.pos.x, state.pos.y - 1);
        ss[3] = GetBirckStateDic(tpos, EBitMask.ACSSESS | EBitMask.FOUND);

        if (diagonal)
        {        
            tpos.Set(state.pos.x + 1, state.pos.y + 1);//右上
            ss[4] = GetBirckStateDic(tpos, EBitMask.ACSSESS | EBitMask.FOUND);
            tpos.Set(state.pos.x + 1, state.pos.y - 1);//右下
            ss[5] = GetBirckStateDic(tpos, EBitMask.ACSSESS | EBitMask.FOUND);
            tpos.Set(state.pos.x - 1, state.pos.y - 1);//左下
            ss[6] = GetBirckStateDic(tpos, EBitMask.ACSSESS | EBitMask.FOUND);
            tpos.Set(state.pos.x - 1, state.pos.y + 1);//左上
            ss[7] = GetBirckStateDic(tpos, EBitMask.ACSSESS | EBitMask.FOUND);
        }

        //四方物体有可能空，临时State是空的
        for (int i = 0; i < arraySize; i++)
        {
            if (ss[i] == null) continue;
            if (ss[i].parentState == null) {
                //Debug.Log("Node parent is null---------->" + ss[i].pos);
                continue;
            }
            if (res == null) res = ss[i];
            else if (res.distance > ss[i].distance) res = ss[i];
        } 
        return res;
    }

    //索引合法性
    protected bool IndexValid(int x, int y)
    {
        if (x >= 0 && x < mapSize.x)
        {
            if (y >= 0 && y < mapSize.y)
                return true;
        }
        return false;
    }

    //字典索引合法性
    protected AIBrickState GetBirckStateDic(Vector2Int key,EBitMask mask = EBitMask.NONE)
    {
        if (!dicBrickStates.ContainsKey(key)){
            //Debug.Log($"Key not valid.---------->{key}");;
            return null;
        }            
        AIBrickState state = dicBrickStates[key];
        if (PermissionMask.ISAllow((int)mask, state.accsessFlag))
            return state;
        return null;
    }

    //矩阵索引合法性
    protected AIBrickState GetBrickStateMatrix(Vector2Int key,EBitMask mask = EBitMask.NONE)
    {
        return null;
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

    //是否是目标节点

    //初始化源节点和目标节点
    protected void InitOriginTargetPos(Vector2Int s, Vector2Int t)
    {
        AIBrickState tgtState = GetBirckStateDic(t, EBitMask.ACSSESS | EBitMask.OBSTACLE | EBitMask.FOUND)
            .SetColor(Color.red).SetColorVariable(false).SetObstacle(false); ;

        AIBrickState srcState = GetBirckStateDic(s, EBitMask.ACSSESS | EBitMask.OBSTACLE | EBitMask.FOUND)
            .SetColor(Color.blue).SetColorVariable(false).SetObstacle(false);
    }

    //判断当前访问的节点是否是目标节点
    protected bool IsTarget(Vector2Int pos)
    {   
        if(pos == targetPos)
            return true;
        return false;
    }

}
