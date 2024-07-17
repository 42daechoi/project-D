using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class UnitAbilites : MonoBehaviour
{
    public float probability;
    public GameObject placedInactiveUnitGround;
    public NavMeshAgent navAgent;

    private void Awake()
    {
        placedInactiveUnitGround = null;
        navAgent = GetComponent<NavMeshAgent>();

        if (navAgent == null)
        {
            Debug.LogError("NavMeshAgent를 찾을 수 없습니다.");
        }
        else
        {
            navAgent.enabled = false; // NavMeshAgent 비활성화
        }
    }

    public void SetActivation(GameObject inactiveUnitGround)
    {
        if (placedInactiveUnitGround != null)
        {
            VerifyActivation placedIugVa = placedInactiveUnitGround.GetComponent<VerifyActivation>();
            placedIugVa.SetActivation(false);
            // navAgent.enabled = true;
        }

        if (inactiveUnitGround != null)
        {
            VerifyActivation iugVa = inactiveUnitGround.GetComponent<VerifyActivation>();
            iugVa.SetActivation(true); 
            // navAgent.enabled = false;
            // 새로운 유닛의 NavMeshAgent 활성화
        }

        placedInactiveUnitGround = inactiveUnitGround;
    }

    public GameObject GetActivation()
    {
        return placedInactiveUnitGround;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(1) && navAgent.enabled) // NavMeshAgent가 활성화된 경우에만 이동
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); // 마우스 포인터에서 Ray 생성
            if (Physics.Raycast(ray, out RaycastHit hit)) // Raycast를 사용하여 충돌 감지
            {
                Vector3 targetPosition = hit.point;
                MoveTo(targetPosition);
            }
        }
    }

    void MoveTo(Vector3 targetPosition)
    {
        if (navAgent != null && navAgent.enabled)
        {
            
            navAgent.SetDestination(targetPosition);
        }
    }
}