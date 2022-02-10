using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
 

using GraphyFW.UI;

/// <summary>
/// 这个是GamePanel的ActorWiget的NeedWidget，
/// 显示Actor的需求信息
/// </summary>
public class NeedWidget : UIWidgetBase
{     
    //营养进度条
    GameObject nutritionBar;
    //精力进度条
    GameObject vigorBar;

    Slider nSlider;

    Slider vSlider;
    /// <summary>
    /// 当widget实例时调用
    /// </summary>
    public override void OnCreate()
    {
        GameObject nutritionBar = parentPanel.FindChild(widgetGo, "PrgBar_Nutrition");
        nSlider = parentPanel.GetChildComponent<Slider>(nutritionBar,"Slider");

        GameObject vigorBar = parentPanel.FindChild(widgetGo, "PrgBar_Vigor");
        vSlider = parentPanel.GetChildComponent<Slider>(vigorBar,"Slider");

        
         InitClicks();
    }

    /// <summary>
    /// 初始化点击事件
    /// </summary>
    public void InitClicks()
    {
        parentPanel.GetChildComponent<Button>(widgetGo,"Btn_NW_Close").onClick.AddListener(()=>
        {
            widgetGo.SetActive(false);
        });
    }


    /// <summary>
    /// 从Actor的组件中获取信息，更新Widget
    /// </summary>
    /// <param name="actor"></param>
    public void UpdateNeedInfo(GameObject actor)
    {
        var comp = actor.GetComponent<GraphyFW.GamePlay.ControlledActor>();
        nSlider.minValue = 0;
        nSlider.maxValue = 100f;
        vSlider.minValue = 0;
        vSlider.maxValue = 100f;
        nSlider.value  = comp.nutrition;
        vSlider.value = comp.vigor;
    }
}
