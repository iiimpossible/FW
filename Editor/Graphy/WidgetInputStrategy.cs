using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEditor;
using System.Reflection;

using MyEiditorWidget.MyAttribute;
namespace MyEiditorWidget
{
    namespace FieldStrategy
    {

        //策略模式

        //策略基类
        public abstract class InputStrategy
        {
            /**
             * @description:
             * 1.遍历所有的组件，生成目标属性的Field
             * 2.生成一个修改所有Field的全局Field
             * 3.给全局Field添加一个回调函数，修改所有的目标属性的Field  
             * @param List<Component> components 所有的目标组件集合
             * @param string PropName 目标属性的名字
             * @param EHorizontalLayout inputField 全局Field所属的水平布局
             * @param EScrollView fieldContent 所有目标属性Field的滚动视口
             */
            virtual public void Call(List<Component> components, string PropName, EHorizontalLayout inputField, EScrollView fieldContent)
            {
            }

            /**
             * @description: 
             * @param {Component} 组件用于获取名字和目标属性值
             * @return {*}
             * 这里泛型需要框框的基类有一个通用的value来存储值，那么这个value类型必须为Object，用一个属性在
             * 对应的框框子类里边string value{get{return oringinValue as string;} set{oringinValue = value;}} 
             * 需要改的太多
             * 泛型：基类EField限定，无参构造函数限定，目标值类型限定
             */
            protected EWidget CreateWidget<FieldType, ValueType>(Component comp, string propName) where FieldType : EField<ValueType>, new()
            {
                FieldType field = new FieldType();
                string name = comp.transform.parent?.name +"."+ comp.transform.name;
                field.label = name;
                field.guiContent.text = name;
                //field.value = comp.GetType().GetProperty(name).GetValue(comp) as C;
                object o = comp.GetType().GetProperty(propName).GetValue(comp);
                if (o is ValueType && o != null)
                {
                    field.value = (ValueType)o;
                }
                field.isRewrite = true;
                field.eventOnValueChange += (ValueType value) =>
                {
                    try
                    {
                        comp.GetType().GetProperty(propName).SetValue(comp, value);
                    }
                    catch (ArgumentException e)
                    {
                        Debug.Log("It is possible property READONLY! \n" + e);
                    }
                    //Debug.LogWarning("Successssss!");
                };
                return field;
            }
            /**
             * @description: 
             * @param {*}
             * @return {*}
             * 修改所有组件的目标属性的值
             */
            protected void ModifyAllComponentsValue<FieldType, ValueType>(List<Component> components, EScrollView fieldContent,
                string PropName, ValueType value) where FieldType : EField<ValueType>
            {
                //修改所有目标组件的值
                for (int i = 0; i < components.Count; i++)
                {
                    //目标组件的目标string类型的值改变
                    try
                    {
                        components[i].GetType().GetProperty(PropName).SetValue(components[i], value);//这里竟然可以直接访问，每个匿名方法都记录了对应的comp吗？
                    }
                    catch (ArgumentException e)
                    {
                        Debug.Log(e);
                    }
                    FieldType t = fieldContent.GetWidget(i) as FieldType;
                    t.value = value;
                }
            }

        }


        //整型
        [MyAttribute.WidgetStrategyAttribute(typeof(IntInputStrategy),new System.Type[] {typeof(Int32)})]
        public class IntInputStrategy : InputStrategy
        {
            public override void Call(List<Component> components, string PropName, EHorizontalLayout inputField, EScrollView fieldContent)
            {
                foreach (Component comp in components)
                {
                    fieldContent.AddWidget(CreateWidget<EIntField, int>(comp, PropName));
                }
                inputField.ClearWidget();
                EIntField w = new EIntField("Input");
                inputField.AddWidget(w);
                w.eventOnValueChange += (int value) =>
                  {
                      ModifyAllComponentsValue<EIntField, int>(components, fieldContent, PropName, value);
                  };
            }
        }

