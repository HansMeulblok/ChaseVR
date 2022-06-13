using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using TMPro;

//[RequireComponent(typeof(Rigidbody))]
public class MandBlok : MonoBehaviour
{
    [SerializeField]
    private List<int> artikelenInMand = new List<int>();

    private GameObject uiContent;
    private GameObject mandPanelPrefab;

    private void Start()
    {
        uiContent = GameObject.Find("MandUiContent");
        mandPanelPrefab = Resources.Load<GameObject>("Mand/MandPanel");
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "schoenen" ||
            other.gameObject.tag == "torso" ||
            other.gameObject.tag == "benen")
        {
            AddClothingToMand(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "schoenen" ||
            other.gameObject.tag == "torso" ||
            other.gameObject.tag == "benen")
        {
            RemoveClothingFromMand(other.gameObject);
        }
    }


    public void AddClothingToMand(GameObject clothing)
    {
        if (clothing.transform.parent != null)
        {
            artikelenInMand.Add(clothing.transform.parent.GetComponent<KledingStuk>().artikelNummer);

            GameObject child = Instantiate(mandPanelPrefab, uiContent.transform);

            child.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = GetItemName(clothing.name, GetTypeKleding(clothing));


            clothing.GetComponent<Rigidbody>().useGravity = true;
            clothing.GetComponent<BoxCollider>().isTrigger = false;
            clothing.GetComponent<XRGrabInteractable>().enabled = true;
        }
    }

    public void RemoveClothingFromMand(GameObject clothing)
    {
        artikelenInMand.Remove(int.Parse(clothing.gameObject.name));

        foreach (Transform child in uiContent.transform)
        {
            if (child.GetComponentInChildren<TextMeshProUGUI>().text == GetItemName(clothing.name, (KledingStuk.TypeKleding)System.Enum.Parse( typeof(KledingStuk.TypeKleding), clothing.tag)))
            {
                Destroy(child.gameObject);
            }
        }
    }

    public string GetItemName(string clothingName, KledingStuk.TypeKleding typeKleding)
    {
        switch (typeKleding)
        {
            case KledingStuk.TypeKleding.torso:

                foreach (GameObject torsoKleding in KledingManager.Instance.torsoKleding)
                {
                    if (torsoKleding.GetComponent<KledingStuk>().artikelNummer == int.Parse(clothingName))
                    {
                        return torsoKleding.name;
                    }
                }

                break;
            
            case KledingStuk.TypeKleding.benen:

                foreach (GameObject benenKleding in KledingManager.Instance.benenKleding)
                {
                    if (benenKleding.GetComponent<KledingStuk>().artikelNummer == int.Parse(clothingName))
                    {
                        return benenKleding.name;
                    }
                }

                break;
            
            case KledingStuk.TypeKleding.schoenen:

                foreach (GameObject schoenenKleding in KledingManager.Instance.schoenenKleding)
                {
                    if (schoenenKleding.GetComponent<KledingStuk>().artikelNummer == int.Parse(clothingName))
                    {
                        return schoenenKleding.name;
                    }
                }

                break;
        }

        return "ITEM NOT FOUND";
    }

    public int GetArtikelNummer(GameObject gameObject)
    {
        if (gameObject.TryGetComponent(typeof(KledingStuk), out Component kledingStuk))
        {
            return kledingStuk.GetComponent<KledingStuk>().artikelNummer;
        }
        else if (gameObject.transform.parent.TryGetComponent(typeof(KledingStuk), out Component kledingStuk2))
        {
            return kledingStuk2.GetComponent<KledingStuk>().artikelNummer;
        }

        return 69;
    }

    public KledingStuk.TypeKleding GetTypeKleding(GameObject gameObject)
    {
        if (gameObject.TryGetComponent(typeof(KledingStuk), out Component kledingStuk))
        {
            return kledingStuk.GetComponent<KledingStuk>().typeKleding;
        }
        else if (gameObject.transform.parent.TryGetComponent(typeof(KledingStuk), out Component kledingStuk2))
        {
            return kledingStuk2.GetComponent<KledingStuk>().typeKleding;
        }

        return KledingStuk.TypeKleding.geenType;
    }
}
