using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace GraphyFW.UI
{
    public enum EScene
    {
        START = 1,
        GAME = 2,
        LOAGING = 3,

        AITEST = 4,

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
            {EScene.LOAGING, new UILoadingScene()},
            {EScene.AITEST,new UIAITestScene()},
        };

        private static UISceneStateBase curScene;

        private List<GameObject> dontDestroyObjects = new List<GameObject>();

        private bool isFirstStart =  true;

        private void Awake()
        {
            instance = this;          
            DontDestroyOnLoad(gameObject);
        }

        private void Start() 
        {
           
        }

        /// <summary>
        /// 打开一个场景
        /// </summary>
        /// <param name="scene"></param>
        /// <returns></returns>
        public void OpenScene(EScene scene)
        {
            curScene?.OnExit();
            UISceneStateBase us = dicScene[scene];
            if(us != null)
            {
                us.OnEnter();
                curScene = us;                
            }

            ///当从GameScene返回到StartScene, 被指定为不销毁的物体就会复制一份，所以在这里销毁
            if(scene == EScene.START && !isFirstStart)
            {
                foreach (var item in dontDestroyObjects)
                {
                    Destroy(item);
                }
                Destroy(gameObject);
            }  
            isFirstStart = false;         
        }


        public void  SetDontDestroyObjet(GameObject obj)
        {
            this.dontDestroyObjects.Add(obj);
            DontDestroyOnLoad(obj);
        }


        

      
    }

}