        //浮点
        [MyAttribute.WidgetStrategyAttribute(typeof(FloatInputStrategy),new System.Type[]  {typeof(float) })]
        public class FloatInputStrategy : InputStrategy
        {
            public override void Call(List<Component> components, string PropName, EHorizontalLayout inputField, EScrollView fieldContent)
            {
                foreach (Component comp in components)
                {
                    fieldContent.AddWidget(CreateWidget<EFloatField, float>(comp, PropName));
                }
                inputField.ClearWidget();
                EFloatField w = new EFloatField("Input");
                inputField.AddWidget(w);
                w.eventOnValueChange += ((float value) =>
                  {
                      ModifyAllComponentsValue<EFloatField, float>(components, fieldContent, PropName, value);
                  });
            }
        }

        //文本
        [MyAttribute.WidgetStrategyAttribute(typeof(TextInputStrategy),new System.Type[] {typeof(string)})]
        public class TextInputStrategy : InputStrategy
        {
            public override void Call(List<Component> components, string PropName, EHorizontalLayout inputField, EScrollView fieldContent)
            {

                //遍历所有组件，生成Field、
                foreach (Component comp in components)
                {
                    fieldContent.AddWidget(CreateWidget<ETextField, string>(comp, PropName));//ETextField field = CreateWidget<ETextField, string>(comp, PropName) as ETextField;
                }
                //生成全局输入文本框
                inputField.ClearWidget();
                ETextField w = new ETextField("Input");
                inputField.AddWidget(w);
                //修改全局属性值
                w.eventOnValueChange += ((string value) =>
                  {
                      ModifyAllComponentsValue<ETextField, string>(components, fieldContent, PropName, value);
                  });
            }
        }

        //布尔
        [MyAttribute.WidgetStrategyAttribute(typeof(BoolInputStrategy),new System.Type[]  {typeof(bool)})]
        public class BoolInputStrategy : InputStrategy
        {
            public override void Call(List<Component> components, string PropName, EHorizontalLayout inputField, EScrollView fieldContent)
            {
                //遍历所有组件，生成Field、
                foreach (Component comp in components)
                {
                    fieldContent.AddWidget(CreateWidget<EToggle, bool>(comp, PropName));
                }

                inputField.ClearWidget();
                EToggle toggle = new EToggle("Input");
                inputField.AddWidget(toggle);
                toggle.eventOnValueChange += ((bool value) =>
                  {
                      ModifyAllComponentsValue<EToggle, bool>(components, fieldContent, PropName, value);
                  });
            }
        }


        [MyAttribute.WidgetStrategyAttribute(typeof(ColorInputStrategy),new System.Type[]  {typeof(Color)})]
        public class ColorInputStrategy : InputStrategy
        {
            public override void Call(List<Component> components, string PropName, EHorizontalLayout inputField, EScrollView fieldContent)
            {
                foreach (Component comp in components)
                {
                    fieldContent.AddWidget(CreateWidget<EColorField, Color>(comp, PropName));
                }
                inputField.ClearWidget();
                EColorField field = new EColorField("Input");
                inputField.AddWidget(field);
                field.eventOnValueChange += ((Color value) =>
                  {
                      ModifyAllComponentsValue<EColorField, Color>(components, fieldContent, PropName, value);
                  });
            }
        }

        [MyAttribute.WidgetStrategyAttribute(typeof(Vector3InputStrategy),new System.Type[]  {typeof(Vector3)})]
        public class Vector3InputStrategy : InputStrategy
        {
            public override void Call(List<Component> components, string PropName, EHorizontalLayout inputField, EScrollView fieldContent)
            {
                foreach (Component comp in components)
                {
                    fieldContent.AddWidget(CreateWidget<EVector3Field, Vector3>(comp, PropName));
                }
                inputField.ClearWidget();
                EVector3Field toggle = new EVector3Field("Input");
                inputField.AddWidget(toggle);
                toggle.eventOnValueChange += ((Vector3 value) =>
                  {
                      ModifyAllComponentsValue<EVector3Field, Vector3>(components, fieldContent, PropName, value);
                  });
            }
        }

