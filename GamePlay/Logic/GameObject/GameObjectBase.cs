using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GraphyFW.GamePlay
{
    public class GameObjectBase
    {
        public GameObject instance { get; private set; }

        public Vector2Int mapPos{get;set;}

        public GameObjectBase()
        {
             
        }

        public void SetGO(GameObject go,Vector2Int pos)
        {
            instance = go;
            mapPos = pos;
        }
    }

}
