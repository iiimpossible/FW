using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GraphyFW.AI
{
    
public class MultiActorController : MonoBehaviour
{



    public static MultiActorController instance{get;private set;}

    private void Awake() {
        instance = this;
    }
  
    void Start()
    {
        MessageManager.instance.AddListener(EMessageType.OnMouseDown_MousePosInWorld_1,ActorsMove_Listener);
    }

   
    void Update()
    {
        
    }

    //
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

    //所有被框选的Actor的移动控制
    private void ActorsMove_Listener(Message message)
    {
        Vector3 worldpos = (Vector3)message.paramsList[0];
        Vector2Int pos = GameMode.instance.mainMap.WorldSpaceToMapSpace(worldpos);
        Debug.LogWarning("Manual target pos is: " + pos);
        foreach (var item in GameMode.instance.selctedGameObjectList)
        {   
            //TODO：为了避免所有选中的Actor都挤到一条路线上，应该目标位置做偏移，每个Actor的目标位置不同（从Pos的周围找合法点）。
            item.GetComponent<ActorController>().MoveTo(pos);
        } 
    }

    private void OnDestroy() {
         MessageManager.instance?.RemoveListener(EMessageType.OnMouseDown_MousePosInWorld_1,ActorsMove_Listener); 
    }

}

}
