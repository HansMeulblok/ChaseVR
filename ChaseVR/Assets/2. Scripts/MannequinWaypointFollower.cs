using System.Collections;
using System.Collections.Generic;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine;

/// DISCLAIMER: dit script is groten geschreven met tegenzin, geen motivatie en 3 uur slaap. 
public class MannequinWaypointFollower : MonoBehaviour
{
    public MannequinManager mannequinManager;
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

        // If close enough to waypoint grab the waypoint after that to pursue.
        if(Vector3.Distance(transform.position, currentWayPoint.position) < distanceThreshold)
        {
            currentWayPoint = waypoints.GetNextWaypoint(currentWayPoint);
        }

        if(currentWayPoint.CompareTag("EndPoint"))
        {
            mannequinManager.UpdateMannequinAmount();
            Destroy(this.gameObject);
        }

        var lookDir = waypoints.GetNextWaypoint(currentWayPoint).position - transform.position;
        lookDir.y = 0;
        transform.rotation = Quaternion.LookRotation(lookDir);
    }
    
}
