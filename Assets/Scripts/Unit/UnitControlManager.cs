using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class UnitControlManager : MonoBehaviour
{
    public void Initialize()
    {
        GameManager.Instance.OnMove += Move;
        GameManager.Instance.OnHold += Hold;
        GameManager.Instance.OnIsAttack += IsAttack;
        GameManager.Instance.OnAttack += Attack;
    }

    private void OnDisable()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnMove -= Move;
            GameManager.Instance.OnHold -= Hold;
        }
    }

    public void Move(Vector3 targetPosition)
    {
        foreach (GameObject unit in GameManager.Instance.GetSelectedUnits())
        {
            UnitAbilities unitAbilties = unit.GetComponent<UnitAbilities>();
            if (unitAbilties != null)
            {   
                unitAbilties.IsMoveToOn();
                unitAbilties.MoveTo(targetPosition);
            }
        }
    }

    public void Hold()
    {
        foreach (GameObject unit in GameManager.Instance.GetSelectedUnits())
        { 
            UnitAbilities unitAbilites = unit.GetComponent<UnitAbilities>();
            if (unitAbilites != null)
            {
                unitAbilites.HoldPosition();
            }
        }   
    }

    public void IsAttack()
    {
        foreach (GameObject unit in GameManager.Instance.GetSelectedUnits())
        {
            UnitAbilities unitAbilties = unit.GetComponent<UnitAbilities>();
            if (unitAbilties != null)
            {
                unitAbilties.AttackRangeMarkerOn();
            }
        }
    }

    public void Attack(Vector3 targetPosition)
    {
        foreach (GameObject unit in GameManager.Instance.GetSelectedUnits())
        {
            UnitAbilities unitAbilties = unit.GetComponent<UnitAbilities>();
            if (unitAbilties != null)
            {
                unitAbilties.AttackToMove(targetPosition);
                Debug.Log("유닛어택!(유닛컨트롤매니저)");
            }
        }
    }
    
}