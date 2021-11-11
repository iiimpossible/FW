using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GraphyFW.AI
{
    using GraphyFW.Common;

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
    public class TaskBase : StateBase
    {      
        protected List<StateBase> _actions = new List<StateBase>();//

        protected StateBase _currentState = null;

        protected StateBase _lastState = null;

        private int _stateIndex = 0;

        public TaskBase(ActorController controller, AIRunData runData):base(controller,runData)
        {
          
        }

        /// <summary>
        /// 当进入时 要刷新装填
        /// </summary>
        public sealed override void ActionEnter()
        {
            _stateIndex = 0;
            if (_actions.Count > 0)
            {
                _currentState = _actions[_stateIndex];
                _isCompleted = false;
            }
            if (_currentState == null) return;
            _currentState.ActionEnter();
        }

        public sealed override void ActionUpdate()
        {
            if(_currentState == null) return;
            _currentState.ActionUpdate();
            SwitchState();           
        }

        public sealed override void ActionExit()
        {
            _stateIndex = 0;
            _currentState = null;
        }

        public sealed override bool ActionCompleted()
        {
            return _isCompleted;        
        } 

        protected void SwitchState()
        {    
            if (_currentState == null) return;
            if (_currentState.ActionCompleted())
            {
                //如果状态完成，检查行为列表还有没有下一个，有就转换状态执行
                _stateIndex += 1;
                if(_actions.Count > _stateIndex)
                {
                    _lastState = _currentState;
                    _currentState = _actions[_stateIndex];
                    _currentState.ActionEnter();
                    _lastState.ActionExit();
                }  
            }
        }


        public void AddAction(StateBase action)
        {
            _actions.Add(action);
        }


        public void RemoveAction(StateBase action)
        {
            _actions.Remove(action);
        }
    }



    /// <summary>
    /// 每个行为是不是应该时静态类。
    /// 搬运任务需要 来回移动指令，一个搬运指令
    /// 定义指令到底需不需要new？
    /// 对于搬运任务
    /// 1.玩家指定或者Actor自动搜索目标物体 （搜索方法或者行为，还是需要一个搜索行为）
    /// 2.确定目标物体的位置并寻路（寻路算法，移动行为）
    /// 3.到达目标物体附近并捡起目标物体（拿起行为）
    /// 4.搬运目标物体到原位置（移动行为）
    /// 5.放下物体（放下行为）
    /// 6.或许整个游戏系统需要一个任务管理器，会检索当前的工作机会，然后给actor派发任务，当有任务
    /// </summary>
    public class TaskCarry : TaskBase
    {
        private ActionSearchMove _actinSearchMove;

         
        public TaskCarry(ActorController controller, AIRunData runData):base(controller,runData)
        {
            
        }
    }

    


}