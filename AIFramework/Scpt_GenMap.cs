using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scpt_GenMap : MonoBehaviour
{
    public Vector2 gridSize;

    public Vector2Int gridNum = new Vector2Int(20,20);

    public Vector2 genOriginPos;

    public GameObject floor;

    public GameObject redFloor;

    public GameObject blueFloor;

    public GameObject blackFloor;

    public GameObject greenFloor;

    public GameObject whiteFloor;

    public Vector2 offset;

    public Vector2Int searchOrigin = new Vector2Int(0,0);

    private List<List<GameObject>> floorSquar = new List<List<GameObject>>();

    private Dictionary<GameObject,Vector2Int> dicObjPos = new Dictionary<GameObject, Vector2Int>();

    private Dictionary<GameObject, bool> dicObjAccssed = new Dictionary<GameObject, bool>();
    // Start is called before the first frame update
    void Start()
    {
        GenMap();

        StartCoroutine(BFS(gridNum.x,gridNum.y));
        //StartCoroutine(GenMap());
    }

    // Update is called once per frame
    void Update()
    {
        
    }



    public void GenMap()
    {
        GameObject floors = GameObject.Find("Floors");

        if(floor == null)
        {
            Debug.LogError("GenMap error, floor is null.");
            return;
        }
        for (int column = 0; column <= gridNum.x; column++)
        {
            floorSquar.Add(new List<GameObject>());
            for (int row = 0; row <= gridNum.y; row++)
            {
                GameObject newGo = Instantiate<GameObject>(RandomFloor(), new Vector3(gridSize.x * column + offset.x,gridSize.y * row + offset.y,0), Quaternion.identity);
                if(floors!= null)newGo.transform.SetParent(floors.transform);
                floorSquar[column].Add(newGo);
                dicObjPos.Add(newGo, new Vector2Int(column,row));
            }

        }
    }

    private GameObject RandomFloor()
    {
        int ran = Random.Range(0, 100);
        if(ran <80)
        {
            return whiteFloor;
        }
        else
        {
            return blackFloor;
        }        
    }

    //public bool PosValid(Vector3 pos)
    //{

    //}

    /// <summary>
    /// 每里0.5秒调用一次这个广度优先遍历，
    /// 1.初始GO入队，NUm= 1,出队，num = 0;遍历
    /// </summary>
    private IEnumerator BFS(int colum,int row)
    {
        Queue<GameObject> que_objs = new Queue<GameObject>();

        que_objs.Enqueue(floorSquar[searchOrigin.x][searchOrigin.y]);//搜索起点入队

        

        int objNumOfLevel = 1;
        while(que_objs.Count > 0)
        {           
            //遍历当前队中的游戏物体的四个方向
            
            //
            while (objNumOfLevel>0)
            {
                GameObject cur = que_objs.Dequeue();

                Vector2Int cur_pos = dicObjPos[cur];
                //上
                GameObject up = GetObjInSquar(cur_pos.x, cur_pos.y + 1);
                if (up)
                {
                    Debug.Log("up");
                    que_objs.Enqueue(up);
                    ProgressObjColor(up);
                }
                //下
                GameObject down = GetObjInSquar(cur_pos.x, cur_pos.y - 1);
                if (down)
                {
                    Debug.Log("down");
                    que_objs.Enqueue(down);
                    ProgressObjColor(down);
                }
                //左
                GameObject left = GetObjInSquar(cur_pos.x - 1, cur_pos.y);
                if (left)
                {
                    Debug.Log("left");
                    que_objs.Enqueue(left);
                    ProgressObjColor(left);
                }
                //右
                GameObject right = GetObjInSquar(cur_pos.x + 1, cur_pos.y);
                if (right)
                {
                    Debug.Log("right");
                    que_objs.Enqueue(right);
                    ProgressObjColor(right); 
                }
                // Debug.Log("yield");               
                yield return 0;
                objNumOfLevel--;
            }
            objNumOfLevel = que_objs.Count;
        }
    }

    private GameObject GetObjInSquar(int x,int y)
    {
        if(x >= 0 && y >= 0)
        {
            if (x < gridNum.x && y < gridNum.y)
            {                
                GameObject ogo = floorSquar[x][y];
                if (dicObjAccssed.ContainsKey(ogo) )
                {
                    return null;
                }
                else
                {
                    dicObjAccssed.Add(ogo, true);
                    return ogo;
                }
                
            }                
        }
        return null;        
    }

    private void ProgressObjColor(GameObject obj)
    {
        if(obj.name == "white(Clone)")
        {
            Debug.Log("white change color to yellow");
            obj.GetComponent<SpriteRenderer>().color = Color.yellow;

        }
        if(obj.name == "black")
        {
            return;
        }
    }
   
}
