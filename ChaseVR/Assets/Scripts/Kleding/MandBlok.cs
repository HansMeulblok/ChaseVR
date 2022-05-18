using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

//[RequireComponent(typeof(Rigidbody))]
public class MandBlok : MonoBehaviour
{
    [SerializeField]
    private List<int> artikelenInMand = new List<int>();

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Schoenen" ||
            other.gameObject.tag == "Torso" ||
            other.gameObject.tag == "Benen")
        {
            AddClothingToMand(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Schoenen" ||
            other.gameObject.tag == "Torso" ||
            other.gameObject.tag == "Benen")
        {
            RemoveClothingFromMand(other.gameObject);
        }
    }


    public void AddClothingToMand(GameObject clothing)
    {
        if (clothing.transform.parent != null)
        {
            artikelenInMand.Add(clothing.transform.parent.GetComponent<KledingStuk>().artikelNummer);

            clothing.GetComponent<Rigidbody>().useGravity = true;
            clothing.GetComponent<BoxCollider>().isTrigger = false;
            clothing.GetComponent<XRGrabInteractable>().enabled = true;
        }
    }

    public void RemoveClothingFromMand(GameObject clothing)
    {
        artikelenInMand.Remove(int.Parse(clothing.gameObject.name));
    }
}
