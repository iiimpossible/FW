using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 绘制运行时可变图形
/// </summary>
public class DrawRunTimeShape
{
    //绘制框选框框
    public static void DrawQuad(Vector3 start, Vector3 end, Color color, Material material )
    {
        if (!material)
            return;
        material.SetPass(0);//为渲染激活给定的pass。
        //Debug.Log($"Draw quad: [{start}  [{end}]");
        GL.PushMatrix();
        GL.LoadProjectionMatrix(Camera.main.projectionMatrix);//使用相机的投影矩阵绘图
        GL.Begin(GL.QUADS);
        GL.Color(color);//设置颜色和透明度，方框内部透明
        //绘制顶点
        GL.Vertex3(start.x, start.y, 0);

        GL.Vertex3(end.x, start.y, 0);

        GL.Vertex3(end.x, end.y, 0);

        GL.Vertex3(start.x, end.y, 0);
        GL.End();      
        GL.PopMatrix();      
    }


    public static void DrawLine(List<Vector2> verts, Color color, Material material)
    {
        if(verts.Count<2) return;
         if (!material)
            return;
        material.SetPass(0);//为渲染激活给定的pass。
        //Debug.Log($"Draw quad: [{start}  [{end}]");
        GL.PushMatrix();        
        GL.LoadProjectionMatrix(Camera.main.projectionMatrix);//使用相机的投影矩阵绘图
        GL.Begin(GL.LINES);
        GL.Color(color);//设置颜色和透明度，方框内部透明
        //绘制顶点
        for(int i = 0; i< verts.Count-1;i++)
        {
            GL.Vertex3(verts[i].x,verts[i].y,0);
            GL.Vertex3(verts[i+1].x,verts[i+1].y,0);

        }
        foreach (var item in verts)
        {
            GL.Vertex3(item.x, item.y, 0);
        }   
        GL.End();      
        GL.PopMatrix();    
    }


}
