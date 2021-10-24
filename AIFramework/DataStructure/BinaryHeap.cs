using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GraphyFW.Common
{




    public class BinaryHeap<T> where T :IComparer<T>
    {
        
        //注意以数组存储二叉堆，为了方便计算p点的子节点，即左子节点= p*2,右子节点 = p*2+1,所以以1为首索引或者所有索引访问-1
        private List<T> heap;


        public int Count {get{return this.heap.Count;}}

        public int Size {get;private set;}
        //分配内存后，此时heap的Count是多大？ 为0
        public BinaryHeap()
        {
            this.heap = new List<T>(16);
        }

        //增删改查
        public void Insert(int i,T node)
        {
            if( i >= heap.Count)
            {
                heap.Capacity = heap.Count * 2;
                heap[i] = node;
            }
            else 
            {
                heap[Size+1] = node;
            }
            Size++;
            //this.heap.Add(new Node<T>());
        }

        public void Remove()
        {

        }


        public void Clear()
        {

        }
        
        /*
            上浮操作：
                将待插入节点与根节点比较，如果小于跟根节点，那么与根节点交换位置（小根堆）
        */
        private void ComeUp(int n)
        {
            
            for(int i = n; i > 1 ; i/=2)
            {
                int b = heap[i].Compare(heap[i],heap[i / 2]);//将节点i 与其父节点的值相比较，对于小顶堆，大于那么就下沉
                if( b > 0)
                    Swap(heap[i], heap[i/2]);             
                else if(b < 0){//留给大顶堆的

                }
               
            }
        }


        //当输入一个值，假设在尾部插入，如果该节点的值
        private void Sink()
        {
            //  for (int i = n, t = son(i); t <= size && heap[t] > heap[i]; i = t, t = son(i))
            //  {

            //  }
            // swap(heap[i], heap[t]);
        }

        //获取n节点需要交换的子节点
        private int Son(int n)
        {
            if( n * 2 + 1 > this.heap.Count ) return n * 2;
            //n的右边节点和左边节点比较
            int b = heap[n].Compare(heap[n * 2 + 1] , heap[n * 2]);
            return n * 2 + Mathf.Clamp(b,0,1) ;
        }

        private void Swap(T a, T b)
        {
            T t = a;
            a = b;
            b = t;
        }

    }


}

