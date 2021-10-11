using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace GraphyEditor
{

    public delegate void MyAction();
    public delegate void MyAction<T>(T t);
    public delegate void MyAction<T1, T2>(T1 t1, T2 t2);


    public abstract class MyWidget
    {
        
        public float m_maxHeight = 20f;

        public float m_maxWidth = 100f;
        public void DrawWidget()
        {

        }
    }
    public class LayouButton:MyWidget
    { 
        public Camera m_camera { get; }
        public string m_name { get; }
        public LayouButton(string name, Camera camera)
        {
            m_name = name;
            m_camera = camera;
        }
        public void DrawWidget(MyAction<Camera> act)
        {
            if (GUILayout.Button(m_name, GUILayout.Width(m_maxWidth), GUILayout.Height(m_maxHeight)))
            {
                m_camera.enabled = true;
                m_camera.depth = 2;
                act.Invoke(m_camera);
            }
        }

        new public void DrawWidget()
        {
            if (GUILayout.Button(m_name, GUILayout.Width(m_maxWidth), GUILayout.Height(m_maxHeight)))
            {
                m_camera.enabled = true;
                m_camera.depth = 2;
            }
        }
    }

    //@descirption: 自定义的文本框，可以控制标签的和框的长度
    public class MyFloatField:MyWidget
    {
        public float m_maxLabelWidth = 50f;
        public float m_maxFieldWidth = 50f;
        public void DrawWidget(string label, ref float num)
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(label,GUILayout.MaxWidth(m_maxLabelWidth));
            num = EditorGUILayout.FloatField(num,GUILayout.MaxWidth(m_maxFieldWidth));
            EditorGUILayout.EndHorizontal();
        }

        public static float DrawWidget(string label, float num, float maxLabelWidth = 50f, float maxFeildWidth = 50f)
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(label,GUILayout.MaxWidth(maxLabelWidth));
            float n = EditorGUILayout.FloatField(num,GUILayout.MaxWidth(maxFeildWidth));
            EditorGUILayout.EndHorizontal();
            return n;
        } 
    }


    public class MyToggleField:MyWidget
    {
        public static bool DrawWidget(string label, bool value, float maxLabelWidth = 50f)
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(label, GUILayout.MaxWidth(maxLabelWidth));
            bool b = EditorGUILayout.Toggle(value);
            EditorGUILayout.EndHorizontal();
            return b;
        }
    }
    public class CameraPanel : EditorWindow
    {
        GameObject currentGameobject;
        private Camera m_currentCamera;
        private List<LayouButton> m_buttons = new List<LayouButton>();
        private List<Camera> m_cameras = new List<Camera>();

        private bool isOne = true;
         
        [MenuItem("Tools/Graphy/CameraPanel")]
        public static void CreateCameraPanel()
        {
            CameraPanel panel = EditorWindow.CreateInstance<CameraPanel>();
            panel.titleContent = new GUIContent("CamraPanel");
            panel.Show();

        }
        private void Awake()
        {

        }

        private void OnGUI()
        {           
            currentGameobject = EditorGUILayout.ObjectField(currentGameobject, typeof(GameObject), true) as GameObject;

            if (GUILayout.Button("InitCamera"))
            {
                InitCamera();
            }

            if(GUILayout.Button("SetOneOrTwo"))
            {
                if(m_cameras.Count>0)
                {
                    foreach (Camera c in m_cameras)
                    {
                        if(isOne)
                        {
                            c.depth = 2;
                        }
                        else
                        {
                            c.depth = 1;
                        }
                        
                    }
                    isOne = !isOne;
                }
            }

            
            if (m_cameras.Count > 0)
            {
                EditorGUILayout.LabelField("Cameras: ");
                foreach (Camera c in m_cameras)
                {
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField(c.transform.name, GUILayout.MinWidth(80f),GUILayout.MaxWidth(100f));                    
                    c.depth = MyFloatField.DrawWidget("depth",c.depth);                    
                    c.enabled = MyToggleField.DrawWidget("enable", c.enabled);
                    //b.m_camera.
                    EditorGUILayout.EndHorizontal();
                }
            }

        }

        private void InitCamera()
        {
            if (currentGameobject == null) return;
            m_cameras.Clear();
            //遍历对象的所有摄像机
            foreach (Transform trans in currentGameobject.transform.GetComponentsInChildren<Transform>())
            {
                Camera c = trans.GetComponent<Camera>();
                if (c != null)
                {
                    m_cameras.Add(c);
                    //m_buttons.Add(new LayouButton(trans.name,c));
                }
            }
        }
        private void ButtonCallBack(Camera camera)
        {
            if (m_currentCamera)
            {
                m_currentCamera.enabled = false;
                m_currentCamera.depth = 1;
            }
            if (camera) m_currentCamera = camera;
        } 
    }

}
