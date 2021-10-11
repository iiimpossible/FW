using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
public class EditorTest
{


    [MenuItem("Tools/Show",false, 1)]
    public static void EditorAddToolItem ()
    {
        Debug.Log("Hellow");
    }

    [MenuItem("Tools/Show/ShownCurObjectName", false, 1)]
    public static void ShowObjectChildren()
    {
        //Debug.Log(Selection.activeObject.name);
        //Debug.Log(Selection.assetGUIDs);

        GameObject cur_obj = Selection.activeGameObject;
        if (cur_obj == null) return;
        Transform[] trans = cur_obj.transform.GetComponentsInChildren<Transform>();
        Debug.Log(CalculateObjctAllChildrenNum(cur_obj.transform));
        //Debug.Log(trans.Length+ " name： "+ trans[0].name);
       
        //
    }

    /// <summary>
    /// 递归计算Gameobject所有子物体，包括子物体的子物体
    /// </summary>
    public static int CalculateObjctAllChildrenNum(Transform root)
    {
        if (root.childCount == 0) return 0; 
        int count = 0;
        Debug.Log(root.name);
        for (int i = 0; i < root.childCount; i++)
        {
            Transform trans = root.GetChild(i);
            
            int t = CalculateObjctAllChildrenNum(trans);
            Debug.Log(t);
            count += t;
        } 
        return count;   
    }


    

}
