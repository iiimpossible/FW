using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GraphyFW.AI
{
    public delegate bool AIFunction();
    public abstract class StateBase
    {
        protected AIRunData m_runData{get;set;}
        protected ActorController m_controller;

        protected bool m_isCompleted{get;set;}

        protected MapBase<AIBrickState> m_map;
        public StateBase nextAction;

        public bool isExecuteError{get;protected set;}
        public StateBase(ActorController controller, AIRunData runData)
        {   
            this.m_controller = controller;
            this.m_runData = runData;
            this.isExecuteError = false;
            m_isCompleted = true;
            m_map = m_runData.GetMapData("MainMap");
        }
         public virtual void ActionEnter()
        {

        }

        public virtual void ActionUpdate()
        {
            
        }
        public virtual void ActionExit()
        {
            m_isCompleted = false;
            isExecuteError = false;
        }

        public virtual bool ActionCompleted()
        {
            return m_isCompleted;
        }

        /// <summary>
        /// 当执行错误时调用此方法，将Action标记为执行错误，会在Task下一次执行Enter或者Update时判断是否应该终止执行Task
        /// </summary>
        /// <param name="log"></param>
        protected void ActionExecuteError(string log)
        {
            Debug.LogWarning("AI run error: " + log);
            isExecuteError = true;
        }
        //TODO:某些行为独立于状态机转换，需要计时以及时调用。即当时间到达时候，立即将状态转换为该状态 再说吧

    }

}
