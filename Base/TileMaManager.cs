using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

/// <summary>
/// 封装TileMap的一些操作,在MVC模式中，这其实是一个控制器（Controller），实际上的Model是TileMap与Grid
/// </summary>
public  class TileMaManager :Singleton<TileMaManager>
{    
    Tilemap m_curTileMap;
    Grid m_curGrid;
    public Tilemap curTileMap
    {
        get
        {
            return m_curTileMap;
        }
    }
    private TileMaManager()
    {
         GameObject go = ScriptableObjectManager.instance.GetPrefab("Grid");
         m_curTileMap = go.transform.GetComponentInChildren<Tilemap>();
         m_curGrid = go.transform.GetComponent<Grid>();

    } 



    public void GenRandomMap()
    {
        Tile tile = new Tile();
        tile.sprite =  ScriptableObjectManager.instance.GetSprite("White");
        tile.colliderType = Tile.ColliderType.Sprite;
        m_curTileMap.SetTile( new Vector3Int(0,0,0), tile);

    }
}

