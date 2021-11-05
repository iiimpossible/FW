using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ai系统管理场景中的所有AIContorller组件，并生成地图？
/// </summary>
public class AISystem : MonoBehaviour
{
    public Vector2Int mapSize;

    public Vector3 mapOrigin;

    public Vector2Int gridOffSet;
    public static AISystem instance;

    public GameObject birck;

    public GameObject birckContainer;

    public GameObject aiActor;

    public Dictionary<string, MapBase<AIBrickState>> dicMaps;

    public MapBase<AIBrickState> mainMap;

    /// <summary>
    /// 生成地图
    /// </summary>
    private void Awake()
    {
        instance = this;
        mainMap = new MapBase<AIBrickState>(mapSize);
        mainMap.offset = gridOffSet;
        mainMap.mapZero = Vector3.zero;
        mainMap.GenMap(birck,birckContainer);

        SpawnAIObject();
    }

    private void Start()
    {


    }
    private void Update()
    {

    }

    public void SpawnAIObject()
    {
        //在地图上选择一个点，
        //采样点
        //像素一个采样点，
        Vector2Int pos = mainMap.GetSpawnPos(mainMap.GetMapCenter());

        Vector3 p3 = mainMap.MapSpaceToWorldSpace(pos);
        Debug.Log("Map center in the world------>" + p3);
        //在这个位置实例一个AI对象
        GameObject.Instantiate(aiActor,p3,Quaternion.identity);
    }



}
