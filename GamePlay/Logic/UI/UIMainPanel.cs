using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


namespace GraphyFW.UI
{
    [System.Serializable]
    public class UIMainPanel : UIBasePanel
    {
        //要热更的话，需要把这些独特的定义都写在Lua中，不能够继承UIBase，应该是UIBase绑定一个Lua文件。
        public static readonly string path = "Prefabs/UI/UIMainPanel";
        //public static readonly string luaPath = "Lua/UI/MainPanel.lua";

        /// <summary>
        /// 怎么把构造函数禁掉？
        /// </summary>
        public UIMainPanel(){}

        public override void OnEnter()
        {
            base.OnEnter();
            InitClicks();
        }


        public override void OnPause()
        {
            base.OnPause();
        }

        public override void OnExit()
        {
            base.OnExit();
        }

        public override void OnResume()
        {
            base.OnResume();
        }


        /// <summary>
        /// 初始化点击事件
        /// </summary>
        private void InitClicks()
        {
            GetChildComponentAnyway<Button>("Btn_Start").onClick.AddListener(() =>
            {
                Debug.Log("Start clicked!");
            //TODO：打开游戏场景
            //TODO: 显示载入中
            //TODO；显示地图
            //SceneManager.LoadScene("Sce_Game");
            ScptUIManager.instance.uiPanelManager.OpenPanel(typeof(UIGameSettingPanel));
        });
            GetChildComponentAnyway<Button>("Btn_Load").onClick.AddListener(() =>
            {
                Debug.Log("Load Clicked!");
            });
            GetChildComponentAnyway<Button>("Btn_Option").onClick.AddListener(() =>
            {
                Debug.Log("Option Clicked!");
            });

            GetChildComponentAnyway<Button>("Btn_Quit").onClick.AddListener(() =>
            {
                Debug.Log("Quit Clicked!");
            });

             GetChildComponentAnyway<Button>("Btn_Test").onClick.AddListener(() =>
            {
               ScptSceneManger.instance.OpenScene(EScene.AITEST);
            });
        }

    }

}
