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
    public GameObject targetPanel {get; private set;}

    public UITool (GameObject panel)
    {
        targetPanel = panel;
    }

    /// <summary>
    /// ���󶨵�UI������������ߴӸ�UI��������
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
