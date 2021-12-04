using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 道具基类，
/// 1.是否被占用
/// 2.是否在存储区，哪个存储区
/// 3.对应的Game Object
/// 4.在地图中的位置
/// 
/// </summary>
public class Prop //: MonoBehaviour
{
    public bool isOccupied{get;set;}
    public bool isStored{get;set;}
    public Vector2Int mapPos{get;set;}
    public GameObject propGo{get;private set;}
    public Prop(GameObject go)
    {
        this.propGo = go;
        isOccupied = false;
        isStored = false;
    }
}
