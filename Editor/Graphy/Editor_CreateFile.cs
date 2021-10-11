using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Text;
using UnityEditor.Experimental.AssetImporters;
public class Editor_CreateFile : Editor
{

    /// <summary>
    /// Unity Editor 下右键创建文本类文件
    /// </summary>
    public class CreateFileEditor : Editor
    {
        [MenuItem("Assets/Create/Lua File", false, 80)]
        static void CreateLuaFile()
        {
            CreateFile("lua");
        }
        [MenuItem("Assets/Create/Text File", false, 81)]
        static void CreateTextFile()
        {
            CreateFile("txt");
        }
        /// <summary>
        /// 创建文件类的文件
        /// </summary>
        /// <param name="fileEx"></param>
        static void CreateFile(string fileEx)
        {
            //获取当前所选择的目录（相对于Assets的路径）
            var selectPath = AssetDatabase.GetAssetPath(Selection.activeObject);
            var path = Application.dataPath.Replace("Assets", "") + "/";
            var newFileName = "new_" + fileEx + "." + fileEx;
            var newFilePath = selectPath + "/" + newFileName;
            var fullPath = path + newFilePath;
            //简单的重名处理
            if (File.Exists(fullPath))
            {
                var newName = "new_" + fileEx + "-" + UnityEngine.Random.Range(0, 100) + "." + fileEx;
                newFilePath = selectPath + "/" + newName;
                fullPath = fullPath.Replace(newFileName, newName);
            }
            //如果是空白文件，编码并没有设成UTF-8
            File.WriteAllText(fullPath, "-- test", Encoding.UTF8);
            AssetDatabase.Refresh();
            //选中新创建的文件
            var asset = AssetDatabase.LoadAssetAtPath(newFilePath, typeof(Object));
            Selection.activeObject = asset;
        }
    }

}


// [ScriptedImporter(1,".lua")]
// public class  MyImpoter: ScriptedImporter
// {
//     public override void OnImportAsset(AssetImportContext ctx)
//     {
//         string str = File.ReadAllText(ctx.assetPath);
//         TextAsset asset = new TextAsset(str);
//         ctx.AddObjectToAsset("obj", asset);
//         ctx.SetMainObject(asset);
//         //throw new System.NotImplementedException();
//     }
// }



/*
    限定：10个模型·
    点赞点踩记录：上滑点赞并记录，下滑点踩并记录，一个周期内不重复，多个周期可重复记录
    用户模型推送：
        一个周期：随机选取一个模型展示，不重复选取，选取完毕则进入下一个周期
        多个周期：完成一个选取周期后消除选取记录，重新选取


*/