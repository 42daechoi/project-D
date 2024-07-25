using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using projectD;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using Vector3 = UnityEngine.Vector3;

public class UnitAbilites : MonoBehaviour
{
    // 유닛 전투 관련 속성
    [Header("UnitCombatData")] 
    public float damage = 10f;
    public float attackSpeed = 1f;
    public float attackRange = 10f;
    
    // 유닛 가챠 확률 속성
    [Header("GachaPercentage")]
    public float probability;
    
    // 유닛 시너지목록
    [Header("UnitSynergies")]
    public List<ScriptableObject> synergiesList;
    
    // 유닛 선택 마커
    public GameObject unitMarker;
    public LineRenderer attackRangeIndicator;
    
    private GameObject placedInactiveUnitGround;
    private NavMeshAgent navAgent;
    

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
        
        if (unitMarker != null)
        {
            unitMarker.SetActive(false);
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
    public void MoveTo(Vector3 targetPosition)
    {
        if (navAgent != null && navAgent.enabled)
        {
            Debug.Log("잘들어왔다 무브 유닛어빌리티!");
            navAgent.isStopped = false;
            navAgent.SetDestination(targetPosition);
        }
    }

    public void HoldPosition()
    {
        if (navAgent != null && navAgent.enabled)
        {
            Debug.Log("잘들어왔다 홀드 유닛어빌리티!");
            navAgent.isStopped = true;
        }
    }

    public void AttackTo(Vector3 targetPosition)
    {
        float distanceToTarget = Vector3.Distance(transform.position, targetPosition);

        // 목표 지점이 공격 사거리 내에 있는지 확인
        if (distanceToTarget <= attackRange)
        {
            Debug.Log("공격최대사거리 접근완료");
            HoldPosition();
        }
        else
        {
            StartCoroutine(MoveAndCheckDistance(targetPosition));
        }
    }

    private IEnumerator MoveAndCheckDistance(Vector3 targetPosition)
    {
        MoveTo(targetPosition); // 여기에서 먼저 MoveTo를 떄려버림

        while (true)
        {
            float distanceToTarget = Vector3.Distance(transform.position, targetPosition);

            if (distanceToTarget <= attackRange)
            {
                Debug.Log("목표 지점에 도달하여 멈춤");
                HoldPosition();
                yield break; // 코루틴 종료
            }

            yield return new WaitForSeconds(0.1f); // 짧은 시간 간격으로 반복 체크
        }
    }
    

    public void SelectUnitMarker()
    {
        if (unitMarker != null)
        {
            unitMarker.SetActive(true);
            Debug.Log("유닛어빌리티 셋엑티브 트루");
        }
    }

    public void DeSelectUnitMarker()
    {
        if (unitMarker != null)
        {
            unitMarker.SetActive(false);
            Debug.Log("유닛어빌리티 셋엑티브 폴스");
        }
    }

}