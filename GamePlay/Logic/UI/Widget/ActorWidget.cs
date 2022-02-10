using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using GraphyFW.UI;

public class ActorWidget : UIWidgetBase
{
    //需要将NeedWidget弄过来
    NeedWidget needWidget;

    //ActorWidget的工具按钮
    GameObject actorWidgetToolBtns ;
    //ActorWidget 的信息按钮
    GameObject actorWidgetInfoBtns;


    public override void OnCreate()
    {
        base.OnCreate();
        InitClicks();
    }
    public override void OnEnter()
    {
        base.OnEnter();

    }


    public override void OnExit()
    {
        parentPanel.CloseWidget(needWidget);
        base.OnExit();
    }


    private void InitClicks()
    {

        needWidget = parentPanel.BindWidget<NeedWidget>(widgetGo, typeof(NeedWidget));
        parentPanel.CloseWidget(needWidget);


        actorWidgetToolBtns = parentPanel.GetChildComponent<RectTransform>(widgetGo, "ToolBtns").transform.gameObject;
        actorWidgetInfoBtns = parentPanel.GetChildComponent<RectTransform>(widgetGo, "InfoBtns").transform.gameObject;

        //Actor widget
        parentPanel.GetChildComponent<Button>(actorWidgetToolBtns, "Btn_CallUp").onClick.AddListener(() =>
         {
             GraphyFW.AI.MultiActorController.instance.ActorsCallUp();
         });

        parentPanel.GetChildComponent<Button>(actorWidgetInfoBtns, "Btn_Need").onClick.AddListener(() =>
         {
             parentPanel.OpenWidget(needWidget);
             if (GameMode.instance.selctedGameObjectList.Count == 1)
             {
                 needWidget.UpdateNeedInfo(GameMode.instance.selctedGameObjectList[0]);
             }

         });

        parentPanel.GetChildComponent<Button>(actorWidgetInfoBtns, "Btn_Property").onClick.AddListener(() =>
        {

        });
    }



}
