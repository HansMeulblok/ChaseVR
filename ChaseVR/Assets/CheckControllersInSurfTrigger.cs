using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckControllersInSurfTrigger : MonoBehaviour
{
    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name.Contains("Hand"))
        {
            Debug.Log(gameObject.name + " is in posiiton");
        }
    }
}
