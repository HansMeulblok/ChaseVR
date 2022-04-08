using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeScaling : MonoBehaviour
{
    [SerializeField] private float maxSize;
    [SerializeField] private float minSize = 0.1f;
    [HideInInspector] public Transform controllerTransform;
    private float maxdistance;
    private float currentDistance;
    void Start()
    {
        // Set the maximum distance and initial scale
        maxSize = transform.localScale.x;
        maxdistance = Vector3.Distance(transform.position, controllerTransform.position);
    }

    void Update()
    {
        currentDistance = Vector3.Distance(transform.position, controllerTransform.position); 
        
        // change scale based on distance from controller
        float newScale = currentDistance / maxdistance;
        if(newScale > minSize && newScale <= maxSize)
        {
            transform.localScale = new Vector3(newScale, newScale, newScale);
        }
    }
}
