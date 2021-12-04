using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public struct QuadVerts
{
    public Vector3 start;
    public Vector3 end;
}


public class CameraSelectObject : MonoBehaviour
{

    public static CameraSelectObject instance { get; private set; }

    //private Material rectMat = null;//画线的材质 不设定系统会用当前材质画线 结果不可控
    public Material rectMat = null;//这里使用Sprite下的defaultshader的材质即可

    private bool isSelected = false;

    private Vector3 pressPos = Vector3.zero;

    Vector2 box2DSize = Vector2.zero;

    private List<QuadVerts> qversList = new List<QuadVerts>();

    private List<List<Vector2>> lineVertList = new List<List<Vector2>>();

    public Color quadColor = new Color(0, 1, 0, 0.1f);

    public Color storageAreaColor = new Color(0,1,0,0.3f);

    public Color lineColor = Color.black;
    private void Awake()
    {
        instance = this;
    }

    void Start()
    {

        //        rectMat = new Material("Shader \"Lines/Colored Blended\" {" +
        //
        //        "SubShader { Pass { " +
        //
        //        "    Blend SrcAlpha OneMinusSrcAlpha " +
        //
        //        "    ZWrite Off Cull Off Fog { Mode Off } " +
        //
        //        "    BindChannels {" +
        //
        //        "      Bind \"vertex\", vertex Bind \"color\", color }" +
        //
        //        "} } }");//生成画线的材质

        rectMat.hideFlags = HideFlags.HideAndDontSave;

        rectMat.shader.hideFlags = HideFlags.HideAndDontSave;//不显示在hierarchy面板中的组合，
        //不保存到场景并且卸载Resources.UnloadUnusedAssets不卸载的对象。
        //MessageManager.instance.AddListener(EMessageType.OnMouseButtonDown_0,OnMouseLeftButtonDownStart_Listener);
        //MessageManager.instance.AddListener(EMessageType.OnMouseButtonUp_0,OnMouseLeftButtonDownEnd_Listener);

    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && !UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())
        {
            //Debug.Log("IsSelected");
            isSelected = true;
            pressPos = Input.mousePosition;
        }
        if (Input.GetMouseButtonUp(0)&& !UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())
        {
            //Debug.Log("Not selected");
            isSelected = false;

            ChooseObjects(pressPos, Input.mousePosition);        

        }
        
    }

    private void OnPostRender()
    {
        if (isSelected)
        {
            DrawQuad(pressPos, Input.mousePosition);
        }

        foreach (var item in qversList)
        {
            DrawRunTimeShape.DrawQuad(item.start, item.end, storageAreaColor, rectMat);
        }

        foreach (var item in lineVertList)
        {
            DrawRunTimeShape.DrawLine(item,lineColor,rectMat);
        }
    }


    private void OnMouseLeftButtonDownStart_Listener(Message message)
    {
        isSelected = true;
        pressPos = Input.mousePosition;
    }

    private void OnMouseLeftButtonDownEnd_Listener(Message message)
    {
        isSelected = false;

        ChooseObjects(pressPos, Input.mousePosition);
    }



    //绘制框选框框
    public void DrawQuad(Vector2 start, Vector2 end)
    {
        if (!rectMat)
            return;
        rectMat.SetPass(0);//为渲染激活给定的pass。
        //Debug.Log($"Draw quad: [{start}  [{end}]");
        GL.PushMatrix();
        GL.LoadPixelMatrix();//设置用屏幕坐标绘图
        GL.Begin(GL.QUADS);
        GL.Color(quadColor);//设置颜色和透明度，方框内部透明
        //绘制顶点
        GL.Vertex3(start.x, start.y, 0);

        GL.Vertex3(end.x, start.y, 0);

        GL.Vertex3(end.x, end.y, 0);

        GL.Vertex3(start.x, end.y, 0);
        GL.End();

        GL.Begin(GL.LINES);//开始绘制线

        GL.Color(Color.green);//设置方框的边框颜色 边框不透明

        GL.Vertex3(start.x, start.y, 0);

        GL.Vertex3(end.x, start.y, 0);

        GL.Vertex3(end.x, start.y, 0);

        GL.Vertex3(end.x, end.y, 0);

        GL.Vertex3(end.x, end.y, 0);

        GL.Vertex3(start.x, end.y, 0);

        GL.Vertex3(start.x, end.y, 0);

        GL.Vertex3(start.x, start.y, 0);

        GL.End();

        GL.PopMatrix();
    }


    /// <summary>
    /// TODO:将这个逻辑移动到GameMode 里边去，这里只负责绘制选择框， 发送start和end
    /// </summary>
    /// <param name="start">屏幕空间的坐标（鼠标）Quad起点</param>
    /// <param name="end">屏幕空间的坐标（鼠标）Quad终点</param>
    public void ChooseObjects(Vector3 start, Vector3 end)
    {
        if( (end - start).magnitude <1) return;
        var worldstart = Camera.main.ScreenToWorldPoint(start);
        var wordlEnd = Camera.main.ScreenToWorldPoint(end);

        Vector3 center = (wordlEnd + worldstart) * 0.5f;
        box2DSize.Set(Mathf.Abs((worldstart - wordlEnd).x), Mathf.Abs((worldstart - wordlEnd).y));

        RaycastHit2D[] hits = Physics2D.BoxCastAll(center, box2DSize, 0, Vector3.forward);
        Debug.Log($"BoxCast worldStart: {worldstart}, worldEnd: {wordlEnd} center: {center}, size: {box2DSize}, derection: {Vector3.forward}");
        if(GameMode.instance.GetPlayerCommad() == GameMode.EPlayCommands.NORMAL)  MessageManager.instance.Dispatch("OnBoxCastAllCollider", EMessageType.OnBoxCastAllCollider,hits);
        if(GameMode.instance.GetPlayerCommad() == GameMode.EPlayCommands.CREATE_STORAGE_AREA) MessageManager.instance.Dispatch("OnFrameSelect", EMessageType.OnFrameSelected,worldstart, wordlEnd, center, box2DSize);

        // QuadVerts verts = new QuadVerts();
        // verts.start = worldstart;
        // verts.end = wordlEnd;
        // AddQVerts(verts);
    }


    public void AddQVerts(QuadVerts verts)
    {
        this.qversList.Add(verts);
    }

    public void AddLineVerts(List<Vector2> verts)
    {
        this.lineVertList.Add(verts);
    }

    public void RemoveLineVerts(List<Vector2> verts)
    {
        this.lineVertList.Remove(verts);
    }
}
