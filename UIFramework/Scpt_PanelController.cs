using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
using System;

namespace GraphyFW
{
    namespace UIFW
    {
        /// <summary>
        /// @TODO
        ///     1.反射获取已经定义好的具体的UIPanel
        ///     2.在该组件Inspector上提供一个按钮，点击显示上述定义好的UIPanel，并能够将要实例的Panel注册到该脚本
        ///     3.在写死UIPanel关联路径（Assets/Resources/Prefabs/UI/Panels）
        ///     4.反射获取定义好的UIPanel的类名作为UIType.name初始化值
        ///     5.UI框架和场景管理的交互
        /// @details 该类是一个脚本挂载到一个全局实例上，管理所有与UI相关的逻辑，并提供UI的Update
        /// </summary>
        public class Scpt_PanelController : MonoBehaviour
        {
            private UIPanelManager panelManager;

            ///包含所有要实例的UIPanel的类型
            private List<UIBasePanel> basePanels;
            private void Awake()
            {
                //实例化UI面板管理器
                panelManager = new UIPanelManager();
            }

            
            void Start()
            {
                //将UI推入栈，并将UI类与UI实例关联
                panelManager.PushPanel(new UIMainPanel());
            }

           
           //每帧更新PanelManager的顶部的面板逻辑
            void Update()
            {
                panelManager.UpdateTopPanel();
            }

            ///将特定的UIPanel类型注册到列表中去
            public void RigisterPanel()
            {
            }

            ///从反射信息中获取所有已经被标记Attribute的UIPanel
            private void GetPanelTypes()
            {
                //Type traget_type = typeof(UIPanelManager);
                //System.Type[] ts = ReflectionProgressFunc.GetClassInTargetNameSpace(traget_type, traget_type.Namespace);

                //foreach (System.Type t in ts)
                //{
                //    bool isdef = Attribute.IsDefined(t,
                //       typeof(WidgetStrategyAttribute));
                //    if (isdef)
                //    {
                //        var target_attrs = t.GetCustomAttribute<WidgetStrategyAttribute>();

                //        if (target_attrs.attributeTag == WidgetStrategyAttributeTag.NormalField)
                //        {
                //            strategyDic.Add(target_attrs.valueTypes[0], System.Activator.CreateInstance(target_attrs.widgetType) as InputStrategy);
                //        }
                //        //如果是ObjectField，就遍历该类的所有值类型，并生成对应的实例
                //        else if (target_attrs.attributeTag == WidgetStrategyAttributeTag.ObjectField)
                //        {
                //            foreach (Type value_type in target_attrs.valueTypes)
                //            {
                //                strategyDic.Add(value_type, new ObjectInputStrategy(value_type));
                //            }
                //        }
                //    }
                //}
            }
        }

    }
}
