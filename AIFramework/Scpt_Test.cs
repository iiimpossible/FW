using System.Collections;
using System.Collections.Generic;
using UnityEngine;


class TData : IComparer<TData>
{
    
    float data ;
    public int Compare(TData a , TData b)
    {
        if(a.data > b.data)
            return 1;
        return -1;
    }
}

public class Scpt_Test : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GraphyFW.Common.BinaryHeap<TData> a = new GraphyFW.Common.BinaryHeap<TData>();
        Debug.Log(a.Count);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
