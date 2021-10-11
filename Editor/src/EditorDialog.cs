using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Reflection;

public class EditorDialog  :ScriptableWizard
{
    public GameObject cur_obj;
    
    public Color color;

    public Component currentComponent;
    public List<string> testList;

    private float deltaTime = 0.5f;
    private float curTime = 0;

    [MenuItem("Tools/Dialog")]
    public static void CreateWizard()
    {

        EditorDialog e = DisplayWizard<EditorDialog>("WizardTest","Apply","Select");
        
       
    }
    private void Awake()
    {
        cur_obj = Selection.activeGameObject;
         
    }

    private void Update()
    {
        
    }

    

   void SetGameObject()
    {

    }




    private void OnWizardCreate()
    {
        
    }

    private void OnWizardUpdate()
    {
        Debug.Log("Please select a gameobjct.");
    }

    /// <summary>
    /// 对选择的游戏物体进行一些操作
    /// </summary>
    private void OnWizardOtherButton()
    {
        if(Selection.activeGameObject)
		{
            GetAllField(currentComponent);
        }
        
    }

    delegate void  DelegateTimeCounter();
    private void TimeCounter(float delay, bool repeating, DelegateTimeCounter func)
    {
        

    }

    /// <summary>
    /// 打印指定的组件的所有字段
    /// </summary>
    /// <param name="comp"></param>
    private void GetAllField(Component comp)
	{
        //获取参数的动态类型
        System.Type dy_type = comp.GetType();
        FieldInfo[] infos = dy_type.GetFields();
        Debug.Log("type is: " + dy_type.Name);
        foreach (FieldInfo item in infos)
		{           
            Debug.Log(item.Name);
		}
	}

    /// <summary>
	/// 打印指定的组件的所有属性
	/// </summary>
    private void GetAllProperty()
    {

    }

    /// <summary>
	/// 打印指定的组件的所有方法
	/// </summary>
    private void GetAllMethoud()
    {

    }


}
