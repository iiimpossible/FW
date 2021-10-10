using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scpt_PanelController: MonoBehaviour
{
    private UIPanelManager panelManager;

    private void Awake()
    {
        panelManager = new UIPanelManager();
    }

    // Start is called before the first frame update
    void Start()
    {
        panelManager.PushPanel(new UIStartPanel());
    }

       

    // Update is called once per frame
    void Update()
    {
        
    }
}
