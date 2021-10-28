using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BHTest
{
    private List<int> heap;

    public void Push()
    {

    }

    //假设
    public void Pop(int data)
    {
        //加入到数组尾部
        heap.Add(data);
        this.ComeUp(heap.Count);

    }

    //假设输入索引以1开头
    //应该是索引数组的时候才减一，其他都是以1开头计算
    private void ComeUp(int n)
    {
        

        for (int i = index; i >= 0; i /= 2)
        {
            if(Index(i) < Index(i/2));
            {
                Swap(i,i/2);
            }
        }

    }

    //假设输入索引以1开头
    private void Sink(int n)
    {
        
        
        for(int i = index; i < heap.Count;i )
    }


    //根据以1开头的索引n,返回该索引对应的最大子节点索引，无子节点返回-1
    private void Son(int n)
    {

    }

    private void Swap(int a,int b)
    {        
        if(!Valid(a) || !Valid(b) )
            return;
        int t = heap[a];
        heap[a] = heap[b];
        heap[b] =  t;
    }

    private bool Valid(int index)
    {
        if(index >=0 && index < heap.Count)
            return true;
        return false;   
    }

    private int Index(int n)
    {
        return Valid(n-1)?heap[n-1]:0;
    }


}
