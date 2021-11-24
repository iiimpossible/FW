using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 地图管理器
/// 1.在正确的位置生成AI对象
/// 2.目标物产生逻辑
/// 3.生成地图TODO：真正意义上的随机地图生成
/// 4.临时存储一些 地图的选项数据
/// 
/// </summary>
public class MapManager : MonoBehaviour
{

    public float blackRate {get;set;}

    public Vector2Int mapSize {get;set;}

    public string mapSeed{get;set;}

    public static MapManager instance;
    void Awake() 
    {
        instance = this;
         
    }
    void Start()
    {
        blackRate = 0.4f;
        mapSize = new Vector2Int(30,30);
          GraphyFW.UI.ScptSceneManger.instance.SetDontDestroyObjet(gameObject);     
    }

    /// <summary>
    /// 生成地图
    /// 1.生成障碍物 填充聚集点 消除孤立点
    /// 2.生成道路
    /// 3.生成植物
    /// 4.生成动物
    /// 5.生成巢穴、种群
    /// 
    /// </summary>
    private void GenMap()
    {

    }  
}
