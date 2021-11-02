using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;
public enum ECoordinations
{
    MOUSE_WORLD_POS = 1,

}

//消息分类
/*
    由系统消息触发的消息如：
        鼠标在屏幕空间点击转换为世界坐标



*/

/// <summary>
/// 消息基类
/// </summary>
public class BaseMessage
{
    public string messageType;

    public virtual void Invoke(){}

}

/// <summary>
/// 当鼠标点击屏幕，转为世界坐标，然后发送
/// </summary>
public class OnMousePosInWorld :BaseMessage
{
    Vector2Int mousePos;   

    UnityAction<Vector2Int> func;
    public override void Invoke()
    {
        func(mousePos);
    }
}

/// <summary>
/// 消息管理器
/// 静态方法:消息注册，消息发送，消息订阅
/// 消息还是要分类，分为普通无参消息，和特定系统消息
/// 调用并传参数还是由事件发送者，存储到
/// </summary>
public class MessageManager 
{
    delegate void MAction();
    Dictionary<string,BaseMessage> dicMessage;

    private List<BaseMessage> messages;
 

    public void SendMessage(BaseMessage message)
    {
        dicMessage.Add(message.messageType,message);
    }

    /// <summary>
    /// 添加一个监听者到一个事件上
    /// </summary>
    public void AddListener(string messageType)
    {
        if(dicMessage.ContainsKey(messageType))
        {
            
        }
    }


    public void Dispatch(string messageType)
    {
        if(dicMessage.ContainsKey(messageType))
        {

        }
    }
        
    

    

}
