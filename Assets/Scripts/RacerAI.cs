using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RacerAI : MonoBehaviour
{
    public bool racerActive = false;
    RacerPlacement racerPlacement;
    public float speed = 10f;
    public float turnSpeed = 5f;
    public WaypointManager waypointManager;
    private int currentWaypoint = 0;

    void Start()
    {
        racerPlacement = GetComponent<RacerPlacement>();
    }

    void Update()
    {
        if (!racerActive) return;

        Transform target = waypointManager.waypoints[currentWaypoint];
        Vector3 direction = (target.position - transform.position).normalized;
        // Turn towards the waypoint
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, turnSpeed * Time.deltaTime);
        // Move forward
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
        // Check if close enough to switch to next waypoint
        if (Vector3.Distance(transform.position, target.position) < 10f)
        {
            currentWaypoint = (currentWaypoint + 1) % waypointManager.waypoints.Count;
            if (currentWaypoint == 0 && racerPlacement.currentLap >= racerPlacement.maxLaps)
                speed = 0;
            racerPlacement.PassedWaypoint();
        }
    }
}
