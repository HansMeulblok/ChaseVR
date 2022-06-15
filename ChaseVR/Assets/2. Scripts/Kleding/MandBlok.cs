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
        if (other.TryGetComponent(out KledingStuk kledingStuk))
        {
            AddClothingToMand(kledingStuk);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out KledingStuk kledingStuk))
        {
            RemoveClothingFromMand(kledingStuk);
        }
    }


    public void AddClothingToMand(KledingStuk clothing)
    {
        artikelenInMand.Add(clothing.artikelNummer);

        GameObject child = Instantiate(mandPanelPrefab, uiContent.transform);

        child.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = GetItemName(clothing.artikelNummer, clothing.typeKleding);


        clothing.GetComponent<Rigidbody>().useGravity = true;
        //clothing.GetComponent<BoxCollider>().isTrigger = false;
        clothing.GetComponent<XRGrabInteractable>().enabled = true;
    }

    public void RemoveClothingFromMand(KledingStuk clothing)
    {
        artikelenInMand.Remove(clothing.artikelNummer);

        foreach (Transform child in uiContent.transform)
        {
            if (child.GetComponentInChildren<TextMeshProUGUI>().text == GetItemName(clothing.artikelNummer, clothing.typeKleding))
            {
                Destroy(child.gameObject);
            }
        }

        clothing.GetComponent<Rigidbody>().useGravity = false;
        //clothing.GetComponent<BoxCollider>().isTrigger = false;
        clothing.GetComponent<XRGrabInteractable>().enabled = false;
    }

    public string GetItemName(int clothingName, KledingStuk.TypeKleding typeKleding)
    {
        switch (typeKleding)
        {
            case KledingStuk.TypeKleding.torso:

                foreach (GameObject torsoKleding in KledingManager.Instance.torsoKleding)
                {
                    if (torsoKleding.GetComponent<KledingStuk>().artikelNummer == clothingName)
                    {
                        return torsoKleding.name;
                    }
                }

                break;
            
            case KledingStuk.TypeKleding.benen:

                foreach (GameObject benenKleding in KledingManager.Instance.benenKleding)
                {
                    if (benenKleding.GetComponent<KledingStuk>().artikelNummer == clothingName)
                    {
                        return benenKleding.name;
                    }
                }

                break;
            
            case KledingStuk.TypeKleding.schoenen:

                foreach (GameObject schoenenKleding in KledingManager.Instance.schoenenKleding)
                {
                    if (schoenenKleding.GetComponent<KledingStuk>().artikelNummer == clothingName)
                    {
                        return schoenenKleding.name;
                    }
                }

                break;
        }

        return "ITEM NOT FOUND";
    }
}
