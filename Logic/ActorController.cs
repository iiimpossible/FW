using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GraphyFW.AI;
public class ActorController :MonoBehaviour
{

    public float speed = 10.0f;
    Vector3 tgPos = new Vector3();

    List<Vector2> poss = new List<Vector2>();

    Vector3 dir = new Vector3();

    private AIStateMachine machine = new AIStateMachine();

    private  ActionMoveTo move ;

    private ActionPatrol patrol;

    private AIRunData runData;

  
    private void Awake() {   
        
    }



    /// <summary>
    /// 初始化运行数据
    /// </summary>
    private void InitRunData()
    {
        //地图数据
        this.runData.SetBrickData("MainMap",new MapBase<AIBrickState>(Vector2Int.zero));
        
    }   


    public void Start()
    {
        ScptInputManager.instance.eventMouseInWorldPos += AddPos;
        runData = new AIRunData();
        move = new ActionMoveTo(this, runData);
        patrol = new ActionPatrol(this,runData);
        move.nextAction = patrol;
        patrol.nextAction = move;
        move.condition = () => { if (this.poss.Count == 0) Debug.Log("Poss count is zero"); return true; };
        machine.SetStartState(move);
    }


    public void Update()
    {
        machine.Update();
        //Move();
    }

    private void Move()
    {
       if(poss.Count>0)
       {
           dir = poss[0] - (Vector2) transform.position;
           transform.Translate(dir.x * Time.deltaTime * speed,dir.y * Time.deltaTime* speed,transform.position.z);
           if(Arrive(poss[0]))
           {
               poss.RemoveAt(0);
           }           
       }
    }

    private void AddPos(Vector2Int pos)
    {
        Debug.Log("Add pos");
        move.SetTarget(new Vector3(pos.x,pos.y,transform.position.z));
        this.poss.Add(pos);
    }

    public bool Arrive(Vector3 pos)
    {
        if ((pos - transform.position).sqrMagnitude < 0.01f)
            return true;
        return false;
    }

    public Vector2 GetDir(Vector3 tg)
    {
        return (tg - transform.position).normalized;
    }
    
    //行为

    
}
