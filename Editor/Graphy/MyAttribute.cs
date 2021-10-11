using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;


namespace MyEiditorWidget
{
      // See the attribute guidelines at
      // http://go.microsoft.com/fwlink/?LinkId=85236
    namespace MyAttribute
    {

        public enum WidgetStrategyAttributeTag
        {
            ObjectField = 1,
            NormalField = 2
        }
    
        /**
         * @description: 
         * Widget策略，即自定义的一个对widget实例、添加观察者的一系列操作的方法。
         * 该特性能够标注Widget策略，在运行时（窗口显示之前）直接将widget策略注册到目标集合中去        
         */
        [System.AttributeUsage(System.AttributeTargets.Class, Inherited = false, AllowMultiple = true)]
        sealed class WidgetStrategyAttribute : System.Attribute
        {
            //readonly 是运行时常量，与const编译期常量不同，readonly可以在不同的构造函数中初始化，在不同的构造函数中有不同的值
            readonly System.Type _widgetType; 
            readonly System.Type[] _valueTypes;
            readonly WidgetStrategyAttributeTag _attributeTag;
            public WidgetStrategyAttribute(System.Type widgetType, System.Type[] valueTypes, WidgetStrategyAttributeTag attributeTag = WidgetStrategyAttributeTag.NormalField)
            {
                this._widgetType = widgetType;
                this._valueTypes = valueTypes;
                this._attributeTag = attributeTag;
                //throw new System.NotImplementedException();
            }

            public System.Type widgetType
            {
                get { return _widgetType; }
            } 

            public WidgetStrategyAttributeTag attributeTag
            {
                get {return _attributeTag;} 
            }

            public System.Type[] valueTypes
            {
                get{return _valueTypes;}
            }

            public int NamedInt { get; set; }
        }


        public class ReflectionProgressFunc
        {
            /**
             * @description: 
             * 从目标命名空间中获得所有类的类型            
             */            
            public static System.Type[]  GetClassInTargetNameSpace(System.Type type, string nameOfNameSpace)
            { 
                List<System.Type>  types = new List<System.Type>();
                foreach (System.Type t in type.Assembly.GetTypes())
                {
                    if (t.Namespace == nameOfNameSpace)
                    { 
                        types.Add(t);
                    }                        
                }
                return types.ToArray();
            }


            public static void ProgressAssemblyExecutingClass(System.Type type, string nameOfNameSpace)
            {
                System.Reflection.Assembly asm = System.Reflection.Assembly.GetExecutingAssembly();
                Module[] mdArr = asm.GetModules(false);
                System.Type[] tparr = mdArr[0].GetTypes();
                foreach(System.Type t in tparr)
                {
                    
                    Debug.Log(t.Name);
                }
            }

            public static void GetTargetAttributesInNameSpace(System.Type type , string nameOfNameSpace )
            {

            }


        }





    }
}

 