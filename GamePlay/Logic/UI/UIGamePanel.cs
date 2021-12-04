using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

namespace GraphyFW.UI
{
    using UnityEngine.UI;
    public class UIGamePanel : UIBasePanel
    {
        //widgets
        //底部按钮列表部件
        GameObject buttomWidget;
        //巢穴部件
        GameObject nestWidget;

        //仓库信息部件
        GameObject storageWidget;

        //其他巢穴概览
        GameObject otherNestWidget;

        //选项按钮
        GameObject optionWidget;

        //当前地图中的动物部件
        GameObject AnimalsWidget;
        //contents

        //巢穴Scroll VIew 的Content
        GameObject nestListContent;
        //仓库的滚动列表Content
        GameObject storageListContent;

        //角色信息，当选中Actor时触发。以后要支持选中道具触发
        GameObject actorWidget;

        GameObject commandWidget;

        //点击tool命令时，显示toolbtns
        GameObject toolBtns;

        //右键菜单
        GameObject rightButtonMenuWidget;

        GameObject commandTipWidget;

        Text commandTipText;

        RectTransform commandTipWidgetRecttrans;

        GameObject lastWidget = null;

        //ActorWidget的工具按钮
        GameObject actorWidgetToolBtns;
        //ActorWidget 的信息按钮
        GameObject actorWidgetInfoBtns;
        //ActorWidget 上边的NeedWidget
        GameObject actorWidget_NeedWidget;


        private Vector3 commandTipOffset = Vector3.zero;
        private bool updateCommandTip = false;
        public override void OnCreate()
        {
            MessageManager.instance.AddListener(EMessageType.OnMouseButtonDown_0,LeftButtonDown_Listener);
            MessageManager.instance.AddListener(EMessageType.OnMouseButtonDown_1,RightButtonDown_Listener);
            MessageManager.instance.AddListener(EMessageType.OnMouseButtonDown_1,RayCastGetObject_Listener);
            MessageManager.instance.AddListener(EMessageType.OnBoxCast_Actors, SelectedActor_Listener);
            InitClicks();
            base.OnCreate();
        }

        public override void OnEnter()
        {
            nestWidget.SetActive(false);
            optionWidget.SetActive(false);
            storageWidget.SetActive(false);
            actorWidget.SetActive(false);
            commandWidget.SetActive(false);
            rightButtonMenuWidget.SetActive(false);
            commandTipWidget.SetActive(false);

             actorWidget_NeedWidget.SetActive(false);
            // foreach (var item in FindChildren(rightButtonMenuWidget.transform))
            // {
            //     item.gameObject.SetActive(false);
            //     Debug.Log(item.name);
            // }
            for(int i = 0; i< rightButtonMenuWidget.transform.childCount;i++)
            {
                rightButtonMenuWidget.transform.GetChild(i).gameObject.SetActive(false);
            }

            base.OnEnter();
        }


        public override void OnUpdate()
        {
            //TODO: 在指针位置出现一个命令指示图片或者文字
            if(updateCommandTip)
            {
                commandTipOffset.Set(40,-50,0);
                commandTipWidgetRecttrans.position = Input.mousePosition + commandTipOffset;
            }

            base.OnUpdate();
        }


