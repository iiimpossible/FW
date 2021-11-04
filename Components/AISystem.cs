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


    public Dictionary<string, MapBase<AIBrickState>> dicMaps;

    public MapBase<AIBrickState> mainMap;

    private void Awake()
    {
        instance = this;
        mainMap = new MapBase<AIBrickState>(mapSize);
        mainMap.offset = gridOffSet;
        mainMap.mapZero = Vector3.zero;
        mainMap.GenMap(birck,birckContainer);
    }

    private void Start()
    {


    }
    private void Update()
    {

    }
}
