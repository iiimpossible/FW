using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// ����������ʹ��һ��ջ���������е����
/// </summary>
public class UIPanelManager 
{
    /// <summary>
    /// �洢�Ѿ�����UI���
    /// </summary>
    private Stack<UIBasePanel> stackPanels;

    /// <summary>
    /// UI������
    /// </summary>
    private UIManager uiManager;

    

    private UIBasePanel lastPanel;

    public UIPanelManager()
    {
        stackPanels = new Stack<UIBasePanel>();

        uiManager = new UIManager();

        
    }

    /// <summary>
    /// UI�����ջ�������˲�������ʾһ��UI
    /// ���������UI���ͣ���������ʾһ��UI
    /// </summary>
    /// <param name="nextPanel">Ҫ��ʾ����� </param>
    public void PushPanel(UIBasePanel nextPanel)
    {
        if(stackPanels.Count>0)
        {
            lastPanel = stackPanels.Peek();//��ȡջ�������
            lastPanel.OnPause();//��֮ǰջ�����������
        }
        stackPanels.Push(nextPanel);
        

        //��Ϊ���ʵ���ϲ���һ���ܹ���ʾ��UI�������뵽UI��������ʹ��·���ҵ�UI��Prefab��Ȼ���¡һ��UI��ע�ᵽUI���������������뵽Canvas��
        GameObject go_panel = uiManager.GetSingleUI(nextPanel.uiType);
        nextPanel.InitializeUITool(new UITool(go_panel));
        nextPanel.OnEnter();
        //nextPanel .uiTool.activePanel = cur_panel;

        //�¼�ע��
        nextPanel.OnExitUI += this.PopPanelCallBack;
        nextPanel.OnCreateUI += this.PushPanel;
    }

    /// <summary>
    /// 
    /// </summary>
    public void PopPanel(UIBasePanel nextPanel)
    {
        //ע�⣺��ջ�������󣬲�ȷ��ջ���Ƿ��ж�������Ҫ�������
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
    /// ִ��λ��ջ����UIPanel��UPdate������������UI�Ϲ��ؽű���ִ��UPdate
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
