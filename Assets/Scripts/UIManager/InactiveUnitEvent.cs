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
		Debug.Log(inactiveUnitGrounds.Length);


		if (selectedUnits == null) return ;

		foreach (GameObject unit in selectedUnits)
		{
            UnitAbilites ua = unit.GetComponent<UnitAbilites>();
            foreach (GameObject iug in inactiveUnitGrounds)
			{
				VerifyActivation verifyActivation = iug.GetComponent<VerifyActivation>();
				if (!verifyActivation.GetActivation())
				{
                    Transform iugTransform = iug.transform;
					Vector3 placePosition = iugTransform.position + new Vector3(0, 1, 0);

                    ua.SetActivation(iug);
                    unit.transform.position = placePosition;
					break ;
				}
			}
		}
	}
}
