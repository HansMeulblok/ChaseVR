using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MannequinManager : MonoBehaviour
{
    [SerializeField] private Transform startWaypoint;
    [SerializeField] private Transform blockPlacementPoint;
    [SerializeField] private Waypoints mannequinWaypoints;
    [SerializeField] private float speed;
    [SerializeField] private float distanceThreshold;
    

    void OnTriggerEnter(Collider collider)
    {
        if(collider.tag == "Etalage")
        {
            Transform mannequinHolder = collider.transform.Find("MannequinHolder");
            foreach (Transform m in mannequinHolder)
            {
                Transform mannequin = Instantiate(m, startWaypoint.position, Quaternion.identity);
                MannequinWaypointFollower mannequinWaypointFollower =  mannequin.gameObject.AddComponent<MannequinWaypointFollower>();
                mannequinWaypointFollower.moveSpeed = speed;
                mannequinWaypointFollower.currentWayPoint = startWaypoint;
                mannequinWaypointFollower.distanceThreshold = this.distanceThreshold;
            }
        }
    }
}
