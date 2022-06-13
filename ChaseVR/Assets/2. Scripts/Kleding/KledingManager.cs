    using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.XR.Interaction.Toolkit;

public class KledingManager : MonoBehaviour
{
    public static KledingManager Instance = null;


    [HideInInspector]
    public List<GameObject> torsoKleding = new List<GameObject>();
    [HideInInspector]
    public List<GameObject> benenKleding = new List<GameObject>();
    [HideInInspector]
    public List<GameObject> schoenenKleding = new List<GameObject>();
    private BoxCollider[] boxColliders;

    [HideInInspector] public List<GameObject> opgevouwenKledingObjects;
    public GameObject opgevouwenKledingObjectToPool;
    public int amountToPool;

    private GameObject rightRayInteractor;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        { 
            Destroy(gameObject);
        }

        torsoKleding = Resources.LoadAll("Kleding/Torso", typeof(GameObject)).Cast<GameObject>().ToList();
        benenKleding = Resources.LoadAll("Kleding/Benen", typeof(GameObject)).Cast<GameObject>().ToList();
        schoenenKleding = Resources.LoadAll("Kleding/Schoenen", typeof(GameObject)).Cast<GameObject>().ToList();
    }

    private void Start()
    {
        rightRayInteractor = GameObject.Find("Right Ray Interactor");
    }


    public void ChangeKledingModel(GameObject kledingArtikel, KledingStuk.KledingStaten kledingStaat)
    {
        switch (kledingStaat)
        {
            case KledingStuk.KledingStaten.opgevouwen:

                kledingArtikel.layer = 7;
                FlipValuesForStateSwitch(kledingArtikel, KledingStuk.KledingStaten.opgevouwen);

                //Debug.Log("changed to opgevouwen");

                break;

            case KledingStuk.KledingStaten.statisch:

                kledingArtikel.layer = 0;

                //Debug.Log("changed to statisch");

                FlipValuesForStateSwitch(kledingArtikel, KledingStuk.KledingStaten.statisch);
                

                break;

            case KledingStuk.KledingStaten.geanimeerd:

                break;

            default:
                Debug.Log("default state");
                break;
        }
    }

    public void ShootKleding(SelectExitEventArgs args)
    {
        if (args.interactableObject.transform.TryGetComponent(out KledingStuk grabbedObject))
        {
            grabbedObject.gameObject.layer = 0;
            grabbedObject.GetComponent<XRGrabInteractable>().enabled = false;
            grabbedObject.GetComponent<Rigidbody>().isKinematic = false;
            grabbedObject.GetComponent<Rigidbody>().AddForce(args.interactableObject.transform.forward * 5f, ForceMode.Impulse);
        }
    }

    private void FlipValuesForStateSwitch(GameObject kledingArtikel, KledingStuk.KledingStaten kledingStaat)
    {
        kledingArtikel.transform.GetChild(0).GetComponent<MeshRenderer>().enabled = !kledingArtikel.transform.GetChild(0).GetComponent<MeshRenderer>().enabled;
        kledingArtikel.transform.GetChild(1).GetComponent<MeshRenderer>().enabled = !kledingArtikel.transform.GetChild(1).GetComponent<MeshRenderer>().enabled;

        boxColliders = kledingArtikel.transform.GetComponentsInChildren<BoxCollider>();

        foreach (BoxCollider boxCollider in boxColliders)
        {
            boxCollider.enabled = !boxCollider.enabled;
        }
    }
}
