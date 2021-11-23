using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// �洢UI��Ϣ�����Դ���������UI
/// </summary>
public class UIDisplayTool
{


    private Dictionary<UIType, GameObject> dicUIObjects;

    public UIDisplayTool()
    {
        dicUIObjects = new Dictionary<UIType, GameObject>();
    }


    /// <summary>
    /// 1.Ѱ��Canvas����
    /// 2.����Ƿ��Ѿ����ڸ����͵�UI����
    /// 3.��������ڸ����͵�UI���󣬴Ӹ����͵�·��������һ��,�������UI�������ø�����
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public GameObject OpenUI(UIType type)
    {
        GameObject canvas = GameObject.Find("Canvas");
        if (!canvas)
        {
            Debug.LogError("Canvas is Not Exist in current scene, please check that Canvas is created.");
            return null;
        }

        if (dicUIObjects.ContainsKey(type))
        {
            return dicUIObjects[type];
        }

        //ע�⣺ʹ��Resources.Loadһ��Ҫ��Resources�ļ�����
        GameObject prefab = Resources.Load<GameObject>(type.uiPrefabPath);
        if(!prefab)
        {
            Debug.LogError("UI prefab path is not valid. "+ type.uiPrefabPath);
            return null;
        }

        GameObject ui = GameObject.Instantiate(prefab, canvas.transform);
        ui.name = type.uiName;
        dicUIObjects.Add(type, ui);
        Debug.Log("Load ui--->" + ui.name);
        return ui;
    }

    /// <summary>
    /// ����һ����Ӧ���͵�UI����
    /// </summary>
    /// <param name="type"></param>
    public void DestroyUI(UIType type)
    {
        if(dicUIObjects.ContainsKey(type))
        {
            GameObject.Destroy(dicUIObjects[type]);
            dicUIObjects.Remove(type);
        }
    }
}
