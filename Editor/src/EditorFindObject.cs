using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
public class WizardFindGameObject : ScriptableWizard
{

    public GameObject curObject;
    public string targetName;

    private List<string> names = new List<string>();
     

    [MenuItem("Tools/Graphy/Dialog")]
    public static void CreateWizard()
    {
        WizardFindGameObject w = DisplayWizard<WizardFindGameObject>("FindGameObject", "Find", "Select");
    }


    private void OnWizardCreate()
    {
        foreach (var item in names)
        {
            Debug.Log(item);
        }

    }

    private void OnWizardUpdate()
    {
        Debug.Log("Please select a gameobjct.");
    }


    private void OnWizardOtherButton()
    {
        curObject = Selection.activeGameObject;
        if (curObject == null) return;
        foreach (var item in curObject.GetComponentsInChildren<Transform>())
        {
            if (item.name == "to_name" || item.name == "name")
            {
                TextMesh mesh = item.GetComponent<TextMesh>();
                if (mesh.text == targetName)
                {
                    names.Add(item.parent.parent.name);
                }
            }
        }
    }

}

