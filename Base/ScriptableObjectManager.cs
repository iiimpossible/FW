using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EMappingType 
{
    FunctionPrefab,
    Texture,
    UIPrefab
}

[System.Serializable]
public struct SpriteElement
{
    public string name;
    public Sprite sprite;
}

[System.Serializable]
public struct PrefabElement
{
    public string name;
    public GameObject prefab;
}

[System.Serializable]
public struct UIPrefabElement
{
    public string name;
    public GameObject prefab;
}



[System.Serializable]
public class ScriptableObjectManager :SingletonMono<ScriptableObjectManager>
{     
    [SerializeField] private List<PrefabElement> list_Prefabs = new List<PrefabElement>();
    [SerializeField] private  List<SpriteElement> list_sprites = new List<SpriteElement>();  
    [SerializeField] private List <UIPrefabElement> list_UI = new List<UIPrefabElement>();
    void Start()
    {
        
    }

     
    void Update()
    {
        
    }

    public GameObject GetPrefab(string name)
    {
        return list_Prefabs.Find(a => a.name == name).prefab;
    }

    public Sprite GetSprite(string name)
    {
        return list_sprites.Find(x => x.name == name).sprite;
    }
}
