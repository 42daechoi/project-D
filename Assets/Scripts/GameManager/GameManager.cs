using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	public static GameManager Instance { get; private set; }
	private GameObject[] SelectedUnits;

	void Awake()
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
    
    public void InitSelectedUnit(List<GameObject> selectedUnitsList)
    {
        if (selectedUnitsList == null || selectedUnitsList.Count == 0)
        {
            SelectedUnits = null;
            return;
        }
        SelectedUnits = selectedUnitsList.ToArray();
        
        for (int i = 0; i < SelectedUnits.Length; i++)
        {
            Debug.Log(SelectedUnits[i]);
        }
    }
}
