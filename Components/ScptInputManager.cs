using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// 输入管理器并不是管理键盘、鼠标输入的，而是和游戏性紧密结合，是于本游戏紧密相关的自定义的一些输入 
/// 1.玩家控制命令输入等
/// 
/// </summary>
public class ScptInputManager : MonoBehaviour
{
    public event UnityAction<Vector2Int> onMouseInWorldPos;
    
    public static ScptInputManager  instance{get;private set;}





    private void Awake() {
        instance = this;

    }

    private void Start() {
        
    }


    Vector2Int tgpos = new Vector2Int();
    
    Vector3 camPos = new Vector3();
    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(1))
        {            
           camPos =  Camera.main.ScreenToWorldPoint(Input.mousePosition);
           tgpos .Set((int)camPos.x,(int)camPos.y);           
           onMouseInWorldPos?.Invoke(tgpos);
           MessageManager.instance.Dispatch("OnMouseInWorldPos",EMessageType.OnMouseDown_MousePosInWorld_1,false,tgpos);
            
        }  

        if(Input.GetMouseButtonDown(0))
        {
            MessageManager.instance.Dispatch(EMessageType.OnMouseButtonDown_0.ToString(),EMessageType.OnMouseButtonDown_0);
        }
    }
    
}
