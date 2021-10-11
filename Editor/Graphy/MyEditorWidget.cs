using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.AnimatedValues;
using UnityEngine.Events;
namespace MyEiditorWidget
{

    public delegate void EAction();
    public delegate void EAction<T>(T t);
    public delegate void EAction<T1, T2>(T1 t1, T2 t2);

    //Widget抽象基类，有一个DrawWidget方法，子类自行实现
    public abstract class EWidget
    {
        public float maxHeight = 20f;

        public float maxWidth = 100f;

        public bool isShow = true;
        virtual public void DrawWidget()
        {
        }
    }
    public class EButton : EWidget
    {
        public string label = "Button";
        private EAction onClick;

        private EAction<EButton> onClick_1;
        public EButton(string name)
        {
            if (name != null) label = name;
        }
        override public void DrawWidget()
        {
            if (!isShow) return;
            if (GUILayout.Button(label, GUILayout.MaxWidth(maxWidth), GUILayout.MaxHeight(maxHeight)))
            {
                if (onClick != null) onClick.Invoke();
                if (onClick_1 != null) onClick_1.Invoke(this);
            }
        }

        public void AddListener(EAction action)
        {
            onClick += action;
        }
        public void AddListener(EAction<EButton> action)
        {

            onClick_1 += action;
        }
    }

    //布局的基类，该类及其子类是一个Widget的容器，能够对这些Widget的位置和大小进行自动控制
    public abstract class ELayout : EWidget
    {
        protected List<EWidget> m_widgetList = new List<EWidget>();
        public void AddWidget(EWidget widget)
        {
            if (widget != null)
                m_widgetList.Add(widget);
        }

        public void ReMoveWidget(EWidget widget)
        {
            if (m_widgetList.Contains(widget))
            {
                m_widgetList.Remove(widget);
            }
        }

        public void ClearWidget()
        {
            m_widgetList.Clear();
        }

        //删除所有子物体的子物体，但不删除子物体
        public void ClearWidgetOfChildren()
        {
            foreach(EWidget widget in m_widgetList)
            {
                if(widget is ELayout)
                {
                    ELayout chi = widget as ELayout;
                    chi.ClearWidget();
                }              
            }
        }

        public EWidget GetWidget(int i)
        {
            if (i < m_widgetList.Count)
                return m_widgetList[i];
            return null;
        }

        public int GetWidgetIndex(EWidget widget)
        {
            return m_widgetList.IndexOf(widget);
        }
    }

    public class EHorizontalLayout : ELayout
    {
        public EHorizontalLayout()
        {
        }
        override public void DrawWidget()
        {
            if (!isShow) return;
            if (m_widgetList.Count > 0)
            {
                EditorGUILayout.BeginHorizontal();
                foreach (EWidget widget in m_widgetList)
                {
                    widget.DrawWidget();
                }
                EditorGUILayout.EndHorizontal();
            }
        }
    }

    public class EVerticalLayout : ELayout
    {
        override public void DrawWidget()
        {
            if (!isShow) return;
            EditorGUILayout.BeginVertical();
            foreach (EWidget widget in m_widgetList)
            {
                widget.DrawWidget();
            }
            EditorGUILayout.BeginVertical();
        }

    }
    public class EScrollView : ELayout
    {
        //设置滚动的位置，一般设置为【0，0】上方起点
        public Vector2 scrollPos = new Vector2(0.0f, 0.0f);
        override public void DrawWidget()
        {
            if (!isShow) return;
            if (m_widgetList.Count > 0)
            {
                scrollPos = EditorGUILayout.BeginScrollView(scrollPos);
                foreach (EWidget widget in m_widgetList)
                {
                    widget.DrawWidget();
                }
                EditorGUILayout.EndScrollView();
            }
        }
    }




    public class ESlider : EWidget
    {

        public float currrentValue = 0.0f;
        public float leftValue = 0.0f;
        public float rightValue = 1.0f;
        public EAction<float> OnValueChange;

        public override void DrawWidget()
        {
            currrentValue = EditorGUILayout.Slider(currrentValue, leftValue, rightValue);
        }

        public void AddListener(EAction<float> action)
        {
            OnValueChange += action;
        }
    }


    public class EFadeGroup: ELayout
    {
        public string label;
        public AnimBool showExtraFields;
        public EFadeGroup(string name,  UnityAction repaint)
        {
            label = name;
            showExtraFields = new AnimBool(true);            
            if(repaint != null) showExtraFields.valueChanged.AddListener(repaint);
        }

        public EFadeGroup()
        {
            showExtraFields = new AnimBool(true);            
        }
        public override void DrawWidget()
        {   
            showExtraFields.target = EditorGUILayout.ToggleLeft(label, showExtraFields.target);       
        if (EditorGUILayout.BeginFadeGroup(showExtraFields.faded))
        {
            //EditorGUI.indentLevel++;        
            if (!isShow) return;
            if (m_widgetList.Count > 0)
            {
                //EditorGUILayout.BeginVertical();
                foreach (EWidget widget in m_widgetList)
                {
                    widget.DrawWidget();
                }
                //EditorGUILayout.EndVertical();
            }   
            //EditorGUI.indentLevel--;
        }
        EditorGUILayout.EndFadeGroup();
        } 
    }


