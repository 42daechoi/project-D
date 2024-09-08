using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class UnitControlManager : MonoBehaviour
{
    public void Initialize()
    {
        if (EventManager.Instance == null)
        {
            Debug.LogError("EventManager.Instance is null in UnitControlManager.Initialize.");
            return;
        }
        EventManager.Instance.OnMove += Move;
        EventManager.Instance.OnHold += Hold;
        EventManager.Instance.OnIsAttack += IsAttack;
        EventManager.Instance.OnAttack += Attack;
        EventManager.Instance.OnTargetAttack += TargetAttack;
    }

    private void OnDisable()
    {
        if (EventManager.Instance != null)
        {
            EventManager.Instance.OnMove -= Move;
            EventManager.Instance.OnHold -= Hold;
            EventManager.Instance.OnIsAttack -= IsAttack;
            EventManager.Instance.OnAttack -= Attack;
            EventManager.Instance.OnTargetAttack -= TargetAttack;
        }
    }

    public void Move(Vector3 targetPosition)
    {
        foreach (GameObject unit in GameManager.Instance.GetSelectedUnits())
        {
            UnitAbilities unitAbilities = unit.GetComponent<UnitAbilities>();
            if (unitAbilities != null)
            {   
                unitAbilities.IsMoveToOn();
                unitAbilities.MoveTo(targetPosition);
            }
        }
    }

    public void Hold()
    {
        foreach (GameObject unit in GameManager.Instance.GetSelectedUnits())
        { 
            UnitAbilities unitAbilities = unit.GetComponent<UnitAbilities>();
            if (unitAbilities != null)
            {
                unitAbilities.HoldPosition();
            }
        }   
    }

    public void IsAttack()
    {
        foreach (GameObject unit in GameManager.Instance.GetSelectedUnits())
        {
            UnitAbilities unitAbilities = unit.GetComponent<UnitAbilities>();
            if (unitAbilities != null)
            {
                //unitAbilities.AttackRangeMarkerOn();
            }
        }
    }

    public void Attack(Vector3 targetPosition)
    {
        foreach (GameObject unit in GameManager.Instance.GetSelectedUnits())
        {
            UnitAbilities unitAbilities = unit.GetComponent<UnitAbilities>();
            if (unitAbilities != null)
            {
                unitAbilities.AttackToMove(targetPosition);
                Debug.Log("유닛어택!(유닛컨트롤매니저)");
            }
        }
    }

    public void TargetAttack(Monster currentTarget)
    {
        Debug.Log("정상적으로 TargetAttack까진 들어왔음");
        foreach (GameObject unit in GameManager.Instance.GetSelectedUnits())
        {
            UnitAbilities unitAbilities = unit.GetComponent<UnitAbilities>();
            if (unitAbilities != null)
            {
                unitAbilities.AttackToMove(currentTarget); //몬스터의 좌표와 유닛의 좌표를 불러와서 계산해서 좌표넣어야함
                Debug.Log("유닛어택!(유닛컨트롤매니저)");
            }
        }
    }
    
}