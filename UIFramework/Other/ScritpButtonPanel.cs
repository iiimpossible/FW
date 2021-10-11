using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ScritpButtonPanel : MonoBehaviour,IBeginDragHandler, IEndDragHandler, IDragHandler
{
    public ScrollRect compScrollRect;
    private GridLayoutGroup compLayout;
    private RectTransform compRectTrans;

    private int columnCount;
    private int childCount;
    public int ySpacing;
    public int yPaddin;
    public int cellSize;

    public int upOffset = 0;
    public int bottomOffset = 0;
    public float lerpK = 0.2f;
    public float heightK = 0.5f;

    private int rowCount = 0;
    private bool isDrag = false;

    private void Awake()
    {
        compRectTrans = GetComponent<RectTransform>();
        compLayout = GetComponent<GridLayoutGroup>();

        columnCount = compLayout.constraintCount;
        childCount = transform.childCount;    
    }

    private int GetPanelHeight()
    {
        //要求整个面板的高度，要知道行数，Y轴向的spacing padding
        //高度 = （行数 * 子物体高度）+ 2* padding + （行数-1）*（spacing）
        //行数 = （子物体数/列数）
        int tag = 0;
        tag = (childCount % columnCount) == 0 ? 0 : 1;
        int row = (childCount / columnCount) + tag;
        rowCount = row;
        int height = (row * cellSize) + 2 * yPaddin + (row - 1) * (ySpacing);
        return height;
    }

    private void Update()
    {        
        float h = GetPanelHeight();
        if (compRectTrans.localPosition.y > heightK * h &&  !isDrag)
        {           
            compScrollRect.StopMovement(); 
            compRectTrans.localPosition = new Vector2(0.0f, Mathf.Lerp(compRectTrans.localPosition.y, (h - h / rowCount - upOffset), lerpK));
        }
        else if (compRectTrans.localPosition.y < 0 && !isDrag)
        {            
            compScrollRect.StopMovement();
            compRectTrans.localPosition = new Vector2(0.0f, Mathf.Lerp(compRectTrans.localPosition.y, 0, lerpK));
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        isDrag = true;
        compScrollRect.OnBeginDrag(eventData);         
    }

    public void OnEndDrag(PointerEventData eventData)
    {               
        compScrollRect.OnEndDrag(eventData);
        isDrag = false;
    }


    public void OnDrag(PointerEventData eventData)
    {         
      compScrollRect.OnDrag(eventData);       
    }


}