using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 存储区绘制
/// 1.材质
/// 2.绘制方法
/// 3.重绘
/// 
/// </summary>
public class ScptStorageArea : MonoBehaviour
{

    [SerializeField]
    [Tooltip("Whether or not draw storage area")]
    private bool _drawArea = false;

    [SerializeField]
    private Material _material = null;

    [SerializeField]
    [Tooltip("Size in world space")]
    private Vector2 _size = Vector2.zero;

    [SerializeField]
    [Tooltip("Center in world space")]
    private Vector2 _center= Vector2.zero;

    [SerializeField]
    [Tooltip("Storage area color")]
    private Color _color = Color.green;

    [SerializeField]
    [Tooltip("Center in map space")]

    private Vector2Int _mapCenter = Vector2Int.zero;

    [SerializeField]
    [Tooltip("Center in map space")]
    private Vector2Int _mapSize = Vector2Int.zero;



    public Material material{get{return _material;} private set{}}

    public Vector2 center {get {return _center;} private set{}}

    public Vector2 size{get{return _size;} private set{}}

    public bool drawArea{get{return _drawArea;} set{_drawArea = value;}}


    private Vector3 _start;

    private Vector3 _end;
    
    [SerializeField]
    private Mesh quadMesh;

    private int[] newTriangles = {0,1,2,2,0,3};

    private void Awake() {
        quadMesh = new Mesh();
    }

    // Start is called before the first frame update
    void Start()
    {       
          //GetComponent<MeshFilter>().mesh = quadMesh;
        
          Vector3[] verts = {new Vector3(_start.x, _start.y, 0),new Vector3(_end.x, _start.y, 0), 
            new Vector3(_end.x, _end.y, 0),new Vector3(_start.x, _end.y, 0)};
        quadMesh.vertices = verts;
        quadMesh.triangles = newTriangles;
        quadMesh.uv = new Vector2[] {new Vector2(0, 0), new Vector2(0, 1), new Vector2(1, 1),new Vector2(1,0)};
    }

    // Update is called once per frame
    void Update()
    {
        if(_drawArea)
        {   
             Graphics.DrawMesh(quadMesh,_center,Quaternion.identity,material,0);
        }
        
    }

    public void SetMesh(Vector3 start, Vector3 end)
    {
        Vector3[] verts = {new Vector3(start.x, start.y, 0),new Vector3(end.x, start.y, 0), 
            new Vector3(end.x, end.y, 0),new Vector3(start.x, end.y, 0)};
        quadMesh.vertices = verts;    
       
    }
}