        public void InitClicks()
        {
            buttomWidget = FindChild("ButtomMenuWidget");
            nestWidget = FindChild("NestWidget");
            optionWidget = FindChild("OptionWidget");
            storageWidget = FindChild("StorageWidget");
            actorWidget = FindChild("ActorWidget");
            commandWidget = FindChild("CommandWidget");
            rightButtonMenuWidget = FindChild("RightButtonMenuWidget");
            commandTipWidget = FindChild("CommandTipWidget");

            actorWidgetToolBtns = GetChildComponent<RectTransform>(actorWidget, "ToolBtns").transform.gameObject;
            actorWidgetInfoBtns = GetChildComponent<RectTransform>(actorWidget, "InfoBtns").transform.gameObject;
            actorWidget_NeedWidget = GetChildComponent<RectTransform>(actorWidget, "NeedsWidget").transform.gameObject;


            nestListContent = FindChild(nestWidget, "Content");
            toolBtns = FindChild(commandWidget, "ToolBtns");
            commandTipText = GetChildComponent<Text>(commandTipWidget,"Txt_CommandTip");
            commandTipWidgetRecttrans = commandTipWidget.GetComponent<RectTransform>();



            //buttomBtns widget
            GetChildComponent<Button>(buttomWidget, "Btn_Nest")?.onClick.AddListener(() =>
             {
                 SwitchWidget(nestWidget);
             });

            GetChildComponent<Button>(buttomWidget, "Btn_Storage")?.onClick.AddListener(() =>
             {
                 SwitchWidget(storageWidget);
             });

            GetChildComponent<Button>(buttomWidget, "Btn_OtherNest")?.onClick.AddListener(() =>
             {
                 Debug.Log("Btn_OtherNest Clicked.");
             });

            GetChildComponent<Button>(buttomWidget, "Btn_Animals")?.onClick.AddListener(() =>
             {
                 Debug.Log("Btn_Animals Clicked.");
             });

            GetChildComponent<Button>(buttomWidget, "Btn_Option")?.onClick.AddListener(() =>
             {
                 SwitchWidget(optionWidget);
             });

            GetChildComponent<Button>(buttomWidget, "Btn_Command").onClick.AddListener(() =>
            {
                SwitchWidget(commandWidget);
            });





            //nestWidget clicks
            GetChildComponent<Button>(nestWidget, "Btn_NW_Close")?.onClick.AddListener(() =>
             {
                 Debug.Log("Btn_NW_Close Clicked.");
                 SwitchWidget(null);
             });





            //optionWiget clicks
            GetChildComponent<Button>(optionWidget, "Btn_OW_Close")?.onClick.AddListener(() =>
             {
                 Debug.Log("Btn_OW_Close Clicked.");
                 SwitchWidget(null);
             });

            GetChildComponent<Button>(optionWidget, "Btn_Continue")?.onClick.AddListener(() =>
             {
                 Debug.Log("Btn_Continue Clicked.");
             });

            GetChildComponent<Button>(optionWidget, "Btn_Save")?.onClick.AddListener(() =>
          {
              Debug.Log("Btn_Save Clicked.");
          });

            GetChildComponent<Button>(optionWidget, "Btn_Load")?.onClick.AddListener(() =>
          {
              Debug.Log("Btn_Load Clicked.");
          });

            GetChildComponent<Button>(optionWidget, "Btn_Settings")?.onClick.AddListener(() =>
          {
              Debug.Log("Btn_Settings Clicked.");
          });

            GetChildComponent<Button>(optionWidget, "Btn_BackToMainMenu")?.onClick.AddListener(() =>
          {
              Debug.Log("Btn_BackToMainMenu Clicked.");
              ScptSceneManger.instance.OpenScene(EScene.START);
          });




            //storagewidget clicks
            GetChildComponent<Button>(storageWidget, "Btn_SW_Close")?.onClick.AddListener(() =>
            {
                Debug.Log("Btn_SW_Close Clicked.");
                SwitchWidget(null);
            });

            GetChildComponent<Button>(storageWidget, "Btn_Food")?.onClick.AddListener(() =>
             {
                 Debug.Log("Btn_Food Clicked.");
             });



            //CommandWidget clicks
            GetChildComponent<Button>(commandWidget, "Btn_Build").onClick.AddListener(() =>
            {
                Debug.Log("Btn_Build Clicked.");
            });

            
            GetChildComponent<Button>(commandWidget, "Btn_Tool").onClick.AddListener(() =>
            {


            });


            GetChildComponent<Button>(commandWidget, "Btn_Cancel").onClick.AddListener(() =>
            {
                GameMode.instance.SetPlayerCommand(GameMode.EPlayCommands.CANCEL);                 
                 OpenCommandTip("取消",true);
            });

            GetChildComponent<Button>(commandWidget, "Btn_CreateStorageArea").onClick.AddListener(() =>
            {
                GameMode.instance.SetPlayerCommand(GameMode.EPlayCommands.CREATE_STORAGE_AREA);               
                OpenCommandTip("存储区",true);
            });

            GetChildComponent<Button>(commandWidget, "Btn_CreateFood").onClick.AddListener(()=>
            {
                GameMode.instance.SetPlayerCommand(GameMode.EPlayCommands.CREATE_FOOD);                
                 OpenCommandTip("食物",true);
            });

            GetChildComponent<Button>(commandWidget, "Btn_CreateObstacle").onClick.AddListener(()=>
            {
                GameMode.instance.SetPlayerCommand(GameMode.EPlayCommands.CRATE_OBSTACLE);               
                OpenCommandTip("障碍物",true);
            });

            GetChildComponent<Button>(commandWidget, "Btn_CreateAnt").onClick.AddListener(()=>
            {
                GameMode.instance.SpawnAIObject();
                OpenCommandTip("工蚁",true);
            });

            GetChildComponent<Button>(commandWidget, "Btn_CreateEnemy").onClick.AddListener(()=>
            {
                GameMode.instance.SpanwEnemy();
                OpenCommandTip("敌人",true);
            });




            //Actor widget
            GetChildComponent<Button>(actorWidgetToolBtns,"Btn_CallUp").onClick.AddListener(()=>
            {
                GraphyFW.AI.MultiActorController.instance.ActorsCallUp();
            });

            GetChildComponent<Button>(actorWidgetInfoBtns,"Btn_Need").onClick.AddListener(()=>
            {
                actorWidget_NeedWidget.SetActive(true);
            });

            GetChildComponent<Button>(actorWidgetInfoBtns, "Btn_Property").onClick.AddListener(() =>
            {

            });

            GetChildComponent<Button>(actorWidget_NeedWidget, "Btn_AW_Close").onClick.AddListener(() =>
            {
                actorWidget_NeedWidget.SetActive(false);
            });





            //右键菜单
            GetChildComponent<Button>(rightButtonMenuWidget, "Btn_RBM_Carry").onClick.AddListener(() =>
            {
                SwitchWidget(null);
            });

            GetChildComponent<Button>(rightButtonMenuWidget, "Btn_RBM_Attack").onClick.AddListener(() =>

            {
                SwitchWidget(null);
            });

            GetChildComponent<Button>(rightButtonMenuWidget, "Btn_RBM_Eat").onClick.AddListener(() =>
            {
                SwitchWidget(null);
            });

        }

