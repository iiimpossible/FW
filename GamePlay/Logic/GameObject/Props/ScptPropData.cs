using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 道具基类。
/// 道具包括地图上的物资（沙粒、食物、等资源）
/// 1.包含一个游戏物体
/// 2.获取该游戏物体上的Prop数据组件。
/// </summary>
public class PropBase  
{
    //　静态常量：是指编译器在编译时候会对常量进行解析，并将常量的值替换成初始化的那个值。
    public readonly string  path ;

    public PropBase(string path)
    { 
        this.path = path;
    }

}


public class ScptPropData : MonoBehaviour
{ 

    /// <summary>
    /// 当前属于哪一个存储区
    /// </summary>
    [SerializeField]
    [Range(0,100)]
    private int storageAreaId;

    /// <summary>
    /// 当前在存储区的位置
    /// </summary>
    [SerializeField]    
    private Vector2Int StorageAreaPos;

    /// <summary>
    /// 是否被拿起
    /// </summary>
    [SerializeField]
    private bool isTakedUp ;


    /// <summary>
    /// 是否被存储
    /// </summary>
    [SerializeField]
    private bool isStoraged;

}
