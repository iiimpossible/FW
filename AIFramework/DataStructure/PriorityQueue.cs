using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GraphyFW.Common
{
    //对于一个优先队列来说，不同于普通队列的先进先出，优先队列为优先级最高的对象先出，涉及新元素入队后后元素排序问题
    public class PriorityQueue<T>
    {
        List<T> listElemnts = new List<T>();
        private Dictionary<T,int> dicPriority = new Dictionary<T, int>();
        public PriorityQueue()
        {
            
        }

        public void DeQueue(){}

        public void EnQUeue(T t,int priority)
        {
            Sort(t,priority);
        }

        public void First(){}

        public void  Remove() {}


        private void Sort(T t, int priority) 
        {
            listElemnts.Add(t);
            dicPriority.Add(t,priority);
        }

    }

}
