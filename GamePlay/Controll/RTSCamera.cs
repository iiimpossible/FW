using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 一个RTS游戏的相机，ADWS控制x轴和y轴或者鼠标中键控制视口移动
/// </summary>
[RequireComponent(typeof(Camera))]
public class RTSCamera : MonoBehaviour
{

    public bool middleKey = true; 

    public float mouseSpeedScale = 40.0f;  

    public float keyboardSpeedScale = 25f;
    public float fieldOfViewScale = 1000.0f;

    public float originFOV = 100;

    public float maxCameraFOV = 160;
    public float minCameraFOV = 30;

    public float maxCameraOrthSize = 40;

    public float minCameraOrthSize = 5;
    private float speed = 1f;  
    
    private Camera myCamera;

    
    void Start()
    {
        myCamera = transform.GetComponent<Camera>(); 
        myCamera.fieldOfView = originFOV;
    }


    void Update()
    {
        keyboardMove();
        MouseMove();
        CameraSize();
    }

    /// <summary>
    /// button 值为 0 表示主按钮（通常为左按钮），1 表示副按钮，2 表示中间按钮。
    /// </summary>
    private void MouseMove()
    {
        if(Input.GetMouseButton(0) && middleKey)
        {
            float x = Input.GetAxis("Mouse X");
            float y = Input.GetAxis("Mouse Y");
            //Camera.main.WorldToViewportPoint(vec3) 世界空间的点在屏幕的位置
            //Input.mousePosition; 鼠标在屏幕的位置
            //Camera.main.ScreenToWorldPosition
            //如果在按下中键的时候鼠标移动，那么视口随鼠标运动对应变化
            //首次移动怎么处理？跳动            
            transform.Translate(new Vector3(-x * mouseSpeedScale * Time.deltaTime,-y* mouseSpeedScale * Time.deltaTime,0));

        }
    }


    private void CamraField()
    {
        float d = -Input.GetAxis("Mouse ScrollWheel");               
        if(d > 0 && myCamera.fieldOfView < maxCameraFOV)
        {            
             myCamera.fieldOfView += d * fieldOfViewScale * Time.deltaTime;
             Debug.Log("myCamera.fieldOfView: " + myCamera.fieldOfView); 
            //myCamera.sensorSize
           
        }
        if( d < 0 && myCamera.fieldOfView > minCameraFOV)
        {
             
            myCamera.fieldOfView += d * fieldOfViewScale * Time.deltaTime;
            Debug.Log("myCamera.fieldOfView: " + myCamera.fieldOfView); 
        }
        //if()
    }

    private void CameraSize()
    {
        float d = -Input.GetAxis("Mouse ScrollWheel");
        if (d > 0 && myCamera.orthographicSize < maxCameraOrthSize)
            myCamera.orthographicSize += d * fieldOfViewScale * Time.deltaTime;

        if (d < 0 && myCamera.orthographicSize > minCameraOrthSize)
            myCamera.orthographicSize += d * fieldOfViewScale * Time.deltaTime;

    }

    private void keyboardMove()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        transform.Translate(new Vector3(h * keyboardSpeedScale * Time.deltaTime, v * keyboardSpeedScale * Time.deltaTime, 0));
    }
}
