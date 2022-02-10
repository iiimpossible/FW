using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 道具基类，
/// 1.是否被占用
/// 2.是否在存储区，哪个存储区
/// 3.对应的Game Object
/// 4.在地图中的位置
/// 
/// </summary>
//[UnityEditor.CustomEditor(typeof(Prop),true)]
[System.Serializable]
public class Prop : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Whether the prop is occupied by actor.")]
    private bool m_isOccupied = false;

    [SerializeField]
    [Tooltip("Whether the prop is stored.")]
    private bool m_isStored = false;

    [SerializeField]
    [Tooltip("The map space coordinate.")]
    private Vector2Int m_mapPos = Vector2Int.zero;

    public bool isOccupied { get { return m_isOccupied; } set {m_isOccupied = value; } }
    public bool isStored { get { return m_isStored; } set {m_isStored = value;} }
    public Vector2Int mapPos { get { return GameMode.instance.GetMap().WorldSpaceToMapSpace(transform.position); } set {m_mapPos = value; } }


    private void Awake()
    {

    }

    
}
