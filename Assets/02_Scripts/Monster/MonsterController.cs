using UnityEngine;

public class MonsterController : MonoBehaviour
{
    private Vector3 startPosition;
    private Vector3[] waypoints;

    private int originalSpeed;
    private int currentSpeed;
    private bool isAlive = true;


    private void FixedUpdate()
    {
        if (isAlive)
        {
            Move();
        }
    }

    public void SetWaypoints(Vector3[] waypoints)
    {
        this.waypoints = waypoints;
    }

    private void Move()
    {
        for (int i = 0; i < waypoints.Length; i++)
        {
            Vector3 nextWaypoint = waypoints[i];
            transform.position = Vector3.MoveTowards(transform.position, nextWaypoint, currentSpeed * Time.deltaTime);
        }
    }

    public void OnRestore()
    {
        isAlive = true;
    }

    public void OnDeath()
    {
        isAlive = false;
    }
}