using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct NamedPrefab
{
    public string name;
    public GameObject prefab;
}

/// <summary>
/// 加载所有的预制体，并且将其储存起来，不必二次加载
/// </summary>
public class PrefabManager : MonoBehaviour
{
    public static PrefabManager instance {get;private set;}

    [SerializeField]
    [Tooltip("Loaded prefabs")]
    //private Dictionary<string,GameObject> m_prefabs = new Dictionary<string,GameObject>();
    private List<NamedPrefab> m_prefabs = new List<NamedPrefab>();

    private void Awake() {
        instance = this;
    }
  
    /// <summary>
    /// 通过Prefab的名字，从Resource/Prefabs下面加载Prefab
    /// </summary>
    /// <param name="path"></param>
    public GameObject LoadPrefab(string path)
    {
        foreach (var item in m_prefabs)
        {
            if( item.name == path)
            {
                return item.prefab;
            }
        }

        GameObject p =  Resources.Load<GameObject>(path);
        if(p == null)
        {
            Debug.LogError("PrefabManager: Load prefab error, prefab name not valid.  path is: " + path);
            return null;
        }
        NamedPrefab namedPrefab;
        namedPrefab.name = path.Substring(path.LastIndexOf('/') + 1) ;
        namedPrefab.prefab = p;
        m_prefabs.Add(namedPrefab);
        return p;
    }

}
