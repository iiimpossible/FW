using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
public class UIType
{
    public string name { get; private set; }
    /// <summary>
    /// UI��·��,ͨ�����·������
    /// </summary>
    public string path { get; private set; }


   

    /// <summary>
    /// ���캯��
    /// </summary>
    /// <param name="path"></param>
    public UIType(string path)
    {
        this.path = path;
        //��path�л��UI�����֣���[Asset/UI/MainiPanel]
        this.name = path.Substring(path.LastIndexOf('/') + 1);       
    }
}

/// <summary>
/// ����UI�ĳ������
/// ע���������һ��UI�����Ӧ�ģ��ٿظö���Ĳ�����һЩ��Ϣ����
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
    /// UI�����ߣ������ӦUI��������ã�����������һЩ����
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
    /// UI����ʱִ�еĲ�����ִ��һ��
    /// </summary>
    public virtual void OnEnter() { OnEnterUI(this); }

    /// <summary>
    /// UI���ڱ����������ߣ���״̬ʱִ�еĲ���
    /// �統����������壬���ĳ����ť����һ��������������屻������Ӧ��ִ�и÷���
    /// </summary>
    public virtual void OnPause() { uiTool.GetOrAddComponent<CanvasGroup>().blocksRaycasts = true; }

    /// <summary>
    /// UI���¼���ʱӦ��ִ�еĲ���
    /// </summary>
    public virtual void OnResume() { uiTool.GetOrAddComponent<CanvasGroup>().blocksRaycasts = false; }

    /// <summary>
    /// UI�˳�ʱִ�еĲ���
    /// </summary>
    public virtual void OnExit() { OnExitUI(this); }
}
