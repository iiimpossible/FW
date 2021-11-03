using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace GraphyFW.AI
{


    /// <summary>
    /// 巡逻行为，每隔一定时间会执行搜索行为，搜索周围是否有食物、敌人等
    /// 1.随机巡逻或者定义巡逻路径
    /// 2.搜索行为
    /// 3.使用一个全局变量，巡逻改变该变量吗？
    /// </summary>
    public class ActionPatrol : ActionBase
    {
        //随机巡逻目标点
        bool _isCompleted;

        Vector3 _currrentPos;
        Vector3 _targetPos;

        Vector3 _RandomPos;

        Vector3 _originPos;

        float _patrolRadius;

        float _delayTime = 1.0f;

        float _timeCounter = 0.0f;
        public ActionPatrol(ActorController controller, AIRunData runData) : base(controller, runData)
        {
           // runData.SetVec3Data()
        }


        public override void ActionEnter()
        {
           _isCompleted = false;
        }

        /// <summary>
        /// 每隔一段时间更新目标位置
        /// </summary>
        public override void ActionUpdate()
        {
            _timeCounter += Time.deltaTime;
            if( _timeCounter > _delayTime){
                _timeCounter = 0.0f; 
                RandowmTarget();
                _runData.SetVec3Data("TargetPos",_RandomPos);
            _isCompleted = true;
            }
        }

        public override void ActionExit()
        {
             
        }


        public override bool ActionCompleted()
        {
            return _isCompleted;
        }

        private void RandowmTarget()
        {
            _RandomPos.Set(Random.Range(_originPos.x, _patrolRadius),Random.Range(_originPos.y, _patrolRadius),_controller.actor.transform.position.z);           
        }
    }


    /// <summary>
    /// 启用该行为，角色会移动到目标位置
    /// 状态转换应该是自动的。当执行完后应该自动转到另一个状态
    /// </summary>
    public class ActionMoveTo : ActionBase
    {
        public float speed = 10.0f;   
        private bool _isMove;

        private Vector3 _target;

        private Vector3 _direction;

        private ActionBase _nextAction;
        public ActionMoveTo(ActorController controller, AIRunData runData) : base(controller, runData)
        {

        }

        public void SetTarget(Vector3 target)
        {
            _target = target;
            _isMove = true;
            _direction = (_target - _controller.actor.transform.position).normalized;
        }

        public void SetNextState(ActionBase action)
        {
            _nextAction = action;
        }

        public override void ActionEnter()
        {
            _target = _runData.GetVec3Data("TargetPos");
            _isMove = true;
            _direction = (_target - _controller.actor.transform.position).normalized;
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


        /// <summary>
        /// 注意这个到达判定大有问题，当在一帧之内，物体到达但是越过了目标，导致检测失败
        /// </summary>
        /// <returns></returns>
        private bool Arrive()
        {            
            if((_target - _controller.actor.transform.position).sqrMagnitude < 0.01f)
                return true;
            return false;
        }
    }

    /// <summary>
    /// 攻击行为，会利用消息系统给目标发送消息
    /// 1.攻击处理器会根据收到的消息，根据两边的攻击、防御计算伤害并发送给被攻击者
    /// 2.攻击会触发攻击动画
    /// </summary>
    public class ActionAttack: ActionBase
    {
        Animator animator;
        ActionAttack(ActorController controller, AIRunData runData) : base(controller, runData)
        {

        }


        public override void ActionUpdate()
        {
            base.ActionUpdate();
        }

        public override bool ActionCompleted()
        {
            return base.ActionCompleted();
        }

    }

}
