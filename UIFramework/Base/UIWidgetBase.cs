using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GraphyFW.UI
{
    /// <summary>
    /// UIWidget 为UIPanel的下一个层级，具有其独特的数据更新方法，防止UI panel的逻辑过于复杂导致难以管理
    /// 1.进入、退出方法
    /// 2.游戏物体绑定
    /// 3.Widget的管理应该属于UIpanel，widget的管理不固定，因此，管理逻辑属于Wiget的Panel
    /// </summary>
    public abstract class UIWidgetBase
    {
        public GameObject widgetGo {get;set;}

        public UIBasePanel parentPanel{get;set;}
 
        //public abstract void OnCreate();

        private bool isInited = false;

        public virtual void OnCreate()
        {
            
        }

        public virtual void OnEnter()
        {
            if(isInited == false)
            {
                Debug.LogError("UIWidget: Widget not init.");
            }
        }


        public virtual void OnExit()
        {
            
        }

        /// <summary>
        /// 初始化widget
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="widget"></param>
        public void InitWidget(UIBasePanel parent, GameObject widget)
        {
            this.widgetGo = widget;
            this.parentPanel = parent;
            isInited = true;
        }

 
    }

}