        [MyAttribute.WidgetStrategyAttribute(typeof(Vector2InputStragety),new System.Type[]  {typeof(Vector2)})]

        public class Vector2InputStragety :InputStrategy
        {
            public override void Call(List<Component> components, string PropName, EHorizontalLayout inputField, EScrollView fieldContent)
            {
                  foreach (Component comp in components)
                {
                    fieldContent.AddWidget(CreateWidget<EVector2Field, Vector2>(comp, PropName));
                }
                inputField.ClearWidget();
                EVector2Field toggle = new EVector2Field("Input");
                inputField.AddWidget(toggle);
                toggle.eventOnValueChange += ((Vector2 value) =>
                  {
                      ModifyAllComponentsValue<EVector2Field, Vector2>(components, fieldContent, PropName, value);
                  });
            }
        }

        

        [MyAttribute.WidgetStrategyAttribute(typeof(ObjectInputStrategy),new System.Type[] {typeof(Sprite),typeof(Material),typeof(Texture2D)}, WidgetStrategyAttributeTag.ObjectField)]
        public class ObjectInputStrategy : InputStrategy
        {
            private Type m_objType;
            public ObjectInputStrategy(Type objectType)
            {
                m_objType = objectType;
            }
            public override void Call(List<Component> components, string PropName, EHorizontalLayout inputField, EScrollView fieldContent)
            {
                foreach (Component comp in components)
                {
                    EObjectField field = new EObjectField(comp.transform.name, m_objType);
                    field.value = comp.GetType().GetProperty(PropName).GetValue(comp) as UnityEngine.Object;
                    fieldContent.AddWidget(field);

                    field.eventOnValueChange += ((UnityEngine.Object t) =>
                       {
                           try
                           {
                               comp.GetType().GetProperty(PropName).SetValue(comp, t);//这里竟然可以直接访问，每个匿名方法都记录了对应的comp吗？
                           }
                           catch (ArgumentException e)
                           {
                               Debug.Log("It is possible property READONLY! \n" + e);
                           }
                       });
                    // fieldContent.AddWidget(CreateWidget<EObjectField,UnityEngine.Object>(comp, PropName));
                }

                inputField.ClearWidget();
                EObjectField f = new EObjectField("Input", m_objType);
                inputField.AddWidget(f);
                f.eventOnValueChange += ((UnityEngine.Object value) =>
                  {
                      for (int i = 0; i < components.Count; i++)
                      {
                          try
                          {
                              components[i].GetType().GetProperty(PropName).SetValue(components[i], value);
                          }
                          catch (ArgumentException e)
                          {
                              Debug.Log("It is possible property READONLY! \n" + e);
                          }
                          EObjectField t = fieldContent.GetWidget(i) as EObjectField;
                          t.value = value;
                      }
                      //ModifyAllComponentsValue<EObjectField,UnityEngine.Object >(components,fieldContent,PropName,value); 
                  });

            }
        }
    
        [MyAttribute.WidgetStrategyAttribute(typeof(CurveInputStrategy),new System.Type[]  {typeof(AnimationCurve)})]
        public class CurveInputStrategy : InputStrategy
        {            
            public override void Call(List<Component> components, string PropName, EHorizontalLayout inputField, EScrollView fieldContent)
            {
                  foreach (Component comp in components)
                {
                    fieldContent.AddWidget(CreateWidget<ECurveField, AnimationCurve>(comp, PropName));
                }
                inputField.ClearWidget();
                ECurveField toggle = new ECurveField("Input");
                inputField.AddWidget(toggle);
                toggle.eventOnValueChange += ((AnimationCurve value) =>
                  {
                      ModifyAllComponentsValue<ECurveField, AnimationCurve>(components, fieldContent, PropName, value);
                  });
            }
        }
    
    
    }

}