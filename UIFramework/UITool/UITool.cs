using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
///  UI�����ߣ�������ȡĳ��UI�Ӷ��������Ĳ���
/// �Ѿ�����
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
    /// �����ҵ������������£���Ѱ��������
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
