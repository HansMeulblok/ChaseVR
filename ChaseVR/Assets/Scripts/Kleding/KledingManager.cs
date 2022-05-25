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
        GameObject tmp;

        for (int i = 0; i < amountToPool; i++)
        {
            tmp = Instantiate(opgevouwenKledingObjectToPool, transform);
            tmp.SetActive(false);
            opgevouwenKledingObjects.Add(tmp);
        }
    }

    public GameObject GetPooledOpgevouwenKledingObject()
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
    }

    public void ChangeKledingModel(MeshRenderer kledingArtikelMR, KledingStuk.KledingStaten kledingStaat)
    {
        switch (kledingStaat)
        {
            case KledingStuk.KledingStaten.opgevouwen:

                if (kledingArtikelMR.gameObject.TryGetComponent(typeof(KledingStuk), out Component kledingStuk))
                {
                    kledingArtikelMR.enabled = false;
                    kledingArtikelMR.gameObject.GetComponent<BoxCollider>().enabled = false;

                    GameObject tmp;

                    tmp = GetPooledOpgevouwenKledingObject();
                    tmp.GetComponent<MeshRenderer>().sharedMaterial = kledingArtikelMR.sharedMaterial;

                    //tmp.AddComponent<KledingStuk>().artikelNummer = kledingArtikelMR.GetComponent<KledingStuk>().artikelNummer;

                    tmp.name = kledingArtikelMR.GetComponent<KledingStuk>().artikelNummer.ToString();//kledingArtikelMR.name + " opgevouwen";
                    tmp.tag = kledingArtikelMR.tag;
                    tmp.transform.SetParent(kledingArtikelMR.transform, false);
                }

                break;

            case KledingStuk.KledingStaten.statisch:

                for (int i = 0; i < amountToPool; i++)
                {
                    if (opgevouwenKledingObjects[i].activeInHierarchy &&
                        opgevouwenKledingObjects[i].gameObject.GetComponent<MeshRenderer>().sharedMaterial == kledingArtikelMR.sharedMaterial)
                    {
                        opgevouwenKledingObjects[i].transform.SetParent(gameObject.transform, false);
                        opgevouwenKledingObjects[i].transform.position = Vector3.zero;
                        //Destroy(opgevouwenKledingObjects[i].gameObject.GetComponent<KledingStuk>());
                        opgevouwenKledingObjects[i].SetActive(false);
                    }
                }
                
                kledingArtikelMR.enabled = true;
                kledingArtikelMR.gameObject.GetComponent<BoxCollider>().enabled = true;
                kledingArtikelMR.gameObject.GetComponent<XRGrabInteractable>().enabled = true;


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
        if (args.interactableObject.transform.TryGetComponent(typeof(Rigidbody), out Component grabbedObject) && !args.interactableObject.transform.TryGetComponent(typeof(BlokEtalage), out Component blokEtalage) && args.interactableObject.transform.name != "ChaseLogo")
        {
            if (grabbedObject.transform.childCount == 0)
            {
                grabbedObject.GetComponent<XRGrabInteractable>().enabled = false;
                grabbedObject.GetComponent<Rigidbody>().useGravity = false;
                grabbedObject.GetComponent<Rigidbody>().AddForce(args.interactableObject.transform.forward * 5f, ForceMode.Impulse);

            }
            else
            {
                grabbedObject.transform.GetChild(0).GetComponent<XRGrabInteractable>().enabled = false;
                grabbedObject.transform.GetChild(0).GetComponent<Rigidbody>().useGravity = false;
                grabbedObject.transform.GetChild(0).GetComponent<Rigidbody>().AddForce(args.interactableObject.transform.forward * 5f, ForceMode.Impulse);
            }
        }
    }
}
