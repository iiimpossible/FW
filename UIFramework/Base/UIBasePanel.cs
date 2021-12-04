using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

 
 /// <summary>
 /// �Ѿ�����
 /// </summary>
public class UIType
{
    public string uiName { get; private set; }
    /// <summary>
    /// UI��·��,ͨ�����·������
    /// </summary>
    public string uiPrefabPath { get; private set; }

    public bool executeLua { get; private set; }

    public string luaCode { get; private set; }

    public string luaName { get; private set; }

    /// <summary>
    /// ���캯��
    /// </summary>
    /// <param name="path"></param>
    public UIType(string prefabPath, bool executeLua = false, string luaPath = "")
    {
        this.uiPrefabPath = prefabPath;
        //��path�л��UI�����֣���[Asset/UI/MainiPanel]
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
/// ����UI�ĳ������
/// ע���������һ��UI�����Ӧ�ģ��ٿظö���Ĳ�����һЩ��Ϣ����
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
    /// UI��Ϊ����lua�ű��ж���UI����Ϊ,�ű���·������ֱ�Ӵ�����Ҫ��������ָ��
    /// </summary>
    public GraphyFW.UIBehavior uIBehavior {get; private set;}

    /// <summary>
    /// UIBasePanel�Ĺ��캯�����������е��ò���ֵUIType(UIʵ��·��)
    public UIBasePanel(){} 

    /// <summary>
    /// ��ʼ��UIBehaviour������luaִ���߼����������е���
    /// @note ���Ҫʹ�ô˷�������Luaִ���߼�����Ҫ�ڳ�������һ����Ϸ�������Scpt_XLuaConfig(FW/ScriptFramework/Scpt_XLuaConfig)
    /// </summary>
    public void InitializeUIBehaviour(GraphyFW.UIBehavior behavior)
    {         
        uIBehavior = behavior;     
        uIBehavior.SetValue(this);//���Լ���Ϊһ��uikeyע�ᵽ�ű���  
    } 


    /// <summary>
    /// ��UI���ɶ����Ǽ���ʱִ��һ�Σ��ܹ�ִֻ��һ�Σ�ί��ע��������
    /// </summary>
    public virtual void OnCreate()
    {

    }

    /// <summary>
    /// ����UI�����ٵ�ʱ�����һ��
    /// </summary>
    public virtual void OnDestroy()
    {

    }

    /// <summary>
    /// UI����ʱִ�еĲ�����ִ��һ��
    /// </summary>
    public virtual void OnEnter() 
    { 
        uiGo?.SetActive(true);
        
        uIBehavior?.Enter(); 

    }

    /// <summary>
    /// UI���ڱ����������ߣ���״̬ʱִ�еĲ���
    /// �統����������壬���ĳ����ť����һ��������������屻������Ӧ��ִ�и÷���
    /// </summary>
    public virtual void OnPause() 
    { 
        GetComponentAnyway<CanvasGroup>().blocksRaycasts = false; 
        uIBehavior?.Pause(); 
    }

    /// <summary>
    /// UI���¼���ʱӦ��ִ�еĲ���
    /// </summary>
    public virtual void OnResume() 
    { 
        GetComponentAnyway<CanvasGroup>().blocksRaycasts = true; 
        uiGo?.SetActive(true);
        uIBehavior?.Resume();
    }

    /// <summary>
    /// UI�˳�ʱִ�еĲ���
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
    /// ���󶨵�UI������������ߴӸ�UI��������
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
    /// �����ҵ������������£���Ѱ��������
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
        //�������
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
