using System.Collections.Generic;
using UnityEngine;
using System;

public class UnitControlManager : MonoBehaviour
{
    public void Initialize()
    {
        Debug.Log("이벤트 구독 시작! 유닛컨트롤러");
        GameManager.Instance.OnMove += Move;
        Debug.Log("이벤트 구독 완료! 유닛컨트롤러");
    }
    // private void OnEnable()
    // {
    //     if (GameManager.Instance != null)
    //     {
    //         Debug.Log("이벤트잘들어왔다! 유닛컨트롤러");
    //         GameManager.Instance.OnMove += Move;
    //     }
    //     else
    //     {
    //         Debug.LogError("OnEnable쪽에 문제임");
    //     }
    // }

    private void OnDisable()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnMove -= Move;
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
}