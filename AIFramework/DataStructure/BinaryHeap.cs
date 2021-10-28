using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GraphyFW.Common
{


    public interface IBinaryHeapData<T, M> where T:IBinaryHeapData<T,M>
                                           where M: struct
    
    {
        int Compare(T a, T b);
        M GetData();
        void SetData(M m);
    }

    public class BinaryHeap<T,M> where T:IBinaryHeapData<T,M>
                                 where M: struct
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

        //在尾部增加一个节点
        public void Insert(T node)
        {
            Debug.Log("Binary insert---------->"+node.GetData());
            this.heap.Add(node);
            int n = heap.IndexOf(node);
            //上浮
            this.ComeUp(n + 1);//
        }

        //去除总是去掉根节点，如果根节点被去除，后边的节点都要移动？数组太拉了
        /*
            Pop思路：将根节点与数组最后一个节点交换，然后对新的根节点做下沉操作
        */
        public T Pop()
        {
            if (this.heap.Count > 0)
            {
                T last = this.heap[heap.Count-1];
                Swap(Top(), Bottom());
                this.heap.RemoveAt(this.heap.Count - 1);
                Sink(1);
                return last;
            }
            //与子节点中最大的那个节点比较，数据交换，出堆
            //下沉？ 将根节点与最后的节点交换数据，然后下沉到正确位置
            return default(T);
        }

        public T Top()
        {
            return HeapSafeIndex(1);
        }

        public T Bottom()
        {
            return HeapSafeIndex(this.heap.Count);
        }

        public void Clear()
        {
            this.heap.Clear();
        }

        /*
            上浮操作：
                将待插入节点与根节点比较，如果小于跟根节点，那么与根节点交换位置（小根堆）
        */
        private void ComeUp(int n)
        {
            for (int i = n; i > 0; i /= 2)
            {
                try
                {
                    int b = HeapSafeIndex(i).Compare(HeapSafeIndex(i), HeapSafeIndex(i / 2));//将节点i 与其父节点的值相比较，对于小顶堆，大于那么就下沉                
                    if (b < 0)
                    {
                        Swap(HeapSafeIndex(i), HeapSafeIndex(i / 2));
                    }
                }
                catch (System.Exception argEx)
                {
                    Debug.LogWarning("BinaryHeap index error:" + argEx.GetType().ToString());
                    break;
                }
            }
        }


        //当输入一个值，假设在尾部插入，如果该节点的值
        //假设输入的是一个以1开始的索引
        /*
            下沉思路：对于一个小根堆，根节点大于子节点，那么就必须与子节点中的较大的那个交换值。一直交换直到索引非法  
            那么，为啥要选大的呢？      
        */
        private void Sink(int n)
        { 
            //与最大的子节点相比较
            int si =  Son(n);//获取最大的子节点
            for(int i = n; si < this.heap.Count;i = si)
            {
                si = Son(i);

                try
                {
                    if (HeapSafeIndex(si).Compare(HeapSafeIndex(i), HeapSafeIndex(si)) > 0)//下沉
                    {
                        Swap(HeapSafeIndex(si), HeapSafeIndex(i));
                    }
                }
                catch (System.Exception argEx)
                {
                    Debug.LogWarning("BinaryHeap index error:" + argEx.ToString());
                    break;
                }

            }
        }

        //获取n节点需要交换的子节点
        //假设输入以1开始的索引
        //获取较大的子节点
        private int Son(int n)
        {
            if( n * 2 + 1 > this.heap.Count ) return n * 2;
            //n的右边节点和左边节点比较
            int b = heap[n].Compare(HeapSafeIndex(n * 2 + 1) ,HeapSafeIndex(n * 2));
            return n * 2 + Mathf.Clamp(b,0,1) ;
        }

        private void Swap(T a, T b)
        {             
            if(a==null || b==null) return;    
            M d = a.GetData();
            a.SetData(b.GetData());
            b.SetData(d);  
        }

        private int Left(int i)
        {
            return i *2;
        }

        private int Right(int i)
        {
            return i * 2 + 1;
        }

        private int Parent(int i)
        {
            return i / 2;//奇数向下取整也没算错
        }

        //假设所有的索引都是以1开始
        public T HeapSafeIndex(int i)
        {
            if( i > 1 && i <= heap.Count)
                return this.heap[i-1];          
            else  
            {
                Debug.Log("Binary heap index from 1.");
                return default(T);
            }
        }

        public void Watch()
        {
            string log = "";
            foreach(var i in heap)
            {
                log +=$"Index:{heap.IndexOf(i)}, Data: {i.GetData()}\n";                
            }
            Debug.Log(log);
        }

      
       

    }


}

