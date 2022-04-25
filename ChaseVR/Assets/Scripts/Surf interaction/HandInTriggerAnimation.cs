using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandInTriggerAnimation : MonoBehaviour
{
    private Vector3 _startPosition;

    void Start()
    {
        _startPosition = transform.position;
    }

    void FixedUpdate()
    {
        transform.position = _startPosition + new Vector3(0.15f * Mathf.Sin(Time.time), 0.0f, 0.0f);
    }
}
