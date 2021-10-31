using System.Collections;
using System.Collections.Generic;
using UnityEngine;


class TData : GraphyFW.Common.IBinaryHeapData<TData,int>
{
    
    int data ;
    public TData(int i)
    {
        this.data = i;
    }

    public int GetData()
    {
        return this.data;
    }

    public void SetData(int i)
    {
        this.data= i;
    }
    public int Compare(TData a , TData b)
    {
        if(a.data > b.data)
            return 1;
        return -1;
    }
}

public class Scpt_Test : MonoBehaviour
{
    BHTest bh = new BHTest();
     GraphyFW.Common.BinaryHeap<TData,int> a = new GraphyFW.Common.BinaryHeap<TData,int>();    
     
    void Start()
    {
       
       Debug.Log("Begin Test.");
        bh.Test();
        // for(int  i = 10 ;i > 0;i--)
        // {
        //     a.Insert( new TData (Random.Range(0,50)));
        // }

        // Debug.Log(a.Count);
        // a.Watch();
        



    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void RemoveCallback()
    {
        a.Pop();
    }
}
