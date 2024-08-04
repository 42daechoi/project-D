using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class UnitAbilities : MonoBehaviour
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
    public int unitId;
    
    // 유닛 선택 마커
    public GameObject unitMarker;
    public GameObject rangeMarker;
    
    // 기타
    private GameObject placedInactiveUnitGround;
    private NavMeshAgent navAgent;
    private Coroutine moveCoroutine;
    private Coroutine attackCoroutine;
    
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
    #region Activation Methods (활성화 메서드)
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
    #endregion
    
    #region Move Methods (이동 관련 메서드)
    public void MoveTo(Vector3 targetPosition)
    {
        if (rangeMarker != false)
        {
            rangeMarker.SetActive(false);
        }

        if (navAgent != null && navAgent.enabled && isAttackToMove == false)
        {
            Debug.Log("움직임으로 인한 이동");
            navAgent.isStopped = false;
            navAgent.SetDestination(targetPosition);
        }
        
        if (navAgent != null && navAgent.enabled && isAttackToMove)
        {
            Debug.Log("공격으로 인한 이동");
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
            navAgent.isStopped = true;
            isAttackToMove = true;
            if (attackCoroutine != null)
            {
                StopCoroutine(attackCoroutine);
            }
            attackCoroutine = StartCoroutine(CheckForMonstersAndAttack());
        }
    }
    
    public void IsMoveToOn()
    {
        if (isAttackToMove)
        {
            Debug.Log("어택투무브 false로 교체");
            isAttackToMove = false;
        }
    }
    #endregion
    
    #region Attack Methods (공격 관련 메서드)
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
                    Monster monster = hitCollider.GetComponent<Monster>();
                    if (monster != null)
                    {
                        navAgent.isStopped = true;
                        if (moveCoroutine != null)
                        {
                            StopCoroutine(moveCoroutine);
                        }
                        if (attackCoroutine != null)
                        {
                            StopCoroutine(attackCoroutine);
                        }
                        attackCoroutine = StartCoroutine(CheckForMonstersAndAttack());
                        yield break;
                    }
                }
            }
            yield return new WaitForSeconds(0.1f);
        }
    }

    private IEnumerator CheckForMonstersAndAttack()
    {
        while (isAttackToMove)
        {
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, attackRange);
            foreach (var hitCollider in hitColliders)
            {
                if (hitCollider.CompareTag("Monster"))
                {
                    Monster monster = hitCollider.GetComponent<Monster>();
                    if (monster != null)
                    {
                        Attack(monster);
                    }
                }
            }
            yield return new WaitForSeconds(1f / attackSpeed); // 공격 속도에 맞춰 주기적으로 공격
        }
    }
    

    private void Attack(Monster monster)
    {
        if (Time.time >= lastAttackTime + 1f / attackSpeed)
        {
            monster.TakeDamage(damage);
            Debug.Log("몬스터를 공격했습니다.");
            lastAttackTime = Time.time;
        }
    }
    #endregion
    
    #region Marker Methods (마커 메서드)
    public void SelectUnitMarker()
    {
        if (unitMarker != null)
        {
            unitMarker.SetActive(true);
        }
    }

    public void DeSelectUnitMarker()
    {
        if (unitMarker != null)
        {
            unitMarker.SetActive(false);
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
    #endregion

}
