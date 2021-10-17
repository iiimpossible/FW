using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class UIMainPanel : UIBasePanel
{    
    //要热更的话，需要把这些独特的定义都写在Lua中，不能够继承UIBase，应该是UIBase绑定一个Lua文件。
    public static readonly string path = "Prefabs/UI/MainPanel";

    public static readonly string luaPath = "Lua/UI/MainPanel.lua";
    
    public UIMainPanel():base(new UIType(path,true,luaPath))
    {      
       
    }    

}
