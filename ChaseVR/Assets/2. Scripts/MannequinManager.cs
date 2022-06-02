using System.Collections;
using System.Collections.Generic;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine;
using UnityEngine.InputSystem;

public class MannequinManager : MonoBehaviour
{
    [Header("Catwalk Settings")]
    [SerializeField] private Transform startWaypoint;
    [SerializeField] private Transform blockPlacementPoint;
    [SerializeField] private Waypoints mannequinWaypoints;
    [SerializeField] private float mannequinSpeed;
    [SerializeField] private float distanceThreshold;
    [SerializeField] private Vector3 desiredScale;
    [SerializeField] private float mannequinSpawnInterval;
    public int mannequinsOnCatwalk;
    public bool paused;
    private List<GameObject> mannequinFollowers = new List<GameObject>();
    GameObject currentEtalage;
    Transform mannequinHolder;
    private SphereCollider[] kledingCollider;


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
            collider.transform.rotation = Quaternion.identity;
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
            MannequinWaypointFollower mannequinWaypointFollower = mannequin.gameObject.AddComponent<MannequinWaypointFollower>();
            mannequinWaypointFollower.moveSpeed = mannequinSpeed;
            mannequinWaypointFollower.currentWayPoint = startWaypoint;
            mannequinWaypointFollower.distanceThreshold = this.distanceThreshold;
            mannequinWaypointFollower.mannequinManager = this.gameObject.GetComponent<MannequinManager>();
            mannequinFollowers.Add(mannequinWaypointFollower.gameObject);
            mannequinsOnCatwalk++;

            if (mannequin.TryGetComponent(typeof (BoxCollider), out Component gameObjectWithCollider))
            {
                BoxCollider[] boxColliders = mannequin.GetComponents<BoxCollider>();
                foreach (BoxCollider boxCollider in boxColliders)
                {
                    if (boxCollider.isTrigger)
                        boxCollider.enabled = true;
                }

                mannequin.GetComponent<XRGrabInteractable>().enabled = true;
                mannequin.GetComponent<Benen>().enabled = true;
            }

            if (mannequin.GetChild(2).gameObject.name == "ClothesHitbox")
            {
                mannequin.gameObject.SetActive(true);
            }
                
                //GetChild(3).gameObject.SetActive(true);
            /*kledingCollider = mannequin.GetChild(3).GetComponentsInChildren<SphereCollider>(includeInactive: true);
            foreach (SphereCollider collider in kledingCollider)
            {
                collider.enabled = true;
                collider.transform.gameObject.layer = 8;
            }*/
            yield return new WaitForSeconds(mannequinSpawnInterval);
        }


    }

    public void Pause(InputAction.CallbackContext context)
    {
        paused = !paused;

        if(paused)
        {
            foreach (var f in mannequinFollowers)
            {
                MannequinWaypointFollower waypointFollower = f.GetComponentInChildren<MannequinWaypointFollower>();
                waypointFollower.canMove = false;
            }
        }
        else
        {
            foreach (var f in mannequinFollowers)
            {
                MannequinWaypointFollower waypointFollower = f.GetComponentInChildren<MannequinWaypointFollower>();
                waypointFollower.canMove = true;
            }
        }

    }
}
