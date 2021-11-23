using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace GraphyFW.UI
{
    using UnityEngine.UI;
    public class UIGamePanel : UIBasePanel
    {
        //widgets
        GameObject buttomWidget;
        GameObject nestWidget;
        GameObject storageWidget;
        GameObject otherNestWidget;
        GameObject optionWidget;
        GameObject AnimalsWidget;
        //contents
        GameObject nestListContent;
        GameObject storageListContent;

        GameObject lastWidget = null; 
        public override void OnCreate()
        {
            InitClicks();
            base.OnCreate();
        }

        public override void OnEnter()
        {
            nestWidget.SetActive(false);
            optionWidget.SetActive(false);
            storageWidget.SetActive(false);
            base.OnEnter();
        }


        public void InitClicks()
        {
            buttomWidget = FindChild("ButtomMenuWidget");
            nestWidget = FindChild("NestWidget");
            optionWidget = FindChild("OptionWidget");
            storageWidget = FindChild("StorageWidget");

            nestListContent = FindChild(nestWidget,"Content");

            //buttomBtns widget
            GetChildComponent<Button>(buttomWidget,"Btn_Nest")?.onClick.AddListener(()=>
            {          
                 SwitchWidget(nestWidget);
            });

            GetChildComponent<Button>(buttomWidget,"Btn_Storage")?.onClick.AddListener(()=>
            {  
                 SwitchWidget(storageWidget);
            });

            GetChildComponent<Button>(buttomWidget,"Btn_OtherNest")?.onClick.AddListener(()=>
            {
                Debug.Log("Btn_OtherNest Clicked.");
            });

            GetChildComponent<Button>(buttomWidget,"Btn_Animals")?.onClick.AddListener(()=>
            {
                Debug.Log("Btn_Animals Clicked.");
            });

            GetChildComponent<Button>(buttomWidget,"Btn_Option")?.onClick.AddListener(()=>
            {                         
                SwitchWidget(optionWidget);
            }); 

          

            //nestWidget clicks
            GetChildComponent<Button>(nestWidget,"Btn_NW_Close")?.onClick.AddListener(()=>
            {  
                 Debug.Log("Btn_NW_Close Clicked.");
                SwitchWidget(null);
            });


            //optionWiget clicks
            GetChildComponent<Button>(optionWidget,"Btn_OW_Close")?.onClick.AddListener(()=>
            {
                Debug.Log("Btn_OW_Close Clicked.");
                 SwitchWidget(null);
            });

            GetChildComponent<Button>(optionWidget,"Btn_Continue")?.onClick.AddListener(()=>
            {
                Debug.Log("Btn_Continue Clicked.");
            });

               GetChildComponent<Button>(optionWidget,"Btn_Save")?.onClick.AddListener(()=>
            {
                Debug.Log("Btn_Save Clicked.");
            });

               GetChildComponent<Button>(optionWidget,"Btn_Load")?.onClick.AddListener(()=>
            {
                Debug.Log("Btn_Load Clicked.");
            });

               GetChildComponent<Button>(optionWidget,"Btn_Settings")?.onClick.AddListener(()=>
            {
                Debug.Log("Btn_Settings Clicked.");
            });

               GetChildComponent<Button>(optionWidget,"Btn_BackToMainMenu")?.onClick.AddListener(()=>
            {
                Debug.Log("Btn_BackToMainMenu Clicked.");
                ScptSceneManger.instance.OpenScene(EScene.START);
            });

            //storagewidget clicks
             GetChildComponent<Button>(storageWidget,"Btn_SW_Close")?.onClick.AddListener(()=>
            {
                Debug.Log("Btn_SW_Close Clicked.");
                SwitchWidget(null);
            });

            GetChildComponent<Button>(storageWidget,"Btn_Food")?.onClick.AddListener(()=>
            {
                Debug.Log("Btn_Food Clicked.");
            });

            
    }

    private void SwitchWidget(GameObject nextWidget)
    { 
        //lastWidget?.SetActive(false);
        if(lastWidget != null) lastWidget.SetActive(false);
        //nestWidget?.SetActive(true);
        if(nextWidget != null) nextWidget.SetActive(true);
        lastWidget = nextWidget;
    }

    }



}
