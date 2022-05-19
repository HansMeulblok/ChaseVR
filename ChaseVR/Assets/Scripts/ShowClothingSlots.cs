using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowClothingSlots : MonoBehaviour
{
    [HideInInspector]
    public ClothesTags clothesTags;
    private Ray ClothesRay;
    public LayerMask MannequinHitboxLayer;
    
    //variables needed for holding checks
    public SphereCollider holdingItemCollider;
    private bool IsHoldingTorso;
    private bool IsHoldingBenen;
    private bool IsHoldingSchoenen;

    //outlines of collider boxes
    Outline outlineChangeTorso;
    Outline outlineChangeBenen;
    Outline outlineChangeSchoenen;

    RaycastHit hitData;
    //string rayTag;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        ClothesRay = new Ray(this.transform.position, transform.forward);
        //Debug.DrawRay(ClothesRay.origin, ClothesRay.direction * 10);
        if (Physics.Raycast(ClothesRay, out hitData, 30,  MannequinHitboxLayer))
        {
            clothesTags = hitData.transform.GetComponent<ClothesTags>();
            outlineChangeTorso = clothesTags.TorsoPosition.gameObject.GetComponent<Outline>();
            outlineChangeBenen = clothesTags.BenenPosition.gameObject.GetComponent<Outline>();
            outlineChangeSchoenen = clothesTags.SchoenenPosition.gameObject.GetComponent<Outline>();
            //nothing is being held
            if (IsHoldingSchoenen == false && IsHoldingBenen == false && IsHoldingTorso == false)
            {
                if (clothesTags._torsoKleding == null)
                    outlineChangeTorso.OutlineColor = Color.green;
                else
                    outlineChangeTorso.OutlineColor = Color.red;
                if (clothesTags._benenKleding == null)
                    outlineChangeBenen.OutlineColor = Color.green;
                else
                    outlineChangeBenen.OutlineColor = Color.red;
                if (clothesTags._schoenenKleding == null)
                    outlineChangeSchoenen.OutlineColor = Color.green;
                else
                    outlineChangeSchoenen.OutlineColor = Color.red;
            }

            //torso
            if (IsHoldingTorso == true && clothesTags._torsoKleding == null)
            {
                outlineChangeTorso.OutlineColor = Color.green;
                outlineChangeBenen.OutlineColor = Color.clear;
                outlineChangeSchoenen.OutlineColor = Color.clear;
            }
            else if (IsHoldingTorso == true && clothesTags._torsoKleding != null)
            {
                outlineChangeTorso.OutlineColor = Color.red;
                outlineChangeBenen.OutlineColor = Color.clear;
                outlineChangeSchoenen.OutlineColor = Color.clear;
            }
            //benen
            if (IsHoldingBenen == true && clothesTags._benenKleding == null)
            {
                outlineChangeTorso.OutlineColor = Color.clear;
                outlineChangeBenen.OutlineColor = Color.green;
                outlineChangeSchoenen.OutlineColor = Color.clear;
            }
            else if (IsHoldingBenen == true && clothesTags._benenKleding != null) {
                outlineChangeTorso.OutlineColor = Color.clear;
                outlineChangeBenen.OutlineColor = Color.red;
                outlineChangeSchoenen.OutlineColor = Color.clear;
            }

            //schoenen
            if (IsHoldingSchoenen == true && clothesTags._schoenenKleding == null)
            {
                outlineChangeTorso.OutlineColor = Color.clear;
                outlineChangeBenen.OutlineColor = Color.clear;
                outlineChangeSchoenen.OutlineColor = Color.green;
            }else if (IsHoldingSchoenen == true && clothesTags._schoenenKleding != null)
            {
                outlineChangeTorso.OutlineColor = Color.clear;
                outlineChangeBenen.OutlineColor = Color.clear;
                outlineChangeSchoenen.OutlineColor = Color.red;
            }
        }
        else
        {
            outlineChangeTorso.OutlineColor = Color.clear;
            outlineChangeBenen.OutlineColor = Color.clear;
            outlineChangeSchoenen.OutlineColor = Color.clear;

        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Torso"))
        {
            IsHoldingTorso = true;
        }
        if (other.gameObject.CompareTag("Benen"))
        {
            IsHoldingBenen = true;
        }
        if (other.gameObject.CompareTag("Schoenen"))
        {
            IsHoldingSchoenen = true;
        }
        
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Torso"))
        {
            IsHoldingTorso = false;
        }
        if (other.gameObject.CompareTag("Benen"))
        {
            IsHoldingBenen = false;
        }
        if (other.gameObject.CompareTag("Schoenen"))
        {
            IsHoldingSchoenen = false;
        }
    }

}
