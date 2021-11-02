using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace GraphyFW.AI
{
    public class ActionPatrol : ActionBase
    {
        public ActionPatrol(ActorController controller) : base(controller)
        {

        }
    }


    /// <summary>
    /// 启用该行为，角色会移动到目标位置
    /// 状态转换应该是自动的。当执行完后应该自动转到另一个状态
    /// </summary>
    public class ActionMoveTo : ActionBase
    {
        public float speed = 1.0f;
        private ActorController _controller;
        private bool _isMove;

        private Vector3 _target;

        private Vector3 _direction;

        private ActionBase _nextAction;
        public ActionMoveTo(ActorController controller) : base(controller)
        {

        }

        public void SetTarget(Vector3 target)
        {
            _target = target;
            _isMove = true;
            _direction = _target - _controller.actor.transform.position;
        }

        public void SetNextState(ActionBase action)
        {
            _nextAction = action;
        }

        public override void ActionEnter()
        {
            
        }

        public override void ActionExit()
        {
            
        }


        public override void ActionUpdate()
        {
            if(_isMove)
                _controller.actor.transform.position+= _direction * speed * Time.deltaTime;
            if(Arrive())
                _isMove = false;            
        }

        public override bool ActionCompleted()
        {
            return !_isMove;
        }


        private bool Arrive()
        {
            if((_target - _controller.actor.transform.position).sqrMagnitude < 0.01f)
                return true;
            return false;
        }
    }

}
