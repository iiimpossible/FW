using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using TMPro;
using UnityEngine.UI;
using System.Text.RegularExpressions;

 
namespace GraphyEditor
{
    public class ToolFunc
    {
        public static void GetAllChildren()
        {

        }


    }

    
    public class ExtensionPanel:EditorWindow
    {

        [MenuItem("Tools/Graphy/ExtensionPanel")]
        public static void CreateWindow()
        {
            ExtensionPanel p = EditorWindow.CreateInstance<ExtensionPanel  >();
            p.titleContent = new GUIContent("Main Panel");
            p.Show();
        }
        private  void OnGUI() 
        {
            //各种功能按钮

            //一键给指定游戏物体添加组件
           
           
            //一键给指定游戏物体添加子物体
            if(GUILayout.Button("Add Children"))
            {
                AddChildren.CreateWizard();
            }

            //一键给指定游戏物体改名
             if(GUILayout.Button("Change Name"))
            {
                ChangeNameWizard.CreateWizard();
            }

            //一键给指定游戏物体删除子物体
            if (GUILayout.Button("Delete Child"))
            {
                DeleteObject.CreateWizard();
            }

            if(GUILayout.Button("Create Text"))
			{
                CalcCameraDistance.CreateWizard();
			}

            if(GUILayout.Button("Calculate Arow Distance"))
			{
                CalculateArowDistance.CreateWizard();
            }

            if (GUILayout.Button("World Space Rotation"))
			{
                GameObject obj = Selection.activeObject as GameObject;
                Debug.Log(obj.name + " World Space Rotation: " + obj.transform.rotation.eulerAngles);
            }

            if(GUILayout.Button("Loacal Space Rotation"))
			{
                GameObject obj = Selection.activeObject as GameObject;
                Debug.Log(obj.name + " Local Space Rotation: " + obj.transform.localRotation.eulerAngles);
			}

            if(GUILayout.Button("ChangeProperties"))
			{
                ChangeProperties.CreateWizard();
            }
        }
    }


    public class ChangeNameWizard: ScriptableWizard
    {
        public GameObject oringinObjct;
        public string targetName;
        public string newName;
        public static void CreateWizard()
        {
            ScriptableWizard.DisplayWizard<ChangeNameWizard>("Chanege Name","Apply","Select");
        }


        private void OnWizardCreate() {
            
        }

        private void OnWizardOtherButton() {
            foreach(Transform trans in oringinObjct.transform.GetComponentsInChildren<Transform>())
            {
                if(trans.name == targetName)
                {
                    trans.name = newName;
                }
            }
        }
    }


    public class AddChildren: ScriptableWizard
    {
        public GameObject oringinObjct;
        public string targetName;
        public GameObject prefabObjct;
        public float scale = 0.00001f;
        public Vector3 rote = new Vector3(0f, 95f, 0f);


        public static void CreateWizard()
        {
            ScriptableWizard.DisplayWizard<AddChildren>("Add Children","Apply","Select");
        }

        
        private void OnWizardCreate() {
            
        }

        private void OnWizardOtherButton() {
            if(oringinObjct == null || prefabObjct == null) return;

            foreach(Transform trans in oringinObjct.transform.GetComponentsInChildren<Transform>())
            {
                if(trans.name == targetName)
                {
                    GameObject t = GameObject.Instantiate(prefabObjct);                    
                    t.transform.SetParent(trans.parent);
                    RectTransform rect = t.GetComponent<RectTransform>();
                    rect.anchoredPosition3D = Vector3.zero; 
                    rect.localRotation = Quaternion.Euler(rote);
                    //rect.SetScale(scale, scale, scale);
                    t.GetComponentInChildren<Text>().text = trans.GetComponent<TextMeshPro>().text;
                    Undo.RegisterCreatedObjectUndo(t, "Create object");

                } 
            } 
        }
    }


    public class DeleteObject: ScriptableWizard
	{
        public GameObject oringinObjct; 
        public string targetName;

        public static void CreateWizard()
		{
            ScriptableWizard.DisplayWizard<DeleteObject>("Delete Object", "Apply", "Select");
        }
        private void OnWizardCreate()
        {

        }

