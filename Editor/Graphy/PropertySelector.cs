using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEditor;
using System.Reflection;

using MyEiditorWidget;
using MyEiditorWidget.FieldStrategy;
using MyEiditorWidget.MyAttribute;

//从某一个游戏物体的子物体上寻找目标组件，并遍历出该组件的所有属性，通过选择某个属性来对所有子物体的该组件的这个属性修改值
public class PropertySelector : EditorWindow
{
    //目标属性的名字
    private string m_targetPropName;

    //目标游戏物体（获得其类型）
    private Component m_targetCompType;

    //目标游戏物体
    private GameObject m_targetGameObject;

    //目标组件的类型
    private Type m_compType;

    //目标组件属性的类型
    private Type m_targetPropType;

    //按键和属性类型的字典（属性选择界面）
    private Dictionary<EButton,Type> m_buttonDic = new Dictionary<EButton, Type>();

    //按键和属性名的字典（属性选择界面）
    private Dictionary<EButton, string> m_buttonPropNameDic = new Dictionary<EButton, string>(  ); 

    //目标类型与目标显示策略的字典
    private Dictionary<Type, MyEiditorWidget.FieldStrategy.InputStrategy> m_typeStgDic ;

    //目标类型与按键FadeGroup的字典
    private Dictionary<Type, EFadeGroup> m_typeFadeGroupDic = new Dictionary<Type, EFadeGroup>();
    //目标组件列表
    private List<Component> m_tragetComps = new List<Component>();
 
    [MenuItem("Tools/Graphy/PropertySelector")]
    public static void CreateWindow()
    {
        PropertySelector p = EditorWindow.CreateInstance<PropertySelector>();
        p.titleContent = new GUIContent("Property Selector");
        p.Show();
        p.minSize = new Vector2(250f,400f);
    }

    //按键：获取目标组件的所有属性
    EButton m_getPropButton = new EButton("GetProp");
    //按键：返回到属性选择界面
    EButton m_backButton = new EButton("Back");

    EButton m_clearButton = new EButton("Clear");

    //水平布局：操作布局
    EHorizontalLayout m_operateButtonLayout = new EHorizontalLayout();

    //滚动视口：所有属性列表
    EScrollView m_propScrollView = new EScrollView();

    //水平布局：目标属性显示水平布局
    EHorizontalLayout m_targetPropHLayout = new EHorizontalLayout();

    //水平布局：找到目标属性及其类型与名字，然后显示出可以修改该属性的Widget
    EHorizontalLayout m_inputValueHLayout = new EHorizontalLayout();

    //滚动视口：所有组件的目标属性列表(在界面切换之后需要将内部的widget清除)
    EScrollView m_compTargetPropScrollView = new EScrollView();

    //文本框：显示选中的目标属性的类型
    ETextField m_targetPropTypeTextField ;
    //文本框：显示选中的目标属性的名字
    ETextField m_targetPropNameTextField;
    private void OnEnable()
    {
        m_operateButtonLayout.AddWidget(m_getPropButton);
        m_operateButtonLayout.AddWidget(m_backButton);    
        m_operateButtonLayout.AddWidget(m_clearButton);      

        m_getPropButton.AddListener(GetProperties);
        m_backButton.AddListener(()=>
        {
            //清空当前的目标属性类型和名字
            if (!m_propScrollView.isShow)
            {
                m_targetPropName = null;
                m_targetPropType = null;
            }
            //返回到属性列表
            m_propScrollView.isShow = true;
            m_targetPropHLayout.isShow = false;
            m_compTargetPropScrollView.isShow = false;
            m_inputValueHLayout.isShow = false;
        });

        m_clearButton.AddListener(()=>
        {
            //m_propScrollView.ClearWidget();
            ClearPropScrollView(m_propScrollView);
        });

        m_targetPropNameTextField = new ETextField ("PropertyName",m_targetPropName);
        m_targetPropTypeTextField = new ETextField("PropertyType",m_targetPropType?.Name);
        m_targetPropNameTextField.isRewrite = false;
        m_targetPropTypeTextField.isRewrite = false;
        m_targetPropHLayout.AddWidget(m_targetPropNameTextField);
        m_targetPropHLayout.AddWidget(m_targetPropTypeTextField);

        m_targetPropHLayout.isShow = false;
        m_compTargetPropScrollView.isShow = false;

        //类型和策略的字典初始化
        m_typeStgDic = new Dictionary<Type, MyEiditorWidget.FieldStrategy.InputStrategy>();

        ProgressInputStrategyAttribute(m_typeStgDic);

        CreateFadeGroupForPropertyType(m_propScrollView);       
    }
    
    private void OnGUI()
    {
        m_targetCompType = EditorGUILayout.ObjectField(m_targetCompType, typeof(Component), true) as Component;
        m_targetGameObject = EditorGUILayout.ObjectField(m_targetGameObject, typeof(GameObject), true) as GameObject;
        //绘制操作按钮
        m_operateButtonLayout.DrawWidget(); 
        //绘制目标组件的所有属性的按钮
        m_targetPropHLayout.DrawWidget();   
        //绘制属性对应按钮（滚动视图）
        m_propScrollView.DrawWidget();
        //绘制全局输入框
        m_inputValueHLayout.DrawWidget();
        //绘制所有组件目标属性按钮
        m_compTargetPropScrollView.DrawWidget();
    }

