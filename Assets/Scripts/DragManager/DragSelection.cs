using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DragSelection : MonoBehaviour
{
    public Image selectionBoxImage;
    private UnitSelector unitSelector;
    private RectTransform selectionBoxRect;

    private Vector2 startPosition;
    private Vector2 endPosition;
    private bool isInAllowArea;

    void Start()
    {
        selectionBoxRect = selectionBoxImage.GetComponent<RectTransform>();
        unitSelector = GetComponent<UnitSelector>();
        selectionBoxImage.gameObject.SetActive(false);
    }

    public void UpdateDragSelection()
    {
        if (Input.GetMouseButtonDown(0))
        {
            isInAllowArea = Input.mousePosition.x > 400 && Input.mousePosition.x < 1500 && Input.mousePosition.y > 160;
            if (isInAllowArea)
            {
                startPosition = Input.mousePosition;
                selectionBoxImage.gameObject.SetActive(true);
            }
        }
        if (Input.GetMouseButton(0) && isInAllowArea)
        {
            endPosition = Input.mousePosition;
            UpdateSelectionBox();
        }
        if (Input.GetMouseButtonUp(0))
        {
            if (isInAllowArea) 
            {
                unitSelector.SelectUnits(selectionBoxRect);
                selectionBoxImage.gameObject.SetActive(false);
            }
            isInAllowArea = false;
        }
    }

    void UpdateSelectionBox()
    {
        Vector2 boxStart = startPosition;
        Vector2 boxEnd = endPosition;

        float width = boxEnd.x - boxStart.x;
        float height = boxEnd.y - boxStart.y;

        selectionBoxRect.sizeDelta = new Vector2(Mathf.Abs(width), Mathf.Abs(height));
        selectionBoxRect.anchoredPosition = boxStart + new Vector2(width / 2, height / 2);
    }
}
