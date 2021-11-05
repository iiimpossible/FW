using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 地图管理器
/// 1.在正确的位置生成AI对象
/// 2.目标物产生逻辑
/// 3.生成地图TODO：真正意义上的随机地图生成
/// 
/// </summary>
public class MapManager : MonoBehaviour
{

    public static MapManager instance;
    void Awake() 
    {
        instance = this;
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
