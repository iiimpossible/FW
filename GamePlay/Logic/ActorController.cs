using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GraphyFW.AI;


namespace GraphyFW.AI
{
    /// <summary>
    /// 工作类型枚举
    /// 1.搬运
    /// 2.喂养（幼虫、蚁后、兵蚁）
    /// 3.攻击
    /// 4.捕猎（各种昆虫，小型哺乳动物）
    /// 5.采集（叶子）
    /// 6.种植（真菌）
    /// 7.放牧（蚜虫，介壳虫）
    /// </summary>
    public enum EAIWorkType
    {
        CARRAY = 1,
        FEED = 2,
        ATTACK = 3,
        HUNT = 4,
        COLLECT = 5,
        PLANT = 6,
        GRAZE = 7

    } 

    /// <summary>
    /// AI的状态
    /// 1.工作中（分为采集中，攻击中等等）
    /// 2.闲逛中
    /// 3.休息中
    /// 4.被攻击
    /// </summary>
    public enum EAIWorkState
    {
        WORKING = 1,
        IDLE = 2,

        ATACKED  = 3,
        ATAKING = 4,

        RESTING = 5,
    }


    /// <summary>
    /// 这是一个Ai的控制器，
    /// 1.存储了一些AI的必要方法，如移动等，
    /// 2.还存储了AI的一些状态，如是否空闲，是否在做某一件事情，
    /// 3.定义了AI的兵种、工作类型等
    /// 4.（临时）接收状态机的请求，获取当前地图中的一些信息（道具等）
    /// </summary>
    public class ActorController : MonoBehaviour
    {
        
         
        private AIStateMachine machine ;//其实是一棵行为树了。遍历所有的Task，判断是否执行这个状态。        
        private AIRunData runData;         
        private TaskCarry taskCarry;
        private TaskCallUp taskCallUp;
        [SerializeField]
        private GameObject frame;

        [SerializeField]
        private bool isSelected; //当一个Actor被选中，那么它就会停止，其他AI行为，转为征召状态，这个状态可以鼠标控制AI的行为

        private void Awake() {
            frame.SetActive(false);
            
           
        }


        public void Start()
        {

            //ScptInputManager.instance.eventMouseInWorldPos += AddPos;
            runData = new AIRunData();
            this.runData.SetMapData("MainMap", AISystem.instance.mainMap);
            machine = new AIStateMachine(this, runData);

            //searchMove = new ActionSearchMove(this, runData);
            //searchProp = new ActionSearchProp(this,runData);
            taskCarry = new TaskCarry(this, runData);

            taskCallUp = new TaskCallUp(this, runData);

           // machine.AddTask(taskCarry);

            machine.AddTask(taskCallUp);
            //move.nextAction = patrol;
            // patrol.nextAction = move;
            // move.condition = () => { if (this.poss.Count == 0) Debug.Log("Poss count is zero"); return true; };
            //machine.SetStartState(taskCarry);         

            //注册消息 消息注册必须在MessageMaager生成之后
            MessageManager.instance.AddListener(EMessageType.OnBoxCastAllCollider,OnBox2DRayCastCallback);
        }

        //接收选中消息
        private void OnBox2DRayCastCallback(Message message)
        {           
            RaycastHit2D[] hit2Ds;
            if (message.paramsList.Count > 0)
            {
                hit2Ds = message.paramsList[0] as RaycastHit2D[];
                foreach (var item in hit2Ds)
                {
                    if(item.collider.gameObject.transform == transform)
                    {
                        isSelected =  true;
                        //TODO: 状态机转为征召？不，征召还是不同，这里应该打断其他状态，转为特定状态
                        Debug.Log($"Recieve message, {item.collider.gameObject.transform.name} be selected.");
                        machine.SetCurrentTask(taskCallUp);
                        frame.SetActive(true);
                    }
                }
            }
           
        }
 

        public void Update()
        {
            machine.Update();
            //Move();
        }

        /// <summary>
        /// 初始化运行数据
        /// </summary>
        private void InitRunData()
        {
            //地图数据
        }
    

    }

}
