using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoints : MonoBehaviour
{
    [Range(0f, 2f)]
    [SerializeField] private float waypointSize;
    [SerializeField] private Color wayPointColour;
    [SerializeField] private Color lineColour;


    private async void OnDrawGizmos()
    {
        // Draw circles for all waypoints
        foreach (Transform t in transform)
        {
            Gizmos.color = wayPointColour;
            Gizmos.DrawWireSphere(t.position, waypointSize);
        }

        // Draw lines between waypoints in hierarchy older
        Gizmos.color = lineColour;
        for (int i = 0; i < transform.childCount -1; i++)
        {
            Gizmos.DrawLine(transform.GetChild(i).position, transform.GetChild(i + 1).position);
        }


        Gizmos.DrawLine(transform.GetChild(transform.childCount - 1).position, transform.GetChild(0).position);
    }

    public Transform GetNextWaypoint(Transform currentWaypoint)
    {
        // Get next waypoit except for when it is null or if you are currently at the last checkpoint
        if(currentWaypoint == null)
        {
            return transform.GetChild(0);
        }

        if(currentWaypoint.GetSiblingIndex()< transform.childCount -1)
        {
            return transform.GetChild(currentWaypoint.GetSiblingIndex() + 1);
        }
        else
        {
            return transform.GetChild(0);
        }
    }
}
