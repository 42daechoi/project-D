using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	public static GameManager Instance { get; private set; }
    public List<GameObject> unitInstanceList;
    public GameObject[] SelectedUnits;
    
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
        unitInstanceList = new List<GameObject> (); ;
    }

    private void Start()
    {
        
    }

    public void AddUnitInstance(GameObject unitInstance)
    {
        unitInstanceList.Add(unitInstance);
    }

    public void RemoveUnitInstance(GameObject unitInstance)
    {
        if (unitInstanceList.Contains(unitInstance))
        {
            unitInstanceList.Remove(unitInstance);
            Destroy(unitInstance);
        }
    }

    public List<GameObject> GetActiveUnitList()
    {
        List<GameObject> list = new List<GameObject>();

        foreach (GameObject unitInstance in unitInstanceList)
        {
            UnitAbilites unitAbilites = unitInstance.GetComponent<UnitAbilites>();

            if (!unitAbilites.GetActivation())
            {
                list.Add(unitInstance);
            }
        }
        return list;
    }

    public GameObject[] GetSelectedUnits()
    {
        return SelectedUnits;
    }

    public void UpdateSelectedUnit(List<GameObject> selectedUnitsList)
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
