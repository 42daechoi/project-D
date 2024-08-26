using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class UnitControlManager : MonoBehaviour
{
    public void Initialize()
    {
        EventManager.Instance.OnMove += Move;
        EventManager.Instance.OnHold += Hold;
        EventManager.Instance.OnIsAttack += IsAttack;
        EventManager.Instance.OnAttack += Attack;
    }

    private void OnDisable()
    {
        if (EventManager.Instance != null)
        {
            EventManager.Instance.OnMove -= Move;
            EventManager.Instance.OnHold -= Hold;
            EventManager.Instance.OnIsAttack -= IsAttack;
            EventManager.Instance.OnAttack -= Attack;
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
                unitAbilities.AttackRangeMarkerOn();
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
    
}