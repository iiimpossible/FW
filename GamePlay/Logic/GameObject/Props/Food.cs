using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace GraphyFW.GamePlay
{
    // [System.Serializable] 
    /// <summary>
    /// 食物类，存储该道具状态
    /// </summary>
    public class Food: Prop
    {      
        public static readonly string prefabPath = "Prefabs/Prop/Food";  

         [SerializeField]
        private int m_storageTime = 10;

         [SerializeField]
        private int m_nutrition = 10;

       /// <summary>
       /// 能够存储的时间，超过则腐坏
       /// </summary>
       /// <value></value>
        public float storageTime
        {
            get { return m_storageTime; }
            private set
            {

            }
        }

        /// <summary>
        /// 一份食物提供的营养值，固定的，需要读取XML文档设置
        /// </summary>
        /// <value></value>
        public float nutrition
        {
            get { return m_nutrition; }
            private set
            {

            }
        }

        private void Awake()
        {

        }
    }
}


