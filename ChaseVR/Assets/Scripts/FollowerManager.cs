using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowerManager : MonoBehaviour
{
    [SerializeField] private GameObject blockEtalage;
    [SerializeField] private int amountOfBlocks;
    [SerializeField] private float spawnInterval = 1.5f;
    [SerializeField] private float speed = 1;
    [SerializeField] Transform blockHolder;
    private List<GameObject> cubes = new List<GameObject>();
    private bool paused = false;
    void Start()
    {
        SpawnCubes();
    }

    public void SpawnCubes()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            GameObject block = Instantiate(blockEtalage, transform.GetChild(i).position, Quaternion.identity, blockHolder);
            WaypointFollower waypointFollower = block.GetComponentInChildren<WaypointFollower>();
            waypointFollower.waypoints = this.gameObject.GetComponent<Waypoints>();
            waypointFollower.currentWayPoint = transform.GetChild(i);
            waypointFollower.moveSpeed = speed;
            waypointFollower.distanceThreshold = 0.5f;
            cubes.Add(block);
        }
    }


    public void Pause()
    {
        // turn off booleans to move if pause is called
        paused = !paused;

        if(paused)
        {
            foreach (var cube in cubes)
            {
                WaypointFollower waypointFollower = cube.GetComponentInChildren<WaypointFollower>();
                waypointFollower.canMove = false;
            }
        }
        else
        {
            foreach (var cube in cubes)
            {
                WaypointFollower waypointFollower = cube.GetComponentInChildren<WaypointFollower>();
                waypointFollower.canMove = false;
            }
        }

    }
}
