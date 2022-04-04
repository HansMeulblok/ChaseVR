using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowerManager : MonoBehaviour
{
    [SerializeField] private GameObject blockEtalage;
    [SerializeField] private int amountOfBlocks;
    [SerializeField] private float interval = 1.5f;
    [SerializeField] Transform blockHolder;
    private List<GameObject> cubes = new List<GameObject>();
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
            block.GetComponentInChildren<WaypointFollower>().index = i;
            // Debug.Log(i);
            block.GetComponentInChildren<WaypointFollower>().moveSpeed = 2;

            cubes.Add(block);
            if(i == amountOfBlocks - 1)
            {
                foreach (var cube in cubes)
                {
                    cube.GetComponent<WaypointFollower>().UpdateIndex();
                }
            }
            yield return new WaitForSeconds(interval);
        }
    }

    public void UpdateEverything()
    {
        foreach (var cube in cubes)
        {
            cube.GetComponent<WaypointFollower>().UpdateIndex();
        }
    }
}
