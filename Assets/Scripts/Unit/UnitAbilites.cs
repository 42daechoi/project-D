using System.Collections;
using System.Collections.Generic;
using System.Threading;
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
    public ParticleSystem particle;
    
    // 유닛 가챠 확률 속성
    [Header("GachaPercentage")]
    public float probability;
    
    // 유닛 시너지목록
    [Header("UnitSynergies")]
    public List<ScriptableObject> synergiesList;
    public int unitId;
    
    // 유닛 선택 마커
    public GameObject unitMarker;
    //private AttackRangeRenderer rangeRenderer;
    
    // 기타
    private GameObject placedInactiveUnitGround;
    private NavMeshAgent navAgent;
    private Coroutine moveCoroutine;
    private Coroutine attackCoroutine;
    private Animator unitAnim;
    public List<ParticleSystem> activeParticles = new List<ParticleSystem>();
    public AudioSource audioSource; // 오디오 소스
    public AudioClip attackSound;   // 공격 사운드 (파티클 효과음)
    
    private void Awake()
    {
        placedInactiveUnitGround = null;
        unitAnim = GetComponent<Animator>();
        navAgent = GetComponent<NavMeshAgent>();
        //rangeRenderer = GetComponentInChildren<AttackRangeRenderer>();
        

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
            StopAllCoroutines();
            isAttackToMove = false;
            unitAnim.SetBool("isAttacking", false);
            unitAnim.SetBool("isWalking", false);
            unitAnim.speed = 1f;
            navAgent.enabled = false;
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
        unitAnim.speed = 1f;
        if (navAgent != null && navAgent.enabled && isAttackToMove == false)
        {
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
                yield break;
            }
            yield return null;
        }
    }

    public void HoldPosition()
    {
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
            isAttackToMove = false;
        }
    }
    #endregion
    
    #region Attack Methods (공격 관련 메서드)

    private void PlayAttackSound()
    {
        if (audioSource != null && attackSound != null)
        {
            audioSource.PlayOneShot(attackSound);
        }
    }
    
    public void AttackToMove(Vector3 targetPosition)
    {
        //AttackRangeMarkerOff();
        isAttackToMove = true;
        MoveTo(targetPosition);
    }

    public void AttackToMove(Monster targetMonster)
    {
        isAttackToMove = true;
        if (moveCoroutine != null)
        {
            StopCoroutine(moveCoroutine);
        }

        moveCoroutine = StartCoroutine(FollowMovingMonster(targetMonster));
    }

    private IEnumerator FollowMovingMonster(Monster targetMonster)
    {
        while (isAttackToMove)
        {
            if (!targetMonster.IsDead)
            {
                navAgent.SetDestination(targetMonster.transform.position);
                float distanceToMonster = Vector3.Distance(transform.position, targetMonster.transform.position);
                if (distanceToMonster <= attackRange)
                {
                    navAgent.isStopped = true;
                    unitAnim.SetBool("isWalking", false);
                    Attack(targetMonster);
                }
                else
                {
                    unitAnim.SetBool("isAttacking", false);
                    navAgent.isStopped = false;
                    navAgent.SetDestination(targetMonster.transform.position);
                    unitAnim.SetBool("isWalking", true);
                    unitAnim.speed = 1f;
                }    
            }
            else
            {
                Debug.Log("타겟 몬스터가 사망했습니다. 새로운 타겟을 찾습니다.");
                StopCoroutine(moveCoroutine);
                yield return StartCoroutine(CheckForMonstersAndAttack());
            }

            yield return new WaitForSeconds(0.1f); // 일정 주기로 업데이트
        }
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
                if (!navAgent.pathPending && navAgent.remainingDistance <= navAgent.stoppingDistance)
                {
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
            bool foundTarget = false;
            
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, attackRange);
            foreach (var hitCollider in hitColliders)
            {
                if (hitCollider.CompareTag("Monster"))
                {
                    Monster monster = hitCollider.GetComponent<Monster>();
                    if (monster != null)
                    {
                        Attack(monster);
                        foundTarget = true;
                        break;
                    }
                }
            }
            if (!foundTarget)
            {
                // 공격 애니메이션을 중지
                unitAnim.SetBool("isAttacking", false);
            }

            yield return new WaitForSeconds(1f / attackSpeed);
        }
    }
    

    private void Attack(Monster monster)
    {
        Vector3 directionToLook = monster.transform.position - transform.position;
        directionToLook.y = 0;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(directionToLook), Time.deltaTime * 10f);
        
        unitAnim.SetBool("isAttacking", true);
        unitAnim.speed = attackSpeed;
        if (Time.time >= lastAttackTime + 1f / attackSpeed)
        {
            if (particle != null)
            {
                AddParticleToAttack(monster);
                PlayAttackSound();
            }
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

    private void AddParticleToAttack(Monster monster)
    {
        if (monster == null) return;
        ParticleSystem particleInstance = Instantiate(particle, transform.position, transform.rotation);
        StartCoroutine(MoveParticle(particleInstance, monster));
    
    }

    private IEnumerator MoveParticle(ParticleSystem particleInstance, Monster targetMonster)
    {
        float moveSpeed = 20.0f;
        activeParticles.Add(particleInstance);

        while (particleInstance != null && targetMonster != null)
        {
            Vector3 targetPosition = targetMonster.transform.position;

            // 파티클이 몬스터의 현재 위치로 이동하도록 설정
            if (Vector3.Distance(particleInstance.transform.position, targetPosition) > 0.1f)
            {
                particleInstance.transform.position = Vector3.MoveTowards(
                    particleInstance.transform.position,
                    targetPosition,
                    moveSpeed * Time.deltaTime
                );
            }
            else
            {
                // 파티클이 도착했을 때 잠시 대기
                targetMonster.TakeDamage(damage);
                Destroy(particleInstance.gameObject);
                activeParticles.Remove(particleInstance);
            }

            yield return null; // 다음 프레임까지 대기
        }

        if (particleInstance != null)
        {
            Destroy(particleInstance.gameObject);
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

    // public void AttackRangeMarkerOn()
    // {
    //     if (rangeRenderer != null)
    //     {
    //         rangeRenderer.UpdateCircle(attackRange);  // 범위를 업데이트
    //         rangeRenderer.Show();  // 범위를 표시
    //     }
    // }
    //
    // public void AttackRangeMarkerOff()
    // {
    //     if (rangeRenderer != null)
    //     {
    //         rangeRenderer.Hide();  // 범위를 숨김
    //     }
    // }
    // private void OnDrawGizmosSelected()
    // {
    //     // 유닛이 선택된 상태에서만 Gizmos를 그립니다.
    //     Gizmos.color = Color.red; // Gizmos의 색상을 설정합니다.
    //     
    //     // 유닛의 위치에서 공격 범위만큼의 구를 그립니다.
    //     Gizmos.DrawWireSphere(transform.position, attackRange);
    // }

    #endregion

}
