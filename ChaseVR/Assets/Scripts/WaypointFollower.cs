using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointFollower : MonoBehaviour
{
    [SerializeField] public Waypoints waypoints;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float distanceThreshold;
    private Transform currentWayPoint;
    private Transform lookAtPoint;

    void Start()
    {
        lookAtPoint = GameObject.Find("LookAtPoint").transform;
        currentWayPoint = waypoints.GetNextWaypoint(currentWayPoint);
        transform.position = currentWayPoint.position;
    }

    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, currentWayPoint.position, moveSpeed * Time.deltaTime);
        if(Vector3.Distance(transform.position, currentWayPoint.position) < distanceThreshold)
        {
            currentWayPoint = waypoints.GetNextWaypoint(currentWayPoint);
        }

        // transform.LookAt(new Vector3(transform.position.x, lookAtPoint.position.y, lookAtPoint.position.z));
        
        var lookDir = lookAtPoint.position - transform.position;
        lookDir.y = 0;
        // lookDir.z = 0;
        transform.rotation = Quaternion.LookRotation(lookDir);

    }
}
