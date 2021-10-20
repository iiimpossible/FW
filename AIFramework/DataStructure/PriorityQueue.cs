using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GraphyFW.Common
{

    
        public interface IGetPriority
        {
            float GetPriority();
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
    public class PriorityQueue<T> where T: IGetPriority
    {

        private List<PQElemet<T>> listElemnts = new List<PQElemet<T>>();
        private HashSet<int> prioSet = new HashSet<int>();

        public bool minPriority {get;set;}
        public PriorityQueue() {this.minPriority = true; }

        public int Count {get{return listElemnts.Count;}}

        public void EnQUeue(T data)
        {
            //if( prioSet.Contains(priority))
            this.listElemnts.Add(new PQElemet<T>(data.GetPriority(), data));
            this.listElemnts.Sort(new PQElemet<T>());
            // if (minPriority)
            //     this.listElemnts.Sort(delegate (PQElemet<T> a, PQElemet<T> b) { return System.Convert.ToInt32((a < b)); });
            // else
            //     this.listElemnts.Sort(delegate (PQElemet<T> a, PQElemet<T> b) { return System.Convert.ToInt32((a > b)); });
        }

        public T DeQueue() 
        {

            if(listElemnts.Count>=1)
            {
                T t = listElemnts[0].data;
                listElemnts.RemoveAt(0);
                this.listElemnts.Sort(new PQElemet<T>());
                return listElemnts[0].data;
            }
            else
            {
                return default(T);
            }

         }

        //public PQElemet<T> First() { if() return listElemnts[0];}

        public void Remove() { }

        public IEnumerator GetEnumerator()
        {
            for (int i = 0; i < listElemnts.Count; i++)
            {
                yield return listElemnts[i].data;
            }
        }

        public void Watch()
        {
            for (int i = 0; i < this.listElemnts.Count; i++)
            {
                Debug.Log("Priority: " + listElemnts[i].priority + "Data: " + listElemnts[i].data);
            }

        }



    }

}
