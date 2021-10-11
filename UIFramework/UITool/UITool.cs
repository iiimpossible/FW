using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
///  UI管理工具，包括获取某个UI子对象的组件的操作
/// </summary>
public class UITool 
{



    /// <summary>
    /// 当前的活动面板
    /// </summary>
    public GameObject targetPanel {get; private set;}

    public UITool (GameObject panel)
    {
        targetPanel = panel;
    }

    /// <summary>
    /// 给绑定的UI面板添加组件或者从该UI面板获得组件
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public T GetOrAddComponent<T>() where T: Component
    { 
        if (targetPanel.GetComponent<T>() == null)
        {
            return targetPanel.AddComponent<T>();
        }
        else
        {
            return targetPanel.GetComponent<T>();
        }
    }


    public GameObject FindChildObjectOfActivePanel(string name)
    {
        foreach (Transform trans in targetPanel.GetComponentsInChildren<Transform>())
        {
            if (trans.name == name)
                return trans.gameObject;
        }
        Debug.LogWarning($"There is Not exit a child of {targetPanel.name}. [{name}]");
        return null;
    }


    public T GetOrAddComponentOfChildOfActivePanel<T>(string name) where T:Component
    {
        GameObject child = FindChildObjectOfActivePanel(name);
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
        return null;
       
    }

}
