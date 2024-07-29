using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class UnitAbilites : MonoBehaviour
{
    // 유닛 전투 관련 속성
    [Header("UnitCombatData")]
    public float damage = 10f;
    public float attackSpeed = 1f;
    public float attackRange = 10f;
    private float lastAttackTime;
    private bool isAttackToMove = false;
    
    // 유닛 가챠 확률 속성
    [Header("GachaPercentage")]
    public float probability;
    
    // 유닛 시너지목록
    [Header("UnitSynergies")]
    public List<ScriptableObject> synergiesList;
    
    // 유닛 선택 마커
    public GameObject unitMarker;
    public GameObject rangeMarker;
    
    private GameObject placedInactiveUnitGround;
    private NavMeshAgent navAgent;
    private Coroutine moveCoroutine;

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
        if (rangeMarker != false)
        {
            rangeMarker.SetActive(false);
        }
        
        if (navAgent != null && navAgent.enabled)
        {
            Debug.Log("잘들어왔다 무브 유닛어빌리티!");
            navAgent.isStopped = false;
            navAgent.SetDestination(targetPosition);

            if (moveCoroutine != null)
            {
                StopCoroutine(moveCoroutine);
            }
            moveCoroutine = StartCoroutine(CheckForMonstersWhileMoving());
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

    public void AttackToMove(Vector3 targetPosition)
    {
        rangeMarker.SetActive(false);
        isAttackToMove = true;
        MoveTo(targetPosition);
    }

    private IEnumerator CheckForMonstersWhileMoving()
    {
        while (isAttackToMove)
        {
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, attackRange);
            foreach (var hitCollider in hitColliders)
            {
                if (hitCollider.CompareTag("Monster"))
                {
                    Debug.Log("안심해 코루틴에들어왔어");
                    HoldPosition();
                    Monster monster = hitCollider.GetComponent<Monster>();
                    if (monster != null)
                    {
                        Attack(monster);
                        Debug.Log("공격하고 빠졌어");
                        isAttackToMove = false;
                        yield break; // 코루틴 종료
                    }
                }
            }
            yield return new WaitForSeconds(0.1f); // 주기적으로 체크 (0.1초 간격)
        }
    }

    private void Attack(Monster monster)
    {
        Debug.Log("어택메서트 초입부분은 들어왔어");
        if (Time.time >= lastAttackTime + 1f / attackSpeed)
        {
            float distanceToMonster = Vector3.Distance(transform.position, monster.transform.position);
            if (distanceToMonster <= attackRange)
            {
                monster.TakeDamage(damage);
                Debug.Log("몬스터를 공격했습니다. 거리: " + distanceToMonster);
                lastAttackTime = Time.time;
            }
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

    public void AttackRangeMarkerOn()
    {
        if (rangeMarker != null)
        {
            rangeMarker.transform.localScale = new Vector3(attackRange * 2, attackRange * 2, attackRange * 2);
            rangeMarker.SetActive(true);
        }
    }
}
