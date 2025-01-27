using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitSelector : MonoBehaviour
{
	public void SelectUnits(RectTransform selectionBoxRect)
	{
		Vector2 min = selectionBoxRect.anchoredPosition - (selectionBoxRect.sizeDelta / 2);
		Vector2 max = selectionBoxRect.anchoredPosition + (selectionBoxRect.sizeDelta / 2);
		List<GameObject> selectedUnitList = new List<GameObject>();

		foreach (GameObject unit in GameManager.Instance.GetActiveUnitList())
		{
			Vector2 unitPosition = RectTransformUtility.WorldToScreenPoint(Camera.main, unit.transform.position);

			if (unitPosition.x >= min.x && unitPosition.x <= max.x && unitPosition.y >= min.y && unitPosition.y <= max.y)
			{
				selectedUnitList.Add(unit);
			}
		}
		GameManager.Instance.UpdateSelectedUnit(selectedUnitList);
	}
}
