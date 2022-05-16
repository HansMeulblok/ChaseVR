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
            tmp = Instantiate(opgevouwenKledingObjectToPool, GameObject.Find("KledingManager").transform);
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

    public void ChangeKledingModel(MeshRenderer kledingArtikelMR, KledingStuk.kledingStaten kledingStaat)
    {
        switch (kledingStaat)
        {
            case KledingStuk.kledingStaten.opgevouwen:

                kledingArtikelMR.enabled = false;
                kledingArtikelMR.gameObject.GetComponent<BoxCollider>().enabled = false;

                GameObject tmp;
                tmp = GetPooledOpgevouwenKledingObject();
                tmp.name = kledingArtikelMR.name + " opgevouwen";
                tmp.GetComponent<MeshRenderer>().material = kledingArtikelMR.material;
                tmp.tag = kledingArtikelMR.tag;
                //rightRayInteractor.GetComponent<XRRayInteractor>().interactablesSelected[0] = tmp.GetComponent<XRGrabInteractable>();// = kledingArtikelMR.gameObject.transform.position;
                tmp.transform.SetParent(kledingArtikelMR.transform, false);

                if (kledingArtikelMR.gameObject.TryGetComponent(typeof(Torso), out Component torso))
                {
                    tmp.AddComponent<Torso>().artikelNummer = kledingArtikelMR.gameObject.GetComponent<Torso>().artikelNummer;
                }
                else if (kledingArtikelMR.gameObject.TryGetComponent(typeof(Benen), out Component benen))
                {
                    tmp.AddComponent<Benen>().artikelNummer = kledingArtikelMR.gameObject.GetComponent<Benen>().artikelNummer;
                }
                else if (kledingArtikelMR.gameObject.TryGetComponent(typeof(Schoenen), out Component schoenen))
                {
                    tmp.AddComponent<Schoenen>().artikelNummer = kledingArtikelMR.gameObject.GetComponent<Schoenen>().artikelNummer;
                }

                

                break;

            case KledingStuk.kledingStaten.statisch:

                for (int i = 0; i < amountToPool; i++)
                {
                    if (opgevouwenKledingObjects[i].activeInHierarchy &&
                        opgevouwenKledingObjects[i].gameObject.GetComponent<MeshRenderer>().material == kledingArtikelMR.material)
                    {
                        opgevouwenKledingObjects[i].SetActive(false);
                        /// todo deparent opgvouwen
                    }
                }

                //kledingArtikelMR.gameObject.transform.parent.gameObject.SetActive(true);
                kledingArtikelMR.enabled = true;
                kledingArtikelMR.gameObject.GetComponent<BoxCollider>().enabled = true;


                break;

            case KledingStuk.kledingStaten.geanimeerd:

                break;
        }
    }


    public void ChangeToOpgevouwen()
    {
        if (rightRayInteractor.GetComponent<XRRayInteractor>().interactablesSelected[0].transform.TryGetComponent(out KledingStuk kledingStuk))
        {
            kledingStuk.kledingStaat = KledingStuk.kledingStaten.opgevouwen;
        }
    }
    
    public void ChangeToStatisch()
    {
        rightRayInteractor.GetComponent<XRRayInteractor>().interactablesSelected[0].transform.GetComponent<KledingStuk>().kledingStaat = KledingStuk.kledingStaten.statisch;
    }

    public void ShootKleding(SelectExitEventArgs args)
    {
        //rightRayInteractor = GameObject.Find("Right Ray Interactor");

        Debug.Log(args.interactableObject.transform.gameObject.name);

        //args.interactableObject.transform.GetComponent<XRGrabInteractable>().enabled = false;

        if (args.interactableObject.transform.TryGetComponent(typeof(Rigidbody), out Component rigidbody))
        {
            Debug.Log("got rigidbody component");
            rigidbody.transform.GetChild(0).GetComponent<XRGrabInteractable>().enabled = false;
            rigidbody.GetComponent<XRGrabInteractable>().enabled = false;
            //rigidbody.GetComponent<Rigidbody>().AddForce(args.interactableObject.transform.forward * 5f, ForceMode.Impulse);
            rigidbody.transform.GetChild(0).GetComponent<Rigidbody>().AddForce(args.interactableObject.transform.forward * 5f, ForceMode.Impulse);
        }
    }
}
