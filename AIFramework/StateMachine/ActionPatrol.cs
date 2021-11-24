using System.Collections.Generic;
using UnityEngine;

namespace GraphyFW.AI
{
    using GraphyFW.GamePlay;


    /// <summary>
    /// 巡逻行为，每隔一定时间会执行搜索行为，搜索周围是否有食物、敌人等
    /// 1.随机巡逻或者定义巡逻路径
    /// 2.搜索行为
    /// 3.使用一个全局变量，巡逻改变该变量吗？
    /// 4.该状态实际是一个idle状态，是在没有任务的时候
    /// </summary>
    public class ActionPatrol : StateBase
    {
        //随机巡逻目标点       

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
           _patrolRadius = 10.0f;
           _originPos = _controller.transform.position;
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
           // Debug.Log("Action ActionPatrol Called.~~~~~");
            _timeCounter += Time.deltaTime;          
            if (_timeCounter > _delayTime)//
            {                
                //if(_isCompleted == true) return;
                _timeCounter = 0.0f;
                RandowmTarget();
                _runData.SetVec3Data("TargetPos", _RandomPos);
                Debug.Log("Target date----->"+ _runData.GetVec3Data("TargetPos"));
                _isCompleted = true;
            }
        }

        public override void ActionExit()
        {
             _isCompleted = false;
        }


        public override bool ActionCompleted()
        {
            return _isCompleted;
        }

