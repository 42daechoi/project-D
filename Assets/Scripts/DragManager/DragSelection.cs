using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DragSelection : MonoBehaviour
{
    public Image selectionBoxImage;
    private RectTransform selectionBoxRect;

    private Vector2 startPosition;
    private Vector2 endPosition;

    void Start()
    {
        selectionBoxRect = selectionBoxImage.GetComponent<RectTransform>();
        selectionBoxImage.gameObject.SetActive(false);
    }

    public void UpdateDragSelection()
    {
        if (Input.GetMouseButtonDown(0) && Input.mousePosition.x > 400 && Input.mousePosition.x < 1500)
        {
            startPosition = Input.mousePosition;
            selectionBoxImage.gameObject.SetActive(true);
        }
        if (Input.GetMouseButton(0))
        {
            endPosition = Input.mousePosition;
            UpdateSelectionBox();
        }
        if (Input.GetMouseButtonUp(0))
        {
            UnitSelector.Instance.SelectUnits(selectionBoxRect);
            selectionBoxImage.gameObject.SetActive(false);
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
