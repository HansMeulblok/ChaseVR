using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowerManager : MonoBehaviour
{
    [SerializeField] private GameObject waypointFollower;
    [SerializeField] private int amountOfBlocks;
    [SerializeField] private float spawnInterval = 1.5f;
    [SerializeField] private float speed = 1;
    [SerializeField] Transform blockHolder;
    private List<GameObject> followers = new List<GameObject>();
    public List<GameObject> blokEtalages = new List<GameObject>();
    private bool paused = false;

    void Start()
    {
        SpawnFollowers();
    }

    public void SpawnFollowers()
    {

        // spawn holder points
        // let holder points spawn cubes
        // let holder points repsawn cubes after 
        
        for (int i = 0; i < transform.childCount; i++)
        {
            GameObject newWaypointFollower = Instantiate(waypointFollower, transform.GetChild(i).position, Quaternion.identity, blockHolder);
            WaypointFollower waypointFollowerComponent = waypointFollower.GetComponentInChildren<WaypointFollower>();
            waypointFollowerComponent.waypoints = this.gameObject.GetComponent<Waypoints>();
            waypointFollowerComponent.followerManager = this.GetComponent<FollowerManager>();
            waypointFollowerComponent.currentWayPoint = transform.GetChild(i);
            waypointFollowerComponent.moveSpeed = speed;
            waypointFollowerComponent.distanceThreshold = 0.5f;
            followers.Add(waypointFollower);
        }
    }


    public void Pause()
    {
        // turn off booleans to move if pause is called
        paused = !paused;

        if(paused)
        {
            foreach (var f in followers)
            {
                WaypointFollower waypointFollower = f.GetComponentInChildren<WaypointFollower>();
                waypointFollower.canMove = false;
            }
        }
        else
        {
            foreach (var f in followers)
            {
                WaypointFollower waypointFollower = f.GetComponentInChildren<WaypointFollower>();
                waypointFollower.canMove = false;
            }
        }

    }

    public GameObject RandomBlok()
    {
        int randomInt = Random.Range(0, blokEtalages.Count);
        return blokEtalages[randomInt];
    }
}
