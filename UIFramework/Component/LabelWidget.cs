using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LabelWidget : MonoBehaviour
{
    [SerializeField]
    private Text m_label;

    [SerializeField]
    private Text m_text;
    [SerializeField]
    private int m_fontSize = 20;


    public Text lable {get{return m_label;} set{m_label = value;}}
    public Text text {get{return m_label;} set{m_label = value;}}
    public int fontSize {get{return m_fontSize;} set{m_fontSize = value; SetFontSize(value);}}

    void Start()
    {
        
    }

    
    void Update()
    {
        
    }

    private void SetFontSize(int size)
    {
        m_label.fontSize = size;
        m_text.fontSize = size;
    }
}
