using UnityEngine;

public class Monster : MonoBehaviour
{
    private float hp;
    [SerializeField]
    private float maxHp;
    [SerializeField]
    private float def;
    [SerializeField]
    private float speed;
    
    public float Hp { get => hp; set => hp = value; }
    public float MaxHp { get => maxHp; set => maxHp = value; }
    public float Def { get => def; set => def = value; }
    public float Speed { get => speed; set => speed = value; }

    public Transform[] waypoints;

    private int waypointIndex = 0;

    void Start()
    {
        Hp = MaxHp;
    }

    void Update()
    {
        Move();
    }

    void Move()
    {
        Transform targetWaypoint = waypoints[waypointIndex];
        transform.position = Vector3.MoveTowards(transform.position, targetWaypoint.position, Speed * Time.deltaTime);

        if (Vector3.Distance(transform.position, targetWaypoint.position) < 0.1f)
        {
            waypointIndex = (waypointIndex + 1) % waypoints.Length;
            Rotate();
        }
    }

    void Rotate()
    {
        transform.rotation *= Quaternion.Euler(0, -90, 0);
    }

    // 캐릭터 공격에 관련된 함수에 불러오면됨
    public void TakeDamage(float damage)
    {
        float actualDamage = Mathf.Max(0, damage - Def);
        Hp -= actualDamage;

        if (Hp <= 0)
        {
            Dead();
        }
    }

    private void Dead()
    {
        Destroy(gameObject);
    }
}