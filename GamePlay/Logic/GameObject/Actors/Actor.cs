using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GraphyFW.GamePlay
{
    /// <summary>
    /// Actor类描述一个此游戏 Actor的最基本的属性
    /// </summary>
    [System.Serializable]
    public class Actor : MonoBehaviour
    {
        [SerializeField]
        private float m_nutrition = 50f;
        [SerializeField]
        private float m_vigor = 50f;
        [SerializeField]
        private float m_health = 50f;
        [SerializeField]
        private float m_healthFactor = 0.05f;

        private float m_maxHealth = 100f;
        private float m_minHealth = 0f;
        private float m_maxVigor = 100f;
        private float m_minVigor = 0f;
        private float m_maxNutrition = 100f;
        private float m_minNutrition = 0f;


        /// <summary>
        /// 营养值，当前的饥饿程度
        /// </summary>
        public float nutrition { get { return m_nutrition; }        set { m_nutrition = Mathf.Clamp(value, 0, m_maxNutrition); } }
        /// <summary>
        /// 精力值，代表当前的疲劳度
        /// </summary>       
        public float vigor { get { return m_vigor; }                set { m_nutrition = Mathf.Clamp(value, 0, m_maxVigor); } }
        /// <summary>
        /// 健康值，当前的血量
        /// </summary>        
        public float health { get { return m_health; }              set { m_nutrition = Mathf.Clamp(value, 0, m_maxHealth); } }
        /// <summary>
        /// 健康回复速度
        /// </summary>        
        public float healthFactor { get { return m_healthFactor; }  private set { } }


        public Vector2Int mapPos { get; set; }


    }

}
