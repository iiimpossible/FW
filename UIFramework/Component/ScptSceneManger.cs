using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace GraphyFW.UIFW
{
    public enum EScene
    {
        START = 1,
        GAME = 2,
        LOAGING = 3,

    }

    /// <summary>
    /// 场景管理器
    /// </summary>
    public class ScptSceneManger : MonoBehaviour
    {

        public static ScptSceneManger instance{get;private set;}
        private static Dictionary<EScene,UISceneStateBase>  dicScene = new Dictionary<EScene, UISceneStateBase>
        {
            {EScene.START, new UIStartScene()},
            {EScene.GAME, new UIGameScene()},
            {EScene.LOAGING, new UILoadingScene()}
        };

        private static UISceneStateBase curScene;

        private void Awake()
        {
            instance =this;
            DontDestroyOnLoad(gameObject);
        }

        private void Start() {
            curScene = SetScene(EScene.START);           
        }
        public UISceneStateBase SetScene(EScene scene)
        {
            curScene?.OnExit();
            UISceneStateBase us = dicScene[scene];
            us?.OnEnter();
            return us;
        }

      
    }

}

