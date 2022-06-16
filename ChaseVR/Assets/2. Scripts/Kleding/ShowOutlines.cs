using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowOutlines : MonoBehaviour
{
    private Ray OutlineRay;
    RaycastHit hitdata;
    //hittable gameobjects
    [HideInInspector]
    public BlokEtalage BlokEtalage;
    public LayerMask OutlineHitboxLayer;
    //outline components
    Outline outlineChangeEtalage;
    Outline outlineChangeKleding;
    //failsafe variables
    private List<Outline> raycastHasHit;
    private GameObject FoldedNormal;
    private KledingStuk kledingstuk;



    // Start is called before the first frame update
    void Start()
    {
        raycastHasHit = new List<Outline>();
        outlineChangeEtalage = gameObject.GetComponentInParent<Outline>(); ;
        outlineChangeKleding = gameObject.GetComponentInParent<Outline>(); ;
    }

    // Update is called once per frame
    void Update()
    {
        OutlineRay = new Ray(this.transform.position, transform.forward);

        if (Physics.Raycast(OutlineRay, out hitdata, 30, OutlineHitboxLayer))
        {
            switch (hitdata.transform.tag)
            {
                case "Etalage":
                    outlineChangeEtalage = hitdata.transform.GetComponent<Outline>();
                    outlineChangeEtalage.OutlineColor = Color.green;
                    Failsafe(outlineChangeEtalage);
                    break;

                case "torso":
                case "benen":

                    foreach (Transform child in hitdata.transform)
                    {
                        if (child.GetComponent<MeshRenderer>().enabled == true)
                        {
                            FoldedNormal = child.transform.gameObject;
                            break;
                        }
                    }
                    outlineChangeKleding = FoldedNormal.transform.GetComponent<Outline>();
                    outlineChangeKleding.OutlineColor = Color.green;
                    Failsafe(outlineChangeKleding);
                    break;

                case "schoenen":

                    if (hitdata.transform.gameObject.GetComponent<KledingStuk>().kledingStaat == KledingStuk.KledingStaten.opgevouwen)
                    {
                        outlineChangeKleding = hitdata.transform.GetChild(2).GetComponent<Outline>();
                        outlineChangeKleding.OutlineColor = Color.green;
                        Failsafe(outlineChangeKleding);
                        break;
                    }
                    else if (hitdata.transform.gameObject.GetComponent<KledingStuk>().kledingStaat == KledingStuk.KledingStaten.statisch)
                    {
                        outlineChangeKleding = hitdata.transform.GetComponent<Outline>();
                        outlineChangeKleding.OutlineColor = Color.green;
                        Failsafe(outlineChangeKleding);
                        break;
                    }
                    break;
            }
        }
        else
        {
            outlineChangeEtalage.OutlineColor = Color.clear;
            outlineChangeKleding.OutlineColor = Color.clear;
        }
    }

    private void Failsafe(Outline outlineChange)
    {
        if (!raycastHasHit.Contains(outlineChange))
        {
            raycastHasHit.Add(outlineChange);
        }
        foreach (Outline OutlineFailsafeCheck in raycastHasHit)
        {
            if (OutlineFailsafeCheck == outlineChange)
            {
                break;
            }
            else
            {
                OutlineFailsafeCheck.OutlineColor = Color.clear;
                raycastHasHit.Remove(OutlineFailsafeCheck);
                break;
            }
        }
    }

}
