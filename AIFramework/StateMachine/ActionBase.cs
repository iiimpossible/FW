using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GraphyFW.AI
{
    public delegate bool AIFunction();
    public class ActionBase
    {
        public AIFunction condition;
        private ActorController _controller;

        public ActionBase nextAction;
        public ActionBase(ActorController controller)
        {
            this._controller = controller;
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

    }

}
