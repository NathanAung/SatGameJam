using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RacerAI : MonoBehaviour
{
    public float speed = 10f;
    public float turnSpeed = 5f;
    public WaypointManager waypointManager;
    private int currentWaypoint = 0;
    void Update()
    {
        Transform target = waypointManager.waypoints[currentWaypoint];
        Vector3 direction = (target.position - transform.position).normalized;
        // Turn towards the waypoint
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, turnSpeed * Time.deltaTime);
        // Move forward
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
        // Check if close enough to switch to next waypoint
        if (Vector3.Distance(transform.position, target.position) < 5f)
        {
            currentWaypoint = (currentWaypoint + 1) % waypointManager.waypoints.Count;
        }
    }
}
