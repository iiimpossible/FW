using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UIStartPanel : UIBasePanel
{
    static public readonly string path = "Prefabs/UI/Panels/StartPanel";
    public UIStartPanel() : base(new UIType(path)) { Debug.Log(uiType.uiName); }


    public override void OnEnter()
    {
        uiTool.GetOrAddComponentOfChildOfActivePanel<UnityEngine.UI.Button>("").onClick.AddListener(() =>
        {

        });
    }

    public override void OnExit()
    {
        base.OnExit();
    }


    public override void OnPause()
    {
        base.OnPause();
    }

    public override void OnResume()
    {
        base.OnResume();
    }
}