        private void SwitchWidget(GameObject nextWidget)
        {
            //lastWidget?.SetActive(false);
            if (lastWidget != null) lastWidget.SetActive(false);
            //nestWidget?.SetActive(true);
            if (nextWidget != null) nextWidget.SetActive(true);
            lastWidget = nextWidget;
        }

        /// <summary>
        /// 设置ActorWidget的值
        /// </summary>
        /// <param name="actors"></param>
        private void SetActorWidget(List<GameObject> actors)
        {
            if (actors.Count == 1)
            {
                GetChildComponent<Text>(actorWidget, "Txt_ActorName").text = actors[0].name;
            }
            else if (actors.Count > 1)
            {
                GetChildComponent<Text>(actorWidget, "Txt_ActorName").text = "Actors x" + actors.Count;
                GetChildComponent<Text>(actorWidget, "Txt_Decription").text = "";
            }

        }

        /// <summary>
        /// 根据射线检测到的对象，控制右键菜单显示
        /// </summary>
        /// <param name="hit"></param>
        private void OpenRightButtonMenu(Collider2D hit)
        {
            //将所有的按钮不显示
            for (int i = 0; i < rightButtonMenuWidget.transform.childCount; i++)
            {
                rightButtonMenuWidget.transform.GetChild(i).gameObject.SetActive(false);
            }
            switch (hit.tag)
            {
                case "Ant":
                    {
                        FindChild(rightButtonMenuWidget,"Btn_RBM_Attack").SetActive(true);
                        break;
                    }
                case "Food":
                    {
                        SwitchWidget(rightButtonMenuWidget);
                        rightButtonMenuWidget.GetComponent<RectTransform>().position = Input.mousePosition;
                        FindChild(rightButtonMenuWidget, "Btn_RBM_Eat").SetActive(true);
                        FindChild(rightButtonMenuWidget, "Btn_RBM_Carry").SetActive(true);
                        break;
                    }
            }
        }

        /// <summary>
        /// 设置命令提示
        /// </summary>
        /// <param name="tip"></param>
        private void OpenCommandTip(string tip,bool open = true)
        {             
            commandTipWidget.SetActive(open);
            commandTipText.text = tip;
            updateCommandTip = open;
        }


        /// <summary>
        /// 当左键或者右键按下，把当前打开的widget都关掉
        /// </summary>
        /// <param name="message"></param>
        private void LeftButtonDown_Listener(Message message)
        {
            SwitchWidget(null);           
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        private void RightButtonDown_Listener(Message message)
        {
            OpenCommandTip("",false);
            SwitchWidget(null);         
        }

        /// <summary>
        /// 右键触发，在鼠标位置发射射线，检测物体类型以进行下一步动作
        /// </summary>
        /// <param name="message"></param>
        private void RayCastGetObject_Listener(Message message)
        {
            Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            var hits = Physics2D.Raycast(pos, Vector3.forward);
            if (hits.collider != null)
            {
                OpenRightButtonMenu(hits.collider);
            }
        }

        /// <summary>
        /// 监听框选物体，将它们设置为被选中
        /// </summary>
        /// <param name="message"></param>
        private void SelectedActor_Listener(Message message)
        {
            if(GameMode.instance.selctedGameObjectList.Count == 0) return;
            foreach (var item in GameMode.instance.selctedGameObjectList)
            {
                item.GetComponent<GraphyFW.AI.ActorController>().Selected(true);             
            }
            SwitchWidget(actorWidget);
            SetActorWidget(GameMode.instance.selctedGameObjectList);
        }

        /// <summary>
        /// 清理回调函数等
        /// </summary>
        public override void OnDestroy()
        {
            MessageManager.instance.RemoveListener(EMessageType.OnMouseButtonDown_0, LeftButtonDown_Listener);
            MessageManager.instance.RemoveListener(EMessageType.OnMouseButtonDown_1, RightButtonDown_Listener);
            MessageManager.instance.RemoveListener(EMessageType.OnMouseButtonDown_1, RayCastGetObject_Listener);
            MessageManager.instance.RemoveListener(EMessageType.OnBoxCast_Actors, SelectedActor_Listener);
            base.OnDestroy();
        }

    }


    




}
