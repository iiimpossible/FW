using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GraphyFW.UI
{
    /// <summary>
    /// ����������ʹ��һ��ջ���������е����
    /// ע�⣺��lua�����£� ÿ��UI������һ���ű�����UIʵ���Ϲ��ص������ִ�С�
    /// ���ԣ���ν�UI�����߼�����UIʵ��������һ�����⣬Ҫ��ֱ��д������ˡ�
    /// </summary>
    public class UIPanelManager
    {
        /// <summary>
        /// �洢�Ѿ�����UI���
        /// </summary>
        private Stack<UIBasePanel> stackPanels = new Stack<UIBasePanel>();
 
        /// <summary>
        /// ����UI���͵�����������ϵUIʵ��
        /// </summary>
        /// <typeparam name="System.Type"></typeparam>
        /// <typeparam name="GameObject"></typeparam>
        /// <returns></returns>
        private Dictionary<System.Type, UIBasePanel> dicUIPanel = new Dictionary<System.Type, UIBasePanel>();

        /// <summary>
        /// ��¼�Ѿ���ջ����һ�����
        /// </summary>
        private UIBasePanel lastPanel;

        /// <summary>
        /// ���캯��
        /// </summary>
        public UIPanelManager() { }

        /// <summary>
        /// 1.��UIBasepanel��ջ
        /// 2.��UIBasePanel�ж�ȡ��Ϣ����ȡUIԤ�����·����Ȼ��ͨ��UIManager��¡Ԥ����
        /// 3.��UI�Ĵ������˳��¼������������ĳ�ս��ջ������
        /// </summary>
        /// <param name="nextPanel">Ҫ��ʾ����� </param>
        public void OpenPanel(System.Type uiType)
        {
            //���ջ��Ϊ�գ���ô��ջ��Ui�������
            if (stackPanels.Count > 0) 
                stackPanels.Peek()?.OnPause();
            //��Ϊ���ʵ���ϲ���һ���ܹ���ʾ��UI�������뵽UI��������ʹ��·���ҵ�UI��Prefab��Ȼ���¡һ��UI��ע�ᵽUI���������������뵽Canvas��
            UIBasePanel bpanel = OpenUI(uiType);
            if(bpanel != null) 
                stackPanels.Push(bpanel);
            //��ʼ��Ui�Ľű�
            // if(nextPanel.uiType.executeLua)
            // {
            //     nextPanel.InitializeUIBehaviour(new GraphyFW.UIBehavior(nextPanel.uiType.luaCode,nextPanel.uiType.luaName,nextPanel));           
            // }  
            bpanel.uiGo?.SetActive(true);
            bpanel?.OnEnter();
            //�¼�ע��
            // bpanel.OnExitUI += this.PopPanelCallBack;
            // bpanel.OnCreateUI += this.PushPanel;
        }

        /// <summary>
        /// ����λ��ջ����panel
        /// </summary>
        public void ClosePanel()
        {
            //ע�⣺��ջ�������󣬲�ȷ��ջ���Ƿ��ж�������Ҫ�������
            if (stackPanels.Count > 0)
            {
                UIBasePanel basePanel = stackPanels.Peek();
                if(basePanel != null)
                {
                    //basePanel.OnExit();
                    basePanel.uiGo.SetActive(false);
                    Debug.Log($"Pop ui [{stackPanels.Peek().uiGo.name}]");
                    stackPanels.Pop();
                }    
                else
                {
                    Debug.Log($"Pop ui failed. [{stackPanels.Peek().uiGo.name}]");
                } 
            }
            if (stackPanels.Count > 0)
            {
                UIBasePanel p = stackPanels.Peek();
                if(p != null)
                {
                    p.uiGo?.SetActive(true);
                    p.OnResume();
                }
                 
            }
        }

        /// <summary>
        /// ���رճ�����ʱ�򣬽���ǰ�����д򿪵�UI����
        /// </summary>
        public void CloseScene()
        {             
            var array = dicUIPanel.Keys;
            foreach (var item in array)
            {
                GameObject.Destroy(dicUIPanel[item].uiGo);                        
            }
            dicUIPanel.Clear();
            stackPanels.Clear();
        }

        /// <summary>
        /// ִ��λ��ջ����UIPanel��UPdate������������UI�Ϲ��ؽű���ִ��UPdate
        /// </summary>
        public void UpdateTopPanel()
        {
            if (stackPanels.Count > 0)
            {
                stackPanels.Peek().Update();
            }
        }

        // public void PopPanelCallBack(UIBasePanel panel)
        // {
        //     PopPanel();
        // }

        // public void PushPanelCallBack(UIBasePanel panel)
        // {
        //     PushPanel(panel);
        // }

        /// <summary>
        /// ����UI���ͣ����ֵ��ֻ�ȡUI����ʵ��
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private UIBasePanel GetUIPanel(System.Type type)
        {
            if (dicUIPanel.ContainsKey(type))
            {
                return dicUIPanel[type];
            }
            return default(UIBasePanel);
        }




        /// <summary>
        /// 1.Ѱ��Canvas����
        /// 2.����Ƿ��Ѿ����ڸ����͵�UI����
        /// 3.��������ڸ����͵�UI���󣬴Ӹ����͵�·��������һ��,�������UI�������ø�����
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public UIBasePanel OpenUI(System.Type type)
        {            
            GameObject canvas = GameObject.Find("Canvas");
            if (!canvas)
            {
                Debug.LogError("Canvas is Not Exist in current scene, please check that Canvas is created.");
                return null;
            }

            if (dicUIPanel.ContainsKey(type))
            {
                return dicUIPanel[type];
            }

            //ע�⣺ʹ��Resources.Loadһ��Ҫ��Resources�ļ�����
            UIBasePanel bPanel = type.Assembly.CreateInstance("GraphyFW.UI."+type.Name) as UIBasePanel;
            if (bPanel == null)
            {
                Debug.LogError($"UIBasePanel create instance failed. Name: [{type.Name}]");
                return default(UIBasePanel);
            }
            //�����������ƣ���Prefabs/UI/ �л�ȡUi����ʵ��
            GameObject prefab = Resources.Load<GameObject>($"Prefabs/UI/{type.Name}");
            if (!prefab)
            {
                Debug.LogError("UI prefab path is not valid.  Path: [Prefabs/UI/]" + type.Name);
                return null;
            }
            
            GameObject go = GameObject.Instantiate(prefab, canvas.transform);
            bPanel.uiGo = go;
            bPanel.OnCreate();//ÿ��UI���������ڣ�ʵ����ʱ��ֻ����һ��
            dicUIPanel.Add(type, bPanel);
            Debug.Log($"Load ui: [{bPanel.GetType().Name}]");
            return bPanel;
        }

        /// <summary>
        /// ����һ����Ӧ���͵�UI����
        /// </summary>
        /// <param name="type"></param>
        public void DestroyUI(System.Type type)
        {
            if (dicUIPanel.ContainsKey(type))
            {
                GameObject.Destroy(dicUIPanel[type].uiGo);
                dicUIPanel.Remove(type);
            }
        }

        /// <summary>
        /// �������ͨ������ʵ��������ô���ö��󹤳�
        /// </summary>
        /// <param name="uiType"></param>
        /// <returns></returns>
        public UIBasePanel UIFactory(string uiType)
        {
            switch (uiType)
            {
                case "UIGameSettingPanel":
                    {
                        return new UIGameSettingPanel();
                }
                default: 
                    return default(UIBasePanel);
            }
        }


    }

}

