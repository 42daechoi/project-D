using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	public static GameManager Instance { get; private set; }
    
    // 유닛 생성 및 선택된 유닛 리스트 속성
    public List<GameObject> unitInstanceList;
    public GameObject[] selectedUnits;
    
    public UnitControlManager unitControlManager;
    private bool isAttack = false;
    
    
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

    private void Start()
    {
        unitInstanceList = new List<GameObject>(); ;

        if (unitControlManager != null)
        {
            unitControlManager.Initialize();
        }
    }

    private void Update()
    {
        if (selectedUnits == null)
        {
            return;
        }
        // 유닛 이동
        if (Input.GetMouseButtonDown(1))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                EventManager.Instance.TriggerMove(hit.point);
            }
        }
        
        // 유닛 홀드
        if (Input.GetKeyDown(KeyCode.H))
        {
            EventManager.Instance.TriggerHold();
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            isAttack = true;
            EventManager.Instance.TriggerIsAttack();
        }

        if (Input.GetMouseButtonDown(0) && isAttack != false)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                EventManager.Instance.TriggerAttack(hit.point);
            }
            isAttack = false;
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
            UnitAbilities unitAbilites = unitInstance.GetComponent<UnitAbilities>();
            
            if (!unitAbilites.GetActivation())
            {
                list.Add(unitInstance);
            }
        }
        return list;
    }

    public GameObject[] GetSelectedUnits()
    {
        return selectedUnits;
    }

    public void UpdateSelectedUnit(List<GameObject> selectedUnitsList)
    {
        if (selectedUnits != null)
        {
            foreach (GameObject unit in selectedUnits)
            {
                UnitAbilities unitAbilities = unit.GetComponent<UnitAbilities>();
                if (unitAbilities != null)
                {
                    unitAbilities.DeSelectUnitMarker();
                }
            }
        }

        if (selectedUnitsList == null || selectedUnitsList.Count == 0)
        {
            selectedUnits = null;
            return;
        }

        selectedUnits = selectedUnitsList.ToArray();

        for (int i = 0; i < selectedUnits.Length; i++)
        {
            UnitAbilities unitAbilities = selectedUnits[i].GetComponent<UnitAbilities>();
            if (unitAbilities != null)
            {
                Debug.Log(selectedUnits[i]);
                unitAbilities.SelectUnitMarker();
            }
        }
    }
    
    public void DeselectUnit(GameObject unit)
    {
        if (selectedUnits == null)
        {
            return;
        }

        List<GameObject> updatedSelectedUnits = new List<GameObject>(selectedUnits);
        if (updatedSelectedUnits.Contains(unit))
        {
            updatedSelectedUnits.Remove(unit);
            selectedUnits = updatedSelectedUnits.ToArray();

            UnitAbilities unitAbilities = unit.GetComponent<UnitAbilities>();
            if (unitAbilities != null)
            {
                unitAbilities.DeSelectUnitMarker();
            }
        }
    }
    
}
