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

    public UnityAction<UIBasePanel> OnCreateUI;

    public UnityAction<UIBasePanel> OnDestroyUI;

    public UnityAction<UIBasePanel> OnExitUI;

    public UnityAction<UIBasePanel> OnResumeUI;

    public UnityAction<UIBasePanel> OnEnterUI;

    /// <summary>
    /// �Ƿ�ͨ��luaִ��UI�߼�
    /// </summary>
    protected bool executeLuaBehaviour;

    /// <summary>
    /// UI�����ߣ������ӦUI��������ã�����������һЩ����
    /// </summary>
    public UITool uiTool { get; private set; }

     /// <summary>
    /// UI���ͣ�����UIԤ�����·���Լ���UIԤ��������֣���Ҫ��������ָ��
    /// </summary>
    public UIType uiType { get; private set; }

    /// <summary>
    /// UI��Ϊ����lua�ű��ж���UI����Ϊ,�ű���·������ֱ�Ӵ�����Ҫ��������ָ��
    /// </summary>
    public GraphyFW.UIBehavior uIBehavior {get; private set;}

    /// <summary>
    /// UIBasePanel�Ĺ��캯�����������е��ò���ֵUIType(UIʵ��·��)
    public UIBasePanel(UIType type)
    {
        uiType = type;
        //uIBehavior = new GraphyFW.UIBehavior();
        //uiTool = new UITool();
    }

    /// <summary>
    /// ��ʼ��UITool����һ��UIʵ����UIPanel��������PanelManager��ߵ���
    /// </summary>
    public void InitializeUITool(UITool tool)
    {
        uiTool = tool;
    }

    /// <summary>
    /// ��ʼ��UIBehaviour������luaִ���߼����������е���
    /// </summary>
    protected void InitializeUIBehaviour(bool executeLua, GraphyFW.UIBehavior behavior)
    {
        executeLuaBehaviour = executeLua;
        uIBehavior = behavior;
    }

    /// <summary>
    /// UI����ʱִ�еĲ�����ִ��һ��
    /// </summary>
    public virtual void OnEnter() 
    { 
        uiTool.targetPanel.SetActive(true);
        //OnEnterUI(this); 
        uIBehavior?.Enter(); 
    }

    /// <summary>
    /// UI���ڱ����������ߣ���״̬ʱִ�еĲ���
    /// �統����������壬���ĳ����ť����һ��������������屻������Ӧ��ִ�и÷���
    /// </summary>
    public virtual void OnPause() { uiTool.GetOrAddComponent<CanvasGroup>().blocksRaycasts = true; uIBehavior?.Pause(); }

    /// <summary>
    /// UI���¼���ʱӦ��ִ�еĲ���
    /// </summary>
    public virtual void OnResume() { uiTool.GetOrAddComponent<CanvasGroup>().blocksRaycasts = false; uIBehavior?.Resume();}

    /// <summary>
    /// UI�˳�ʱִ�еĲ���
    /// </summary>
    public virtual void OnExit() { OnExitUI(this); uIBehavior?.Exit();}

    public virtual void Update()
    {
        uIBehavior?.Update();
    }
}
