using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using System.Reflection;
using UnityEngine.Scripting;
using Custom.AttributeProgress;
using Custom.CustomAttribute;


public abstract class MyWiget
{

    public void DrawWidget(object obj)
    {
        Debug.Log("Draw widget.");
    }
}


public class MyTextField : MyWiget
{
    string m_FieldName;//是该文本框关联的属性的名字 
    PropertyInfo targetInfo;
    public MyTextField(string propertyName)
    {
        m_FieldName = propertyName;
    }
    /// <summary>
    /// 传递进来一个窗口对象，该对象须包含带stringAttribute特性的【属性】
    /// </summary>
    /// <param name="obj"></param>
   new public void DrawWidget(object obj)
    {
        string init_text = obj.GetType().GetField(m_FieldName).GetValue(obj) as string;
        Debug.Log(init_text);
       
        string text = EditorGUILayout.TextField(m_FieldName, init_text);
        if (text != null)
        {
            obj.GetType().GetProperty(m_FieldName).SetValue(obj, text);
            Debug.Log("Input text is: " + text);
        }
    }


    public class MyButton:MyWiget
    {
        public Rect m_rect { get; set; }
        public string m_text { get; set; }
         MyButton(Rect rect,string text)
        {
            m_rect = rect;
            m_text = text;
        }

        new public void DrawWidget(object obj)
        {
            GUI.Button(m_rect, m_text);
        }

    }

    public class EditorWindowTest : EditorWindow
    {
        [StringAttribute]
        public string str_test;
        [StringAttribute]
        public string str_test2;

        
        

        public delegate void WindowAction();
        public delegate string TextAction();
        private GameObject curObject;

        public List<MyTextField> textFields = new List<MyTextField>();

        private List<MyWiget> widgets = new List<MyWiget>();

        [MenuItem("Tools/Window/MyWindow")]
        public static void CreateEditorWindow()
        {
            EditorWindowTest w = CreateInstance<EditorWindowTest>();
            w.Show();
        }


        struct WidgetHandle
        {
            int widgetID;
            Vector2 position;
            Vector2 size;
        }


        private void Awake()
        {
            Custom.AttributeProgress.AttributeProgressor.FieldAttributeProgress(this, typeof(StringAttribute), (Attribute attr) =>
            {
                StringAttribute s = attr as StringAttribute;
                AddTextField(s.m_MemberName);
            });
        }

        private void OnGUI()
        {
            DrawTextField();

            AddButton("Select", () =>
            {
                curObject = Selection.activeObject as GameObject;
            });

            AddButton("Close", () =>
            {
                this.Close();
            });
            GetWindowSize();
        }

        private void AddButton(string title, WindowAction action)
        {
            if (GUILayout.Button(title))
            {
                action.Invoke();
            }
        }

        private void AddTextField(string propertyName)
        {
            textFields.Add(new MyTextField(propertyName));
        }

        private void DrawTextField()
        {
            foreach (var item in textFields)
            {
                item.DrawWidget(this);
            }
        }


        private void GetWindowSize()
        {
            Debug.Log("widow size is: "+ position);
            Debug.Log(position.width);
            Debug.Log(position.height);
        }


    }
}
