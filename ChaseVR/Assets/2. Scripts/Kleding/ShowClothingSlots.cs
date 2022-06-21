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

    private GameObject Torso;
    private GameObject Benen;
    private GameObject Schoenen;


    //variables needed for holding checks
    public SphereCollider holdingItemCollider;
    private bool IsHoldingTorso = false;
    private bool IsHoldingBenen = false;
    private bool IsHoldingSchoenen = false;
    private List<Outline> raycastHasHit;

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
        raycastHasHit = new List<Outline>();
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

                foreach(Transform Clothes in clothesTags.transform.root)
                {
                    
                    if (Clothes.tag == "torso")
                    {
                        Debug.Log("Torso");
                        outlineChangeTorso =  Clothes.GetComponent<Outline>();
                        Torso = Clothes.gameObject;
                        Debug.Log(Clothes.transform.name);
                    }
                    if (Clothes.CompareTag("benen"))
                    {
                        Debug.Log("Benen");
                        outlineChangeBenen = Clothes.GetComponent<Outline>();
                        Benen = Clothes.gameObject;
                        Debug.Log(Clothes.transform.name);
                    }
                    if (Clothes.CompareTag("schoenen"))
                    {
                        Debug.Log("Schoenen");
                        outlineChangeSchoenen = Clothes.GetComponent<Outline>();
                        Schoenen = Clothes.gameObject;
                    }
                    else
                    {
                        /*outlineChangeTorso = clothesTags.torsoPosition.gameObject.GetComponent<Outline>();
                        outlineChangeBenen = clothesTags.benenPosition.gameObject.GetComponent<Outline>();
                        outlineChangeSchoenen = clothesTags.schoenenPosition.gameObject.GetComponent<Outline>();*/
                    }
                }

              /*  if (clothesTags.transform.parent.CompareTag("Torso"))
                {
                    
                    outlineChangeTorso = clothesTags.GetComponentInParent<Torso>().gameObject.GetComponent<Outline>();
                    Debug.Log(outlineChangeTorso.gameObject.name);
                    //outlineChangeTorso = GameObject.FindGameObjectWithTag("Torso").transform.par;
                    //outlineChangeTorso = clothesTags.gameObject.transform.parent.gameObject. .gameObject.GetComponent<Outline>();
                }
                else
                {
                    outlineChangeTorso = clothesTags.torsoPosition.gameObject.GetComponent<Outline>();
                    Debug.Log(clothesTags.transform.parent.Find("Torso").tag);
                    //outlineChangeTorso = ;
                }*/
               
                //outlineChangeBenen = clothesTags.benenPosition.gameObject.GetComponent<Outline>();
                //outlineChangeSchoenen = clothesTags.schoenenPosition.gameObject.GetComponent<Outline>();


                //outlineChangeBenen = clothesTags.benenPosition.gameObject.GetComponent<Outline>();
                //outlineChangeSchoenen = clothesTags.schoenenPosition.gameObject.GetComponent<Outline>();
            }
            //incase wanting to see meshfilter
            //Debug.Log(hitData.transform.gameObject.GetComponent<MeshFilter>().mesh.name);
            if (hitData.transform.gameObject.GetComponent<MeshFilter>() != null &&  hitData.transform.gameObject.GetComponent<MeshFilter>().mesh.name == "Cube Instance")
            {
                foldedClothes = hitData.transform.gameObject;
                outlineChangeOpgevouwen = foldedClothes.gameObject.GetComponent<Outline>();
                outlineChangeOpgevouwen.OutlineColor = Color.green;
                //Debug.Log(foldedClothes.gameObject.name);
            }

            //setting bloketalage outline components
            if (IsHoldingTorso == false && IsHoldingBenen == false && IsHoldingSchoenen == false && hitData.transform.gameObject.tag == "Etalage")
            {
                GameObject bloketalage = hitData.transform.gameObject;
                outlinechangeEtalage = bloketalage.transform.GetChild(1).gameObject.GetComponent<Outline>();
                outlinechangeEtalage.OutlineColor = Color.green;
                if (!raycastHasHit.Contains(outlinechangeEtalage))
                {
                    raycastHasHit.Add(outlinechangeEtalage);

                }

                foreach (Outline etalage in raycastHasHit)
                {
                    if (etalage == outlinechangeEtalage)
                    {
                        break;
                    }
                    else
                    {
                        etalage.OutlineColor = Color.clear;
                        raycastHasHit.Remove(etalage);
                        break;
                    }


                }

            }
            if (clothesTags != null)
            {
                if (hitData.transform == Torso)
                {
                    //set torso outlines
                    outlineColorChanger(outlineChangeTorso, outlineChangeBenen, outlineChangeSchoenen, clothesTags._torsoKleding, IsHoldingBenen, IsHoldingSchoenen);
                }
                else if (hitData.transform == Benen)
                {
                    //set benen outlines
                    outlineColorChanger(outlineChangeBenen, outlineChangeTorso, outlineChangeSchoenen, clothesTags._benenKleding, IsHoldingTorso, IsHoldingSchoenen);
                }
                else if (hitData.transform == Schoenen)
                {
                    //set schoenen outlines
                    outlineColorChanger(outlineChangeSchoenen, outlineChangeTorso, outlineChangeBenen, clothesTags._schoenenKleding, IsHoldingTorso, IsHoldingBenen);
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
