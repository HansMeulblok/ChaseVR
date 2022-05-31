using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowClothingSlots : MonoBehaviour
{
    [HideInInspector]
    public ClothesTags clothesTags;
    //public FollowerManager folowerManager;
    [HideInInspector]
    public BlokEtalage bloketalage;
    [HideInInspector]
    private GameObject foldedClothes;
    private Ray ClothesRay;
    public LayerMask MannequinHitboxLayer;
    //public MeshFilter OpgevouwenFilter;
    
    //variables needed for holding checks
    public SphereCollider holdingItemCollider;
    private bool IsHoldingTorso = false;
    private bool IsHoldingBenen = false;
    private bool IsHoldingSchoenen;

    //outlines of collider boxes
    Outline outlineChangeTorso;
    Outline outlineChangeBenen;
    Outline outlineChangeSchoenen;
    Outline outlinechangeEtalage;
    Outline outlineChangeOpgevouwen;

    RaycastHit hitData;
    //string rayTag;

    // Start is called before the first frame update
    void Start()
    {
        outlineChangeTorso = gameObject.GetComponentInParent<Outline>();
        outlineChangeBenen = gameObject.GetComponentInParent<Outline>();
        outlineChangeSchoenen = gameObject.GetComponentInParent<Outline>();
        outlinechangeEtalage = gameObject.GetComponentInParent<Outline>();
        outlineChangeOpgevouwen = gameObject.GetComponentInParent<Outline>();
        foldedClothes = gameObject.transform.parent.gameObject;
       // bloketalage.gameObject.transform = gameObject.transform.parent.gameObject.transform;
    }

    // Update is called once per frame
    void Update()
    {
        ClothesRay = new Ray(this.transform.position, transform.forward);
        //Debug.DrawRay(ClothesRay.origin, ClothesRay.direction * 10);

        if (Physics.Raycast(ClothesRay, out hitData, 30,  MannequinHitboxLayer))
        {
            //Debug.Log(hitData.transform.parent.gameObject.name);
            //setting clotes outline components
            if (hitData.transform.parent != null && hitData.transform.parent.gameObject.name == "ClothesHitbox")
                {
                clothesTags = hitData.transform.parent.GetComponent<ClothesTags>();
                outlineChangeTorso = clothesTags.torsoPosition.gameObject.GetComponent<Outline>();
                outlineChangeBenen = clothesTags.benenPosition.gameObject.GetComponent<Outline>();
                outlineChangeSchoenen = clothesTags.schoenenPosition.gameObject.GetComponent<Outline>();
            }
            //incase wanting to see meshfilter
            //Debug.Log(hitData.transform.gameObject.GetComponent<MeshFilter>().mesh.name);
            if (hitData.transform.gameObject.GetComponent<MeshFilter>() != null &&  hitData.transform.gameObject.GetComponent<MeshFilter>().mesh.name == "Cube Instance")
            {
                foldedClothes = hitData.transform.gameObject;
                outlineChangeOpgevouwen = foldedClothes.gameObject.GetComponent<Outline>();
                Debug.Log(foldedClothes.gameObject.name);
            }

            //setting bloketalage outline components
            if (IsHoldingTorso == false && IsHoldingBenen == false && IsHoldingSchoenen == false && hitData.transform.gameObject.tag == "Etalage")
            {
                GameObject bloketalage = hitData.transform.gameObject;
                outlinechangeEtalage = bloketalage.transform.GetChild(0).gameObject.GetComponent<Outline>();
            }

            if (clothesTags != null) 
            {
                if (hitData.transform == clothesTags.torsoPosition.gameObject.transform)
                {
                    //set torso outlines
                    outlineColorChanger(outlineChangeTorso, outlineChangeBenen, outlineChangeSchoenen, clothesTags._torsoKleding, IsHoldingBenen, IsHoldingSchoenen);
                }
                else if (hitData.transform == clothesTags.benenPosition.gameObject.transform)
                {
                    //set benen outlines
                    outlineColorChanger(outlineChangeBenen, outlineChangeTorso, outlineChangeSchoenen, clothesTags._benenKleding, IsHoldingTorso, IsHoldingSchoenen);
                }
                else if (hitData.transform == clothesTags.schoenenPosition.gameObject.transform)
                {
                    //set schoenen outlines
                    outlineColorChanger(outlineChangeSchoenen, outlineChangeTorso, outlineChangeBenen, clothesTags._schoenenKleding, IsHoldingTorso, IsHoldingBenen);
                }
                else if (hitData.transform == foldedClothes.gameObject.transform)
                {
                    outlineChangeOpgevouwen.OutlineColor = Color.green;
                }
                else if (hitData.transform == outlinechangeEtalage.gameObject.transform)
                {
                    outlinechangeEtalage.OutlineColor = Color.green;
                }
            }
        }
        else
        {
            outlineChangeTorso.OutlineColor = Color.clear;
            outlineChangeBenen.OutlineColor = Color.clear;
            outlineChangeSchoenen.OutlineColor = Color.clear;
            outlinechangeEtalage.OutlineColor = Color.clear;
            outlineChangeOpgevouwen.OutlineColor = Color.clear;
        }
    }
    private void outlineColorChanger(Outline selectedOutline, Outline wrongOutline1, Outline wrongOutline2,GameObject clothesTag,bool isHoldingWrongItem, bool IsholdingWrongItem2)
    {
        if (isHoldingWrongItem || IsholdingWrongItem2)
        {
            selectedOutline.OutlineColor = Color.clear;
        }
        else if (clothesTag == null)
        {
            selectedOutline.OutlineColor = Color.green;
            wrongOutline1.OutlineColor = Color.clear;
            wrongOutline2.OutlineColor = Color.clear;

        }
        else if (clothesTag != null)
        {
            selectedOutline.OutlineColor = Color.red;
            wrongOutline1.OutlineColor = Color.clear;
            wrongOutline2.OutlineColor = Color.clear;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("torso"))
        {
            IsHoldingTorso = true;
        }
        if (other.gameObject.CompareTag("benen"))
        {
            IsHoldingBenen = true;
        }
        if (other.gameObject.CompareTag("schoenen"))
        {
            IsHoldingSchoenen = true;
        }
        
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("torso"))
        {
            IsHoldingTorso = false;
        }
        if (other.gameObject.CompareTag("benen"))
        {
            IsHoldingBenen = false;
        }
        if (other.gameObject.CompareTag("schoenen"))
        {
            IsHoldingSchoenen = false;
        }
    }


}
