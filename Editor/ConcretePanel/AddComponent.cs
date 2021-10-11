using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Text.RegularExpressions;
public class AddComponent :ScriptableWizard
{
    public string prefix = "to_";//ƥ��ǰ׺
    public string suffix;//ƥ���׺
    public string pattern = @"\w{2}\S\d";//����ƥ��ģʽ

    public GameObject curObject;
    public BoxCollider targetComp;

    private List<string> names = new List<string>(); 

    [MenuItem("Tools/Graphy/AddComponent")]
    public static void CreateWizard()
    {
        AddComponent w = DisplayWizard<AddComponent>("AddComp", "Add", "Select");
    }


    private void OnWizardCreate()
    {
        if (curObject == null) return;

        foreach (var item in curObject.GetComponentsInChildren<Transform>())
        {
            if (GetMatch(item.name))
            {
                item.gameObject.AddComponent<BoxCollider>();
            }
        }

    }

    private void OnWizardUpdate()
    {
        Debug.Log("Please select a gameobjct.");
    }


    private void OnWizardOtherButton()
    {
        curObject = Selection.activeGameObject;       
    }

    private bool GetMatch(string input)
    {
        MatchCollection m = Regex.Matches(input, pattern);
        int count = Regex.Matches(input, pattern).Count;        
        if (count > 0)
        {
            Debug.Log("input: " + input + " pattern: " + pattern);
            names.Add(prefix + m[0].Value+ suffix);           
            return true;
        }        
            return false;          
    }
}
