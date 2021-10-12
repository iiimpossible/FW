using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XLua;

//提供一个Xlua虚拟机的单例，并做一些控制操作
namespace GraphyFW
{
    ///<summary>
    ///</summary>    
    public class Scpt_XluaConfig : MonoBehaviour
    {
        internal static readonly LuaEnv luaEnv = new LuaEnv();

        private static float lastGCTime = 0;
        private const float GCInterval = 1;//1 second 
        private void Awake()
        {            
        }

        //每秒执行一次GC操作。什么时候产生垃圾？当一个table中有元素，将这个table = nil 时，其中的值的内存并没有回收，只是删除了table对它们的引用。GC操作会回收0引用的对象的内存
        private void Update()
        {
            if (Time.time - Scpt_XluaConfig.lastGCTime > GCInterval)
            {
                luaEnv.Tick();
                Scpt_XluaConfig.lastGCTime = Time.time;
            }
        }

    }


}
