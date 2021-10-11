using System;
using System.Reflection;
using UnityEngine;
namespace Custom
{
    namespace CustomAttribute
    {
        public class StringAttribute : Attribute
        { 
            public string m_MemberName; 
        } 
    }


    namespace AttributeProgress
    {
        public class AttributeProgressor
        {
            public delegate void AttributeAction(Attribute attr);
            public static void AttributProgress(object assembly, Type attribute, AttributeAction action)
            {
                Debug.Log("Progress attributes?");
                Assembly ay = assembly.GetType().Assembly;
                Attribute[] at = Attribute.GetCustomAttributes(ay, attribute);                
                foreach (Attribute item in at)
                {                    
                    if (item.GetType() == attribute)
                    {
                        dynamic i = item; 
                        //Debug.Log(s.str);
                        Debug.Log(i.text);
                        Debug.Log("is my attribute?");
                       // action.Invoke();
                    }
                    Debug.Log(item.GetType());
                }
            }

            /// <summary>
            /// 用于命名空间（就是程序集？）的特性处理
            /// </summary>
            /// <param name="assembly"></param>
            /// <param name="attribute"></param>
            public static void AttributProgress(object assembly, Type attribute)
            {
               
                Assembly ay = assembly.GetType().Assembly;
                Attribute[] at = Attribute.GetCustomAttributes(ay, attribute);
                int count = 0;
                Debug.Log(at.Length);
                bool isdef = Attribute.IsDefined(ay,
                attribute);
                if (isdef)
                {
                    Debug.Log(ay.GetName().Name);
                }
                else
                {
                    Debug.Log( "attribute not def. "+ ay.GetName());
                }
                foreach (Attribute item in at)
                {
                    if (item.GetType() == attribute)
                    {
                        dynamic i = item;                        
                        Debug.Log(i.text);
                        Debug.Log("is my attribute?");                       
                    }
                    Debug.Log(item.GetType());
                    Debug.Log(count++);
                }

            }

            /// <summary>
            /// 用于类属性成员的特性处理
            /// </summary>
            /// <param name="type"> 类对象的引用，用于获取动态类型</param>
            /// <param name="attribute"> 目标特性，用于检索类成员是否具有该特性</param>
            /// <param name="action"> 回调函数，用于处理检索到的特性</param>
            public static void FieldAttributeProgress(object type, Type attribute, AttributeAction action)
            {
                Type obj_type = type.GetType();
                FieldInfo[] info = obj_type.GetFields();//通过反射信息，获取该类的所有的成员
                foreach (FieldInfo f in info)
                {
                    if (!Attribute.IsDefined(f, attribute)) continue;//如果某个成员不含该特性，跳过
                    Attribute[] attr = Attribute.GetCustomAttributes(f, attribute);//获取某个成员的所有特性
                    foreach (Attribute a in attr)
                    {
                        if(a.GetType() == attribute)
                        {
                            dynamic d = a;
                            d.m_MemberName = f.Name;
                            action.Invoke(a);//通过回调函数处理该特性
                        }
                    }
                }
            }

            public static void PropertyAttributeProgress(object type, Type attribute, AttributeAction action)
            {
                Type obj_type = type.GetType();
                PropertyInfo[] info = obj_type.GetProperties();//通过反射信息，获取该类的所有的成员
                foreach (PropertyInfo p in info)
                {
                    if (!Attribute.IsDefined(p, attribute)) continue;//如果某个成员不含该特性，跳过
                    Attribute[] attr = Attribute.GetCustomAttributes(p, attribute);//获取某个成员的所有特性
                    foreach (Attribute a in attr)
                    {
                        if (a.GetType() == attribute)
                        {
                            dynamic d = a;
                            d.m_MemberName = p.Name;
                            action.Invoke(a);//通过回调函数处理该特性
                        }
                    }
                }
            }

        }



    }
}
