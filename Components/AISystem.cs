using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GraphyFW.GamePlay;

/// <summary>
/// ai系统管理场景中的所有AIContorller组件，并生成地图？
/// 2021.11.23
/// 以后改为地图管理器
/// 2021.11.24
/// 这个地图管理器，存储所有的运行时地图信息
/// 1.所有的Actor
/// 2.所有的道具
/// 3.地图信息（地块、地形、树木等）
/// 4.额外信息（存储区等）
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

    private List<MapStorageArea> _storageAreas = new List<MapStorageArea>();

    /// <summary>
    /// 当前激活的所有actor，以供搜索算法查询
    /// </summary>
    private List<GameObject> listActiveActors = new List<GameObject>();

    private List<Food> listFoods = new List<Food>();

    /// <summary>
    /// 生成地图
    /// </summary>
    private void Awake()
    {   
        instance = this;   
    }

    private void Start()
    {
        mainMap = new MapBase<AIBrickState>(MapManager.instance.mapSize);
        mainMap.onMapGenerated += this.MapGeneratedCallback;
        mainMap.blackRate = MapManager.instance.blackRate;
        mainMap.offset = gridOffSet;
        mainMap.mapZero = Vector3.zero;
        mainMap.GenMap(birck, birckContainer);
        

        LoadPrefab();
        SpawnAIObject();
        SpanwFoodObject();
        //Spawn(typeof(AntNest),new Vector2Int(1,1),"Building");
        ScptInputManager.instance.onMouseInWorldPos += this.SpawnFood;
        //StartCoroutine(mainMap.NoiseElimination());

        MessageManager.instance.AddListener(EMessageType.OnFrameSelected,SpawnStorageArea);
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
        Vector3 worldPos = mainMap.MapSpaceToWorldSpace(pos);   

        GameObject prefab = Resources.Load<GameObject>(Food.prefabPath);    
         
        GameObject go = GameObject.Instantiate(prefab,worldPos,Quaternion.identity);
        go.tag = "Food";
        listFoods.Add(new Food(go));
    }

    /// <summary>
    /// 在指定位置生成指定类型的游戏物体
    /// </summary>
    /// <param name="pos"></param>
    public void Spawn(System.Type type,Vector2Int pos, string folder = "Actor")
    {
        Vector3 worldPos = mainMap.MapSpaceToWorldSpace(pos); 
        GameObjectBase goBase = type.Assembly.CreateInstance("GrapyFW.GamePlay." + type.Name) as GameObjectBase;
        GameObject prefab = Resources.Load<GameObject>("Prefabs/"+folder+ "/" + type.Name);
        GameObject go = GameObject.Instantiate(prefab, worldPos, Quaternion.identity);
        goBase.SetGO(go,pos);
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


     private void MapGeneratedCallback()
     {
         MessageManager.instance.Dispatch("OnMapGenerated",EMessageType.OnMapLoaded);
     }


    /// <summary>
    /// 生成存储区
    /// 1.将浮点Size转为intsize
    /// 
    /// </summary>
    /// <param name="start"></param>
    /// <param name="end"></param>
    private void SpawnStorageArea(Message message)
    {        
        Vector3 worldStart = (Vector3)message.paramsList[0];
        Vector3 worldEnd = (Vector3)message.paramsList[1];
        Vector3 center = (Vector3)message.paramsList[2];
        Vector2 size2D = (Vector2)message.paramsList[3];
        // GameObject prefab = Resources.Load<GameObject>("Prefabs/Building/StorageArea");
        // GameObject go = GameObject.Instantiate<GameObject>(prefab,center,Quaternion.identity);
        // ScptStorageArea comp = go.GetComponent<ScptStorageArea>();
        // comp.SetMesh(worldStart,worldEnd);
        // comp.drawArea = true;

        var c = mainMap.WorldSpaceToMapSpace(center);
         var d = mainMap.WorldSpaceToMapSpace(size2D);
        
        Vector2Int i2DSize = new Vector2Int(Mathf.RoundToInt(d.x),Mathf.RoundToInt(d.y));//四舍六入五取偶
        Vector2Int i2DCenter = new Vector2Int(Mathf.RoundToInt(c.x),Mathf.RoundToInt(c.y));
        Debug.Log($"SpawnStorageArea: size: {i2DSize}, center: {i2DCenter}");

        _storageAreas.Add(new MapStorageArea(i2DCenter,i2DSize ));

        //在这个地图管理器上记录存储区

    }

    public MapStorageArea GetStorageArea()
    {
        foreach (var item in _storageAreas)
        {
            if(!item.Full())
            {
                return item;
            }
        }
        Debug.Log("Can't get StorageArea");
        return null;
    }

}
