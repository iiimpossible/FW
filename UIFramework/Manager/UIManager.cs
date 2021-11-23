using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 存储UI信息并可以创建和销毁UI
/// </summary>
public class UIDisplayTool
{


    private Dictionary<UIType, GameObject> dicUIObjects;

    public UIDisplayTool()
    {
        dicUIObjects = new Dictionary<UIType, GameObject>();
    }


    /// <summary>
    /// 1.寻找Canvas对象
    /// 2.检测是否已经存在该类型的UI对象
    /// 3.如果不存在该类型的UI对象，从该类型的路径中生成一个,并给这个UI对象设置父对象
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

        //注意：使用Resources.Load一定要在Resources文件夹下
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
    /// 销毁一个对应类型的UI对象
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
