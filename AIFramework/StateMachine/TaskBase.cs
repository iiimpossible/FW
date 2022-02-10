using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GraphyFW.AI
{
    /// <summary>
    /// 【任务】是描述做一件事情所需要的【行为】的集合
    /// 1.保存现场，数据，状态
    /// 2.执行指令（行为）
    /// 3.恢复现场，
    /// 4.对于一个AI，常态就是 闲逛（idel）状态（行为），执行完任务后就该回到Idel状态，然后每隔一定的时间，会去搜索场景，
    /// 将任务添加到任务队列中。任务队列需要根据权重排序，或者优先队列
    /// 任务也是一个状态，于行为兼容
    /// 
    /// </summary>
    public abstract class TaskBase : StateBase
    {

        /// <summary>
        /// 所有实现该任务的指令集合
        /// </summary>
        /// <typeparam name="StateBase"></typeparam>
        /// <returns></returns>
        protected List<StateBase> m_actions = new List<StateBase>();

        /// <summary>
        /// 当前被执行的指令
        /// </summary>
        protected StateBase m_currentState = null;

        /// <summary>
        /// 上一个被执行的指令
        /// </summary>
        protected StateBase _lastState = null; 

        //指令索引
        private int m_stateIndex = 0;

        /// <summary>
        /// 该Task是否是静态的（仅仅手动控制，不进行轮询判断激活）
        /// </summary>
        /// <value></value>
        public bool isStatic {get; protected set;}

        public TaskBase(ActorController controller, AIRunData runData) : base(controller, runData)
        {
            isStatic = false;
        }

        /// <summary>
        /// 当进入时 要刷新装填
        /// </summary>
        public sealed override void ActionEnter()
        {
            m_stateIndex = 0;
            if (m_actions.Count > 0)
            {
                m_currentState = m_actions[m_stateIndex];
                m_isCompleted = false;
            }
            if (m_currentState == null) return;
                m_currentState.ActionEnter();

             //如果执行错误，直接退出该任务
            if (m_currentState.isExecuteError)
            {
                m_isCompleted = true;
                 Debug.LogWarning("TaskBase: Action run error. Current action is: " + m_currentState.GetType());
                return;
            }

        }

        public sealed override void ActionUpdate()
        {

            if (m_currentState == null) return;
            m_currentState.ActionUpdate();
            //如果执行错误，直接退出该任务
            if (m_currentState.isExecuteError)
            {
                m_isCompleted = true;
                return;
            }
            SwitchState();
        }

        public sealed override void ActionExit()
        {
            m_stateIndex = 0;
            m_currentState = null;
            m_isCompleted = false;
            foreach (var item in m_actions)
            {
                item.ActionExit();
            }
        }

        public sealed override bool ActionCompleted()
        {
            return m_isCompleted;
        }

        /// <summary>
        /// 在手动模式下的控制
        /// </summary>
        public virtual void SetManualModeInput()
        {

        }

        /// <summary>
        /// 指令转换
        /// </summary>
        protected void SwitchState()
        {
            if (m_currentState == null) return;
            //Debug.Log("Switch state ~~~~");
            if (m_currentState.ActionCompleted())
            {
                //如果状态完成，检查行为列表还有没有下一个，有就转换状态执行
                m_stateIndex += 1;
                if (m_actions.Count > m_stateIndex)
                {
                    _lastState = m_currentState;
                    m_currentState = m_actions[m_stateIndex];
                    m_currentState.ActionEnter();
                    _lastState.ActionExit();
                }
                else
                {
                    m_isCompleted = true;
                }
            }
        }

        /// <summary>
        /// 添加指令
        /// </summary>
        /// <param name="action"></param>
        public void AddAction(StateBase action)
        {
            m_actions.Add(action);
        }

        /// <summary>
        /// 移除指令
        /// </summary>
        /// <param name="action"></param>
        public void RemoveAction(StateBase action)
        {
            m_actions.Remove(action);
        }

        /// <summary>
        /// 任务结束条件
        /// </summary>
        /// <returns></returns>
        public virtual bool TaskExecutable()
        {
            return false;
        }
    }

}