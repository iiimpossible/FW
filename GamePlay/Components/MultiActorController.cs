using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GraphyFW.AI
{
    /// <summary>
    /// 多个Actor控制器。
    /// 该类会保存框选到的Actors，然后会给它们统一发送命令（调用事件方法）
    /// 耦合度：需要了解ActorController的行为方法，依赖MessageManger接收消息（框选的物体）
    /// </summary>
    public class MultiActorController : MonoBehaviour
    {
        public static MultiActorController instance { get; private set; }

        private void Awake()
        {
            instance = this;
        }

        void Start()
        {
            MessageManager.instance.AddListener(EMessageType.OnMouseDown_MousePosInWorld_1, ActorsMove_Listener);
            MessageManager.instance.AddListener(EMessageType.OnMouseButtonDown_MousePos_1, OnMouseDown1_RayCast_Listener);
        }
 
        /// <summary>
        /// 取消所有选中
        /// </summary>
        public void UnCheckObjects()
        {
            foreach (var item in GameMode.instance.selctedGameObjectList)
            {
                item.GetComponent<ActorController>().Selected(false);
            } 
        }

        
        /// <summary>
        /// 将所有的被框选的Actor征召,在UI中按下按钮调用
        /// </summary>
        public void ActorsCallUp()
        {
            foreach (var item in GameMode.instance.selctedGameObjectList)
            {
                item.GetComponent<ActorController>().CallUp();
            }
        }

        /// <summary>
        /// 应该不是所有的Actor吃东西，应该是单独
        /// </summary>
        /// <param name="food"></param>
        public void ActorsEat(GameObject food)
        {
            if(GameMode.instance.selctedGameObjectList.Count != 1) return;
            foreach (var item in GameMode.instance.selctedGameObjectList)
            {
                item.GetComponent<ActorController>().Eat(food);
            }
        }

      
        /// <summary>
        /// 所有被框选的Actor的移动控制
        /// 1.当右键射线检测到目标位置 有其他可以触发右键菜单的东西，那么就不移动
        /// </summary>
        /// <param name="message"></param>
        private void ActorsMove_Listener(Message message)
        {
            Vector3 worldpos = (Vector3)message.paramsList[0];
            var hits = Physics2D.Raycast(worldpos, Vector3.forward);
            if(hits.collider != null) return;
            Vector2Int pos = GameMode.instance.mainMap.WorldSpaceToMapSpace(worldpos);
            Debug.LogWarning("Manual target pos is: " + pos);
            foreach (var item in GameMode.instance.selctedGameObjectList)
            {
                //TODO：为了避免所有选中的Actor都挤到一条路线上，应该目标位置做偏移，每个Actor的目标位置不同（从Pos的周围找合法点）。
                item.GetComponent<ActorController>().MoveTo(pos);
            }
        }

        /// <summary>
        /// 当 右键按下， 发射射线
        /// </summary>
        private void OnMouseDown1_RayCast_Listener(Message message)
        {
            //Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            //var hits = Physics2D.Raycast(pos, Vector3.forward);
           //System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
           
        }

        private void OnDestroy()
        {
            MessageManager.instance?.RemoveListener(EMessageType.OnMouseDown_MousePosInWorld_1, ActorsMove_Listener);          
            MessageManager.instance?.RemoveListener(EMessageType.OnMouseButtonDown_MousePos_1, OnMouseDown1_RayCast_Listener);
        }

    }

}
