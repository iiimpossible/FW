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
     
    public UIType uiType { get; private set; }

    public UnityAction<UIBasePanel> OnCreateUI;

    public UnityAction<UIBasePanel> OnDestroyUI;

    public UnityAction<UIBasePanel> OnExitUI;

    public UnityAction<UIBasePanel> OnResumeUI;

    public UnityAction<UIBasePanel> OnEnterUI;
    /// <summary>
    /// UI管理工具，保存对应UI对象的引用，并对他进行一些操作
    /// </summary>
    public UITool uiTool { get; private set; }

    public UIBasePanel(UIType type)
    {
        uiType = type;

        //uiTool = new UITool();
    }

    public void InitializeUITool(UITool tool)
    {
        uiTool = tool;
    }

    /// <summary>
    /// UI激活时执行的操作，执行一次
    /// </summary>
    public virtual void OnEnter() { OnEnterUI(this); }

    /// <summary>
    /// UI处于被锁定（休眠）的状态时执行的操作
    /// 如当当打开设置面板，点击某个按钮弹出一个弹窗，设置面板被锁定，应该执行该方法
    /// </summary>
    public virtual void OnPause() { uiTool.GetOrAddComponent<CanvasGroup>().blocksRaycasts = true; }

    /// <summary>
    /// UI重新激活时应该执行的操作
    /// </summary>
    public virtual void OnResume() { uiTool.GetOrAddComponent<CanvasGroup>().blocksRaycasts = false; }

    /// <summary>
    /// UI退出时执行的操作
    /// </summary>
    public virtual void OnExit() { OnExitUI(this); }
}
