using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class UIMainPanel : UIBasePanel
{    
    public static readonly string path = "Prefabs/UI/MainPanel";

    public static readonly string luaPath = "Lua/UI/MainPanel.lua";
    
    public UIMainPanel():base(new UIType(path,true,luaPath))
    {      
       
    }    

}
