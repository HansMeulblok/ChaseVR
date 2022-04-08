using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointFollower : MonoBehaviour
{
    [HideInInspector] public Waypoints waypoints;
    [HideInInspector] public float moveSpeed;
    [HideInInspector] public bool canMove = true;
    [HideInInspector] public float distanceThreshold;
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
        if(!canMove)
        return;

        // Move towards next waypoint
        transform.position = Vector3.MoveTowards(transform.position, currentWayPoint.position, moveSpeed * Time.deltaTime);

        // If close enough to waypoint grab the waypoint after that to pursue.
        if(Vector3.Distance(transform.position, currentWayPoint.position) < distanceThreshold)
        {
            currentWayPoint = waypoints.GetNextWaypoint(currentWayPoint);
        }

        // Calculcate look direction and set Y to 0 so they only rotate towards the look point on 1 axis. 
        var lookDir = lookAtPoint.position - transform.position;
        lookDir.y = 0;
        transform.rotation = Quaternion.LookRotation(lookDir);
    }

    public void Replace(Transform controller)
    {
        // Turn this object off and spawn a replacement without this script and WITH the cubescaling script. 
        GameObject tempCube = Instantiate(this.gameObject, transform.position, Quaternion.identity);
        tempCube.AddComponent<CubeScaling>();
        tempCube.GetComponent<CubeScaling>().controllerTransform = controller;
        Destroy(tempCube.GetComponent<WaypointFollower>());
        gameObject.SetActive(false);
    } 
}
