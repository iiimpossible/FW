using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GraphyFW.GamePlay;

/// <summary>
/// ai系统管理场景中的所有AIContorller组件，并生成地图？
/// </summary>
public class AISystem : MonoBehaviour
{

    [Range(0,1)]
    public float blackRate = 0;

    public Vector2Int mapSize;

    public Vector3 mapOrigin;

    public Vector2Int gridOffSet;
    public static AISystem instance{get;private set;}

    public GameObject birck;

    public GameObject birckContainer;

    public GameObject aiActor;

    private GameObject food;

    public Dictionary<string, MapBase<AIBrickState>> dicMaps;

    public MapBase<AIBrickState> mainMap;

    /// <summary>
    /// 当前激活的所有actor，以供搜索算法查询
    /// </summary>
    private List<GameObject> listActiveActors;

    private List<Food> listFoods = new List<Food>();

    /// <summary>
    /// 生成地图
    /// </summary>
    private void Awake()
    {
        listActiveActors = new List<GameObject>();

        instance = this;
        Debug.Log("In AI system: mapsize---->" + mapSize);
        mainMap = new MapBase<AIBrickState>(mapSize);
        mainMap.blackRate = this.blackRate;
        mainMap.offset = gridOffSet;
        mainMap.mapZero = Vector3.zero;
        mainMap.GenMap(birck,birckContainer);

        LoadPrefab();
        SpawnAIObject();
        SpanwFoodObject();
    }

    private void Start()
    {
        ScptInputManager.instance.onMouseInWorldPos += this.SpawnFood;
        //StartCoroutine(mainMap.NoiseElimination());

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
        //Debug.LogError("Birth pos is；" + p3) ;  
        //在这个位置实例一个AI对象
        GameObject a = GameObject.Instantiate(aiActor,p3,Quaternion.identity);
        a.tag =  "Ant";
        listActiveActors .Add(a);
    }

    public void SpanwFoodObject()
    {
        Vector2Int pos = mainMap.GetSpawnPos(mainMap.GetMapCorner(0));
        Vector3 p3 = mainMap.MapSpaceToWorldSpace(pos);   

        GameObject f = Resources.Load<GameObject>(Food.prefabPath);    
         
        GameObject go = GameObject.Instantiate(f,p3,Quaternion.identity);
        go.tag = "Food";
        listFoods.Add(new Food(go));
       // Debug.Log("activeactor num-->" + activeActors.Count);
    }

    public void SpawnFood(Vector2Int pos)
    {      
        Vector3 p3 = mainMap.MapSpaceToWorldSpace(pos);   

        GameObject f = Resources.Load<GameObject>(Food.prefabPath);    
         
        GameObject go = GameObject.Instantiate(f,p3,Quaternion.identity);
        go.tag = "Food";
        listFoods.Add(new Food(go));
    }

    /// <summary>
    /// 从容器中查询离输入位置最近的一个Food
    /// </summary>
    public Food GetFoodObject()
    {
        foreach(var f in listFoods)
        {
            if(f.isStored == false)
                return f;
        }
        return default(Food);
    }


    public void LoadPrefab()
    {
        food = Resources.Load<GameObject>("Prefabs/Food");
         
    }

     private void OnDestroy() {
         ScptInputManager.instance.onMouseInWorldPos -= this.SpawnFood;
     }


}
