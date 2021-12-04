using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

/// <summary>
/// 输入管理器并不是管理键盘、鼠标输入的，而是和游戏性紧密结合，是于本游戏紧密相关的自定义的一些输入 
/// 1.玩家控制命令输入等
/// 
/// </summary>
public class ScptInputManager : MonoBehaviour
{

    public static ScptInputManager instance { get; private set; }





    private void Awake()
    {
        instance = this;

    }

    private void Start()
    {

    }


    Vector2Int tgpos = new Vector2Int();

    Vector3 camPos = new Vector3();
    // Update is called once per frame
    void Update()
    {
        //当左键按下
        if (Input.GetMouseButtonDown(0))
        {
            if (!EventSystem.current.IsPointerOverGameObject())
            {
                camPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                tgpos.Set((int)camPos.x, (int)camPos.y);

                MessageManager.instance.Dispatch(EMessageType.OnMouseDown_MousePosInWorld_0.ToString(), EMessageType.OnMouseDown_MousePosInWorld_0, tgpos);
                MessageManager.instance.Dispatch(EMessageType.OnMouseButtonDown_0.ToString(), EMessageType.OnMouseButtonDown_0);
            }

        }

        //当右键按下
        if (Input.GetMouseButtonDown(1))
        {

            //当不在UI上的时候发送消息
            if (!EventSystem.current.IsPointerOverGameObject())
            {
                camPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);               
                MessageManager.instance.Dispatch(EMessageType.OnMouseDown_MousePosInWorld_1.ToString(), EMessageType.OnMouseDown_MousePosInWorld_1, camPos);
                MessageManager.instance.Dispatch(EMessageType.OnMouseButtonDown_1.ToString(), EMessageType.OnMouseButtonDown_1);
            }

        }

        //当中键按下
        if (Input.GetMouseButtonDown(2))
        {
            if (EventSystem.current.IsPointerOverGameObject()) return;
            MessageManager.instance.Dispatch(EMessageType.OnMouseButtonDown_2.ToString(), EMessageType.OnMouseButtonUp_2);

        }


        //当左键释放
        if (Input.GetMouseButtonUp(0))
        {
            if (EventSystem.current.IsPointerOverGameObject()) return;
            MessageManager.instance.Dispatch(EMessageType.OnMouseButtonUp_0.ToString(), EMessageType.OnMouseButtonUp_0);
        }

        //当右键释放
        if (Input.GetMouseButtonUp(1))
        {
            if (EventSystem.current.IsPointerOverGameObject()) return;
            MessageManager.instance.Dispatch(EMessageType.OnMouseButtonUp_1.ToString(), EMessageType.OnMouseButtonUp_1);
        }

        //当中键释放
        if (Input.GetMouseButtonUp(2))
        {
            if (EventSystem.current.IsPointerOverGameObject()) return;
            MessageManager.instance.Dispatch(EMessageType.OnMouseButtonUp_2.ToString(), EMessageType.OnMouseButtonUp_2);
        }




    }

}
