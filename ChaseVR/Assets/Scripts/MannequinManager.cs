using System.Collections;
using System.Collections.Generic;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine;

public class MannequinManager : MonoBehaviour
{
    public int mannequinsOnCatwalk;
    [SerializeField] private Transform startWaypoint;
    [SerializeField] private Transform blockPlacementPoint;
    [SerializeField] private Waypoints mannequinWaypoints;
    [SerializeField] private float speed;
    [SerializeField] private float distanceThreshold;
    [SerializeField] private Vector3 desiredScale;
    [SerializeField] private float mannequinSpawnInterval;
    GameObject currentEtalage;
    Transform mannequinHolder;


    void OnTriggerEnter(Collider collider)
    {
        if(collider.tag == "Etalage")
        {
            Destroy(currentEtalage);
            currentEtalage = collider.gameObject;
            Destroy(collider.GetComponent<XRGrabInteractable>());
            Destroy(collider.GetComponent<Rigidbody>());
            collider.transform.localScale = desiredScale;
            collider.transform.position = blockPlacementPoint.position;
            mannequinHolder = collider.transform.Find("MannequinHolder");
            StartCoroutine(SpawnMannequins());
            
        }
    }

    public void UpdateMannequinAmount()
    {
        mannequinsOnCatwalk--;
        if(mannequinsOnCatwalk == 0)
        {
            Destroy(currentEtalage);
        }
    }

    IEnumerator SpawnMannequins()
    {
        foreach (Transform m in mannequinHolder)
        {
            Transform mannequin = Instantiate(m, startWaypoint.position, Quaternion.identity);
            MannequinWaypointFollower mannequinWaypointFollower =  mannequin.gameObject.AddComponent<MannequinWaypointFollower>();
            mannequinWaypointFollower.moveSpeed = speed;
            mannequinWaypointFollower.currentWayPoint = startWaypoint;
            mannequinWaypointFollower.distanceThreshold = this.distanceThreshold;
            mannequinWaypointFollower.mannequinManager = this.gameObject.GetComponent<MannequinManager>();
            mannequinsOnCatwalk++;
            yield return new WaitForSeconds(mannequinSpawnInterval);
        }
    }
}
