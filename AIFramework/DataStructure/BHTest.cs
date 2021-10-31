using System.Collections;
using System.Collections.Generic;
using UnityEngine;



    /*
        复杂度分析
            对于一个n个元素的二叉堆
            1.插入操作
                最坏情况从堆底部上浮到堆顶部，需要进行log2(n)此上浮操作
            2.删除操作
                最坏情况从堆顶部下沉到堆底部，需要进行log2(n)次下沉操作  
    */
public class BHTest
{
    private List<int> heap;


    public BHTest()
    {
        heap = new List<int>();
    }

    /// <summary>
    /// 元素入堆
    /// </summary>
    /// <param name="data"></param>
    public void Push(int data)
    {
        //加入到数组尾部
        heap.Add(data);
        this.ComeUp(heap.Count);
    }

    /// <summary>
    /// 二叉堆顶的元素出堆。思路：将堆顶与堆尾部交换，尾部出堆，顶部下沉
    /// </summary>
    /// <returns>返回堆顶的值</returns>
    public int Pop()
    {
        if (heap.Count == 1)
        {
            int res = heap[heap.Count - 1];
            heap.RemoveAt(heap.Count - 1);
            return res;
        }
        //首尾交换
        if (TopToButtom())
        {
            int res = heap[heap.Count - 1];
            heap.RemoveAt(heap.Count - 1);
            Sink(1);
            return res;
        }
        Debug.Log("BinaryHeap count less than 2.");
        return default(int);
    }

    //假设输入索引以1开头
    //应该是索引数组的时候才减一，其他都是以1开头计算
    //虽然索引值时以1开头，但是实际访问数组的值是以0开头，必须封装
    private void ComeUp(int n)
    {        
        for (int i = n; i > 0; i /= 2)//死循环预警：可能不会小于0
        {
             Debug.Log($"(Cur index {i},data {Index(i)}) (Farth index {i/2},data {Index(i/2)})");
            if (Index(i) < Index(i / 2))//当i/2非法时，Index返回int.minValue 当n节点小于它的父节点，那么就交换
            {               
                if (!Swap(i, i / 2)) { Debug.Log("[BinaryHeap] ComeUp over."); return; };//当索引非法，不继续进行操作
            }
        }

    }

    //假设输入索引以1开头
    //@description:与子节点相比较，如果大于字节点那么就下沉
    private void Sink(int n)
    {       
        int si = Son(n);
        Debug.Log($"The fist son is--------->{si}");
        for (int i = n; i < heap.Count && i > 0; i = si)
        {
            if (Index(i) > Index(si,false))//如果son(i)非法，index返回index.minValue，不管如何，过不了swap判断
            {
                if (!Swap(i, si)) { Debug.Log("[BinaryHeap] Sink over."); return; };
            }
            Debug.Log($"Son index is----->{si}");
            si = Son(si);            
        }
    }


    /// <summary>
    /// 根据以1开头的索引n,返回该索引对应的最大子节点索引，无子节点返回-1
    /// </summary>
    /// <param name="n"></param>
    /// <returns>返回儿子的（堆）索引</returns>
    private int Son(int n)
    {
        if (Valid(n * 2 - 1) && Valid(n * 2))//Valid判断以数组索引（0开头）为依据
        {
            return Index(n * 2) > Index(n * 2 + 1) ? n * 2 : n * 2 + 1;
        }
        else if (Valid(n * 2 - 1))
            return n * 2;
        else if (Valid(n * 2))
            return n * 2 + 1;
        return -1;//索引非法  
    }

    /// <summary>
    /// 交换数组中的两个值.
    /// 注意：在此方法中输入是堆索引，操作的是数组索引！
    /// </summary>
    /// <param name="a">以1开头的当前节点索引</param>
    /// <param name="b">以1开头的父节点或者子节点索引</param>
    /// <returns></returns>
    private bool Swap(int a, int b)
    {
        int sa,sb;
        sa = a - 1;
        sb = b - 1;
        if (!Valid(sa) || !Valid(sb))
        {
            Debug.Log($"Invalid index,index {b},data {Index(b)}");
            return false;
        }
        Debug.Log($"(index {a},data {heap[sa]}) swap to (index {b},data {heap[sb]})");
        int t = heap[sa];
        heap[sa] = heap[sb];
        heap[sb] = t;        
        return true;
    }


    /// <summary>
    /// 判断索引合法性
    /// </summary>
    /// <param name="index"></param>
    /// <returns> 索引是否合法</returns>
    private bool Valid(int index)
    {
        if (index >= 0 && index < heap.Count)
            return true;
        return false;
    }

 
    /// <summary>
    /// 带堆索引和数组索引转换且进行索引合法判断
    /// </summary>
    /// <param name="n"></param>
    /// <returns>数组的对应索引的值</returns>
    private int Index(int n,bool isMin = true)
    {
        return Valid(n - 1) ? heap[n - 1] : (isMin? int.MinValue:int.MaxValue);
    }

    /// <summary>
    /// 将堆顶和堆底的元素交换
    /// </summary>
    /// <returns>是否交换成功</returns>
    private bool TopToButtom()
    {
        if (heap.Count > 1)
            return Swap(1,heap.Count);
        return false;
    }

    /// <summary>
    /// 输出堆中的值
    /// </summary>
    public void Watch()
    {
        string log = "";
        for(int i = 0;i< heap.Count;i++)
        {
            log += $"Total Index{i + 1}, Data----------->{heap[i]}\n";
        }
        Debug.Log(log);
    }

    /// <summary>
    /// 测试
    /// </summary>
    public BHTest Test()
    {        
        //插入数据
        
        for(int i = 0;i< 10;i++)
        {
            int r = Random.Range(0,10);
            Debug.Log("Current Push ------>" + r);            
            this.Push(r);
             this.Watch();            
        }
        this.Push(-100);

        this.Watch();

        //删除数据
        for(int i = 0;i < 10;i++)
        {
            Debug.Log("Current pop ------>" +this.Pop());
            this.Watch();
        }

        return this;
    }

}
