using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GraphyFW.GamePlay;

/// <summary>
/// RunTimeMapManager
/// ai系统管理场景中的所有AIContorller组件，并生成地图？
/// 2021.11.23
/// 以后改为地图管理器
/// 
/// 2021.11.24
/// 这个地图管理器，存储所有的运行时地图信息
/// 1.所有的Actor
/// 2.所有的道具
/// 3.地图信息（地块、地形、树木等）
/// 4.额外信息（存储区等）
/// 
/// 2021.12.04
/// 这个管理器，只管理所有的游戏物体的生成，销毁，分类存储游戏物体引用以供检索。未来游戏中控制台生成游戏物体，就是调用这个管理器的方法
/// 1.多个Actor的控制，应该有一个独立的Actor管理器。
/// 2.地图管理应该有一个独立的地图管理器
/// </summary>
public class GameMode : MonoBehaviour
{
 
    public Vector2Int gridOffSet;
    public static GameMode instance{get;private set;}

    public GameObject birck;

    public GameObject birckContainer;

    public GameObject aiActor;

    private GameObject food;
 

    public MapBase<AIBrickState> mainMap;

    private List<ScptStorageArea> _storageAreas = new List<ScptStorageArea>();

    /// <summary>
    /// 当前激活的所有actor，以供搜索算法查询
    /// </summary>
    private List<GameObject> listActiveActors = new List<GameObject>();
    
    private List<GameObject> _listSelectedObjects = new List<GameObject>();
    /// <summary>
    /// 被选中的所有游戏物体
    /// </summary>
    /// <typeparam name="GameObject"></typeparam>
    /// <returns></returns>
    public List<GameObject> selctedGameObjectList {get{return _listSelectedObjects;}}

    /// <summary>
    /// 生成的所有的食物
    /// </summary>
    /// <typeparam name="Food"></typeparam>
    /// <returns></returns>
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

        mainMap.blackRate = MapManager.instance.blackRate;
        mainMap.offset = gridOffSet;
        mainMap.mapZero = Vector3.zero;
        mainMap.GenMap(birck, birckContainer);


        LoadPrefab();
        SpawnAIObject();
        SpanwFoodObject();
        //Spawn(typeof(AntNest),new Vector2Int(1,1),"Building");
       
        //StartCoroutine(mainMap.NoiseElimination());

        MessageManager.instance.AddListener(EMessageType.OnFrameSelected, SpawnStorageArea);
        MessageManager.instance.AddListener(EMessageType.OnMouseDown_MousePosInWorld_0,SpawnFood);
        MessageManager.instance.AddListener(EMessageType.OnMouseButtonDown_1,OnCancelCommand_Listener);
        MessageManager.instance.AddListener(EMessageType.OnBoxCastAllCollider, this.OnBox2DRayCast_Listener);
    }
    private void Update()
    {

    }


    private void OnDestroy()
    {
        MessageManager.instance?.RemoveListener(EMessageType.OnFrameSelected, SpawnStorageArea);
        MessageManager.instance?.RemoveListener(EMessageType.OnMouseDown_MousePosInWorld_0, SpawnFood);
        MessageManager.instance?.RemoveListener(EMessageType.OnMouseButtonDown_1, OnCancelCommand_Listener);
        MessageManager.instance?.RemoveListener(EMessageType.OnBoxCastAllCollider, this.OnBox2DRayCast_Listener);
    }


    #region 地图管理方法
    public MapBase<AIBrickState> GetMap(int index = 0)
    {
        return mainMap;
    }
#endregion




