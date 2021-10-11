using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class UIMainPanel : UIBasePanel
{
    
    public static readonly string path = "Prefabs/UI/MainPanel";

    TextAsset luaText;
    public UIMainPanel() : base(new UIType(path))
    {
        luaText = Resources.Load<TextAsset>("Lua/UI/MainPanel.lua");
        if(luaText)InitializeUIBehaviour(true,new GraphyFW.UIBehavior(luaText.text,luaText.name));
    }

}
