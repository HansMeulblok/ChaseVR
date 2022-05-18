using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointFollower : MonoBehaviour
{
    public Waypoints waypoints;
    public Transform currentWayPoint;
    public GameObject currentEtalage;
    public BlokEtalage currentComponent;
    public FollowerManager followerManager;
    public float distanceThreshold;
    public float respawnTime ;
    public bool canMove = true;
    public float moveSpeed = 3;
    public float desiredScale;
    private float timer;


    // Start is called before the first frame update
    void Start()
    {
        currentEtalage = SpawnEtalage();
        currentWayPoint = waypoints.GetNextWaypoint(currentWayPoint);
        transform.position = currentWayPoint.position;
    }

    // Update is called once per frame
    void Update()
    {
        if(currentComponent.holding)
        {
            currentEtalage = null;
        }
        
        if(currentEtalage == null)
        {
            timer += Time.deltaTime;
            if(timer >= respawnTime)
            {
                currentEtalage = SpawnEtalage();
                timer = 0;
            }
        }

        if(!canMove)
        return;

        // Move towards next waypoint
        transform.position = Vector3.MoveTowards(transform.position, currentWayPoint.position, moveSpeed * Time.deltaTime);

        // If close enough to waypoint grab the waypoint after that to pursue.
        if(Vector3.Distance(transform.position, currentWayPoint.position) < distanceThreshold)
        {
            currentWayPoint = waypoints.GetNextWaypoint(currentWayPoint);
        }
    }

    private GameObject SpawnEtalage()
    {
        GameObject newObj = Instantiate(followerManager.RandomBlok(), transform.position, Quaternion.identity);
        newObj.transform.localScale = new Vector3(desiredScale, desiredScale, desiredScale);
        currentComponent = newObj.GetComponent<BlokEtalage>();
        currentComponent.holderPoint = this.transform;
        return newObj;
    }
}