    //获得所有属性，并用一个button显示
    public void GetProperties()
    {
        //
        if(!(m_typeStgDic.Count > 0)) 
        {
            Debug.LogWarning("Field Strategy not found.");
            return;
        }
        Debug.Log("Get property.");     
        Debug.Log(m_typeStgDic.Keys.Count); 
        m_propScrollView .isShow= true;
        m_targetPropHLayout.isShow = false;
        m_inputValueHLayout .isShow = false;

        //清除滚动视口中的按钮，并清除与按钮关联的数据（需要改，太暴力）
        ClearPropScrollView(m_propScrollView);
        m_buttonDic.Clear();
        m_buttonPropNameDic.Clear();
        m_compTargetPropScrollView.ClearWidget();

        m_compType = m_targetCompType.GetType();

        //遍历所有的属性，并以按键的形式显示
        CreateButtonForProperty(m_compType.GetProperties());       
    }    


    //根据属性类型生成FadeGroup，并产生一个映射 从类型===>FadeGroup
    private void CreateFadeGroupForPropertyType(EScrollView propScrollView)
    {
        var t = m_typeStgDic.Keys;
        foreach(Type type in t)
        {
            EFadeGroup temp_fg = new EFadeGroup(type.Name,Repaint);
            propScrollView.AddWidget(temp_fg);
            m_typeFadeGroupDic.Add(type,temp_fg);
        }
    }


    //查询类型到FadeGroup的映射，生成Button
    private void CreateButtonForProperty(PropertyInfo[] infos)
    {
        foreach (PropertyInfo info in infos)
        {

            if (!m_typeStgDic.ContainsKey(info.PropertyType))
            {
                Debug.Log("Porperty type not exist in the Dic. " + info.PropertyType.Name);
                continue;
            }

            Debug.Log(info.Name);
            EButton b = new EButton(info.Name);
            b.maxWidth = 200f;
            m_buttonDic.Add(b, info.PropertyType);
            m_buttonPropNameDic.Add(b, info.Name);
            b.AddListener(ButtonClickCallBack);
            m_typeFadeGroupDic[info.PropertyType].AddWidget(b);
        }
    }


    //清除属性滚动视口子物体的所有子物体
    private void ClearPropScrollView(EScrollView propScrollView)
    {
         propScrollView.ClearWidgetOfChildren();
    }

    //处理当前选中的属性
    public void ProgressCurrentProperty(MyEiditorWidget.FieldStrategy.InputStrategy stg)
    {
        stg.Call(m_tragetComps,m_targetPropName,m_inputValueHLayout,m_compTargetPropScrollView);
    }


    //获得所有子物体的目标组件
    private void GetAllChildComponent(List<Component> comps)
    {
         foreach (Transform trans in m_targetGameObject.GetComponentsInChildren<Transform>())
        {
            if(trans.name == m_targetGameObject.name) continue;
            Component c = trans.GetComponent(m_targetCompType.GetType());
            if(c != null) m_tragetComps.Add(c);
        }
    }

    //与属性对应的Button的点击回调函数
    private void ButtonClickCallBack(EButton b)
    {      
       
        m_targetPropType = m_buttonDic[b];//从字典中查找按键对应的属性类型
        m_targetPropName = m_buttonPropNameDic[b];//从字典中查找按键对应的属性名

        m_propScrollView.isShow= false;//关闭属性列表

        m_targetPropHLayout.isShow = true;//打开目标属性显示
        m_inputValueHLayout.isShow =  true;//打开属性值输入
     
        //设置目标属性类型与目标属性名
        m_targetPropTypeTextField.value = m_targetPropType.Name;        
        m_targetPropNameTextField.value = m_targetPropName;

        if (m_targetGameObject == null) return;
         //显示目标物体的所有子物体的目标组件的目标属性的值
        m_tragetComps.Clear();
        m_compTargetPropScrollView.ClearWidget();
        m_compTargetPropScrollView.isShow = true;//显示遍历出来的所有目标组件的值

        //获得所有子物体的目标组件
        GetAllChildComponent(m_tragetComps);

        //处理选中后的属性,依赖上文遍历的组件信息
        if (m_typeStgDic.ContainsKey(m_targetPropType))
        {
            ProgressCurrentProperty(m_typeStgDic[m_targetPropType]);
        }
        else
        {
            Debug.LogWarning("At present it is not support this type.");
            return;
        } 
    }

    //处理所有已经定义的输入框， 自动添加与属性类型相对应的输入框类型支持（反射）
    private void ProgressInputStrategyAttribute(Dictionary<Type, MyEiditorWidget.FieldStrategy.InputStrategy> strategyDic)
    { 
        Type traget_type = typeof(InputStrategy);
        System.Type[] ts = ReflectionProgressFunc.GetClassInTargetNameSpace(traget_type, traget_type.Namespace);

        foreach (System.Type t in ts)
        {
            bool isdef = Attribute.IsDefined(t,
               typeof(WidgetStrategyAttribute));
            if (isdef)
            {               
                var target_attrs = t.GetCustomAttribute<WidgetStrategyAttribute>();

                if (target_attrs.attributeTag == WidgetStrategyAttributeTag.NormalField)
                {
                    strategyDic.Add(target_attrs.valueTypes[0], System.Activator.CreateInstance(target_attrs.widgetType) as InputStrategy);
                }
                //如果是ObjectField，就遍历该类的所有值类型，并生成对应的实例
                else if (target_attrs.attributeTag == WidgetStrategyAttributeTag.ObjectField)
                {
                    foreach(Type value_type in target_attrs.valueTypes)
                    {
                        strategyDic.Add(value_type, new ObjectInputStrategy(value_type));
                    }                    
                }                
            }
        }
    }
}

