using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
/// 消息管理器
/// 静态方法:消息注册，消息发送，消息订阅
/// 消息还是要分类，分为普通无参消息，和特定系统消息
/// 调用并传参数还是由事件发送者，存储到
/// </summary>
public class MessageManager 
{
    delegate void MAction();
    Dictionary<string,MAction> dicMessage;

    public void RigistMessage(string message)
    {

    }

    public void SendMessage()
    {

    }

    public void AddListener()
    {

    }


}
