using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIStartScene : UISceneStateBase
{ 

    private readonly string sceneName = "Sce_Enter";
    public override void OnEnter()
    {
        SceneManager.sceneLoaded += this.OnSceneLoaded;
        //TODO: 找到UiManger,打开根UI
        SceneManager.LoadScene(sceneName);
        
        //SceneManager.GetActiveScene().on

    }

    public override void OnExit()
    {
        //TODO：从场景管理器里边去掉回调
        SceneManager.sceneLoaded -=  this.OnSceneLoaded;
    }

    public override void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log(string.Format($"Scene [{sceneName}] is loaded."));
    }
}
