using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace GraphyFW.UI
{
    public class UIAITestPanel : UIBasePanel
    {

        GameObject btns;

        public override void OnCreate()
        {
            btns = FindChild("Btns");
            InitClicks();

        }
        public override void OnEnter()
        {

        }

        private void InitClicks()
        {
            //开始搜索
            GetChildComponent<Button>(btns, "Btn_Search").onClick.AddListener(() =>
             {
                 Debug.Log("Btn_Search");
                 Scpt_GenMap.instance.SearchPath();
             });

            //清除搜索路劲
            GetChildComponent<Button>(btns, "Btn_Clear").onClick.AddListener(() =>
           {
                Debug.Log("Btn_Search");
                Scpt_GenMap.instance.ClearPath();
           });

            //重置地图
            GetChildComponent<Button>(btns, "Btn_ResetMap").onClick.AddListener(() =>
           {
                Debug.Log("Btn_Search");
                Scpt_GenMap.instance.ResetMap();
           });

            //重置目标位置
            GetChildComponent<Button>(btns, "Btn_SetSource").onClick.AddListener(() =>
           {
               Scpt_GenMap.instance.SetIsSetSourcePos(true);
           });

            //重置起始位置
            GetChildComponent<Button>(btns, "Btn_SetTarget").onClick.AddListener(() =>
           {
               Scpt_GenMap.instance.SetIsSetTargetPos(true);

           });

            //重置起始位置
            GetChildComponent<Button>(btns, "Btn_Back").onClick.AddListener(() =>
           {
               ScptSceneManger.instance.OpenScene(EScene.START);

           });
        }
    }

}
