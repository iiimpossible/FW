using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace GraphyFW.AI
{
    using GraphyFW.GamePlay;

    /// <summary>
    /// 根据某种指令来设置DataKey
    /// </summary>
    public enum ERunDataKey
    {
        ORIGIN_POS = 1,
        Vec2I_TARGET_POS = 2,

        MAIN_MAP = 3,

        Vec2I_PLAYER_POS = 4,
        Prop = 5 ,

        Nest_Pos = 6,   

        PROP_POS = 7,    

        IS_SELECTED = 8,//Actor被选中

        STORAGE_AREA = 9,

        STORAGE_VACANCY_POS = 10,//移动指令的目标位置

    }

    /// <summary>
    /// 一个状态机拥有一个运行时数据实例
    /// 默认关键字：目标位置 "TargetPos"
    /// </summary>
    public class AIRunData
    {
       
        /// <summary>
        /// 数据关键字定义
        /// </summary>
        /// <value></value>
        public static Dictionary<ERunDataKey, string> dicKeys = new Dictionary<ERunDataKey, string>
    {
        {ERunDataKey.Vec2I_TARGET_POS,"TargetPos"},//寻路时的目标位置，vec2i
        {ERunDataKey.ORIGIN_POS,"OriginPos"},//寻路时的起始位置，vec2i
        {ERunDataKey.MAIN_MAP, "MainMap"},//当前的游戏地图
        {ERunDataKey.Vec2I_PLAYER_POS,"PlayerPos"},//玩家在地图上的位置，vec2i
        {ERunDataKey.Prop, "Prop"},//临时获得的道具引用，搬运时存储，Prop
        {ERunDataKey.Nest_Pos, "NestPos"},//巢穴位置
        {ERunDataKey.PROP_POS, "PropPos"},//临时获取的道具的位置，用于搬运，vec2i
        {ERunDataKey.IS_SELECTED,"IsSelected"},//当前Actor是否被选中，bool
        {ERunDataKey.STORAGE_AREA, "StorageArea"},//当前获得的存储区引用
        {ERunDataKey.STORAGE_VACANCY_POS, "StorageVacancyPos"},//从存储区获取的空位，vec2i

    };

        Dictionary<string, float> _dicFloatData = null;
        Dictionary<string, int> _dicIntData = null;

        Dictionary<string, Vector3> _dicVec3Data = null;
        Dictionary<string, Vector2> _dicVec2Data = null;
        Dictionary<string, Vector2Int> _dicVec2IData = null;

        Dictionary<string, GameObject> _dicGoData = null;

        Dictionary<string, MapBase<AIBrickState>> _dicBrickData = null;

        Dictionary<string, Prop> _dicPropData = null;

        Dictionary<string, object> _dicObjectData = null;

        //List<string> defaultKey ;
        public AIRunData()
        {
            //defaultKey .Add("TargetPos");
        }

        /// <summary>
        /// float类型数据存取
        /// </summary>
        /// <param name="dataName"></param>
        /// <param name="data"></param>
        public void SetFloatData(string dataName, float data)
        {
            if (_dicFloatData != null)
            {
                if (_dicFloatData.ContainsKey(dataName))
                {
                    _dicFloatData[dataName] = data;
                }
                else
                {
                    _dicFloatData.Add(dataName, data);
                }
            }
            else
            {
                _dicFloatData = new Dictionary<string, float>();
                _dicFloatData.Add(dataName, data);
            }
        }

        public float GetFloatData(string dataName)
        {
            if (_dicFloatData != null)
                return _dicFloatData[dataName];
            return default(float);
        }

        /// <summary>
        /// Int类型数据存取
        /// </summary>
        /// <param name="dataName"></param>
        /// <param name="data"></param>
        public void SetIntData(string dataName, int data)
        {
            if (_dicIntData != null)
            {
                if (_dicIntData.ContainsKey(dataName))
                {
                    _dicIntData[dataName] = data;
                }
                else
                {
                    _dicIntData.Add(dataName, data);
                }
            }
            else
            {
                _dicIntData = new Dictionary<string, int>();
                _dicIntData.Add(dataName, data);
            }
        }

        public int GetIntData(string dataName)
        {
            if (_dicIntData != null)
                return _dicIntData[dataName];
            return default(int);
        }

        /// <summary>
        /// vec2类型数据存取
        /// </summary>
        /// <param name="dataName"></param>
        /// <param name="data"></param>
        public void SetVec2Data(string dataName, Vector2 data)
        {
            if (_dicVec2Data != null)
            {
                if (_dicVec2Data.ContainsKey(dataName))
                {
                    _dicVec2Data[dataName] = data;
                }
                else
                {
                    _dicVec2Data.Add(dataName, data);
                }
            }
            else
            {
                _dicVec2Data = new Dictionary<string, Vector2>();
                _dicVec2Data.Add(dataName, data);
            }
        }

        public Vector2 GetVec2Data(string dataName)
        {
            if (_dicVec2Data != null)
                return _dicVec2Data[dataName];
            return default(Vector2);
        }


        /// <summary>
        /// Vec2Int 类型数据存取
        /// </summary>
        /// <param name="dataName"></param>
        /// <param name="data"></param>
        public void SetVec2IData(string dataName, Vector2Int data)
        {
            if (_dicVec2IData != null)
            {
                if (_dicVec2IData.ContainsKey(dataName))
                {
                    _dicVec2IData[dataName] = data;
                }
                else
                {
                    _dicVec2IData.Add(dataName, data);
                }
            }
            else
            {
                _dicVec2IData = new Dictionary<string, Vector2Int>();
                _dicVec2IData.Add(dataName, data);
            }
        }

        public Vector2Int GetVec2IData(string dataName)
        {
            if (_dicVec2IData != null)

            {
                if(_dicVec2IData.ContainsKey(dataName )== false)
                {
                    Debug.LogError($"Key not exist: [{dataName}]");
                }
                return _dicVec2IData[dataName];
            }
            return default(Vector2Int);
        }


        /// <summary>
        /// vec3类型数据存取
        /// </summary>
        /// <param name="dataName"></param>
        /// <param name="data"></param>
        public void SetVec3Data(string dataName, Vector3 data)
        {
            if (_dicVec3Data != null)
            {
                if (_dicVec3Data.ContainsKey(dataName))
                {
                    _dicVec3Data[dataName] = data;
                }
                else
                {
                    _dicVec3Data.Add(dataName, data);
                }
            }
            else
            {
                _dicVec3Data = new Dictionary<string, Vector3>();
                _dicVec3Data.Add(dataName, data);
            }
        }

        public Vector3 GetVec3Data(string dataName)
        {
            if (_dicVec3Data != null)
                return _dicVec3Data[dataName];
            return default(Vector3);
        }

        /// <summary>
        /// GameObject类型数据存取
        /// </summary>
        /// <param name="dataName"></param>
        /// <param name="data"></param>
        public void SetGoData(string dataName, GameObject data)
        {
            if (_dicGoData != null)
            {
                if (_dicGoData.ContainsKey(dataName))
                {
                    _dicGoData[dataName] = data;
                }
                else
                {
                    _dicGoData.Add(dataName, data);
                }
            }
            else
            {
                _dicGoData = new Dictionary<string, GameObject>();
                _dicGoData.Add(dataName, data);
            }
        }

        public GameObject GetGoData(string dataName)
        {
            if (_dicGoData != null)
                return _dicGoData[dataName];
            return default(GameObject);
        }


        /// <summary>
        /// Map类型数据存取
        /// </summary>
        /// <param name="dataName"></param>
        /// <param name="data"></param>
        public void SetMapData(string dataName, MapBase<AIBrickState> data)
        {
            if (_dicBrickData != null)
            {
                if (_dicBrickData.ContainsKey(dataName))
                {
                    _dicBrickData[dataName] = data;
                }
                else
                {
                    _dicBrickData.Add(dataName, data);
                }
            }
            else
            {
                _dicBrickData = new Dictionary<string, MapBase<AIBrickState>>();
                _dicBrickData.Add(dataName, data);
            }
        }

        public MapBase<AIBrickState> GetMapData(string dataName)
        {
            if (_dicBrickData != null)
                return _dicBrickData[dataName];
            return default(MapBase<AIBrickState>);
        }



        /// <summary>
        /// Prop类型数据存取
        /// </summary>
        /// <param name="dataName"></param>
        /// <param name="data"></param>
        public void SetPropData(string dataName, Prop data)
        {
            if (_dicPropData != null)
            {
                if (_dicPropData.ContainsKey(dataName))
                {
                    _dicPropData[dataName] = data;
                }
                else
                {
                    _dicPropData.Add(dataName, data);
                }
            }
            else
            {
                _dicPropData = new Dictionary<string, Prop>();
                _dicPropData.Add(dataName, data);
            }
        }

        public Prop GetPropData(string dataName)
        {
            if (_dicPropData != null)
                return _dicPropData[dataName];
            return default(Prop);
        }


        /// <summary>
        /// object类型数据存取
        /// </summary>
        /// <param name="dataName"></param>
        /// <param name="data"></param>
         public void SetObjectData(string dataName, object data)
        {
            if (_dicObjectData != null)
            {
                if (_dicObjectData.ContainsKey(dataName))
                {
                    _dicObjectData[dataName] = data;
                }
                else
                {
                    _dicObjectData.Add(dataName, data);
                }
            }
            else
            {
                _dicObjectData = new Dictionary<string, object>();
                _dicObjectData.Add(dataName, data);
            }
        }

        public object GetObjectData(string dataName)
        {
            if (_dicObjectData != null)
                return _dicObjectData[dataName];
            return default(object);
        }


        

    }
}
