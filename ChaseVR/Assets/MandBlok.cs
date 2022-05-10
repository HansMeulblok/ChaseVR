using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[RequireComponent(typeof(Rigidbody))]
public class MandBlok : MonoBehaviour
{
    //Rigidbody rb;

    private void Start()
    {
        
        /*rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeAll;*/
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Schoenen" ||
            other.gameObject.tag == "Torso" ||
            other.gameObject.tag == "Benen")
        {
            Debug.Log(other.GetComponent<KledingStuk>().artikelNummer);
        }
    }
}
