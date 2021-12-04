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
/// 2021.12.04
/// TODO：需要提供回调函数重复加入检测
/// </summary>
public  class Message
{
    public string messageType;
    public List<object> paramsList = new List<object>();   
}
 

public enum EMessageType
{
    //左键按下，发送鼠标世界位置
    OnMouseDown_MousePosInWorld_0 = 1,

    //右键按下，发送鼠标世界位置
    OnMouseDown_MousePosInWorld_1 = 2,
    OnMapLoaded = 3,
    //发射射线 检测到一个 GameObjectBase
    OnRayCastGameObjectBase = 4,

    //
    OnBoxCastAllCollider = 5,

    //尝试框选区域，发送起始点和终点 但是现在这个命令和 射线命令耦合
    OnFrameSelected = 6,

    //框选中的Actor
    OnBoxCast_Actors  = 7,

//-------------------------------------------------------
    //鼠标左键按下
    OnMouseButtonDown_0 = 20,

    //鼠标右键按下
    OnMouseButtonDown_1 = 21,

    //鼠标中键按下
    OnMouseButtonDown_2 = 22,

    //鼠标左键按下，发送鼠标屏幕位置
    OnMouseButtonDown_MousePos_0 = 23,

    //鼠标右键按下，发送鼠标屏幕位置
    OnMouseButtonDown_MousePos_1 = 24,

    OnMouseButtonUp_0 = 25,

    OnMouseButtonUp_1 = 26,

    OnMouseButtonUp_2 = 27,    
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
        if (dicMessage.ContainsKey(type))
        {
            var list = dicMessage[type];
            if (list.Contains(action))
                list.Remove(action);
        }
        else{
            Debug.LogWarning($"MessageManager: Listener not exist. type: {type}, actor: {action}");
        }
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
        {
             dicMessage.Add(type, new List<UnityAction<Message>>());
             return;
        }
        else
        {
            if(dicMessage[type].Count == 0)
            {
                 //Debug.LogWarning("Dispatch faild, not exist listener. " + messageStr);
                return;
            }
        }
           
        foreach (var item in dicMessage[type])
        {
            if (datas.Length > 0)//当消息参数列表不为空，生成消息
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
#if UNITY_EDITOR
                Debug.Log("Diapatch message: " + messageStr + " ListenderCount: " + dicMessage[type].Count + " Log: " + log);
#endif
                item?.Invoke(message);
            }
            else//当消息参数列表为空
            {
#if UNITY_EDITOR
                Debug.Log("Diapatch message: " + messageStr + " ListenderCount: " + dicMessage[type].Count + " Log: " + log);
#endif
                item?.Invoke(null);
            }
        }
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
