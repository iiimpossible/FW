using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GraphyFW.AI;

using GraphyFW.Common;
/*
    写这个C#文件的时候，开始非常乱，下边这个状态类都没有，直接在逻辑里边各种改数据，导致代码杂乱
    通过各种方法封装，
*/

[System.Serializable]
public enum ESearchType
{
    DFS,
    BFS,
    AStar,
    Dijkstra,
    AStarSort
}




public class Scpt_GenMap : MonoBehaviour
{

    public static Scpt_GenMap instance;

    public ESearchType searchType = ESearchType.DFS;
 

    public Vector2Int gridNum = new Vector2Int(20, 20);
 
    [Range(0, 10)]
    public float delayTime = 0.2f;

    [Range(0, 1)]
    public float blackRate = 0.8f;

    public GameObject brick;
 
    [Range(0.1f, 2)]
    public float toFartherNodeFactor =1.0f;
    [Range(0.1f, 2)]
    public float toTargetNodeFactor = 1.0f;

    public Vector2Int searchOrigin = new Vector2Int(0, 0);

    public Vector2Int targetPos = new Vector2Int();

    public bool palyOld = false;

    public bool radomTarget = false;

    //private List<List<GameObject>> squarObjs = new List<List<GameObject>>();

    private Dictionary<GameObject, AIBrickState> dicBrickStates = new Dictionary<GameObject, AIBrickState>();

    AIStrategy strategy = new AIStrategy();

    private MapBase<AIBrickState> map;
    private AISearchBase aiSearch;

    private bool isSetSourcePos = false;
    private bool isSetTargetPos = false;

    private void Awake() {
        instance = this;
    }


    void Start()
    {
        map = new MapBase<AIBrickState>(gridNum);
        map.blackRate = blackRate;
        map.GenMap(brick, GameObject.Find("Floors"));
    

        if(radomTarget)
        {
            targetPos = new Vector2Int(Random.Range(0, gridNum.x - 1), Random.Range(0, gridNum.y - 1));
        }
        switch (searchType)
        {
            case ESearchType.DFS:
                {
                    aiSearch = new AIDFSSerch(gridNum);
                    break;
                }

            case ESearchType.BFS:
                {
                    aiSearch = new AIBFSSearch(gridNum);
                    break;
                }
            case ESearchType.Dijkstra:
                {
                    aiSearch = new AIDijkstraSearch(gridNum);
                    break;
                }
            case ESearchType.AStar:
                {
                    var astar = new AIAStarSearch(gridNum);
                    astar.SetFactor(toFartherNodeFactor,toTargetNodeFactor);
                    aiSearch = astar;
                    break;
                }
            case ESearchType.AStarSort:
                {
                    aiSearch = new AIAstarSort(gridNum);
                    break;
                }
            default:
                {
                    aiSearch = new AIBFSSearch(gridNum);
                    break;
            }
        }
        aiSearch.map = map;
        aiSearch.SetSourcePos(searchOrigin);         
        aiSearch.SetTargetPos(targetPos);       
        aiSearch.levelDelayTime = delayTime;
        //StartCoroutine(aiSearch.Search());

           MessageManager.instance.AddListener(EMessageType.OnMouseDown_MousePosInWorld_0,SetSource);
            MessageManager.instance.AddListener(EMessageType.OnMouseDown_MousePosInWorld_0,SetTarget);
    }


    public void SearchPath()
    {   ClearPath();
        StartCoroutine(aiSearch.Search());
    }

    public void ClearPath()
    {
        aiSearch.Clear();
        StopAllCoroutines();
        aiSearch.SetSourcePos(searchOrigin);
        aiSearch.SetTargetPos(targetPos);
    }

    /// <summary>
    /// 重置地图
    /// </summary>
    public void ResetMap()
    {
        ClearPath();
        map.RandomMap();
        aiSearch.SetSourcePos(searchOrigin);
        aiSearch.SetTargetPos(targetPos);
    }

    public void SetSource(Message message)
    {
        if(isSetSourcePos)
        {
             Vector2Int vec = (Vector2Int)message.paramsList[0];
             Debug.Log(vec);
            aiSearch.SetSourcePos(vec);
            searchOrigin = vec;
            isSetSourcePos  = false;
        }
    }

    public void SetTarget(Message message)
    {
          if(isSetTargetPos)
        {
             Vector2Int vec = (Vector2Int)message.paramsList[0];
              Debug.Log(vec);
            aiSearch.SetTargetPos(vec); 
            targetPos = vec;
            isSetTargetPos  = false;
        }
    }

    public void SetIsSetSourcePos(bool b)
    {
        isSetSourcePos = b;
    }

    public void SetIsSetTargetPos(bool b)
    {
        isSetTargetPos = b;
    }
  
}
