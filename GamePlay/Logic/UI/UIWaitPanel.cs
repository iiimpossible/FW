using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GraphyFW.UI
{
    using UnityEngine.UI;
    public class UIWaitPanel : UIBasePanel
    {
        GameObject waitWidget;
        public override void OnCreate()
        {
            InitWidget();
            base.OnCreate();
        }

        public override void OnEnter()
        {
            base.OnEnter();
        }


        public void InitWidget()
        {
            waitWidget = FindChild("WaitWidget");

            GetChildComponent<Text>("Txt_Loading").text = "Generating map...";
        }


        


    }
}