    public abstract class EField<T> : EWidget
    {
        public T value;//一个Fieldwidget 必须有的值，用于从框框中获取数据  
        public string label = "Field";
        public bool isRewrite = true;
        public GUIContent guiContent = new GUIContent();
        public EField(string name)
        {
            label = name;
            guiContent.text = name;
        }

        public EField()
        {
        }

        public event EAction<T> eventOnValueChange;
        public void OnValueChange(T value)
        {
            if (eventOnValueChange != null) eventOnValueChange.Invoke(value);
        }

    }


    public class EToggle : EField<bool>
    {
        private EAction<EToggle> onToggle_1;
        public EToggle(string label)
        : base(label)
        {
            this.value = true;
        }
        public EToggle()
        {
        }
        override public void DrawWidget()
        {
            if (!isShow) return;
            bool t_value = EditorGUILayout.Toggle(label, value);
            if (onToggle_1 != null) onToggle_1.Invoke(this);
            if (t_value != value)
            {
                base.OnValueChange(t_value);
            }
            value = t_value;
        }
        public void AddListener(EAction<EToggle> action)
        {
            onToggle_1 += action;
        }
    }


    public class ETextField : EField<string>
    {    
        public ETextField(string name, string text)
        : base(name)
        {

            if (name != null) this.label = name;
            if (text != null) this.value = text;
        }

        public ETextField()
        {
        }

        public ETextField(string name)
        : base(name)
        {
            if (name != null) this.label = name;
        }

        override public void DrawWidget()
        {
            if (!isShow) return;
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(label, GUILayout.MaxWidth(maxWidth), GUILayout.MaxHeight(maxHeight));
            string t_text = EditorGUILayout.TextField(value);
            EditorGUILayout.EndHorizontal();

            //当输入的text与存储的text不同的时候，触发text改变事件
            if (value != t_text && t_text != null)
            {
                base.OnValueChange(t_text);
            }
            if (isRewrite) value = t_text;
        }
    }

    public class EIntField : EField<int>
    {
        public EIntField(string name)
        : base(name)
        {
        }
        public EIntField() { }
        public override void DrawWidget()
        {
            int t_v = EditorGUILayout.IntField(label, value);
            if (t_v != value)
            {
                base.OnValueChange(t_v);
            }
            value = t_v;
        }
    }

    public class EFloatField : EField<float>
    {
        public EFloatField(string name)
       : base(name)
        {
        }

        public EFloatField()
        : base()
        {
        }

        public override void DrawWidget()
        {
            float t_v = EditorGUILayout.FloatField(label, value);
            if (t_v != value)
            {
                base.OnValueChange(t_v);
            }
            value = t_v;
        }
    }

    public class EObjectField : EField<UnityEngine.Object>
    {
        public System.Type objectType;
        public EObjectField(string name, System.Type type)
        : base(name)
        {
            objectType = type;
        }

        public EObjectField()
        {
        }

        public override void DrawWidget()
        {
            //允许SceneObject赋值
            UnityEngine.Object t_v = EditorGUILayout.ObjectField(label, value, objectType, true);
            if (t_v != value)
            {
                base.OnValueChange(t_v);
            }
            value = t_v;
        }
    }

    public class EColorField : EField<Color>
    {
        public bool showEyedropper = false;
        public bool showAlpha = true;
        public bool hdrColor = false;
        private EAction<EColorField> onSelf;
        public EColorField(string name)
        : base(name)
        {            
            maxWidth = 300f;
            value = new Color(1.0f, 1.0f, 1.0f, 1.0f);
        }

        public EColorField()
        {
        }
        public override void DrawWidget()
        {
            Color t_color = EditorGUILayout.ColorField(guiContent, value, showEyedropper, showAlpha, hdrColor,  GUILayout.Height(maxHeight));

            if (t_color != value)
            {
                base.OnValueChange(t_color);
                if (onSelf != null) onSelf.Invoke(this);
            }
            value = t_color;
        }

        public void AddListener(EAction<EColorField> action)
        {           
            onSelf += action;
        }
    }

    public class EVector3Field : EField<Vector3>
    {
        public EVector3Field(string name)
        : base(name)
        {
        }
        public EVector3Field()
        {
        }
        public override void DrawWidget()
        {
            Vector3 t_vec = EditorGUILayout.Vector3Field(label, value);
            if (t_vec != value)
            {
                base.OnValueChange(t_vec);
            }
            value = t_vec;
        }
    }

    public class EVector2Field: EField<Vector2>
    {
        public EVector2Field(string name)
        :base(name)
        {
        }
        public EVector2Field()
        {
        }
        public override void DrawWidget()
        {
             Vector2 t_vec = EditorGUILayout.Vector2Field(label, value);
            if (t_vec != value)
            {
                base.OnValueChange(t_vec);
            }
            value = t_vec;             
        }
    }

    public class EMat4Field : EField<Matrix4x4>
    {
        public EMat4Field(string name)
        : base(name)
        {
        }
    }

    public class  ECurveField: EField<AnimationCurve>
    {               
        public ECurveField(string name)
        :base(name)
        {
        }

        public ECurveField()
        {
        }

        public override void DrawWidget()
        {
             AnimationCurve t_vec = EditorGUILayout.CurveField(label, value,GUILayout.MinHeight(100f));
            if (t_vec != value)
            {
                base.OnValueChange(t_vec);
            }
            value = t_vec;             
        }
    }
}