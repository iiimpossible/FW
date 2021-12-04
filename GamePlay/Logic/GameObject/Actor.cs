using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GraphyFW.GamePlay
{
    public class Actor
    {
        /// <summary>
        /// 营养值，当前的饥饿程度
        /// </summary>
        [SerializeField]
        private float nutrition;

        /// <summary>
        /// 精力值，代表当前的疲劳度
        /// </summary>
        [SerializeField]
        private float vigor;

        /// <summary>
        /// 健康值，当前的血量
        /// </summary>
        [SerializeField]
        private float health;

        /// <summary>
        /// 健康回复速度
        /// </summary>
        [SerializeField]
        private float healthFactor;

        public GameObject instance { get; private set; }

        public Vector2Int mapPos { get; set; }

        public Actor()
        {

        }

        public void SetGO(GameObject go, Vector2Int pos)
        {
            instance = go;
            mapPos = pos;
        }
    }

}
