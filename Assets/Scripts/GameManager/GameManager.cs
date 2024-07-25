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
    
    // 유닛컨트롤 관련 이벤트
    public event Action<Vector3> OnMove;
    public event Action OnHold;
    public event Action<Vector3> OnAttack;
    
    
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
        
        // 유닛 이동
        if (Input.GetMouseButtonDown(1))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                Debug.Log("이벤트발동! (게임매니저)");
                Debug.Log(hit.point);
                OnMove?.Invoke(hit.point);
            }
        }
        
        // 유닛 홀드
        if (Input.GetKeyDown(KeyCode.H))
        {
            OnHold?.Invoke();
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            isAttack = true;
            Debug.Log("isAttack = true");
        }

        if (Input.GetMouseButtonDown(0) && isAttack != false)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                Debug.Log("공격 이벤트 발동! (게임매니저)");
                OnAttack?.Invoke(hit.point);
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
        return selectedUnits;
    }

    public void UpdateSelectedUnit(List<GameObject> selectedUnitsList)
    {
        if (selectedUnits != null)
        {
            foreach (GameObject unit in selectedUnits)
            {
                UnitAbilites unitAbilities = unit.GetComponent<UnitAbilites>();
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
            UnitAbilites unitAbilites = selectedUnits[i].GetComponent<UnitAbilites>();
            if (unitAbilites != null)
            {
                Debug.Log(selectedUnits[i]);
                unitAbilites.SelectUnitMarker();
            }
        }
    }
    
    
}
