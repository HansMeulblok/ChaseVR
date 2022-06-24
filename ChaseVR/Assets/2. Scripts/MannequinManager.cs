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
    //[HideInInspector]
    public List<GameObject> mannequinFollowers = new List<GameObject>();
    GameObject currentEtalage;
    Transform mannequinHolder;
    private SphereCollider[] kledingCollider;
    private bool canMoveMannequins = true;

    private Waypoints waypoints;
    public Transform firstWaypoint;

    private void Start()
    {
        waypoints = FindObjectOfType<Waypoints>();
    }


    void OnTriggerEnter(Collider collider)
    {
        // #TODO idee van jord, gebruik een queue
        if(collider.tag == "Etalage" && (currentEtalage == null))
        {
            Destroy(currentEtalage);
            currentEtalage = collider.gameObject;
            Destroy(collider.GetComponent<XRGrabInteractable>());
            Destroy(collider.GetComponent<Rigidbody>());
            Destroy(collider.GetComponent<Outline>());
            collider.transform.localScale = desiredScale;
            collider.transform.position = blockPlacementPoint.position;
            collider.transform.rotation = gameObject.transform.rotation;

            mannequinHolder = collider.transform.GetChild(0).transform;

            StartCoroutine(SpawnMannequins());
        }
    }

    private void FixedUpdate()
    {
        CanMoveMannequins();
    }

    public void CanMoveMannequins()
    {
        if (mannequinFollowers.Count == 0)
        {
            Destroy(currentEtalage);
        }

        if (!canMoveMannequins)
        { 
            foreach (GameObject mannequin in mannequinFollowers)
            {
                int indexOfMannequin = mannequinFollowers.IndexOf(mannequin);
                MannequinWaypointFollower mannequinWaypointFollower = mannequin.GetComponent<MannequinWaypointFollower>();

                if (indexOfMannequin != 0 &&
                    Vector3.Distance(mannequin.transform.position,
                                     mannequinFollowers[indexOfMannequin - 1].transform.position)
                                     <= 1.8f)
                {
                    mannequinWaypointFollower.canMove = false;
                }
                else if (indexOfMannequin != 0 &&
                         Vector3.Distance(mannequin.transform.position,
                                          mannequinFollowers[indexOfMannequin - 1].transform.position)
                                          > 1.8f)
                {
                    mannequinWaypointFollower.canMove = true;
                }
            }
        }
    }

    public void UpdateMannequinAmount(GameObject mannequin)
    {
        mannequinsOnCatwalk--;
        mannequinFollowers.Remove(mannequin);

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
            m.gameObject.SetActive(false);

            MannequinWaypointFollower mannequinWaypointFollower = mannequin.gameObject.AddComponent<MannequinWaypointFollower>();
            mannequinWaypointFollower.moveSpeed = mannequinSpeed;
            mannequinWaypointFollower.currentWayPoint = startWaypoint;
            mannequinWaypointFollower.distanceThreshold = this.distanceThreshold;
            mannequinWaypointFollower.mannequinManager = this.gameObject.GetComponent<MannequinManager>();
            mannequinWaypointFollower.name += " " + mannequinsOnCatwalk;
            mannequinFollowers.Add(mannequinWaypointFollower.gameObject);
            mannequinsOnCatwalk++;

            // First Item check
            if (mannequin.childCount >= 4 &&
                mannequin.GetChild(3).TryGetComponent(typeof(BoxCollider), out Component firstItemWithCollider))
            {
                EnableCorrectBoxCollider(firstItemWithCollider.gameObject);
            }

            // Second Item check
            if (mannequin.childCount >= 5 &&
                mannequin.GetChild(4).TryGetComponent(typeof(BoxCollider), out Component secondItemWithCollider))
            {
                EnableCorrectBoxCollider(secondItemWithCollider.gameObject);
            }

            // Third Item check
            if (mannequin.childCount >= 6 && 
                mannequin.GetChild(5).TryGetComponent(typeof(BoxCollider), out Component thirdItemWithCollider))
            {
                EnableCorrectBoxCollider(thirdItemWithCollider.gameObject);
            }

            if (mannequin.transform.Find("ClothesHitbox"))
            {
                mannequin.transform.Find("ClothesHitbox").gameObject.SetActive(true);
            }
    
            foreach (Transform clothing in mannequin)
            {
                if (clothing.TryGetComponent(out KledingStuk clothingComponent))
                {
                    foreach(Transform t in clothingComponent.transform)
                    {
                        clothing.GetComponentInChildren<Outline>().enabled = true;
                    }
                }                 
            }

            while (Vector3.Distance(mannequin.transform.position, firstWaypoint.position/*waypoints.transform.GetChild(0).transform.position*/) >= 0.2f)
            {
                yield return null;
            }
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
                canMoveMannequins = false;
            }
        }
        else
        {
            foreach (var f in mannequinFollowers)
            {
                MannequinWaypointFollower waypointFollower = f.GetComponentInChildren<MannequinWaypointFollower>();
                waypointFollower.canMove = true;
                canMoveMannequins = true;
            }
        }

    }

    public void EnableCorrectBoxCollider(GameObject gameObjectWithCollider)
    {
        BoxCollider[] boxColliders = gameObjectWithCollider.GetComponents<BoxCollider>();
        foreach (BoxCollider boxCollider in boxColliders)
        {
            boxCollider.enabled = true;
            break;
        }

        gameObjectWithCollider.GetComponent<XRGrabInteractable>().enabled = true;
        gameObjectWithCollider.GetComponent<KledingStuk>().enabled = true;


        /*switch (gameObjectWithCollider.GetComponent<KledingStuk>().typeKleding)
        {
            case KledingStuk.TypeKleding.torso:
                gameObjectWithCollider.GetComponent<Torso>().enabled = true;

                break;

            case KledingStuk.TypeKleding.benen:
                gameObjectWithCollider.GetComponent<Benen>().enabled = true;
                break;

            case KledingStuk.TypeKleding.schoenen:
                gameObjectWithCollider.GetComponent<Schoenen>().enabled = true;
                break;

        }*/
    }
}
