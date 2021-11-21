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

        public void Start()
        {

            //ScptInputManager.instance.eventMouseInWorldPos += AddPos;
            runData = new AIRunData();
            this.runData.SetMapData("MainMap", AISystem.instance.mainMap);
            machine = new AIStateMachine(this, runData);

            //searchMove = new ActionSearchMove(this, runData);
            //searchProp = new ActionSearchProp(this,runData);
            taskCarry = new TaskCarry(this, runData);

            machine.AddTask(taskCarry);
            //move.nextAction = patrol;
            // patrol.nextAction = move;
            // move.condition = () => { if (this.poss.Count == 0) Debug.Log("Poss count is zero"); return true; };
            //machine.SetStartState(taskCarry);
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

        private void SpawnFood()
        {

        }      

    }

}
