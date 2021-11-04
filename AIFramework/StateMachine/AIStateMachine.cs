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
    /// </summary>
    public class AIStateMachine
    {
        private ActionBase lastAction;

        private ActionBase globalAction;

        private ActionBase curAction;

        public AIStateMachine()
        {
            //dicActions = new dic
        }


        /// <summary>
        /// 循环执行状态
        /// </summary>
        public void Update()
        {            
            if(curAction == null) return;
            curAction.ActionUpdate();
            SwicthState();          
        }

        /// <summary>
        /// 怎么条件转换？
        /// </summary>
        public void SwicthState()
        {
            if (curAction == null) return;
            if (curAction.ActionCompleted())
            {
                lastAction = curAction;
                if (curAction.nextAction != null)
                {
                    Debug.Log("Switch state.~~~~~~~~");
                    curAction = curAction.nextAction;
                    curAction.ActionEnter();
                    lastAction.ActionExit();
                }
            }
        }

        public void SwitchToLastState()
        {

        } 

        public void SetStartState(ActionBase action)
        {
            curAction = action;
            curAction.ActionEnter();
        }
    }

}
