/*
 *@FileName:		SinMaskEffect
 *@Author:	        ZhaoMengYao
 *@Date:	        #ZMY_DATETIME#
 *@Description:  
*/
using UnityEngine;
using UnityEngine.UI;

public class SinMaskEffect : MonoBehaviour
{
    public Transform m_Canvas;
    public Camera uiCamera;
    [Range(0, 0.5f)]
    public float yScale = 0;

    Vector4 maskRect;//x-left,y-right,down-z,up-w

    public RectTransform m_rectTrans;

    float m_halfWidth, m_halfHeight, m_canvasScale;
    Material maskMat;

    private readonly Vector3[] m_Corners = new Vector3[4];
    RenderTexture rt;
    RawImage ri;

    bool init = false;
    private void Awake()
    {
       // InitSinMask();
    }
    public void InitSinMask()
    {
        if (init)
            return;
        rt = new RenderTexture(Screen.width, Screen.height, 24, RenderTextureFormat.ARGB32);
        uiCamera.targetTexture = rt;
 
        RenderTexture.active = rt;

        ri = GetComponent<RawImage>();
        maskMat = ri.material;
        init = true;
    }
    private void OnDisable()
    {
        Destroy(rt);
    }

    void UpdateSinMask()
    {
        maskMat.SetTexture("_MainTex", rt);
        ri.rectTransform.sizeDelta = new Vector2(Screen.width, Screen.height);
        m_canvasScale = m_Canvas.localScale.x;
        m_halfWidth = m_rectTrans.sizeDelta.x * 0.5f * m_canvasScale;
        m_halfHeight = m_rectTrans.sizeDelta.y * 0.5f * m_canvasScale;
        Vector4 area = new Vector4(m_rectTrans.position.x - m_halfWidth, m_rectTrans.position.y - m_halfHeight, m_rectTrans.position.x + m_halfWidth, m_rectTrans.position.y + m_halfHeight);
        maskMat.SetVector("_ClipArea", area);

        m_rectTrans.GetWorldCorners(m_Corners);
        maskRect.x = uiCamera.WorldToScreenPoint(m_Corners[0]).x;
        maskRect.y = uiCamera.WorldToScreenPoint(m_Corners[2]).x;
        maskRect.z = uiCamera.WorldToScreenPoint(m_Corners[0]).y;
        maskRect.w = uiCamera.WorldToScreenPoint(m_Corners[1]).y;
        //Debug.Log("x: " + maskRect.x + " y: " + maskRect.y + " z: " + maskRect.z + " w: " + maskRect.w);
        maskRect.x /= Screen.width;
        maskRect.y /= Screen.width;
        maskRect.z /= Screen.height;
        maskRect.w /= Screen.height;
        maskMat.SetVector("_ClipUV", maskRect);
        maskMat.SetFloat("_YScale", yScale);
        //Debug.Log("222x: " + maskRect.x + " y: " + maskRect.y + " z: " + maskRect.z + " w: " + maskRect.w);

        maskMat.SetFloat("_UDelta", maskRect.y - maskRect.x);
        maskMat.SetFloat("_VDelta", maskRect.w - maskRect.z);

        // Debug.Log("Udelta: " + (maskRect.y - maskRect.x) + " vdelta: " + (maskRect.w - maskRect.z));
    }

    // Update is called once per frame
    void Update()
    {
        if (init)
            UpdateSinMask();

    }
}
