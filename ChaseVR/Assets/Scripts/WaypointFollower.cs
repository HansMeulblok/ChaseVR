using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointFollower : MonoBehaviour
{
    [HideInInspector] [SerializeField] public Waypoints waypoints;
    [HideInInspector] public float moveSpeed;
    [SerializeField] private float distanceThreshold;
    [SerializeField] private float distanceToNextWaypoint;
    [SerializeField] private float distanceToNextCube;
    private Transform currentWayPoint;
    private Transform lookAtPoint;
    public int index;
    public int nextIndex;

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

        var lookDir = lookAtPoint.position - transform.position;
        lookDir.y = 0;
        transform.rotation = Quaternion.LookRotation(lookDir);

        distanceToNextWaypoint = Vector3.Distance(transform.position, waypoints.GetNextWaypoint(currentWayPoint).transform.position);

        if(waypoints.transform.GetChild(nextIndex) != null)
        {

            distanceToNextCube = Vector3.Distance(transform.position, waypoints.transform.GetChild(nextIndex).position);

            // Debug.Log( "index = "+ index + " distance to next cube " + distanceToNextCube);
        }
    }

    public void UpdateIndex()
    {
        nextIndex = waypoints.getNextCube(index).GetSiblingIndex();
        Debug.Log(index + " "+ nextIndex);
    }

    void PauseState()
    {
        // handle pause state
        // move to next waypoint, stop moving after that
        // bool for stopping the update also updated here
    }

    void OnDestroy()
    {
        
    }
}
