using UnityEngine;

public class Monster : MonoBehaviour
{
    public float _speed = 2f;
    public Transform[] waypoints;
    
    private int waypointIndex = 0;

    void Start()
    {

    }

    void Update()
    {
        Move();
    }

    void Move()
    {
        Transform targetWaypoint = waypoints[waypointIndex];
        transform.position = Vector3.MoveTowards(transform.position, targetWaypoint.position, _speed * Time.deltaTime);

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
}