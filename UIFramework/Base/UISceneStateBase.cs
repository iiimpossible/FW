using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// 场景状态类
/// 1.加载场景并处理场景进入退出
/// 2.场景加载回调
/// </summary>
public abstract class UISceneStateBase  
{
    public abstract void OnEnter();
    public abstract void OnExit();
    public abstract void OnSceneLoaded(Scene scene, LoadSceneMode mode);    
}