        private void OnWizardOtherButton()
        {
            if (oringinObjct == null) return;

			foreach (Transform trans in oringinObjct.transform.GetComponentsInChildren<Transform>())
			{
				if (trans.name == targetName)
				{
					//Debug.Log("deleted name: " + trans.name);
					Undo.DestroyObjectImmediate(trans.gameObject);

				}
			}
			//Debug.Log(oringinObjct.transform.GetComponentsInChildren<Transform>().Length);
   //         foreach(Transform trans in oringinObjct.transform.GetComponentsInChildren<Transform>())
			//{
   //             Debug.Log(trans.name);
			//}
        }
    }


    public class CalcCameraDistance : ScriptableWizard
    {
        public GameObject oringinObjct;//是旧的文本父物体
        public GameObject constructionCanvas;//画布
        public GameObject prefabObject;//文本预制体
        public string targetname;//是旧的文本的位置 
        public float textScale = 0.01f;
        public Vector3 posOffset = Vector3.zero;
        public Vector3 roteOffset = Vector3.zero;
        public string prefix = "name";
        public string breakName = "SYZ_jianzhu2";

        public bool isLog = true;
        //public List<Vector3> cameraPos;
        public static void CreateWizard()
        {
            ScriptableWizard.DisplayWizard<CalcCameraDistance>("CaculateCameraDis", "Apply", "Select");
        }


        private void OnWizardOtherButton()
        {
            CalculateDistance();
        }

        private void CalculateDistance()
        {
            if (oringinObjct == null || constructionCanvas == null || prefabObject == null) return;
            if (isLog) Debug.Log("Calculate old text from canvas");
            if (isLog) Debug.Log("canvas anchoredPosistion: " + constructionCanvas.GetComponent<RectTransform>().anchoredPosition3D);
            //遍历场景中的相机的位置，并计算其与Canvas的相对位置，然后生成一个Text并加上已经计算好的相对位置偏移
            foreach (Transform trans in oringinObjct.GetComponentsInChildren<Transform>())
			{
                if(trans.name == targetname)//trans是原来的textmesh，这里需要用到其位置信息
				{
                    if (isLog) Debug.Log("text name is: " + trans.name);
                    Vector3 dis = trans.position - constructionCanvas.GetComponent<RectTransform>().anchoredPosition3D;
                    
                    GameObject new_text = GameObject.Instantiate(prefabObject);//生成一个Text
                    RectTransform rect = new_text.GetComponent<RectTransform>();
                    rect.anchoredPosition3D = Vector3.zero;
                    rect.anchoredPosition3D += dis;
                                       
                   // rect.SetScale(textScale, textScale, textScale);
                    rect.eulerAngles = trans.eulerAngles;
                    //rect.localRotation = Quaternion.Euler(rect.localRotation.eulerAngles + roteOffset);
                    new_text.GetComponent<Text>().text = trans.GetComponent<TextMeshPro>().text;
                    new_text.transform.SetParent(constructionCanvas.transform);
                    rect.localPosition += rect.parent.InverseTransformDirection(posOffset);
                    new_text.name = GetParent(trans);
                    rect.pivot = new Vector2(0.0f, 0.5f);

                    Undo.RegisterCreatedObjectUndo(new_text, "Create object");
                } 
            } 
		}



        private string GetParent(Transform parent)
		{
            if (parent.parent.name == breakName)
            {
                Regex reg = new Regex(@"(judian)(.*)(\d*)");
                GroupCollection gr = reg.Match(parent.name).Groups; 
                return prefix + gr[2];
			}          
            return GetParent(parent.parent);
		}

    }

    public class CalculateArowDistance:ScriptableWizard
	{
        public GameObject oringinObject;//箭头的父物体
        public GameObject arowCanvas;//箭头上的文本的canvas
        public GameObject newText;//新的文字
        public string targetNamePattern = @"(to_)(\d{1,2})";//箭头的物体名字       
        public string breakName = "jiantou_ok2";
        private float textScale = 0.00001f;

        public Vector3 rotaOffset = new Vector3(-67f, -16f,136.5f);
        public Vector3 posOffset = new Vector3(-0.02476f,0.02744f,-0.01457f);

        public static void CreateWizard()
        {
            ScriptableWizard.DisplayWizard<CalculateArowDistance>("CalculateArowDistance", "Apply", "Select");
        }


