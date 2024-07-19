using System;
using System.Collections;
using System.Collections.Generic;
using projectD;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

public class UnitAbilites : MonoBehaviour
{
    public float probability;
    public float damage;
    public GameObject placedInactiveUnitGround;
    public NavMeshAgent navAgent;
    public List<ScriptableObject> synergiesList;

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
            navAgent.enabled = true;
        }

        if (inactiveUnitGround != null)
        {
            VerifyActivation iugVa = inactiveUnitGround.GetComponent<VerifyActivation>();
            iugVa.SetActivation(true); 
            navAgent.enabled = false;
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
        
    }

    public void MoveTo(Vector3 targetPosition)
    {
        if (navAgent != null && navAgent.enabled)
        {
            Debug.Log("잘들어왔다 유닛어빌리티!");
            navAgent.isStopped = false;
            navAgent.SetDestination(targetPosition);
        }
    }

    public void HoldPosition()
    {
        if (navAgent != null && navAgent.enabled)
        {
            Debug.Log("잘들어왔다 유닛어빌리티!");
            navAgent.isStopped = true;
        }
    }
}