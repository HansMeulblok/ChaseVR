using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

//[RequireComponent(typeof(Rigidbody))]
public class MandBlok : MonoBehaviour
{
    private List<int> artikelenInMand = new List<int>();

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Schoenen" ||
            other.gameObject.tag == "Torso" ||
            other.gameObject.tag == "Benen")
        {
            artikelenInMand.Add(other.transform.parent.GetComponent<KledingStuk>().artikelNummer);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Schoenen" ||
            other.gameObject.tag == "Torso" ||
            other.gameObject.tag == "Benen")
        {
            artikelenInMand.Remove(other.transform.parent.GetComponent<KledingStuk>().artikelNummer);
        }
    }
}
