using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitSelector : MonoBehaviour
{
    public static UnitSelector Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SelectUnits(RectTransform selectionBoxRect)
    {
        Vector2 min = selectionBoxRect.anchoredPosition - (selectionBoxRect.sizeDelta / 2);
        Vector2 max = selectionBoxRect.anchoredPosition + (selectionBoxRect.sizeDelta / 2);

        foreach (GameObject unit in GameObject.FindGameObjectsWithTag("Unit"))
        {
            if (unit.GetComponent<UnitAbilites>().GetActivation())
            {
                Vector2 unitPosition = RectTransformUtility.WorldToScreenPoint(Camera.main, unit.transform.position);

                if (unitPosition.x >= min.x && unitPosition.x <= max.x && unitPosition.y >= min.y && unitPosition.y <= max.y)
                {
                    Debug.Log("Unit Selected: " + unit.name);
                }
            }
        }
    }
}
