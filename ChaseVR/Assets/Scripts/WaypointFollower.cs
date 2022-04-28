using System.Collections;
using System.Collections.Generic;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine;

/// DISCLAIMER: dit script is groten geschreven met tegenzin, geen motivatie en 3 uur slaap. 
public class WaypointFollower : MonoBehaviour
{
    [HideInInspector] public Waypoints waypoints;
    [HideInInspector] public float moveSpeed;
    public float force = 30;
    public bool canMove = true;
    [HideInInspector] public float distanceThreshold;
    [HideInInspector] public Transform controllerTransform;
    [HideInInspector] public bool holding;
    public Transform currentWayPoint;
    private Transform lookAtPoint;
    private GameObject shootingTarget;

    void Start()
    {   
        controllerTransform = GameObject.Find("RightHand Controller").transform;
        shootingTarget = GameObject.FindGameObjectWithTag("ShootingTarget");
        if(!canMove) return;
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

    public void Replace(SelectEnterEventArgs args)
    {        
        if(holding) 
        {
            return;
        }
        
        shootingTarget.GetComponent<MeshRenderer>().enabled = true;
        holding = true;
        canMove = false;
        force = 30;
        gameObject.AddComponent<CubeScaling>();
    } 

    public void ShootCube(SelectExitEventArgs args)
    {
        GetComponent<XRGrabInteractable>().enabled = false;
        GetComponent<Rigidbody>().AddForce(controllerTransform.forward * force, ForceMode.Impulse);
        holding = false;
    }

    IEnumerator DelayedTurnOn()
    {
        yield return new WaitForSeconds(1);
        GetComponent<XRGrabInteractable>().enabled = true;
        shootingTarget.GetComponent<MeshRenderer>().enabled = false;
    }
}
