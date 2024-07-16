using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InactiveUnitEvent : MonoBehaviour
{
	public Button inactiveUnitButton;

	// Start is called before the first frame update
	void Start()
	{
		if (inactiveUnitButton != null)
		{
			inactiveUnitButton.onClick.AddListener(InactiveUnits);
		}
	}

	void InactiveUnits()
	{
		GameObject[] selectedUnits = GameManager.Instance.GetSelectedUnits();
		GameObject[] inactiveUnitGrounds = GameObject.FindGameObjectsWithTag("InactiveUnitGround");

		if (selectedUnits == null) return ;

		foreach (GameObject unit in selectedUnits)
		{
            UnitAbilites ua = unit.GetComponent<UnitAbilites>();
            for(int i = inactiveUnitGrounds.Length - 1; i >= 0; i--)
			{
				VerifyActivation verifyActivation = inactiveUnitGrounds[i].GetComponent<VerifyActivation>();
				if (!verifyActivation.GetActivation())
				{
                    Transform iugTransform = inactiveUnitGrounds[i].transform;
					Vector3 placePosition = iugTransform.position + new Vector3(0, 1, 0);

					ua.SetActivation(inactiveUnitGrounds[i]);
                    unit.transform.position = placePosition;
					break ;
				}
			}
		}
	}
}
