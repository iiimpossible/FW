using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class UIGameSettingPanel : UIBasePanel
{
    private static readonly string info =
    @"<color=white><size=50>小黄家蚁</size>
<size=30> 描述：
小黄家蚁，Monomoriumpharaonis(Linnaeus,1758)，中文正式名为“法老小家蚁”，属昆虫纲、膜翅目、蚁科、切叶蚁亚科，小家蚁属的一种蚂蚁。
体型：
	小
特性：
真菌农场：可以将植物的叶子切割下来运进洞穴培育真菌，从而以真菌为食物
分化：

    雄蚁：普通
    工蚁：切叶，真菌种植
    蚁后：普通
</size>
</color>";

    public static readonly string path = "Prefabs/UIGameSettingPanel";
    public UIGameSettingPanel() : base(new UIType(path))
    {

    }

    public override void OnEnter()
    {


        base.OnEnter();
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

    public void InitClicks()
    {

    }


    /// <summary>
    /// 获取蚂蚁类型列表，将当前定义的蚂蚁列表显示
    /// 1.获取content
    /// 2.读取蚂蚁类型，然后实例对应的item，填充到content里边
    /// </summary>
    private void InitAntTypeList()
    {
        GameObject content = uiTool.FindChild("AntTypeWidget/List_AnyType/Viewport/Content");
        //TODO: 实例Item到content下边去
        
    }
}
