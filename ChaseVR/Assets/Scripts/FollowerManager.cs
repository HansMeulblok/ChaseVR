using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowerManager : MonoBehaviour
{
    [SerializeField] private GameObject blockEtalage;
    [SerializeField] private int amountOfBlocks;
    [SerializeField] private float interval = 1.5f;
    [SerializeField] private float speed = 1;
    [SerializeField] Transform blockHolder;
    private List<GameObject> cubes = new List<GameObject>();
    private bool paused = false;
    void Start()
    {
        StartCoroutine(BlokSetup());
    }

    IEnumerator BlokSetup()
    {
        for (int i = 0; i < amountOfBlocks; i++)
        {
            GameObject block = Instantiate(blockEtalage, Vector3.zero, Quaternion.identity, blockHolder);
            block.GetComponentInChildren<WaypointFollower>().waypoints = this.gameObject.GetComponent<Waypoints>();
            block.GetComponentInChildren<WaypointFollower>().moveSpeed = speed;

            cubes.Add(block);

            yield return new WaitForSeconds(interval);
        }
    }


    public void Pause()
    {
        paused = !paused;

        if(paused)
        {
            foreach (var cube in cubes)
            {
                cube.GetComponentInChildren<WaypointFollower>().canMove = false;
            }
        }
        else
        {
            foreach (var cube in cubes)
            {
                cube.GetComponentInChildren<WaypointFollower>().canMove = true;
            }
        }

    }
}
