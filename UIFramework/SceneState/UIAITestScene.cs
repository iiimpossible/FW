using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GraphyFW.UI
{
    public class UIAITestScene : UISceneStateBase
    {
      
        public override void OnEnter()
        {
            MessageManager.instance.AddListener(EMessageType.OnMapLoaded, OnMapLoaded);
            SceneManager.sceneLoaded += this.OnSceneLoaded;
            //TODO: 找到UiManger,打开根UI
            SceneManager.LoadScene("Sce_AITest");
            
        }

        public override void OnExit()
        {  
            SceneManager.sceneLoaded -= this.OnSceneLoaded;
        }

        public override void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            Debug.Log("Open Panel Ui Game panel");
            ScptUIManager.instance.uiPanelManager.CloseScene();
            ScptUIManager.instance.uiPanelManager.OpenPanel(typeof(UIWaitPanel));           

        }

        private void OnMapLoaded(Message message)
        {
            Debug.Log("Map loaded.");          
            ScptUIManager.instance.uiPanelManager.ClosePanel();
            ScptUIManager.instance.uiPanelManager.OpenPanel(typeof(UIAITestPanel));
        }
    }

}
