using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GraphyFW.UI
{
    /// <summary>
    /// 面板管理器，使用一个栈来管理所有的面板
    /// 注意：在lua环境下， 每个UI类型是一个脚本，在UI实例上挂载的组件上执行。
    /// 所以，如何将UI类型逻辑，于UI实例关联是一个问题，要不直接写组件算了。
    /// </summary>
    public class UIPanelManager
    {
        /// <summary>
        /// 存储已经激活UI面板
        /// </summary>
        private Stack<UIBasePanel> stackPanels = new Stack<UIBasePanel>();
 
        /// <summary>
        /// 根据UI类型的类型名，联系UI实例
        /// </summary>
        /// <typeparam name="System.Type"></typeparam>
        /// <typeparam name="GameObject"></typeparam>
        /// <returns></returns>
        private Dictionary<System.Type, UIBasePanel> dicUIPanel = new Dictionary<System.Type, UIBasePanel>();

        /// <summary>
        /// 记录已经出栈的上一个面板
        /// </summary>
        private UIBasePanel lastPanel;

        /// <summary>
        /// 构造函数
        /// </summary>
        public UIPanelManager() { }

        /// <summary>
        /// 1.将UIBasepanel进栈
        /// 2.从UIBasePanel中读取信息，获取UI预制体的路径，然后通过UIManager克隆预制体
        /// 3.将UI的创建于退出事件与面板管理器的出战入栈方法绑定
        /// </summary>
        /// <param name="nextPanel">要显示的面板 </param>
        public void OpenPanel(System.Type uiType)
        {
            //如果栈不为空，那么让栈顶Ui面板阻塞
            if (stackPanels.Count > 0) 
                stackPanels.Peek()?.OnPause();
            //因为面板实际上不是一个能够显示的UI，它必须到UI管理器中使用路径找到UI的Prefab，然后克隆一个UI，注册到UI管理器，并被插入到Canvas中
            UIBasePanel bpanel = OpenUI(uiType);
            if(bpanel != null) 
                stackPanels.Push(bpanel);
            //初始化Ui的脚本
            // if(nextPanel.uiType.executeLua)
            // {
            //     nextPanel.InitializeUIBehaviour(new GraphyFW.UIBehavior(nextPanel.uiType.luaCode,nextPanel.uiType.luaName,nextPanel));           
            // }  
            bpanel.uiGo?.SetActive(true);
            bpanel?.OnEnter();
            //事件注册
            // bpanel.OnExitUI += this.PopPanelCallBack;
            // bpanel.OnCreateUI += this.PushPanel;
        }

        /// <summary>
        /// 弹出位于栈顶的panel
        /// </summary>
        public void ClosePanel()
        {
            //注意：当栈顶弹出后，不确定栈中是否还有对象，所有要检测两次
            if (stackPanels.Count > 0)
            {
                UIBasePanel basePanel = stackPanels.Peek();
                if(basePanel != null)
                {
                    //basePanel.OnExit();
                    basePanel.uiGo.SetActive(false);
                    Debug.Log($"Pop ui [{stackPanels.Peek().uiGo.name}]");
                    stackPanels.Pop();
                }    
                else
                {
                    Debug.Log($"Pop ui failed. [{stackPanels.Peek().uiGo.name}]");
                } 
            }
            if (stackPanels.Count > 0)
            {
                UIBasePanel p = stackPanels.Peek();
                if(p != null)
                {
                    p.uiGo?.SetActive(true);
                    p.OnResume();
                }
                 
            }
        }

        /// <summary>
        /// 当关闭场景的时候，将当前的所有打开的UI销毁
        /// </summary>
        public void CloseScene()
        {             
            var array = dicUIPanel.Keys;
            foreach (var item in array)
            {
                GameObject.Destroy(dicUIPanel[item].uiGo);                        
            }
            dicUIPanel.Clear();
            stackPanels.Clear();
        }

        /// <summary>
        /// 执行位于栈顶的UIPanel的UPdate方法，避免在UI上挂载脚本以执行UPdate
        /// </summary>
        public void UpdateTopPanel()
        {
            if (stackPanels.Count > 0)
            {
                stackPanels.Peek().Update();
            }
        }

        // public void PopPanelCallBack(UIBasePanel panel)
        // {
        //     PopPanel();
        // }

        // public void PushPanelCallBack(UIBasePanel panel)
        // {
        //     PushPanel(panel);
        // }

        /// <summary>
        /// 根据UI类型，从字典种获取UI类型实例
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private UIBasePanel GetUIPanel(System.Type type)
        {
            if (dicUIPanel.ContainsKey(type))
            {
                return dicUIPanel[type];
            }
            return default(UIBasePanel);
        }




        /// <summary>
        /// 1.寻找Canvas对象
        /// 2.检测是否已经存在该类型的UI对象
        /// 3.如果不存在该类型的UI对象，从该类型的路径中生成一个,并给这个UI对象设置父对象
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public UIBasePanel OpenUI(System.Type type)
        {            
            GameObject canvas = GameObject.Find("Canvas");
            if (!canvas)
            {
                Debug.LogError("Canvas is Not Exist in current scene, please check that Canvas is created.");
                return null;
            }

            if (dicUIPanel.ContainsKey(type))
            {
                return dicUIPanel[type];
            }

            //注意：使用Resources.Load一定要在Resources文件夹下
            UIBasePanel bPanel = type.Assembly.CreateInstance("GraphyFW.UI."+type.Name) as UIBasePanel;
            if (bPanel == null)
            {
                Debug.LogError($"UIBasePanel create instance failed. Name: [{type.Name}]");
                return default(UIBasePanel);
            }
            //根据类型名称，从Prefabs/UI/ 中获取Ui对象实例
            GameObject prefab = Resources.Load<GameObject>($"Prefabs/UI/{type.Name}");
            if (!prefab)
            {
                Debug.LogError("UI prefab path is not valid.  Path: [Prefabs/UI/]" + type.Name);
                return null;
            }
            
            GameObject go = GameObject.Instantiate(prefab, canvas.transform);
            bPanel.uiGo = go;
            bPanel.OnCreate();//每个UI生命周期内（实例的时候）只调用一次
            dicUIPanel.Add(type, bPanel);
            Debug.Log($"Load ui: [{bPanel.GetType().Name}]");
            return bPanel;
        }

        /// <summary>
        /// 销毁一个对应类型的UI对象
        /// </summary>
        /// <param name="type"></param>
        public void DestroyUI(System.Type type)
        {
            if (dicUIPanel.ContainsKey(type))
            {
                GameObject.Destroy(dicUIPanel[type].uiGo);
                dicUIPanel.Remove(type);
            }
        }

        /// <summary>
        /// 如果不能通过反射实例对象，那么就用对象工厂
        /// </summary>
        /// <param name="uiType"></param>
        /// <returns></returns>
        public UIBasePanel UIFactory(string uiType)
        {
            switch (uiType)
            {
                case "UIGameSettingPanel":
                    {
                        return new UIGameSettingPanel();
                }
                default: 
                    return default(UIBasePanel);
            }
        }


    }

}

