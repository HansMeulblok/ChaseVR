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

    public int amountToPool;

    [HideInInspector]
    public GameObject rightRayInteractor;


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

                kledingArtikel.layer = LayerMask.NameToLayer("KledingHitbox");

                if (kledingArtikel.GetComponent<KledingStuk>().typeKleding != KledingStuk.TypeKleding.schoenen)
                    FlipValuesForStateSwitch(kledingArtikel);
                else
                    FlipValuesForShoes(kledingArtikel);

                break;

            case KledingStuk.KledingStaten.statisch:

                kledingArtikel.layer = LayerMask.NameToLayer("Default");

                if (kledingArtikel.GetComponent<KledingStuk>().typeKleding != KledingStuk.TypeKleding.schoenen)
                    FlipValuesForStateSwitch(kledingArtikel);
                else
                    FlipValuesForShoes(kledingArtikel);

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
            grabbedObject.gameObject.layer = LayerMask.NameToLayer("Default");
            grabbedObject.GetComponent<Rigidbody>().useGravity = false;
            grabbedObject.GetComponent<Rigidbody>().isKinematic = false;
            grabbedObject.GetComponent<Rigidbody>().AddForce(args.interactableObject.transform.forward * 5f, ForceMode.Impulse);
        }
    }

    private void FlipValuesForStateSwitch(GameObject kledingArtikel)
    {
        kledingArtikel.transform.GetChild(0).GetComponent<MeshRenderer>().enabled = !kledingArtikel.transform.GetChild(0).GetComponent<MeshRenderer>().enabled;
        kledingArtikel.transform.GetChild(1).GetComponent<MeshRenderer>().enabled = !kledingArtikel.transform.GetChild(1).GetComponent<MeshRenderer>().enabled;

        boxColliders = kledingArtikel.transform.GetComponentsInChildren<BoxCollider>();

        foreach (BoxCollider boxCollider in boxColliders)
        {
            boxCollider.enabled = !boxCollider.enabled;
        }
    }

    private void FlipValuesForShoes(GameObject kledingArtikel)
    {
        kledingArtikel.transform.GetChild(1).GetComponent<MeshRenderer>().enabled = !kledingArtikel.transform.GetChild(1).GetComponent<MeshRenderer>().enabled;
    }
}
