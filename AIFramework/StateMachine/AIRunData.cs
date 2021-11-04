using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 一个状态机拥有一个运行时数据实例
/// 默认关键字：目标位置 "TargetPos"
/// </summary>
public class AIRunData 
{
    Dictionary<string, float> _dicFloatData;
    Dictionary<string, int> _dicIntData;

    Dictionary<string, Vector3> _dicVec3Data;
    Dictionary<string, Vector2> _dicVec2Data;
    Dictionary<string, Vector2Int> _dicVec2IData;

    Dictionary<string, GameObject> _dicGoData;

    Dictionary<string,MapBase<AIBrickState>> _dicBrickData;

    //List<string> defaultKey ;
    public AIRunData()
    {       
        //defaultKey .Add("TargetPos");
    }
 
    public void SetFloatData(string dataName, float data)
    {
        if(_dicFloatData != null)
        {
            if(_dicFloatData.ContainsKey(dataName))
            {
                _dicFloatData[dataName] = data;
            }
            else
            {
                _dicFloatData.Add(dataName,data);
            }
        }
        else
        {
            _dicFloatData = new Dictionary<string, float>();
            _dicFloatData.Add(dataName,data);
        }
    }

    public float GetFloatData(string dataName)
    {
        if(_dicFloatData != null)
            return _dicFloatData[dataName];
        return default(float);
    }

    public void SetIntData(string dataName, int data)
    {
        if(_dicIntData != null)
        {
            if(_dicIntData.ContainsKey(dataName))
            {
                _dicIntData[dataName] = data;
            }
            else
            {
                _dicIntData.Add(dataName,data);
            }
        }
        else
        {
            _dicIntData = new Dictionary<string, int>();
            _dicIntData.Add(dataName,data);
        }
    }

    public int GetIntData(string dataName)
    {
        if(_dicIntData != null)
            return _dicIntData[dataName];
        return default(int);
    }

    
    public void SetVec2Data(string dataName, Vector2 data)
    {
        if(_dicVec2Data != null)
        {
            if(_dicVec2Data.ContainsKey(dataName))
            {
                _dicVec2Data[dataName] = data;
            }
            else
            {
                _dicVec2Data.Add(dataName,data);
            }
        }
        else
        {
            _dicVec2Data = new Dictionary<string, Vector2>();
            _dicVec2Data.Add(dataName,data);
        }
    }

    public Vector2 GetVec2Data(string dataName)
    {
        if(_dicVec2Data != null)
            return _dicVec2Data[dataName];
        return default(Vector2);
    }


    
    public void SetVec2IData(string dataName, Vector2Int data)
    {
        if(_dicVec2IData != null)
        {
            if(_dicVec2IData.ContainsKey(dataName))
            {
                _dicVec2IData[dataName] = data;
            }
            else
            {
                _dicVec2IData.Add(dataName,data);
            }
        }
        else
        {
            _dicVec2IData = new Dictionary<string, Vector2Int>();
            _dicVec2IData.Add(dataName,data);
        }
    }

    public Vector2Int GetVec2IData(string dataName)
    {
        if(_dicVec2IData != null)
            return _dicVec2IData[dataName];
        return default(Vector2Int);
    }


    
    public void SetVec3Data(string dataName, Vector3 data)
    {
        if(_dicVec3Data != null)
        {
            if(_dicVec3Data.ContainsKey(dataName))
            {
                _dicVec3Data[dataName] = data;
            }
            else
            {
                _dicVec3Data.Add(dataName,data);
            }
        }
        else
        {
            _dicVec3Data = new Dictionary<string, Vector3>();
            _dicVec3Data.Add(dataName,data);
        }
    }

    public Vector3 GetVec3Data(string dataName)
    {
        if(_dicVec3Data != null)
            return _dicVec3Data[dataName];
        return default(Vector3);
    }

      public void SetGoData(string dataName, GameObject data)
    {
        if(_dicGoData != null)
        {
            if(_dicGoData.ContainsKey(dataName))
            {
                _dicGoData[dataName] = data;
            }
            else
            {
                _dicGoData.Add(dataName,data);
            }
        }
        else
        {
            _dicGoData = new Dictionary<string, GameObject>();
            _dicGoData.Add(dataName,data);
        }
    }

    public GameObject GetGoData(string dataName)
    {
        if(_dicGoData != null)
            return _dicGoData[dataName];
        return default(GameObject);
    }



          public void SetBrickData(string dataName, MapBase<AIBrickState> data)
    {
        if(_dicBrickData != null)
        {
            if(_dicBrickData.ContainsKey(dataName))
            {
                _dicBrickData[dataName] = data;
            }
            else
            {
                _dicBrickData.Add(dataName,data);
            }
        }
        else
        {
            _dicBrickData = new Dictionary<string,  MapBase<AIBrickState>>();
            _dicBrickData.Add(dataName,data);
        }
    }

    public  MapBase<AIBrickState> GetBrickData(string dataName)
    {
        if(_dicBrickData != null)
            return _dicBrickData[dataName];
        return default( MapBase<AIBrickState>);
    }
    
}
