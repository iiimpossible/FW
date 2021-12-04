using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

 
 /// <summary>
 /// 已经废弃
 /// </summary>
public class UIType
{
    public string uiName { get; private set; }
    /// <summary>
    /// UI的路径,通过这个路径加载
    /// </summary>
    public string uiPrefabPath { get; private set; }

    public bool executeLua { get; private set; }

    public string luaCode { get; private set; }

    public string luaName { get; private set; }

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="path"></param>
    public UIType(string prefabPath, bool executeLua = false, string luaPath = "")
    {
        this.uiPrefabPath = prefabPath;
        //从path中获得UI的名字，如[Asset/UI/MainiPanel]
        this.uiName = prefabPath.Substring(prefabPath.LastIndexOf('/') + 1);

        if (executeLua)
        {
            TextAsset luaText = Resources.Load<TextAsset>(luaPath);
            luaCode = luaText.text;
            luaName = luaText.name;                 
        }
        this.executeLua = executeLua;
    }
}

/// <summary>
/// 所有UI的抽象基类
/// 注意该类是与一个UI对象对应的，操控该对象的并保存一些信息的类
/// </summary>
public abstract class UIBasePanel 
{   

    public UnityAction<UIBasePanel> OnCreateUI;

    public UnityAction<UIBasePanel> OnDestroyUI;

    public UnityAction<UIBasePanel> OnExitUI;

    public UnityAction<UIBasePanel> OnResumeUI;

    public UnityAction<UIBasePanel> OnEnterUI;

    public GameObject uiGo {get;set;} 

    /// <summary>
    /// UI行为，在lua脚本中定义UI的行为,脚本的路径或者直接代码需要在子类中指定
    /// </summary>
    public GraphyFW.UIBehavior uIBehavior {get; private set;}

    /// <summary>
    /// UIBasePanel的构造函数，在子类中调用并赋值UIType(UI实例路径)
    public UIBasePanel(){} 

    /// <summary>
    /// 初始化UIBehaviour，开启lua执行逻辑，在子类中调用
    /// @note 如果要使用此方法开启Lua执行逻辑，需要在场景中用一个游戏物体挂载Scpt_XLuaConfig(FW/ScriptFramework/Scpt_XLuaConfig)
    /// </summary>
    public void InitializeUIBehaviour(GraphyFW.UIBehavior behavior)
    {         
        uIBehavior = behavior;     
        uIBehavior.SetValue(this);//将自己作为一个uikey注册到脚本表  
    } 


    /// <summary>
    /// 当UI生成而不是激活时执行一次，总共只执行一次，委托注册在这里
    /// </summary>
    public virtual void OnCreate()
    {

    }

    /// <summary>
    /// 当该UI被销毁的时候调用一次
    /// </summary>
    public virtual void OnDestroy()
    {

    }

    /// <summary>
    /// UI激活时执行的操作，执行一次
    /// </summary>
    public virtual void OnEnter() 
    { 
        uiGo?.SetActive(true);
        
        uIBehavior?.Enter(); 

    }

    /// <summary>
    /// UI处于被锁定（休眠）的状态时执行的操作
    /// 如当当打开设置面板，点击某个按钮弹出一个弹窗，设置面板被锁定，应该执行该方法
    /// </summary>
    public virtual void OnPause() 
    { 
        GetComponentAnyway<CanvasGroup>().blocksRaycasts = false; 
        uIBehavior?.Pause(); 
    }

    /// <summary>
    /// UI重新激活时应该执行的操作
    /// </summary>
    public virtual void OnResume() 
    { 
        GetComponentAnyway<CanvasGroup>().blocksRaycasts = true; 
        uiGo?.SetActive(true);
        uIBehavior?.Resume();
    }

    /// <summary>
    /// UI退出时执行的操作
    /// </summary>
    public virtual void OnExit() 
    { 
        if(OnExitUI != null)
        {
            OnExitUI(this); 
        }        
        uIBehavior?.Exit();
    }

    public virtual void OnUpdate()
    {
        uIBehavior?.Update();
    }


    /// <summary>
    /// 给绑定的UI面板添加组件或者从该UI面板获得组件
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public T GetComponentAnyway<T>() where T: Component
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


    public GameObject FindChild(string childName, bool includeInactive = true)
    {
        foreach (Transform trans in uiGo.GetComponentsInChildren<Transform>(includeInactive))
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
    public GameObject FindChild(GameObject farther, string childName, bool includeInactive = true)
    {
        if(farther == null) return null;
        foreach (Transform trans in farther.GetComponentsInChildren<Transform>(includeInactive))
        {
            if (trans.name == childName)
                return trans.gameObject;
        }
        Debug.LogError($"Can't find child. [{uiGo.name}].[{childName}]");
        return null;
    }

    public List<Transform> FindChildren(Transform farther, bool includeInactive = true)
    {
        if(farther == null) return null;
        //广度优先
        Queue<Transform> transforms_queue = new Queue<Transform>();
        List<Transform> transforms_list = new List<Transform>();
        transforms_queue.Enqueue(farther);
        Transform top;
        while(transforms_queue.Count > 0)
        {
            top  = transforms_queue.Dequeue();
            for(int i = 0; i <top.childCount;i++ )
            {
                transforms_queue.Enqueue(top.GetChild(i));
                transforms_list.Add(top.GetChild(i));
            }
        }
        return transforms_list;
    }


    public T GetChildComponentAnyway<T>(string name) where T:Component
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

     public T GetChildComponent<T>(string name) where T:Component
    {
        GameObject child = FindChild(name);
        if(child != null)
        {
            var comp = child.GetComponent<T>();
            if(comp != null) 
                return comp;               
        }      
        Debug.LogError($"Can't find component. [{uiGo.name}].[{name}]");
        return default(T);       
    }

    public T GetChildComponent<T>(GameObject farther, string name) where T : Component
    {
        GameObject child = FindChild(farther,name);
        if (child != null)
        {
            var comp = child.GetComponent<T>();
            if (comp != null)
                return comp;
        }
        Debug.LogError($"Can't find component. [{uiGo.name}].[{name}]");
        return default(T);
    }
}
