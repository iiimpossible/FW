using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
///  UI�����ߣ�������ȡĳ��UI�Ӷ��������Ĳ���
/// </summary>
public class UITool 
{
    /// <summary>
    /// ��ǰ�Ļ���
    /// </summary>
    public GameObject uiGo {get; private set;}

    public UITool (GameObject panel)
    {
        uiGo = panel;
    }

    /// <summary>
    /// ���󶨵�UI������������ߴӸ�UI��������
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


    public GameObject FindChild(string name)
    {
        foreach (Transform trans in uiGo.GetComponentsInChildren<Transform>())
        {
            if (trans.name == name)
                return trans.gameObject;
        }
        Debug.LogWarning($"There is Not exit a child of {uiGo.name}. [{name}]");
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
        return null;       
    }

}
