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

    private List<List<GameObject>> floorSquar = new List<List<GameObject>>();
    // Start is called before the first frame update
    void Start()
    {
        GenMap();
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
        for (int column = 1; column <= gridNum.x; column++)
        {
            floorSquar.Add(new List<GameObject>());
            for (int row = 1; row <= gridNum.y; row++)
            {
                GameObject newGo = Instantiate<GameObject>(RandomFloor(), new Vector3(gridSize.x * column + offset.x,gridSize.y * row + offset.y,0), Quaternion.identity);
                if(floors!= null)newGo.transform.SetParent(floors.transform);
                floorSquar[column].Add(newGo);
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

   
}
