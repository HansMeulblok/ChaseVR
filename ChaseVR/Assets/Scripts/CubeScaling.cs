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
        maxSize = transform.localScale.x;
        maxdistance = Vector3.Distance(transform.position, controllerTransform.position);
    }

    void Update()
    {
        currentDistance = Vector3.Distance(transform.position, controllerTransform.position); 
        float newScale = currentDistance / maxdistance;
        if(newScale > minSize)
        {
            transform.localScale = new Vector3(newScale, newScale, newScale);
        }
    }
}
