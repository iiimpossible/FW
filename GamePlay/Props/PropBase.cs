using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 道具基类。
/// 道具包括地图上的物资（沙粒、食物、等资源）
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
