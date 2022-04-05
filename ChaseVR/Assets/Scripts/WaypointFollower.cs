using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine;

public class WaypointFollower : MonoBehaviour
{
    [HideInInspector] public Waypoints waypoints;
    [HideInInspector] public float moveSpeed;
    [HideInInspector] public bool canMove = true;
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
        if(!canMove)
        return;

        transform.position = Vector3.MoveTowards(transform.position, currentWayPoint.position, moveSpeed * Time.deltaTime);
        if(Vector3.Distance(transform.position, currentWayPoint.position) < distanceThreshold)
        {
            currentWayPoint = waypoints.GetNextWaypoint(currentWayPoint);
        }

        var lookDir = lookAtPoint.position - transform.position;
        lookDir.y = 0;
        transform.rotation = Quaternion.LookRotation(lookDir);
    }

    public void Replace(Transform controller)
    {
        GameObject tempCube = Instantiate(this.gameObject, transform.position, Quaternion.identity);
        tempCube.AddComponent<CubeScaling>();
        tempCube.GetComponent<CubeScaling>().controllerTransform = controller;
        Destroy(tempCube.GetComponent<WaypointFollower>());
        gameObject.SetActive(false);
    } 
}
