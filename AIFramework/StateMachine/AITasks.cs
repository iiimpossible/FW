using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace GraphyFW.AI
{
    using GraphyFW.Common;
    


   


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
        private ActionPathMove _pathMoveGo;

        private ActionPathMove _pathMoveBack;
        private ActionFindProp _findProp;

        private ActionTakeUp _takeUp;

        private ActionPutDown _putDown;

        private Vector2Int _originPos = Vector2Int.one;
         
         private bool _isGO = false;
        public TaskCarry(ActorController controller, AIRunData runData):base(controller,runData)
        {            
            _findProp = new ActionFindProp(controller,runData);
            _pathMoveGo = new ActionPathMove(controller,runData,AIRunData.dicKeys[ERunDataKey.PROP_POS]);
            _pathMoveBack = new ActionPathMove(controller,runData, AIRunData.dicKeys[ERunDataKey.STORAGE_VACANCY_POS]);
            _takeUp = new ActionTakeUp(controller,runData);
            _putDown = new ActionPutDown(controller,runData);
            _actions.Add(_findProp);
            _actions.Add(_pathMoveGo);
            _actions.Add(_takeUp);
            _actions.Add(_pathMoveBack);
            _actions.Add(_putDown);//
        
        }

        public override bool TaskExecutable()
        {            
            GameMode.instance.GetStorageArea(out _isGO ) ;
           if( GameMode.instance.GetFoodObject() != null && _isGO )
           {  
               return true;
           }         
           return false;
        }
    }

    /// <summary>
    /// AI无事可做瞎逛
    /// 指令：随机点，MoveTo,从控制器请求事件 或者搜索地图中是否有工作要做
    /// 如：搬运，等
    /// 1.随机点指令
    /// 2.移动指令
    /// 3.延迟指令
    /// 4.任务申请指令（待定，应该时状态机直接访问控制器的状态）
    /// </summary>
    public class TaskIdle : TaskBase
    {
        private ActionPathMove _pathMove;
        private ActionDelay _delay;
        private ActionRandomPos _randomPos;
        private Vector2Int _tgPos = Vector2Int.zero;

        public TaskIdle(ActorController controller, AIRunData runData) : base(controller, runData)
        {
            _pathMove = new ActionPathMove(controller, runData, AIRunData.dicKeys[ERunDataKey.Vec2I_TARGET_POS]);
            _delay = new ActionDelay(controller, runData);
            _randomPos = new ActionRandomPos(controller,runData);

            this._actions.Add(_randomPos);
            this._actions.Add(_pathMove);
            this._actions.Add(_delay);
        }

        
        public override bool TaskExecutable()
        {
            return base.TaskExecutable();
        }

    }


    /// <summary>
    /// 征召
    /// </summary>
    public class TaskCallUp : TaskBase
    {
        public TaskCallUp(ActorController controller, AIRunData runData) : base(controller, runData)
        {

        }

        public override bool TaskExecutable()
        {
            return base.TaskExecutable();
        }
    }

    /// <summary>
    /// 对于一个专门管理移动的状态，应该有多重模式。
    /// 1.随机移动模式（巡逻）
    /// 2.手动指定目标模式
    /// </summary>
    public class TaskMoveTo:TaskBase
    {
        ActionPathMove _pathMove;
        public TaskMoveTo(ActorController controller, AIRunData runData) :base(controller, runData)
        {
            _pathMove = new ActionPathMove(controller, runData, AIRunData.dicKeys[ERunDataKey.Vec2I_TARGET_POS]);
            this.isStatic = true;
            this.AddAction(_pathMove);
        }

        public void SetMoveTarget(Vector2Int target)
        {
            _runData.SetVec2IData(AIRunData.dicKeys[ERunDataKey.Vec2I_TARGET_POS],target);
            Debug.Log("Set pos is: "+ target);
        }

       
    }

 



}