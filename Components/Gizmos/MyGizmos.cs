using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MyGizmos  : MonoBehaviour
{
    
       public Transform m_Transform;
       public float m_Radius = 1; // 圆环的半径
       [Range(0.0001f,2f)]
       public float m_Theta = 0.1f; // 值越低圆环越平滑
       public Color m_Color = Color.green; // 线框颜色       
      
       public Vector2Int GridSize = Vector2Int.one;

    public Vector2 GriddOffset = Vector2.one;

    [SerializeField]
    private Vector2 pos1 = Vector2.zero;
    [SerializeField]
    private Vector2 pos2 = Vector2.zero;

    private bool isBoxChoose = false;
    void Start()
       {
              if (m_Transform == null)
              {
                     throw new Exception("Transform is NULL.");
              }
       }
       void OnDrawGizmos()
       {
              if (m_Transform == null) return;           
              // 设置矩阵
              Matrix4x4 defaultMatrix = Gizmos.matrix;
              Gizmos.matrix = m_Transform.localToWorldMatrix;
              // 设置颜色
              Color defaultColor = Gizmos.color;
              Gizmos.color = m_Color;
              // 绘制圆环
              Vector3 beginPoint = Vector3.zero;
              Vector3 firstPoint = Vector3.zero;
              for (float theta = 0; theta < 2 * Mathf.PI; theta += m_Theta)
              {
                     float x = m_Radius * Mathf.Cos(theta);
                     float z = m_Radius * Mathf.Sin(theta);
                     Vector3 endPoint = new Vector3(x, 0, z);
                     if (theta == 0)
                     {
                            firstPoint = endPoint;
                     }
                     else
                     {
                            Gizmos.DrawLine(beginPoint, endPoint);
                     }
            beginPoint = endPoint;
        }
        // 绘制最后一条线段
        Gizmos.DrawLine(firstPoint, beginPoint);
        // 恢复默认颜色
        Gizmos.color = defaultColor;
        // 恢复默认矩阵
        Gizmos.matrix = defaultMatrix;

        DrawGridGraph(m_Transform.position, GridSize, GriddOffset);


        DrawSquareOnMouseDown();
    }


    /// <summary>
    /// 在某个平面上绘制网格
    /// </summary>
    /// <param name="center">网格中心</param>
    /// <param name="size">网格水平和垂直方向的格子数目</param>
    /// <param name="offset">实际上是网格格子的大小</param>
    public void DrawGridGraph(Vector3 center, Vector2Int size, Vector2 offset)
    {
        //Gizmos.DrawLine(Vector3.zero,new Vector3(10,10,10));
        //Gizmos.DrawCube(m_Transform.position,new Vector3(10,10,10));

        /*
               +-------------+
               |             |
               |             |
               |             |
               |             |
               +-------------+
        */


        //从当前位置向两边偏移 绘制垂直线段
        Vector3 setLinePos1 = Vector3.zero;
        Vector3 setLinePos2 = Vector3.zero;
        for (int i = 0; i < size.x; i++)
        {
            //绘制原点正x方向 对于+x方向， 偏移 = pos1.x = center.x + offset * i   长度 = pos1.y = offset * size.y
            setLinePos1.Set(center.x + i * offset.x, offset.y * (size.y - 1) + center.y, 0);//当size.x 增加时，垂直线的长度增加不一致
            setLinePos2.Set(center.x + i * offset.x, -offset.y * (size.y - 1) + center.y, 0);
            Gizmos.DrawLine(setLinePos1, setLinePos2);
            //绘制原点-x方向
            setLinePos1.Set(center.x + i * -offset.x, offset.y * (size.y - 1) + center.y, 0);
            setLinePos2.Set(center.x + i * -offset.x, -offset.y * (size.y - 1) + center.y, 0);
            Gizmos.DrawLine(setLinePos1, setLinePos2);

        }
        for (int j = 0; j < size.y; j++)
        {
            //绘制原点+y方向
            setLinePos1.Set(offset.x * (size.x - 1) + center.x, center.y + j * offset.y, 0);
            setLinePos2.Set(-offset.x * (size.x - 1) + center.x, center.y + j * offset.y, 0);
            Gizmos.DrawLine(setLinePos1, setLinePos2);
            //绘制原点-y方向
            setLinePos1.Set(offset.x * (size.x - 1) + center.x, center.y + j * -offset.y, 0);
            setLinePos2.Set(-offset.x * (size.x - 1) + center.x, center.y + j * -offset.y, 0);
            Gizmos.DrawLine(setLinePos1, setLinePos2);
        }

        //绘制水平线段
       // Debug.Log("Mouse 0 down.");
    }
    public void DrawSquare(Vector2 pos1, Vector2 pos2)
    {
        Vector2 linepos1 = Vector2.zero;
        Vector2 linepos2 = Vector2.zero;
        linepos1.Set(pos1.x, pos1.y);
        linepos2.Set(pos2.x, pos1.y);
        Gizmos.DrawLine(linepos1, linepos2);

        linepos1.Set(pos2.x, pos1.y);
        linepos2.Set(pos2.x, pos2.y);
        Gizmos.DrawLine(linepos1, linepos2);

        linepos1.Set(pos2.x, pos2.y);
        linepos2.Set(pos1.x, pos2.y);
        Gizmos.DrawLine(linepos1, linepos2);

        linepos1.Set(pos1.x, pos2.y);
        linepos2.Set(pos1.x, pos1.y);
        Gizmos.DrawLine(linepos1, linepos2);
    }


    public void DrawSquareOnMouseDown()
    {
        if (Input.GetMouseButtonDown(0))
        {
               Debug.Log("Mouse 0 down.");
            if (!isBoxChoose)
            {
                pos1 = Input.mousePosition;
                isBoxChoose = true;
            }

        }


        if (Input.GetMouseButtonDown(0))
        {
            if (isBoxChoose)
            {
                pos2 = Input.mousePosition;
                DrawSquare(pos1, pos2);
            }

        }
        if (Input.GetMouseButtonUp(0))
        {
            if (isBoxChoose)
                isBoxChoose = false;
        }
    }


}
