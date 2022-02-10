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
           _originPos = m_controller.transform.position;
        }


        public override void ActionEnter()
        {
           m_isCompleted = false;
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
                m_runData.SetVec3Data("TargetPos", _RandomPos);
                Debug.Log("Target date----->"+ m_runData.GetVec3Data("TargetPos"));
                m_isCompleted = true;
            }
        }

        public override void ActionExit()
        {
             m_isCompleted = false;
        }


        public override bool ActionCompleted()
        {
            return m_isCompleted;
        }

        private void RandowmTarget()
        {
            _RandomPos.Set(Random.Range(_originPos.x, _patrolRadius),Random.Range(_originPos.y, _patrolRadius),m_controller.transform.position.z);           
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
            _direction = (_target - m_controller.transform.position).normalized;
        }

        public void SetNextState(StateBase action)
        {
            _nextAction = action;
        }

        public override void ActionEnter()
        {
            if(_isMove == true)return;
            _target = m_runData.GetVec3Data("TargetPos");
            Debug.Log("Enter actioin MoveTO"+ _target);
            _isMove = true;
            _direction = (_target - m_controller.transform.position).normalized;
        }

        public override void ActionExit()
        {
            
        }


        public override void ActionUpdate()
        {
            Debug.Log("Action MoveTo Called.~~~~~");
            if(_isMove)
                m_controller.transform.position+= _direction * speed * Time.deltaTime;
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
            if((_target - m_controller.transform.position).sqrMagnitude < 0.01f)
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
            m_runData.SetVec2IData("TargetPos",  m_map.GetMapCorner(1));
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
            m_isCompleted = false;   
            //这个actor在地图中的位置，应该找到对应的状态类？
            AIAlgorithm.AstarSearch(m_map,m_map.WorldSpaceToMapSpace(m_controller.transform.position),m_runData.GetVec2IData("TargetPos"),_path);
        }

        public override void ActionUpdate()
        {            
            if (_path.Count > 0)
            {
                _dir = _path[_path.Count-1] - (Vector2)m_controller.transform.position;
                m_controller.transform.Translate(_dir.x * Time.deltaTime * speed, _dir.y * Time.deltaTime * speed, m_controller.transform.position.z);
                if (Arrive(_path[_path.Count-1]))
                {
                    _path.RemoveAt(_path.Count-1);
                }
            }
            else
            {
                m_isCompleted = true;
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
            return m_isCompleted;
        }


        public bool Arrive(Vector3 pos)
        {
            if ((pos - m_controller.transform.position).sqrMagnitude < 0.01f)
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
        bool m_isGot = true;
        public ActionFindProp(ActorController controller, AIRunData runData):base (controller,runData)
        {

        }

        public override void ActionEnter()
        {
            //从地图管理器获取可以搬运的道具
            Food fd = GameMode.instance.GetFoodObject().GetComponent<Food>();   
            fd.isOccupied = true;        
            if(fd == null)
            {
                Debug.LogError("Food is null");
                this.isExecuteError = true;
                return;
            }
            ScptStorageArea area = GameMode.instance.GetStorageArea();
            //设置道具位置    
            m_runData.SetVec2IData(AIRunData.dicKeys[ERunDataKey.PROP_POS], m_map.WorldSpaceToMapSpace(fd.transform.position));
            //设置道具引用
            m_runData.SetPropData(AIRunData.dicKeys[ERunDataKey.Prop], fd);
            //设置存储区空位
            m_runData.SetVec2IData(AIRunData.dicKeys[ERunDataKey.STORAGE_VACANCY_POS],area.GetEmptyPos());
            Debug.LogWarning("Find prop enter.");
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
        private Vector2 m_dir = Vector2.zero;
        private Vector3 m_deltaPos = Vector3.zero;


        private string m_valueKey;

        List<Vector2> m_path = new List<Vector2>();
        public ActionPathMove(ActorController controller, AIRunData runData, string valueKey) : base(controller, runData)
        {
            m_valueKey = valueKey;
        }


        public override void ActionEnter()
        {
           bool is_searched  = AIAlgorithm.AstarSearch(m_map, m_map.WorldSpaceToMapSpace(m_controller.transform.position),
                  m_runData.GetVec2IData(m_valueKey), m_path);
            if(is_searched == false)
            {
                this.isExecuteError = true;
                this.ActionExit();
               
                return;
            }
            this.m_isCompleted = false;
            CameraSelectObject.instance.AddLineVerts(m_path);//将路径点传给MainCamera绘制
        }

        public override void ActionUpdate()
        {
            if (m_path.Count > 0)
            {
                
                m_dir = (m_path[m_path.Count - 1] - (Vector2)m_controller.transform.position).normalized;//计算方向
                m_controller.transform.up = new Vector3(m_dir.x, m_dir.y,0);//修改actor的up方向
                m_deltaPos.Set(m_dir.x * Time.deltaTime * speed, m_dir.y * Time.deltaTime * speed, 0);
                m_controller.transform.position += m_deltaPos;
                if (AIAlgorithm.Arrive(m_path[m_path.Count - 1], m_controller.transform.position))
                {
                    m_path.RemoveAt(m_path.Count - 1);
                }
            }
            else
            {
                //重定位，抵消舍入误差
                m_deltaPos.Set(Mathf.Round(m_controller.transform.position.x),Mathf.Round(m_controller.transform.position.y) ,0);
                m_controller.transform.position = m_deltaPos;
                m_isCompleted = true;
                CameraSelectObject.instance.RemoveLineVerts(m_path);
            }
        }

        public override void ActionExit()
        {

            m_path.Clear();
            base.ActionExit();
            m_isCompleted = false;
        }

        public override bool ActionCompleted()
        {
            return m_isCompleted;
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
            prop = m_runData.GetPropData(AIRunData.dicKeys[ERunDataKey.Prop]);
            prop.transform.SetParent(m_controller.transform);
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
            prop = m_runData.GetPropData(AIRunData.dicKeys[ERunDataKey.Prop]);
            if (prop == null)
            {
                Debug.Log("Cant get prop from run data, go is null");
                return;
            }
            prop.transform.parent = null;
            prop.transform.eulerAngles = Vector3.zero;
            m_runData.SetPropData(AIRunData.dicKeys[ERunDataKey.Prop], null);
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
            if(_curTime > delayTime) m_isCompleted = true;            
        }

        public override void ActionExit()
        {             
            //Debug.Log("Exit delay ----->");
            _curTime = 0;
            m_isCompleted = false;
            delayTime = 1.5f;
        }

        public override bool ActionCompleted()
        {
            return m_isCompleted;
        }
    }

    /// <summary>
    /// 提供一个随机的位置
    /// </summary>
    public class ActionRandomPos : StateBase
    {
        public int range = 10;
        private Vector2Int _pos = Vector2Int.zero;

        private Vector2Int _actorPos = Vector2Int.zero;
        public ActionRandomPos(ActorController controller, AIRunData runData) : base(controller, runData)
        {

        }

        public override void ActionEnter()
        {
           
            _actorPos = m_map.WorldSpaceToMapSpace(m_controller.transform.position);
            _pos = m_map.RandomPos(_actorPos,10);            
            m_runData.SetVec2IData(AIRunData.dicKeys[ERunDataKey.Vec2I_TARGET_POS],_pos);
            //Debug.Log("Random pos is -->" + _pos);
            m_isCompleted = true;            
        } 

    }


    /// <summary>
    /// Action: 吃东西
    /// 1.吃完后，根据食物的属性，给Actor增加各种属性
    /// 2.只要重写Enter方法
    /// </summary>
    public class ActionEat : StateBase
    {
        GameObject food;
        public ActionEat(ActorController controller, AIRunData runData) : base(controller, runData)
        {

        }

        public override void ActionEnter()
        {
            food = m_runData.GetGoData(AIRunData.dicKeys[ERunDataKey.FOOD]);
            if(food == null)
            {
                ActionExecuteError("Attempt to access a null FOOD.");
                return;
            }
            Food fcomp = food.GetComponent<Food>();
            ControlledActor actor = m_controller.transform.GetComponent<ControlledActor>();
            
            actor.nutrition += fcomp.nutrition;
            GameMode.instance.DestroyFood(food);
            //TODO：属性增加

            base.ActionEnter();
            m_isCompleted = true;
        }
    }









}
