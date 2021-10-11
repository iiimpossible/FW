using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 面板管理器，使用一个栈来管理所有的面板
/// </summary>
public class UIPanelManager 
{
    /// <summary>
    /// 存储已经激活UI面板
    /// </summary>
    private Stack<UIBasePanel> stackPanels;

    /// <summary>
    /// UI管理器
    /// </summary>
    private UIManager uiManager;

    

    private UIBasePanel lastPanel;

    public UIPanelManager()
    {
        stackPanels = new Stack<UIBasePanel>();

        uiManager = new UIManager();

        
    }

    /// <summary>
    /// UI面板入栈操作，此操作会显示一个UI
    /// 根据输入的UI类型，创建并显示一个UI
    /// </summary>
    /// <param name="nextPanel">要显示的面板 </param>
    public void PushPanel(UIBasePanel nextPanel)
    {
        if(stackPanels.Count>0)
        {
            lastPanel = stackPanels.Peek();//获取栈顶的面板
            lastPanel.OnPause();//让之前栈顶的面板阻塞
        }
        stackPanels.Push(nextPanel);
        

        //因为面板实际上不是一个能够显示的UI，它必须到UI管理器中使用路径找到UI的Prefab，然后克隆一个UI，注册到UI管理器，并被插入到Canvas中
        GameObject go_panel = uiManager.GetSingleUI(nextPanel.uiType);
        nextPanel.InitializeUITool(new UITool(go_panel));
        nextPanel.OnEnter();
        //nextPanel .uiTool.activePanel = cur_panel;

        //事件注册
        nextPanel.OnExitUI += this.PopPanelCallBack;
        nextPanel.OnCreateUI += this.PushPanel;
    }

    /// <summary>
    /// 
    /// </summary>
    public void PopPanel(UIBasePanel nextPanel)
    {
        //注意：当栈顶弹出后，不确定栈中是否还有对象，所有要检测两次
        if (stackPanels.Count > 0)
        {
            stackPanels.Peek().OnExit();
            stackPanels.Pop();
        }
        if (stackPanels.Count > 0)
        {
            stackPanels.Peek().OnResume();
        }
    }

    /// <summary>
    /// 执行位于栈顶的UIPanel的UPdate方法，避免在UI上挂载脚本以执行UPdate
    /// </summary>
    public void UpdateTopPanel()
    {
        if(stackPanels.Count > 0)
        {
            stackPanels.Peek().Update();
        }
    }

    public void PopPanelCallBack(UIBasePanel panel)
    {
        PopPanel(panel);
    }

    public void PushPanelCallBack(UIBasePanel panel)
    {
        PushPanel(panel);
    }


}