        private void OnWizardOtherButton()
        {
            CalculateArowFromCanvas();
        }
        private void CalculateArowFromCanvas()
        {
            if(oringinObject == null || arowCanvas == null || newText == null)
			{
                Debug.LogError("Null reference.");
            }
            Regex reg = new Regex(targetNamePattern);
            foreach (Transform trans in oringinObject.GetComponentsInChildren<Transform>())
			{
                Match m =  reg.Match(trans.name);
                if(m.Success)
				{
                    Vector3 dis = trans.position - arowCanvas.GetComponent<RectTransform>().anchoredPosition3D;

                    GameObject new_text = GameObject.Instantiate(newText);//生成一个Text
                    RectTransform rect = new_text.GetComponent<RectTransform>();
                    rect.anchoredPosition3D = Vector3.zero;
                    rect.anchoredPosition3D += dis;
                     rect.localPosition += posOffset;                     
                    rect.localScale = new Vector3(0.00005f, 0.00005f, 0.00005f);
                    //rect.eulerAngles = trans.eulerAngles;
                    //rect.rotation = CaclRotetion(trans.rotation, arowCanvas.transform.rotation);
                    rect.localEulerAngles += rotaOffset;
                     
                    //rect.eulerAngles = trans.TransformDirection(trans.localEulerAngles);
                    //rect.localRotation = Quaternion.Euler(rect.localRotation.eulerAngles + roteOffset);
                    new_text.GetComponent<Text>().text = trans.GetChild(0).GetComponent<TextMeshPro>().text;
                    new_text.transform.SetParent(arowCanvas.transform);
                    //trans.rotation = trans.parent.InverseTransformDirection(new_text.transform.rotation)
                    new_text.name = GetParent(trans) + m.Value;
                    Undo.RegisterCreatedObjectUndo(new_text, "Create object");
                }
               
            }
        }

        private string GetParent(Transform parent)
        {
            if (parent.parent == null) return null;
            return "name_" + parent.parent.name + "_";
        }

        private Quaternion CaclRotetion(Quaternion oldRect, Quaternion cavasRect)
		{
            Vector3 temp_rote = oldRect.eulerAngles - cavasRect.eulerAngles;
            return  Quaternion.Euler(Vector3.zero); 
		}

        private Quaternion CalcRoteOffset(Vector3 offset, Quaternion oringin)
		{
            return Quaternion.Euler(oringin.eulerAngles + offset);
		}

	}


	public class ChangeProperties : ScriptableWizard
	{
        public GameObject constructionObject;
        public GameObject arrowObject;
        public static void CreateWizard()
		{
            ScriptableWizard.DisplayWizard<ChangeProperties>("ChangeProperties", "Apply", "Select");
        }

        private void OnWizardCreate()
		{

		}
    
        private void OnWizardOtherButton()
		{
            ChangeConstruction();
            ChangeArrow();
        }

        /// <summary>
		/// BestFit = false fontSIze = 50 pivot = [0.5,0.5] scale = 0.0002
		/// </summary>
        private void ChangeConstruction()
		{
            //遍历所有子物体
            if (constructionObject == null) return;    
            foreach(RectTransform trans in constructionObject.GetComponentsInChildren<RectTransform>())
			{
                if (trans.name == constructionObject.name) continue;
                if(trans != null)
				{
                   

                    Text t = trans.GetComponent<Text>();
                    if(t != null)
                    {
                        t.resizeTextForBestFit = false;
                        t.raycastTarget = false;
                        t.fontSize = 50;                       
                        t.alignment = TextAnchor.MiddleCenter;
                        trans.sizeDelta = new Vector2(1000f, 500f);
                    }
                    Outline o = trans.GetComponent<Outline>();
                    if(o != null)
					{
                        o.effectDistance = new Vector2(2f, 2f);
					}
                    trans.pivot = new Vector2(0.5f, 0.5f);
                    trans.localScale = new Vector3(0.0002f, 0.0002f, 0.0002f);
                }
			}
		}

        private void ChangeArrow()
        {
            //遍历所有子物体
            if (arrowObject == null) return;
            foreach (RectTransform trans in arrowObject.GetComponentsInChildren<RectTransform>())
            {
                if (trans.name == arrowObject.name) continue;
                if (trans != null)
                { 
                    
                    Text t = trans.GetComponent<Text>();
                    if (t != null)
                    {
                        t.raycastTarget = false;
                        t.resizeTextForBestFit = false;
                        t.fontSize = 50; 
                        t.alignment = TextAnchor.MiddleCenter;
                    }
                    Outline o = trans.GetComponent<Outline>();
                    if (o != null)
                    {
                        o.effectDistance = new Vector2(2f, 2f);
                    }
                    trans.localScale = new Vector3(0.0002f, 0.0002f, 0.0002f);
                }
            }
        }

    }


}

