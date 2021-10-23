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
    Dijkstra
}




public class Scpt_GenMap : MonoBehaviour
{
    public ESearchType searchType = ESearchType.DFS;
    public Vector2 gridSize;

    public Vector2Int gridNum = new Vector2Int(20, 20);

    public Vector2 genOriginPos;

    [Range(0, 10)]
    public float delayTime = 0.2f;

    [Range(0, 1)]
    public float blackRate = 0.8f;

    public GameObject brick;

    public Vector2 offset;

    public Vector2Int searchOrigin = new Vector2Int(0, 0);

    public Vector2Int targetPos = new Vector2Int();

    public bool palyOld = false;

    public bool radomTarget = false;

    private List<List<GameObject>> squarObjs = new List<List<GameObject>>();

    private Dictionary<GameObject, AIBrickState> dicBrickStates = new Dictionary<GameObject, AIBrickState>();

    private List<Vector2> path = new List<Vector2>();

    AIStrategy strategy = new AIStrategy();

    private Timer runTotal = new Timer("Total timer");
    private Timer runFourChild = new Timer("Four children");
    private Timer runList = new Timer("List");
    private Timer runGetNear = new Timer("GetNear");


    private AISearchBase aiSearch;
    void Start()
    {
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
                     aiSearch = new AIAStarSearch(gridNum);
                    break;
                }
            default :
            {
                  aiSearch = new AIBFSSearch(gridNum);
                    break;
            }

        }

        //


        if (palyOld)
        {
            strategy.SetTargetPos(targetPos);
            strategy.SetSourcePos(searchOrigin);
            GenMap();
            StartCoroutine(strategy.BFSSearch(targetPos));
        }
        else
        {
            
            aiSearch.SetSourcePos(searchOrigin);
            aiSearch.SetTargetPos(targetPos).SetGridSize(gridSize).blackRate = this.blackRate;
            aiSearch.GenMap(brick, GameObject.Find("Floors"));
            aiSearch.levelDelayTime = delayTime;
            StartCoroutine(aiSearch.Search());
        }

       
    }


    public void SearchBFS()
    {
        StartCoroutine(aiSearch.Search());
    }

    public void ClearBFS()
    {
        aiSearch.Clear();
        StopAllCoroutines();
    }

    //生成随机地图
    public void GenMap()
    {
        GameObject brickContainer = GameObject.Find("Floors");

        if (brickContainer == null || brick == null)
        {
            Debug.LogError("GenMap error, floor is null.");
            return;
        }
        for (int column = 0; column < gridNum.x; column++)
        {
            strategy.matrixObjs.Add(new List<GameObject>());
            for (int row = 0; row < gridNum.y; row++)
            {
                GameObject newGo = Instantiate<GameObject>(brick, new Vector3(gridSize.x * column + offset.x, gridSize.y * row + offset.y, 0), Quaternion.identity);
                if (!newGo)
                {
                    Debug.Log("Invalid prefab.");
                    return;
                }
                newGo.transform.SetParent(brickContainer.transform);

                strategy.matrixObjs[column].Add(newGo);
                strategy.dicBrickStates.Add(newGo, new AIBrickState(new Vector2Int(column, row), newGo));//将游戏物体与其状态关联      
                RandomIsObstacle(newGo, strategy.dicBrickStates);//设置状态
            }
        }
        strategy.InitOriginTargetPos(searchOrigin,targetPos);
    }

    //随机生成障碍物（黑方块）
    private void RandomIsObstacle(GameObject go, Dictionary<GameObject, AIBrickState> dic)
    {
        int ran = Random.Range(0, 100);
        AIBrickState state = dic[go];
        if (ran > 100 * (1 - blackRate))
            state.SetObstacle();
    }
  
}
