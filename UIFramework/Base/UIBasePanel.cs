using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
public class UIType
{
    public string name { get; private set; }
    /// <summary>
    /// UI的路径,通过这个路径加载
    /// </summary>
    public string path { get; private set; }   

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="path"></param>
    public UIType(string path)
    {
        this.path = path;
        //从path中获得UI的名字，如[Asset/UI/MainiPanel]
        this.name = path.Substring(path.LastIndexOf('/') + 1);       
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

    /// <summary>
    /// 是否通过lua执行UI逻辑
    /// </summary>
    protected bool executeLuaBehaviour;

    /// <summary>
    /// UI管理工具，保存对应UI对象的引用，并对他进行一些操作
    /// </summary>
    public UITool uiTool { get; private set; }

     /// <summary>
    /// UI类型，包含UI预制体的路径以及该UI预制体的名字，需要在子类中指定
    /// </summary>
    public UIType uiType { get; private set; }

    /// <summary>
    /// UI行为，在lua脚本中定义UI的行为,脚本的路径或者直接代码需要在子类中指定
    /// </summary>
    public GraphyFW.UIBehavior uIBehavior {get; private set;}

    /// <summary>
    /// UIBasePanel的构造函数，在子类中调用并赋值UIType(UI实例路径)
    public UIBasePanel(UIType type)
    {
        uiType = type;
        //uIBehavior = new GraphyFW.UIBehavior();
        //uiTool = new UITool();
    }

    /// <summary>
    /// 初始化UITool，将一个UI实例与UIPanel关联，在PanelManager里边调用
    /// </summary>
    public void InitializeUITool(UITool tool)
    {
        uiTool = tool;
    }

    /// <summary>
    /// 初始化UIBehaviour，开启lua执行逻辑，在子类中调用
    /// </summary>
    protected void InitializeUIBehaviour(bool executeLua, GraphyFW.UIBehavior behavior)
    {
        executeLuaBehaviour = executeLua;
        uIBehavior = behavior;
    }

    /// <summary>
    /// UI激活时执行的操作，执行一次
    /// </summary>
    public virtual void OnEnter() 
    { 
        uiTool.targetPanel.SetActive(true);
        //OnEnterUI(this); 
        uIBehavior?.Enter(); 
    }

    /// <summary>
    /// UI处于被锁定（休眠）的状态时执行的操作
    /// 如当当打开设置面板，点击某个按钮弹出一个弹窗，设置面板被锁定，应该执行该方法
    /// </summary>
    public virtual void OnPause() { uiTool.GetOrAddComponent<CanvasGroup>().blocksRaycasts = true; uIBehavior?.Pause(); }

    /// <summary>
    /// UI重新激活时应该执行的操作
    /// </summary>
    public virtual void OnResume() { uiTool.GetOrAddComponent<CanvasGroup>().blocksRaycasts = false; uIBehavior?.Resume();}

    /// <summary>
    /// UI退出时执行的操作
    /// </summary>
    public virtual void OnExit() { OnExitUI(this); uIBehavior?.Exit();}

    public virtual void Update()
    {
        uIBehavior?.Update();
    }
}
