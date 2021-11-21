using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace GraphyFW.Common
{
    using System;

    public interface IResetable
    {
        public void Reset();
    }

    /// <summary>
    /// 对象池
    /// 1.池大小，申请内存
    /// 2.获取对象
    /// 3.回收对象
    /// </summary>
    public class ObjectPool<T> where T : class, IResetable,new()
    {
        private Stack<T> _stkObjects = new Stack<T>();
       
        private Action<T> _resetAction;

        private Action<T> _initAction;
        public ObjectPool(int size, Action<T> resetAction, Action<T> initAction)
        {
          _resetAction = resetAction;
          _initAction = initAction;
        }

        /// <summary>
        /// 从对象池种申请一个对象
        /// </summary>
        /// <returns></returns>
        public T New()
        {            
            if(_stkObjects.Count > 0)
            {
                T t = _stkObjects.Pop();
                t.Reset();
                if(_resetAction != null) _resetAction.Invoke(t);
                return t;
            }
            else
            {
                T t = new T();
                if(_initAction != null) _initAction.Invoke(t);
                return t;
            }          
        }

        public void Resycle(T obj)
        {
            _stkObjects.Push(obj);
        }

    }



    /// <summary>
    /// 对象池，
    /// 有一些类型不需要在一系列帧中存留，
    /// 仅在帧结束前就失效了。在这种情况下，
    /// 我们可以在一个合适的时机将所有已经池化的对象(pooled objects)再次存储于池中。
    /// 现在，我们重写该池使之更加简单高效。
    /// 
    /// 相比于原始的ObjectPool<T>，改动还是蛮大的。
    /// 先不管类的签名，可以看到，Store()已经被ResetAll()代替了,
    /// 且仅在所有已经分配的对象需要被放入池中时调用一次。
    /// 在类内部，Stack<T>被List<T>代替，其中保存了所有已分配的对象（包括正在使用的对象）的引用。
    /// 我们也可以跟踪最近创建或释放的对象在list中索引，由此，New()便可以知道是创建一个新的对象还是重置一个已有的对象。
    /// </summary>
    public class ObjectPoolWithCollectiveReset<T> where T : class, IResetable, new()
    {
        private List<T> m_objectList;
        private int m_nextAvailableIndex = 0;

        private Action<T> m_resetAction;
        private Action<T> m_onetimeInitAction;

        public ObjectPoolWithCollectiveReset(int initialBufferSize, Action<T>
            ResetAction = null, Action<T> OnetimeInitAction = null)
        {
            m_objectList = new List<T>(initialBufferSize);
            m_resetAction = ResetAction;
            m_onetimeInitAction = OnetimeInitAction;
        }

        public T New()
        {
            if (m_nextAvailableIndex < m_objectList.Count)
            {
                // an allocated object is already available; just reset it
                T t = m_objectList[m_nextAvailableIndex];
                m_nextAvailableIndex++;

                if (m_resetAction != null)
                    m_resetAction(t);

                return t;
            }
            else
            {
                // no allocated object is available
                T t = new T();
                m_objectList.Add(t);
                m_nextAvailableIndex++;

                if (m_onetimeInitAction != null)
                    m_onetimeInitAction(t);

                return t;
            }
        }

        public void ResetAll()
        {
            //重置索引
            m_nextAvailableIndex = 0;
        }
    }

}
