using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckControllersInSurfTrigger : MonoBehaviour
{
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.name.Contains("Hand"))
        {
            
        }
    }
}
