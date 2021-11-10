using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GraphyFW.Common
{
    /// <summary>
    /// 二叉堆，用于实现优先队列
    /// 元素必须实现二叉堆的GetData  SetData Compare 接口
    /// </summary>
    public class NewBinaryHeap<T,DT> where T:IBinaryHeapData<T,DT>
                                    where DT:struct
    {
        private List<T> heap ;

        public int Count {get {return this.heap.Count;}}

        public NewBinaryHeap()
        {
            heap = new List<T>();
        }

        public void Push(T d)
        {
            //Debug.Log($"[Push]Push data----->{d.GetData()}");
            heap.Add(d);
            this.ComeUp(heap.Count);
        }

        public T Pop()
        {
            if(heap.Count == 1)
            {
                 //Debug.Log($"[Pop]Current pop index {1} data{heap[heap.Count -1].GetData()}");
                 T res = heap[heap.Count - 1];
                heap.RemoveAt(heap.Count - 1);
               
                return res;
            }
            //首尾交换
            if (TopToButtom())
            {
                //Debug.Log($"[Pop]Current pop index {1} data{heap[heap.Count -1].GetData()}");
                T res = heap[heap.Count - 1];
                heap.RemoveAt(heap.Count - 1);                
                Sink(1);
                return res;
            }
            
            //Debug.Log("[Pop]BinaryHeap count less than 2.");
            return default(T);
        }

        private void Sink(int n)
        {            
            int si = Son(n);
            //Debug.Log($"[Sink]The fist son is--------->{si}");
            for (int i = n; i < heap.Count && i > 0; i = si)
            {
                //如果这两个有非法，不能进行
                T a = Index(i);
                T b = Index(si);
                if (a.Compare(a,b) > 0)//如果son(i)非法，index返回index.minValue，不管如何，过不了swap判断
                {
                    if (!Swap(i, si)) { Debug.Log("[Sink] Sink over."); return; };
                }
                //Debug.Log($"[Sink]Son index is----->{si}");
                si = Son(si);
            }
        }

        private void ComeUp(int n)
        {
            for (int i = n; i > 0; i /= 2)//死循环预警：可能不会小于0
            {
                //Debug.Log($"[ComeUp](Cur index {i},data {Index(i).GetData()}) (Farth index {i / 2},data {Index(i / 2)?.GetData()})");
                T a = Index(i);
                T b = Index(i / 2);
                if(a == null) return;
                if (a.Compare(a,b ) < 0)//当i/2非法时，Index返回int.minValue 当n节点小于它的父节点，那么就交换
                {
                    if (!Swap(i, i / 2)) { Debug.Log("[ComeUp] ComeUp over."); return; };//当索引非法，不继续进行操作
                }
                else
                    break;
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
                T a = Index(n * 2);
                T b = Index(n * 2 + 1);
                //TODO:返回小的那个儿子的索引
                return a.Compare(a,b) < 0 ? n * 2 : n * 2 + 1;
            }
            else if (Valid(n * 2 - 1))
                return n * 2;
            else if (Valid(n * 2))
                return n * 2 + 1;
            return -1;//索引非法  
        }


        /// <summary>
        /// 输入一个堆索引，返回一个对象（安全的）
        /// </summary>
        /// <param name="n"></param>
        /// <returns>一个T类型对象，索引非法返回T的默认值</returns>
        private T Index(int n)
        {
            return Valid(n - 1) ? heap[n - 1] : default(T);
        }  

        private bool Valid(int index)
        {
            if (index >= 0 && index < heap.Count)
                return true;
            return false;
        }

        private bool TopToButtom()
        {
            if (heap.Count > 1)
                return Swap(1, heap.Count);
            return false;
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
            int sa, sb;
            sa = a - 1;
            sb = b - 1;
            if (!Valid(sa) || !Valid(sb))
            {             
                return false;
            }
            //Debug.Log($"[Swap](index {a},data {heap[sa].GetData()}) swap to (index {b},data {heap[sb].GetData()})");
            T t = heap[sa];
            heap[sa] = heap[sb];
            heap[sb] = t;
            return true;
        }


        public void Clear()
        {
            this.heap.Clear();
        }



        /// <summary>
        /// 输出堆中的值
        /// </summary>
        public void Watch()
        {
            string log = "[Watch]";
            for (int i = 0; i < heap.Count; i++)
            {
                log += $"Total Index{i + 1}, Data----------->{heap[i].GetData()}\n";
            }
            Debug.Log(log);
        }



    }

}