#region  游戏物体实例方法

    public void SpawnAIObject()
    {
        //在地图上选择一个点，
        //采样点
        //像素一个采样点，
        Vector2Int pos = mainMap.GetSpawnPos(mainMap.GetMapCenter());
        Vector3 p3 = mainMap.MapSpaceToWorldSpace(pos);
        //Debug.LogError("Birth pos is；" + p3) ;  
        //在这个位置实例一个AI对象
        GameObject a = GameObject.Instantiate(aiActor, p3, Quaternion.identity);
        a.tag = "Ant";
        listActiveActors.Add(a);
    }

    public void SpanwEnemy()
    {

    }

    public void SpanwFoodObject()
    {
        Vector2Int pos = mainMap.GetSpawnPos(mainMap.GetMapCorner(0));
        Vector3 p3 = mainMap.MapSpaceToWorldSpace(pos);

        GameObject f = Resources.Load<GameObject>(Food.prefabPath);

        GameObject go = GameObject.Instantiate(f, p3, Quaternion.identity);
        go.tag = "Food";
        listFoods.Add(new Food(go));
        // Debug.Log("activeactor num-->" + activeActors.Count);
    }

    public void SpawnFood(Message message)
    {
        if(this.playerCommand != EPlayCommands.CREATE_FOOD) return;
        Vector2Int pos = (Vector2Int)message.paramsList[0];
        Vector3 worldPos = mainMap.MapSpaceToWorldSpace(pos);

        GameObject prefab = Resources.Load<GameObject>(Food.prefabPath);

        GameObject go = GameObject.Instantiate(prefab, worldPos, Quaternion.identity);
        go.tag = "Food";
        listFoods.Add(new Food(go));
        Debug.LogWarning($"Food object num is: {listFoods.Count}");
    }

    /// <summary>
    /// 在指定位置生成指定类型的游戏物体
    /// </summary>
    /// <param name="pos"></param>
    public void Spawn(System.Type type, Vector2Int pos, string folder = "Actor")
    {
        Vector3 worldPos = mainMap.MapSpaceToWorldSpace(pos);
        Actor goBase = type.Assembly.CreateInstance("GrapyFW.GamePlay." + type.Name) as Actor;
        GameObject prefab = Resources.Load<GameObject>("Prefabs/" + folder + "/" + type.Name);
        GameObject go = GameObject.Instantiate(prefab, worldPos, Quaternion.identity);
        goBase.SetGO(go, pos);
    }

    /// <summary>
    /// 从容器中查询离输入位置最近的一个Food
    /// </summary>
    public Food GetFoodObject()
    {
        foreach (var f in listFoods)
        {
            if (!f.isStored && !f.isOccupied)
            {              
                 return f;
            }
               
        }
        Debug.LogWarning($"Food object num is: {listFoods.Count}");
        return default(Food);
    }


    public void LoadPrefab()
    {
        food = Resources.Load<GameObject>("Prefabs/Food");

    }








    /// <summary>
    /// 生成存储区
    /// 1.将浮点Size转为intsize
    /// 
    /// TODO: 在鼠标的位置上生成一个图片或者文字指示
    /// </summary>
    /// <param name="start"></param>
    /// <param name="end"></param>
    private void SpawnStorageArea(Message message)
    {        
        if( this.playerCommand != EPlayCommands.CREATE_STORAGE_AREA) return;
        Vector3 worldStart = (Vector3)message.paramsList[0];
        Vector3 worldEnd = (Vector3)message.paramsList[1];
        Vector3 center = (Vector3)message.paramsList[2];
        Vector2 size2D = (Vector2)message.paramsList[3];

        GameObject prefab = Resources.Load<GameObject>("Prefabs/Building/StorageArea");
        GameObject go = GameObject.Instantiate<GameObject>(prefab,center,Quaternion.identity);
        ScptStorageArea comp = go.GetComponent<ScptStorageArea>();         

        var c = mainMap.WorldSpaceToMapSpace(center);
         var d = mainMap.WorldSpaceToMapSpace(size2D);
        
        // Vector2Int i2DSize = new Vector2Int(Mathf.RoundToInt(d.x),Mathf.RoundToInt(d.y));//四舍六入五取偶
        // Vector2Int i2DCenter = new Vector2Int(Mathf.RoundToInt(c.x),Mathf.RoundToInt(c.y));
        // Debug.Log($"SpawnStorageArea: size: {i2DSize}, center: {i2DCenter}");

        comp.InitArea(c,d);

        _storageAreas.Add(comp);

        //在这个地图管理器上记录存储区

    }


#endregion


#region 获取游戏对象

    


    /// <summary>
    /// 获取一个不为满的存储区
    /// </summary>
    /// <returns></returns>
    public Vector2Int GetStorageArea(out bool isGot)
    {
        foreach (var item in _storageAreas)
        {
            if(!item.Full())
            {
                isGot = true;
                return item.GetEmptyPos();
            }
        }
        isGot = false;
        return Vector2Int.zero;
    }

 


 

    
    /// <summary>
    /// 接收物体选中观察者
    /// </summary>
    /// <param name="message"></param>
    private void OnBox2DRayCast_Listener(Message message)
    {
        _listSelectedObjects.Clear();
        RaycastHit2D[] hit2Ds;
        if (message.paramsList.Count > 0)
        {
            hit2Ds = message.paramsList[0] as RaycastHit2D[];
            foreach (var item in hit2Ds)
            {
               //item.collider.GetComponent<GraphyFW.AI.ActorController>().SetSelected(false);
               
               if(item.collider.tag == "Ant")
               {
                   _listSelectedObjects.Add(item.collider.gameObject);

               }
               //1.是Actor
               //2.是Prop
               //3.是
            }

            if(_listSelectedObjects.Count > 0)
            {
                //
                MessageManager.instance.Dispatch(EMessageType.OnBoxCast_Actors.ToString(),EMessageType.OnBoxCast_Actors);
            }

        }

    }

#endregion

    #region  游戏状态控制，如命令状态（是否是生成存储区，是否选择游戏物体，是否创建挖掘区域）     
    [System.Serializable]
    public enum EPlayCommands
    {
        SLECTE_OBJECT = 1,          //选择当前场景中的物体
        NORMAL = SLECTE_OBJECT,     //Selcte_object 即为基础状态

        CANCEL = 10,
        CREATE_STORAGE_AREA = 2,    //创建存储区   
        CREATE_DIG_AEREA = 3,       //创建挖掘区域
        CREATE_BUILDING_AREA = 4,   //创建建造区域
        CREATE_PLANT_AREA = 5,      //创建种植区域
        CREATE_GRAZE_AREA = 6,      //创建放牧区域（蚜虫，介壳虫，蜜蝉）
        CREATE_HUNT_AREA = 7,       //创建打猎区域

        //简单测试用
        CREATE_FOOD = 8,            //创建食物

        CRATE_OBSTACLE = 9,         //创建障碍物

    }

    [SerializeField]
    private EPlayCommands playerCommand = EPlayCommands.SLECTE_OBJECT;

    public void SetPlayerCommand(EPlayCommands commands)
    {
        this.playerCommand = commands;
    }

    public EPlayCommands GetPlayerCommad()
    {
        return this.playerCommand;
    }

    /// <summary>
    /// 当玩家命令面板按下按钮，改变当前的命令
    /// </summary>
    /// <param name="message"></param>
    private void OnPlayCommandChange_Listener(Message message)
    {
        playerCommand = (EPlayCommands)message.paramsList[0];
    }

    /// <summary>
    /// 按下右键取消当前的命令，回到Normal（Select Object）
    /// </summary>
    /// <param name="message"></param>
    private void OnCancelCommand_Listener(Message message)
    {
        playerCommand = EPlayCommands.SLECTE_OBJECT;
    }

    #endregion

    

}
