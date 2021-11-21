using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace GraphyFW.AI
{

    // struct StateTransformPath
    // {
    //     string curState;
    //     string tgState;

    //     UnityAction<>
    // }

    /// <summary>
    /// AI的状态机
    /// 1.状态机含有许多状态（或者说行为）
    /// 2.状态转换需要条件
    /// 3.设置行为优先级
    /// 4.状态的对象一定是一个可以被控制的游戏物体
    /// 5.一个状态应该由一个回调函数和某些数据组成，回调函数控制数据，状态根据数据变化？
    /// 6.对于一个状态机，应该有当前状态、过去状态以及全局状态（每次FMS更新都会调用该状态）
    /// 7.该状态机需要一个优先队列，从队列中获取事件，从而转换状态。一个事件需要执行多种行为
    /// 2021.11.19
    /// 1.对于一个比较复杂的状态机，三层结构
    /// 根状态机--->任务状态机--->任务之指令
    /// 2.根状态机存储了AI的所有能执行的动作，称之为“任务Task”，而任务又有许多的基本动作指令组成，称之为“动作Action”
    /// 一个根状态机有一个“根任务RootTask”，即其他所有的任务完成后，会回到这个根任务，当有外部条件或者内部条件导致状态机状态改变，那么就会相应的转换状态
    /// 3.状态转换逻辑，检查每个任务的转换函数是否为真，为真那么就转换为当前任务
    /// </summary>
    public class AIStateMachine
    {
        private StateBase lastAction;

        private StateBase globalAction;

        private StateBase curAction;

        private TaskIdle rootTask;

        private List<TaskBase> tasks = new List<TaskBase>();

        public AIStateMachine(ActorController controller, AIRunData runData)
        {
            //dicActions = new dicrootTask
            rootTask =  new TaskIdle(controller,runData);
            tasks.Add(rootTask);
            curAction = rootTask;
            curAction.ActionEnter();
        }


        /// <summary>
        /// 循环执行状态
        /// </summary>
        public void Update()
        {            
            if(curAction == null) return;
            curAction.ActionUpdate();
            ForeachTask();       
        }

        /// <summary>
        /// 当curAction完成的时候，转换状态
        /// </summary>
        public void SwicthState()
        {
            if (curAction == null) return;
            if (curAction.ActionCompleted())
            {
                lastAction = curAction;
                if (curAction.nextAction != null)
                {
                    //Debug.Log("Switch state.~~~~~~~~");
                    curAction = curAction.nextAction;
                    curAction.ActionEnter();
                    lastAction.ActionExit();
                }
            }
        }

        public void SwitchState(int index)
        {
            //Debug.Log("State ma switch");
            if(index >= tasks.Count) return;
             //Debug.Log("State ma switch 22");
            curAction.ActionExit();
            curAction = tasks[index];
            curAction.ActionEnter();
        }

          public void SwitchState(StateBase state)
        { 
            if(state == null) return;
            curAction.ActionExit();
            curAction = state;
            curAction.ActionEnter();
        }

        public void SetStartState(StateBase action)
        {
            curAction = action;
            curAction.ActionEnter();
        }


        /// <summary>
        /// 遍历当前除了Idle Task的其他所有Task
        /// 1.当Task为未完成时，不进行转换
        /// 2.当Task完成，转为Idle，下一帧遍历所有的Task，判断是否有需要执行的任务
        /// </summary>
        public void ForeachTask()
        {
            if (curAction == null) return;
            if (curAction.ActionCompleted())
            {
                Debug.Log("curAction is complete! seach next task");
               
                for (int i = 1; i < tasks.Count; i++)
                {
                     Debug.Log("Switch to  " + tasks[i].GetType());
                    if (tasks[i].TaskExecutable())
                    {
                        Debug.Log("Switch to  " + i);
                       SwitchState(i);
                       return;
                    }                
                }
                 SwitchState(0);                 
            }

        }

        public void AddTask(TaskBase task)
        {
            if(task == null) return;
            this.tasks.Add(task);
        }


    }

}
