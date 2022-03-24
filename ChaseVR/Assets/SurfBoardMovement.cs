using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SurfBoardMovement : MonoBehaviour
{
    public float speed = 1f;

    private void Start()
    {
        speed *= 0.01f;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(transform.position.x + speed, transform.position.y, transform.position.z);
    }
}
