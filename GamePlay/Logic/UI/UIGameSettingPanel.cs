using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace GraphyFW.UI
{
    /// <summary>
    /// 游戏开始设置界面
    /// </summary>
    public class UIGameSettingPanel : UIBasePanel
    {
        //数据从哪里查找，又怎么读取呢？
        private static readonly string info =
        @"<color=white><size=50>小黄家蚁</size>
<size=30> 描述：
小黄家蚁，Monomoriumpharaonis(Linnaeus,1758)，中文正式名为“法老小家蚁”，属昆虫纲、膜翅目、蚁科、切叶蚁亚科，小家蚁属的一种蚂蚁。
体型：
	小
特性：
真菌农场：可以将植物的叶子切割下来运进洞穴培育真菌，从而以真菌为食物
分化：

    雄蚁：普通
    工蚁：切叶，真菌种植
    蚁后：普通
</size>
</color>";

        public static readonly string path = "Prefabs/UI/UIGameSettingPanel";

        private GameObject content;
        private GameObject settingWidget;
        private int _mapSize = 0;
        private float _blackRate = 0;
        private string _mapSeed = "";

        public override void OnCreate()
        {
            InitClicks();
        }

        public override void OnEnter()
        {
            settingWidget.SetActive(false);
            base.OnEnter();
        }

        public override void OnExit()
        {
            Debug.Log("Child exit");
            base.OnExit();
        }

        public void InitClicks()
        {
            //获得关键Widget的引用
            GameObject AntTypeWdiget = FindChild("AntTypeWidget");
            content = FindChild(AntTypeWdiget, "Content");
            settingWidget = FindChild("SettingWidget");

            //初始化列表的按钮的点击事件
            if (content != null)
            {
                for (int i = 0; i < content.transform.childCount; i++)
                {
                    Transform item = content.transform.GetChild(i);
                    if (item)
                    {
                        //string type = item.Find("Text").GetComponent<Text>().text;
                        item.GetComponent<Button>().onClick.AddListener(() =>
                        {
                            var txt = GetChildComponentAnyway<Text>("Txt_Info");
                            txt.text = "info";
                        });
                    }
                }
            }

            //返回按钮
            GetChildComponent<Button>("Btn_Back")?.onClick.AddListener(() =>
            {
                ScptUIManager.instance.uiPanelManager.ClosePanel();
            });

            //应用（确认）按钮 开始游戏
            GetChildComponent<Button>("Btn_Apply")?.onClick.AddListener(() =>
            {
                ScptSceneManger.instance.OpenScene(EScene.GAME);
            });

            GetChildComponent<Button>("Btn_Setting")?.onClick.AddListener(() =>
            {
                //TODO：打开设置面板
                settingWidget.SetActive(true);
                //this.OnPause();
            });

            GetChildComponent<Button>(settingWidget, "Btn_STW_Back")?.onClick.AddListener(() =>
            {
                //TODO: 将MapManager 的数据设置为当前的数据
                MapManager.instance.mapSize = new Vector2Int(_mapSize, _mapSize);
                MapManager.instance.blackRate = _blackRate;
                MapManager.instance.mapSeed = _mapSeed;
                settingWidget.SetActive(false);
                //this.OnResume();

            });

            GetChildComponent<InputField>(settingWidget, "Inf_MapSize")?.onValueChanged.AddListener((value) =>
            {
                _mapSize = System.Convert.ToInt32(value);
                Debug.Log(_mapSize);
            });

            GetChildComponent<InputField>(settingWidget, "Inf_MapSeed")?.onValueChanged.AddListener((value) =>
            {
             _mapSeed = value;
             Debug.Log(_mapSeed);
            });

            GetChildComponent<InputField>(settingWidget, "Inf_BlackRate")?.onValueChanged.AddListener((value) =>
            {
             _blackRate = (float)System.Convert.ToDouble(value);
             Debug.Log(_blackRate);
            });

        }


        /// <summary>
        /// 获取蚂蚁类型列表，将当前定义的蚂蚁列表显示
        /// 1.获取content
        /// 2.读取蚂蚁类型，然后实例对应的item，填充到content里边
        /// </summary>
        private void InitAntTypeList()
        {
           
            // int antTypeName = 7;
            // for (int i = 0; i < antTypeName; i++)
            // {
            //     GameObject item = GameObject.Instantiate<GameObject>(Resources.Load<GameObject>("Prefabs/UIAnyTypeListItem"));
            //     item.transform.SetParent(content.transform);
            // }

            //TODO: 实例Item到content下边去

        }
    }

}