        private void RandowmTarget()
        {
            _RandomPos.Set(Random.Range(_originPos.x, _patrolRadius),Random.Range(_originPos.y, _patrolRadius),_controller.transform.position.z);           
        }
    }


    /// <summary>
    /// 启用该行为，角色会移动到目标位置
    /// 状态转换应该是自动的。当执行完后应该自动转到另一个状态
    /// </summary>
    public class ActionMoveTo : StateBase
    {
        public float speed = 10.0f;   
        private bool _isMove;

        private Vector3 _target;

        private Vector3 _direction;

        private StateBase _nextAction;
        public ActionMoveTo(ActorController controller, AIRunData runData) : base(controller, runData)
        {

        }

        public void SetTarget(Vector3 target)
        {
            _target = target;
            _isMove = true;
            _direction = (_target - _controller.transform.position).normalized;
        }

        public void SetNextState(StateBase action)
        {
            _nextAction = action;
        }

        public override void ActionEnter()
        {
            if(_isMove == true)return;
            _target = _runData.GetVec3Data("TargetPos");
            Debug.Log("Enter actioin MoveTO"+ _target);
            _isMove = true;
            _direction = (_target - _controller.transform.position).normalized;
        }

        public override void ActionExit()
        {
            
        }


        public override void ActionUpdate()
        {
            Debug.Log("Action MoveTo Called.~~~~~");
            if(_isMove)
                _controller.transform.position+= _direction * speed * Time.deltaTime;
            if(Arrive())
                _isMove = false;            
        }

        public override bool ActionCompleted()
        {
            // if(condition != null)
            // {
            //     bool c = condition.Invoke();
            //     if(_isMove != c) _isMove = false;                 
            // }
            
            return !_isMove;
        }


        /// <summary>
        /// 注意这个到达判定大有问题，当在一帧之内，物体到达但是越过了目标，导致检测失败
        /// </summary>
        /// <returns></returns>
        private bool Arrive()
        {            
            if((_target - _controller.transform.position).sqrMagnitude < 0.01f)
                return true;
            return false;
        }
    }

    /// <summary>
    /// 攻击行为，会利用消息系统给目标发送消息
    /// 1.攻击处理器会根据收到的消息，根据两边的攻击、防御计算伤害并发送给被攻击者
    /// 2.攻击会触发攻击动画
    /// </summary>
    public class ActionAttack: StateBase
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



    /// <summary>
    /// 调用搜索算法，寻找目标物体或者寻路
    /// 1.寻路算法和移动行为、巡逻行为怎么联动？
    /// </summary>
    public class ActionSearchMove : StateBase
    {

        public float speed = 10f;

        private Vector2 _dir;
        private List<Vector2> _path;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="runData"></param>
        /// <returns></returns>
        public ActionSearchMove(ActorController controller, AIRunData runData): base(controller, runData)
        {            
            _runData.SetVec2IData("TargetPos",  _map.GetMapCorner(1));
            _path = new List<Vector2>();
            _dir = new Vector2();            
        }

        /// <summary>
        /// 当actor进入这个状态，进行寻路
        /// 要想访问格子上的物体，是否需要更新格子的信息？如当前游戏物体是否在格子上？
        /// </summary>
        public override void ActionEnter()
        {
            //Debug.Log("Action search move ActionEnter.");
            _isCompleted = false;   
            //这个actor在地图中的位置，应该找到对应的状态类？
            AIAlgorithm.AstarSearch(_map,_map.WorldSpaceToMapSpace(_controller.transform.position),_runData.GetVec2IData("TargetPos"),_path);
        }

        public override void ActionUpdate()
        {            
            if (_path.Count > 0)
            {
                _dir = _path[_path.Count-1] - (Vector2)_controller.transform.position;
                _controller.transform.Translate(_dir.x * Time.deltaTime * speed, _dir.y * Time.deltaTime * speed, _controller.transform.position.z);
                if (Arrive(_path[_path.Count-1]))
                {
                    _path.RemoveAt(_path.Count-1);
                }
            }
            else
            {
                _isCompleted = true;
            }
        }


        /// <summary>
        /// 处理一些状态，还原？
        /// </summary>
        public override void ActionExit()
        {
             //Debug.Log("Action search move ActionExit.");
            _path.Clear();
        }


        public override bool ActionCompleted()
        {
             //Debug.Log("Action search move ActionCompleted.");
            return _isCompleted;
        }


        public bool Arrive(Vector3 pos)
        {
            if ((pos - _controller.transform.position).sqrMagnitude < 0.01f)
                return true;
            return false;
        }
    }

 

    /// <summary>
    /// 该行为专门用于搜寻道具（以后可能改为搜寻任务）
    /// rundata 关键字 "PropPos"
    /// </summary>
    public class ActionFindProp:StateBase
    {
        public ActionFindProp(ActorController controller, AIRunData runData):base (controller,runData)
        {

        }

        public override void ActionEnter()
        {
            //从地图管理器获取可以搬运的道具
            Food fd = AISystem.instance.GetFoodObject();
            //从地图管理器获取可以存储的存储区
            MapStorageArea area = AISystem.instance.GetStorageArea();
            //设置道具位置    
            _runData.SetVec2IData(AIRunData.dicKeys[ERunDataKey.PROP_POS], _map.WorldSpaceToMapSpace(fd.propGo.transform.position));
            //设置道具引用
            _runData.SetPropData(AIRunData.dicKeys[ERunDataKey.Prop], fd);
            //设置存储区引用
            _runData.SetVec2IData(AIRunData.dicKeys[ERunDataKey.MOVE_TARGET_POS], area.GetEmptyPos());
        }

        public override bool ActionCompleted()
        {
            return true;
        }
    }

    /// <summary>
    /// 输入终点，搜寻路径，沿路径走到目标位置
    /// </summary>
    public class ActionPathMove : StateBase
    {

        public float speed = 2f;
        private Vector2 _dir = Vector2.zero;
        private Vector3 _deltaPos = Vector3.zero;


        private string _valueKey;

        List<Vector2> _path = new List<Vector2>();
        public ActionPathMove(ActorController controller, AIRunData runData, string valueKey) : base(controller, runData)
        {
            _valueKey = valueKey;
        }


        public override void ActionEnter()
        {
            AIAlgorithm.AstarSearch(_map, _map.WorldSpaceToMapSpace(_controller.transform.position),
                  _runData.GetVec2IData(_valueKey), _path);
            this._isCompleted = false;
            CameraSelectObject.instance.AddLineVerts(_path);
        }

        public override void ActionUpdate()
        {
            if (_path.Count > 0)
            {
                
                _dir = (_path[_path.Count - 1] - (Vector2)_controller.transform.position).normalized;
                _controller.transform.up = new Vector3(_dir.x, _dir.y,0);
                _deltaPos.Set(_dir.x * Time.deltaTime * speed, _dir.y * Time.deltaTime * speed, 0);
                _controller.transform.position += _deltaPos;
                if (AIAlgorithm.Arrive(_path[_path.Count - 1], _controller.transform.position))
                {
                    _path.RemoveAt(_path.Count - 1);
                }
            }
            else
            {
                //重定位，抵消舍入误差
                _deltaPos.Set(Mathf.Round(_controller.transform.position.x),Mathf.Round(_controller.transform.position.y) ,0);
                _controller.transform.position = _deltaPos;
                _isCompleted = true;
                CameraSelectObject.instance.RemoveLineVerts(_path);
            }
        }

        public override void ActionExit()
        {

            _path.Clear();
            base.ActionExit();
            _isCompleted = false;
        }

        public override bool ActionCompleted()
        {
            return _isCompleted;
        }
    }


    /// <summary>
    /// 拿起物体行为
    /// 1.目标物体引用
    /// 怎么搜索物体？使用一个写好的方法搜索吗
    /// 当点击一个Actor，直接让它去捡起地图上的任意一个物体又该如何实现？
    /// 
    /// </summary>
    public class ActionTakeUp :StateBase
    {
        Prop prop;
        public ActionTakeUp(ActorController controller, AIRunData runData):base(controller, runData)
        {
           
        }

        public override void ActionEnter()
        {
            prop = _runData.GetPropData(AIRunData.dicKeys[ERunDataKey.Prop]);
            prop.propGo.transform.SetParent(_controller.transform);
            Debug.Log("Take up a prop." + prop.GetType());
            prop.isOccupied = true;
        }

        public override void ActionExit()
        {
            prop = null;
            base.ActionExit();
        }

        public override bool ActionCompleted()
        {
            return true;
        }
    }

    /// <summary>
    /// 放下物体
    /// </summary>
    public class ActionPutDown : StateBase
    {
        Prop prop;
        public ActionPutDown(ActorController controller, AIRunData runData) : base(controller, runData)
        {

        }

        public override void ActionEnter()
        {
             PutDown();
            prop.isStored = true;
        }

        public override void ActionExit()
        {
           if(prop != null) PutDown();
            prop = null;
            base.ActionExit();
        }

        public override bool ActionCompleted()
        {
            return true;
        }

        private void PutDown()
        {
            prop = _runData.GetPropData(AIRunData.dicKeys[ERunDataKey.Prop]);
            if (prop == null)
            {
                Debug.Log("Cant get prop from run data, go is null");
                return;
            }
            prop.propGo.transform.parent = null;
            _runData.SetPropData(AIRunData.dicKeys[ERunDataKey.Prop], null);
            prop.isOccupied = false;

        }

    }


    /// <summary>
    /// 延迟一定时间
    /// </summary>
    public class ActionDelay:StateBase
    {
        public float delayTime {get;set;}
        private float _curTime = 0;
        public ActionDelay(ActorController controller, AIRunData runData,float delayTime = 1.5f) : base(controller, runData)
        {
            this.delayTime = delayTime;
        }

      
        public override void ActionEnter()
        {
            base.ActionEnter();
        }

        /// <summary>
        /// 按照预定的延迟时间，设置完成状态
        /// </summary>
        public override void ActionUpdate()
        {
            _curTime += Time.deltaTime;
            if(_curTime > delayTime) _isCompleted = true;            
        }

        public override void ActionExit()
        {             
            Debug.Log("Exit delay ----->");
            _curTime = 0;
            _isCompleted = false;
            delayTime = 1.5f;
        }

        public override bool ActionCompleted()
        {
            return _isCompleted;
        }
    }

    /// <summary>
    /// 提供一个随机的位置
    /// </summary>
    public class ActionRandomPos : StateBase
    {
        public int range = 10;
        private Vector2Int _pos = Vector2Int.zero;
        public ActionRandomPos(ActorController controller, AIRunData runData) : base(controller, runData)
        {

        }

        public override void ActionEnter()
        {
            _pos.Set(Random.Range(0,range),Random.Range(0,range));
            _runData.SetVec2IData(AIRunData.dicKeys[ERunDataKey.TARGET_POS],_pos);
            //Debug.Log("Random pos is -->" + _pos);
            _isCompleted = true;            
        } 

    }









}
