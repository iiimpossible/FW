using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace GraphyFW.GamePlay
{
    /// <summary>
    /// 食物类，存储该道具状态
    /// </summary>
    public class Food: Prop
    {      
        public static readonly string prefabPath = "Prefabs/Food";  
        //新鲜度
        public int freshness { get; set; }

        //提供的能量
        public int energy { get; set; }

        public Food(GameObject go) : base(go)
        {            
        }
    }
}


