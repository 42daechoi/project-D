using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonBinder : MonoBehaviour
{
	public GameObject unitObject;
	public GachaManager gachaManager;

	private void Start()
	{
		SpawnUnitButtonSetting();
	}

	public void SpawnUnitButtonSetting()
	{
		GameObject gachaUnit = gachaManager.GachaUnits();
		unitObject = gachaUnit;
	}
}
