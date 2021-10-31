using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GraphyFW.Common
{

    
        public interface IGetPriority 
        {
            float GetPriority();
            void SetPriority(float p);
        }
        struct PQElemet<M> : IComparer<PQElemet<M>>
        {
            public M data;
            public float priority;
            public PQElemet(float priority, M data)
            {
                this.priority = priority;
                this.data = data;
            }

            public static bool operator >(PQElemet<M> a, PQElemet<M> b)
            {
                if (a.priority > b.priority)
                    return true;
                return false;
            }

            public static bool operator <(PQElemet<M> a, PQElemet<M> b)
            {
                if (a.priority < b.priority)
                    return true;
                return false;
            }

            public int Compare(PQElemet<M> a, PQElemet<M> b)
            {
                return a.priority.CompareTo(b.priority);
            }

        }
        
    //对于一个优先队列来说，不同于普通队列的先进先出，优先队列为优先级最高的对象先出，涉及新元素入队后后元素排序问题
    //应该使用二叉堆实现，目前使用一个普通排序，因为数据量还不大
    public class PriorityQueue<T> where T: IGetPriority,IBinaryHeapData<T,float>
    {

         public delegate float NoneName(T a, T b);
        
        private List<T> listElemnts = new List<T>();
       
        private NewBinaryHeap<T, float> newHeap  = new NewBinaryHeap<T, float>();

        public bool minPriority {get;set;}
        public PriorityQueue() {this.minPriority = true; }

        public int Count {get{return listElemnts.Count;}}

        public int bhCout {get {return newHeap.Count;}}

        
        public void EnQueueBh(T data)
        {
            Debug.Log($"Push data----->{data.GetData()}");
            this.newHeap.Push(data);
        }


        public T DeQueueBh()
        {
            return this.newHeap.Pop();
        }


        public void WatchBh()
        {
            this.newHeap.Watch();
        }

        public void EnQUeue(T data)
        {
            //if( prioSet.Contains(priority))
           
            this.listElemnts.Add(data);
            this.listElemnts.Sort(delegate(T a, T b){return (a.GetPriority() < b.GetPriority()) ?-1:1;});           
       
        }


        public T DeQueue() 
        {

            if(listElemnts.Count>0)
            {
                T t = listElemnts[0];
                listElemnts.RemoveAt(0);                 
                this.listElemnts.Sort(delegate(T a, T b){return (a.GetPriority() < b.GetPriority()) ?-1:1;});
                return t;
            }
            else
            {
                return default(T);
            }

         }

         public void Refresh()
         {
             this.listElemnts.Sort((T a, T b) =>{return (a.GetPriority() < b.GetPriority()) ? -1:1;});              
         }

        //public PQElemet<T> First() { if() return listElemnts[0];}

        public void Remove() { }


        public T First()
        {
            if(listElemnts.Count > 0)
                return listElemnts[0];
            return default(T);
        }

        public IEnumerator GetEnumerator()
        {
            for (int i = 0; i < listElemnts.Count; i++)
            {
                yield return listElemnts[i];
            }
        }

        public void Watch()
        {
            string log = "None------>"+ listElemnts.Count;
            for (int i = 0; i < this.listElemnts.Count; i++)
            {
                log = log + "Priority: " + listElemnts[i].GetPriority()+ " ";
            }
            Debug.Log(log);

        }





    }

}
