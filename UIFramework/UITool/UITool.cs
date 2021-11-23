using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
///  UI管理工具，包括获取某个UI子对象的组件的操作
/// 已经废弃
/// </summary>
public class UITool 
{
    /// <summary>
    /// 当前的活动面板
    /// </summary>
    public GameObject uiGo {get; private set;}

    public UITool (GameObject panel)
    {
        uiGo = panel;
    }

    /// <summary>
    /// 给绑定的UI面板添加组件或者从该UI面板获得组件
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public T GetComponent<T>() where T: Component
    { 
        if (uiGo.GetComponent<T>() == null)
        {
            return uiGo.AddComponent<T>();
        }
        else
        {
            return uiGo.GetComponent<T>();
        }
    }


    public GameObject FindChild(string childName)
    {
        foreach (Transform trans in uiGo.GetComponentsInChildren<Transform>())
        {
            if (trans.name == childName)
                return trans.gameObject;
        }
        Debug.LogError($"Can't find child. [{uiGo.name}].[{childName}]");
        return null;
      
    }

    /// <summary>
    /// 在先找到父物体的情况下，再寻找子物体
    /// </summary>
    /// <param name="farther"></param>
    /// <param name="child"></param>
    /// <returns></returns>
    public GameObject FindChild(GameObject farther, string childName)
    {
        foreach (Transform trans in farther.GetComponentsInChildren<Transform>())
        {
            if (trans.name == childName)
                return trans.gameObject;
        }
        Debug.LogError($"Can't find child. [{uiGo.name}].[{childName}]");
        return null;
    }


    public T GetChildComponent<T>(string name) where T:Component
    {
        GameObject child = FindChild(name);
        if(child)
        {
            if (child.GetComponent<T>() == null)
            {
                return child.AddComponent<T>();
            }
            else
            {
                return child.GetComponent<T>();
            }
        }      
        Debug.LogError($"Can't find component. [{uiGo.name}].[{name}]");
        return null;       
    }

}
