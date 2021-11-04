using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GraphyFW.AI
{
    public delegate bool AIFunction();
    public class ActionBase
    {
        public AIFunction condition;

        protected AIRunData _runData{get;set;}
        protected ActorController _controller;

        public ActionBase nextAction;
        public ActionBase(ActorController controller, AIRunData runData)
        {
            if(controller == null) Debug.Log  ("At ActionBase cotroll null~~~~~~~");
            this._controller = controller;
            this._runData = runData;
        }
         public virtual void ActionEnter()
        {

        }

        public virtual void ActionUpdate()
        {
            
        }
        public virtual void ActionExit()
        {

        }

        public virtual bool ActionCompleted()
        {
            return true;
        }

        //TODO:某些行为独立于状态机转换，需要计时以及时调用。即当时间到达时候，立即将状态转换为该状态 再说吧


    }

}
