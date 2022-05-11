using System.Collections;
using System.Collections.Generic;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine;

/// DISCLAIMER: dit script is groten geschreven met tegenzin, geen motivatie en 3 uur slaap. 
public class MannequinWaypointFollower : MonoBehaviour
{
    public Waypoints waypoints;
    public float moveSpeed = 4;
    public bool canMove = true;
    public float distanceThreshold = 0.5f;
    public Transform currentWayPoint;

    void Start()
    {   
        waypoints = GameObject.Find("mannequin waypoints").GetComponent<Waypoints>();   
        currentWayPoint = waypoints.GetNextWaypoint(currentWayPoint);
        transform.position = currentWayPoint.position;
    }

    void Update()
    {
        if(!canMove)
        return;

        // Move towards next waypoint
        transform.position = Vector3.MoveTowards(transform.position, currentWayPoint.position, moveSpeed * Time.deltaTime);

        Debug.Log(Vector3.Distance(transform.position, currentWayPoint.position));

        // If close enough to waypoint grab the waypoint after that to pursue.
        if(Vector3.Distance(transform.position, currentWayPoint.position) < distanceThreshold)
        {
            Debug.Log("go to next waypoint");
            currentWayPoint = waypoints.GetNextWaypoint(currentWayPoint);
        }
    }
}
