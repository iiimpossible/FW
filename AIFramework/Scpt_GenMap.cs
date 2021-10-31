using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GraphyFW.AI;

using GraphyFW.Common;
/*
    д���C#�ļ���ʱ�򣬿�ʼ�ǳ��ң��±����״̬�඼û�У�ֱ�����߼���߸��ָ����ݣ����´�������
    ͨ�����ַ�����װ��
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

    //private List<List<GameObject>> squarObjs = new List<List<GameObject>>();

    private Dictionary<GameObject, AIBrickState> dicBrickStates = new Dictionary<GameObject, AIBrickState>();

    AIStrategy strategy = new AIStrategy();

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
        aiSearch.SetSourcePos(searchOrigin);
        aiSearch.SetTargetPos(targetPos).SetGridSize(gridSize).blackRate = this.blackRate;
        aiSearch.GenMap(brick, GameObject.Find("Floors"));
        aiSearch.levelDelayTime = delayTime;
        StartCoroutine(aiSearch.Search());
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

    //��������ϰ���ڷ��飩
    private void RandomIsObstacle(GameObject go, Dictionary<GameObject, AIBrickState> dic)
    {
        int ran = Random.Range(0, 100);
        AIBrickState state = dic[go];
        if (ran > 100 * (1 - blackRate))
            state.SetObstacle();
    }
  
}
