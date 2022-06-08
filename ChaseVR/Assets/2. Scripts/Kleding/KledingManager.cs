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
        ///GameObject tmp;

        /*for (int i = 0; i < amountToPool; i++)
        {
            tmp = Instantiate(opgevouwenKledingObjectToPool, transform);
            tmp.SetActive(false);
            opgevouwenKledingObjects.Add(tmp);
        }*/
    }

   /* public GameObject GetPooledOpgevouwenKledingObject()
    {
        for (int i = 0; i < amountToPool; i++)
        {
            if (!opgevouwenKledingObjects[i].activeInHierarchy)
            {
                opgevouwenKledingObjects[i].SetActive(true);

                return opgevouwenKledingObjects[i];
            }
        }
        return null;
    }*/

    public void ChangeKledingModel(GameObject kledingArtikel, KledingStuk.KledingStaten kledingStaat)
    {
        switch (kledingStaat)
        {
            case KledingStuk.KledingStaten.opgevouwen:

                FlipValuesForStateSwitch(kledingArtikel, KledingStuk.KledingStaten.opgevouwen);

                break;

            case KledingStuk.KledingStaten.statisch:

                FlipValuesForStateSwitch(kledingArtikel, KledingStuk.KledingStaten.statisch);

                break;

            case KledingStuk.KledingStaten.geanimeerd:

                break;

            default:
                Debug.Log("default state");
                break;
        }
    }


    public void ChangeToOpgevouwen()
    {
        if (rightRayInteractor.GetComponent<XRRayInteractor>().interactablesSelected[0].transform.TryGetComponent(out KledingStuk kledingStuk))
        {
            kledingStuk.kledingStaat = KledingStuk.KledingStaten.opgevouwen;
        }
    }
    
    public void ChangeToStatisch()
    {
        rightRayInteractor.GetComponent<XRRayInteractor>().interactablesSelected[0].transform.GetComponent<KledingStuk>().kledingStaat = KledingStuk.KledingStaten.statisch;
    }

    public void ShootKleding(SelectExitEventArgs args)
    {
        if (args.interactableObject.transform.TryGetComponent(out Benen grabbedObject))
        {
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

        /*if (kledingStaat == KledingStuk.KledingStaten.statisch)
        {
            kledingArtikel.gameObject.GetComponent<XRGrabInteractable>().enabled = true;
            kledingArtikel.gameObject.GetComponent<Rigidbody>().isKinematic = true;

            
        }
        else if (kledingStaat == KledingStuk.KledingStaten.opgevouwen)
        {
            //kledingArtikel.gameObject.GetComponent<Rigidbody>().isKinematic = false;
        }*/

    }
}
