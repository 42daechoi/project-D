using System.Collections;
using System.Collections.Generic;
using projectD;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class UnitAbilities : MonoBehaviour
{
    // 유닛 전투 관련 속성
    [Header("UnitCombatData")]
    public float damage = 10f;
    public float attackSpeed = 1f;
    public float attackRange = 2f;
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
    private AttackRangeRenderer rangeRenderer;
    
    // 기타
    private GameObject placedInactiveUnitGround;
    private NavMeshAgent navAgent;
    private Coroutine moveCoroutine;
    private Coroutine attackCoroutine;
    private Animator unitAnim;
    
    private void Awake()
    {
        placedInactiveUnitGround = null;
        unitAnim = GetComponent<Animator>();
        navAgent = GetComponent<NavMeshAgent>();
        rangeRenderer = GetComponentInChildren<AttackRangeRenderer>();
        

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
            navAgent.angularSpeed = 1080f;
            navAgent.acceleration = 1000f;

        }

        if (inactiveUnitGround != null)
        {
            VerifyActivation iugVa = inactiveUnitGround.GetComponent<VerifyActivation>();
            iugVa.SetActivation(true);
            navAgent.enabled = false;
            Debug.Log("대기열에있지만 활성화중");
            // 새로운 유닛의 NavMeshAgent 활성화
            GameManager.Instance.DeselectUnit(gameObject);
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
        AttackRangeMarkerOff();

        if (navAgent != null && navAgent.enabled && isAttackToMove == false)
        {
            Debug.Log("움직임으로 인한 이동");
            unitAnim.SetBool("isAttacking", false);
            unitAnim.SetBool("isWalking", true);
            navAgent.isStopped = false;
            navAgent.SetDestination(targetPosition);
            if (moveCoroutine != null)
            {
                StopCoroutine(moveCoroutine);
            }
            moveCoroutine = StartCoroutine(CheckArrival());
        }
        
        if (navAgent != null && navAgent.enabled && isAttackToMove)
        {
            Debug.Log("공격으로 인한 이동");
            navAgent.isStopped = false;
            unitAnim.SetBool("isWalking", true);
            navAgent.SetDestination(targetPosition);

            if (moveCoroutine != null)
            {
                StopCoroutine(moveCoroutine);
            }
            moveCoroutine = StartCoroutine(CheckForMonstersWhileMoving());
        }
    }
    private IEnumerator CheckArrival()
    {
        while (navAgent != null && navAgent.enabled)
        {
            // remainingDistance가 stoppingDistance 이하일 때 도착으로 판단
            if (!navAgent.pathPending && navAgent.remainingDistance <= navAgent.stoppingDistance)
            {
                // 도착 시 애니메이션 정지
                unitAnim.SetBool("isWalking", false);
                navAgent.isStopped = true;
                yield break; // 코루틴 종료
            }
            yield return null; // 다음 프레임까지 대기
        }
    }

    public void HoldPosition()
    {
        //unitAnim.SetTrigger("Stand");
        if (navAgent != null && navAgent.enabled)
        {
            navAgent.isStopped = true;
            isAttackToMove = true;
            unitAnim.SetBool("isWalking", false);
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
        AttackRangeMarkerOff();
        isAttackToMove = true;
        MoveTo(targetPosition);
    }

    private IEnumerator CheckForMonstersWhileMoving()
    {
        while (isAttackToMove)
        {
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, attackRange);
            bool monsterFound = false;
            foreach (var hitCollider in hitColliders)
            {
                if (hitCollider.CompareTag("Monster"))
                {
                    Monster monster = hitCollider.GetComponent<Monster>();
                    if (monster != null)
                    {
                        monsterFound = true;
                        navAgent.isStopped = true;
                        unitAnim.SetBool("isWalking", false);
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
            if (!monsterFound)
            {
                // 몬스터가 없으면 목표 지점으로 계속 이동
                if (!navAgent.pathPending && navAgent.remainingDistance <= navAgent.stoppingDistance)
                {
                    // 목표 지점에 도착하면 HoldPosition 호출
                    HoldPosition();
                    yield break;
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
        unitAnim.SetBool("isAttacking", true);
        if (Time.time >= lastAttackTime + 1f / attackSpeed)
        {
            monster.TakeDamage(damage);
            Debug.Log("몬스터를 공격했습니다.");
            lastAttackTime = Time.time;

            foreach (ScriptableObject so in synergiesList)
            {
                Synergy synergy = (so as Synergy);
                if (synergy.target == "Monster")
                {
                    synergy.ApplySynergyToMonster(monster, this);
                }
            }
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
        if (rangeRenderer != null)
        {
            rangeRenderer.UpdateCircle(attackRange);  // 범위를 업데이트
            rangeRenderer.Show();  // 범위를 표시
        }
    }
    
    public void AttackRangeMarkerOff()
    {
        if (rangeRenderer != null)
        {
            rangeRenderer.Hide();  // 범위를 숨김
        }
    }
    private void OnDrawGizmosSelected()
    {
        // 유닛이 선택된 상태에서만 Gizmos를 그립니다.
        Gizmos.color = Color.red; // Gizmos의 색상을 설정합니다.
        
        // 유닛의 위치에서 공격 범위만큼의 구를 그립니다.
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }

    #endregion

}
