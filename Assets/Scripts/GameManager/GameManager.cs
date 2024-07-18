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
    public UnitControlManager unitControlManager;
    public event Action<Vector3> OnMove;
    
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
        
        if (unitControlManager != null)
        {
            unitControlManager.Initialize();
        }
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                Debug.Log("이벤트발동! (게임매니저)");
                Debug.Log(hit.point);
                OnMove?.Invoke(hit.point);
                //if (unitControlManager != null)
                //{
                    //unitControlManager.gameObject.SetActive(true); // 우클릭 시 활성화
                //}
            }
        }
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
