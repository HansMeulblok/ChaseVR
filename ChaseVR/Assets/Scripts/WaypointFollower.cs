using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class WaypointFollower : MonoBehaviour
{
    [HideInInspector] public Waypoints waypoints;
    [HideInInspector] public float moveSpeed;
    [SerializeField] private float distanceThreshold;
    // [SerializeField] private float distanceToNextWaypoint;
    // [SerializeField] private float distanceToNextCube;
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

        var lookDir = lookAtPoint.position - transform.position;
        lookDir.y = 0;
        transform.rotation = Quaternion.LookRotation(lookDir);
    }

    public void Replace(InputAction.CallbackContext context)
    {
        GameObject tempCube = Instantiate(this.gameObject, transform.position, Quaternion.identity);
        Destroy(tempCube.GetComponent<WaypointFollower>());
        gameObject.SetActive(false);
    } 

    public void Revert()
    {
        
    }
}
