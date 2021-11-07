using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ScptInputManager : MonoBehaviour
{
    public UnityAction<Vector2Int> eventMouseInWorldPos;
    
    public static ScptInputManager  instance{get;private set;}

    private void Awake() {
        instance = this;
    }
    void Start()
    {
        
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
           Debug.Log("pos: " + tgpos);
           eventMouseInWorldPos?.Invoke(tgpos);
        }
    }

    //鼠标控制
}
