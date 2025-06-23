using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RacerPlacement : MonoBehaviour
{
    [SerializeField] bool player = false;
    public int currentLap = 0;
    public int maxLaps = 3;
    private bool firstLap = true;
    public int currentWaypoint = 0;
    public float distanceToNext = 0f;
    private WaypointManager waypointManager;
    private List<Transform> waypoints;

    void Start()
    {
        waypointManager = FindObjectOfType<WaypointManager>();
        waypoints = waypointManager.waypoints;
    }

    void Update()
    {
        Transform next = waypoints[currentWaypoint];
        distanceToNext = Vector3.Distance(transform.position, next.position);
    }

    public void PassedWaypoint()
    {
        currentWaypoint++;

        if (currentWaypoint >= waypoints.Count)
        {
            currentWaypoint = 0;
            currentLap++;
        }
    }
}
