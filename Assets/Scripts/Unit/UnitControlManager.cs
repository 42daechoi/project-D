using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class UnitControlManager : MonoBehaviour
{
    public void Initialize()
    {
        Debug.Log("이벤트 구독 시작! 유닛컨트롤러");
        GameManager.Instance.OnMove += Move;
        GameManager.Instance.OnHold += Hold;
        GameManager.Instance.OnIsAttack += IsAttack;
        GameManager.Instance.OnAttack += Attack;
        Debug.Log("이벤트 구독 완료! 유닛컨트롤러");
    }

    private void OnDisable()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnMove -= Move;
            GameManager.Instance.OnHold -= Hold;
            Debug.Log("이벤트잘빠져나갔다! 유닛컨트롤러");
        }
        else
        {
            Debug.LogError("온디세이블쪽이문제임");
        }
    }

    public void Move(Vector3 targetPosition)
    {
        foreach (GameObject unit in GameManager.Instance.GetSelectedUnits())
        {
            UnitAbilites unitAbilties = unit.GetComponent<UnitAbilites>();
            if (unitAbilties != null)
            {   
                Debug.Log("유닛어빌리티 컴포넌트 잘끌고왔따 Move(유닛컨트롤매니저)");
                unitAbilties.MoveTo(targetPosition);
                Debug.Log("유닛이동!(유닛컨트롤매니저)");
            }
        }
    }

    public void Hold()
    {
        foreach (GameObject unit in GameManager.Instance.GetSelectedUnits())
        { 
            UnitAbilites unitAbilites = unit.GetComponent<UnitAbilites>();
            if (unitAbilites != null)
            {
                Debug.Log("유닛어빌리티 컴포넌트 잘끌고왔따 Move(유닛컨트롤매니저)");
                unitAbilites.HoldPosition();
            }
        }   
    }

    public void IsAttack()
    {
        foreach (GameObject unit in GameManager.Instance.GetSelectedUnits())
        {
            UnitAbilites unitAbilties = unit.GetComponent<UnitAbilites>();
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
            UnitAbilites unitAbilties = unit.GetComponent<UnitAbilites>();
            if (unitAbilties != null)
            {
                unitAbilties.Attack();
                Debug.Log("유닛어택!(유닛컨트롤매니저)");
            }
        }
    }
    
}