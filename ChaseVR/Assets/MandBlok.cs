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
            artikelenInMand.Add(other.GetComponent<KledingStuk>().artikelNummer);
            AddKledingVisual(other.gameObject, other.gameObject.tag);
        }
    }

    public void AddKledingVisual(GameObject kledingStuk, string tag)
    {
        kledingStuk.GetComponent<KledingStuk>().kledingStaat = KledingStuk.kledingStaten.opgevouwen;

        switch (tag)
        {
            case "Torso":

                
                //KledingManager.Instance.torsoKleding.Where(gameObject.GetComponent<KledingStuk>().artikelNummer => artikelNummer)

                break;

            case "Benen":



                break;            
            
            case "Schoenen":



                break;
        
        }
        
    }
}
