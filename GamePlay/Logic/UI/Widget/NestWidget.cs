using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using GraphyFW.UI;

/// <summary>
/// NestWidget 管理当前巢穴中的工蚁、蚁后、兵蚁、幼虫等
/// </summary>
public class NestWidget : UIWidgetBase
{
    
    GameObject nestWidgetItemPrefab;

    GameObject itemContent;

    public override void OnCreate()
    {        
        base.OnCreate();
        InitClicks();
        nestWidgetItemPrefab = PrefabManager.instance.LoadPrefab("Prefabs/UI/Items/UINestWidgetItem");
    }

    public override void OnEnter()
    {
        base.OnEnter();
    }

    public override void OnExit()
    {
        base.OnExit();
    }

    private void InitClicks()
    {
        itemContent = parentPanel.FindChild(widgetGo, "Content");

        parentPanel.GetChildComponent<Button>(widgetGo, "Btn_Close").onClick.AddListener(()=>
        {
            UIGamePanel p =  parentPanel as UIGamePanel;
            p.SwitchWidget(null);
            //parentPanel.CloseWidget(this);
        });
    }

    /// <summary>
    /// 添加条目
    /// </summary>
    /// <param name="actor"></param>
    public void AddItem(GameObject actor)
    {
        var item = GameObject.Instantiate<GameObject>(nestWidgetItemPrefab,itemContent.transform.localPosition,Quaternion.identity);
        item.transform.SetParent(itemContent.transform);
        parentPanel.GetChildComponent<Text>(item, "Txt_Label").text = actor.name;
        parentPanel.GetChildComponent<Text>(item, "Txt_Value").text = actor.GetComponent<GraphyFW.GamePlay.ControlledActor>().health.ToString();
    }

    public void RemoveItem()
    {

    }

}
