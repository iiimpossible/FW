using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;
 
//消息分类
/*
    由系统消息触发的消息如：
        鼠标在屏幕空间点击转换为世界坐标
*/

/// <summary>
/// 消息基类
/// </summary>
public  class Message
{
    public string messageType;
    public List<object> paramsList = new List<object>();   
}
 

public enum EMessageType
{
    //右键按下，发送鼠标世界位置
    OnMouseDown_MousePosInWorld_1 = 1,

    //左键按下，发送鼠标世界位置
    OnMouseDown_MousePOsInWorld_0 = 2,
    OnMapLoaded = 2,
    //发射射线 检测到一个 GameObjectBase
    OnRayCastGameObjectBase = 3,

    //
    OnBoxCastAllCollider = 4,

    //尝试框选区域，发送起始点和终点 但是现在这个命令和 射线命令耦合
    OnFrameSelected = 5,

    //鼠标左键按下
    OnMouseButtonDown_0 = 6,

    //鼠标右键按下
    OnMouseButtonDown_1 = 7,

    //鼠标中键按下
    OnMouseButttonDown_2 = 8,

    //鼠标左键按下，发送鼠标屏幕位置
    OnMouseButtonDown_MousePos_0 = 9,

    //鼠标右键按下，发送鼠标屏幕位置
    OnMouseButtonDown_MousePos_1 = 10,


    
}

/// <summary>
/// 消息管理器
/// 静态方法:消息注册，消息发送，消息订阅
/// 消息还是要分类，分为普通无参消息，和特定系统消息
/// 调用并传参数还是由事件发送者，存储到
/// TODO: 消息是否传递到？消息是否解析成功？
/// </summary>
public class MessageManager :MonoBehaviour 
{

    public static MessageManager instance{get;private set;}

    /// <summary>
    /// 存储对应事件的监听者
    /// </summary>
    private Dictionary<EMessageType,List<UnityAction<Message>>> dicMessage = new Dictionary<EMessageType, List<UnityAction<Message>>>();


    private void Awake()
    {
        instance = this;

    }

    private void Start()
    {
        GraphyFW.UI.ScptSceneManger.instance.SetDontDestroyObjet(gameObject);
    }

    // private void Update()
    // {

    // }


    /// <summary>
    /// 添加一个监听者到一个事件上
    /// 传入一个事件类型列表
    /// /// TODO: 监听者可以同时监听多个事件
    /// </summary>
    public void AddListener(EMessageType type, UnityAction<Message> action)
    {
        if (!dicMessage.ContainsKey(type))
            dicMessage.Add(type, new List<UnityAction<Message>>());
        //检测是否已经添加过这个监听者
        if (dicMessage[type].Contains(action)) return;
        dicMessage[type].Add(action);
        //Debug.Log("Add listner: "+type );
    }


    /// <summary>
    /// 从字典中对应消息类型的委托列表中去除委托
    /// </summary>
    /// <param name="type"></param>
    /// <param name="action"></param>
    public void RemoveListener(EMessageType type, UnityAction<Message> action)
    {
        var list = dicMessage[type];
        list.Remove(action);
    }

    /// <summary>
    /// 当事件出发后分发事件
    /// </summary>
    /// <param name="type"></param>
    /// <param name="message"></param>
    public void Dispatch(string messageStr, EMessageType type, params object[] datas)
    {
#if UNITY_EDITOR
        string log = "";
#endif
        if (!dicMessage.ContainsKey(type))
            dicMessage.Add(type, new List<UnityAction<Message>>());
        foreach (var item in dicMessage[type])
        {
            if (datas.Length > 0)
            {
                Message message = new Message();
                message.messageType = messageStr;
#if UNITY_EDITOR
                log += item.GetType() + "\n";
#endif
                foreach (var data in datas)
                {
                    message.paramsList.Add(data);
                }
                item?.Invoke(message);
            }
            else
            {
                item?.Invoke(null);
            }
        }
#if UNITY_EDITOR
        if (dicMessage[type].Count == 0)
            Debug.Log("Dispatch faild, not exist listener. " + messageStr);
        Debug.Log("Diapatch message: " + messageStr + " ListenderCount: " + dicMessage[type].Count + " Log: " + log);
#endif
    }

    /// <summary>
    /// 无参消息分发
    /// </summary>
    /// <param name="type"></param>
    public void Dispatch(EMessageType type)
    {
        if (!dicMessage.ContainsKey(type))
            dicMessage.Add(type, new List<UnityAction<Message>>());
        foreach (var item in dicMessage[type])
        {
            item?.Invoke(null);
        }
    }



    //private void DelayDispatch()

}
