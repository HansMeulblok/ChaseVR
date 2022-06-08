
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class ClothesTags : MonoBehaviour
{
    [HideInInspector]
    public Transform torsoPosition;
    [HideInInspector]
    public Transform benenPosition;
    [HideInInspector]
    public Transform schoenenPosition;
    [HideInInspector]
    public Transform kledingPivotPoint;
    //public GameObject Mannequin;

    [HideInInspector]
    public GameObject _torsoKleding = null;
    [HideInInspector]
    public GameObject _benenKleding = null;
    [HideInInspector]
    public GameObject _schoenenKleding = null;

    private void Start()
    {
        torsoPosition = transform.GetChild(2);
        benenPosition = transform.GetChild(1);
        schoenenPosition = transform.GetChild(0);

        kledingPivotPoint = transform.parent.GetChild(1);
    }


    private void OnTriggerEnter(Collider Clothes)
    {
        //Debug.Log($"({Clothes.name}, enter) parent: {Clothes.transform.parent?.name ?? "none"}");
        if (Clothes.TryGetComponent(typeof (KledingStuk), out Component kledingStuk))
        {
            switch (Clothes.GetComponent<KledingStuk>().typeKleding)
            {
                case KledingStuk.TypeKleding.torso:

                    if (_torsoKleding == null)
                    {
                        _torsoKleding = Clothes.gameObject;
                        SetCorrectClothesTransform(Clothes, kledingPivotPoint);
                        break;
                    }
                    else
                    {
                        Destroy(Clothes.gameObject);
                    }

                    break;


                case KledingStuk.TypeKleding.benen:

                    if (_benenKleding == null)
                    {
                        _benenKleding = Clothes.gameObject;

                        SetCorrectClothesTransform(Clothes, kledingPivotPoint);
                        break;
                    }
                    else
                    {
                        Destroy(Clothes.gameObject);
                    }

                    break;

                case KledingStuk.TypeKleding.schoenen:
                    if (_schoenenKleding == null)
                    {
                        _schoenenKleding = Clothes.gameObject;
                        SetCorrectClothesTransform(Clothes, kledingPivotPoint);
                        break;
                    }
                    else
                    {
                        Destroy(Clothes.gameObject);
                    }

                    break;


                default:
                    break;
            }
        }        
    }

    private void OnTriggerExit(Collider Clothes)
    {
        ResetClothesTransform(Clothes);
        //Debug.Log($"({Clothes.name}, exit) parent: {Clothes.transform.parent?.name ?? "none"}");
    }

    public void SetCorrectClothesTransform(Collider clothes, Transform clothingTransform)
    {
        clothes.transform.position = clothingTransform.position;
        clothes.transform.rotation = clothingTransform.rotation;
        clothes.transform.SetParent(gameObject.transform.parent, true);

        clothes.attachedRigidbody.isKinematic = true;
        clothes.GetComponent<XRGrabInteractable>().enabled = true;

        clothes.attachedRigidbody.velocity = Vector3.zero;
        clothes.attachedRigidbody.angularVelocity = Vector3.zero;


        if (clothes.transform.parent.TryGetComponent(typeof(KledingStuk), out Component component) && 
            component.GetComponent<KledingStuk>().kledingStaat != KledingStuk.KledingStaten.statisch)
        {
            component.GetComponent<KledingStuk>().kledingStaat = KledingStuk.KledingStaten.statisch;
        }
    }

    public void ResetClothesTransform(Collider clothes)
    {
        switch (clothes.GetComponentInParent<KledingStuk>().typeKleding)
        {
            case KledingStuk.TypeKleding.torso:

                _torsoKleding = null;

                break;

            case KledingStuk.TypeKleding.benen:

                _benenKleding = null;

                break;

            case KledingStuk.TypeKleding.schoenen:

                _schoenenKleding = null;

                break;

            default:
                break;
        }

        clothes.GetComponentInParent<KledingStuk>().kledingStaat = KledingStuk.KledingStaten.opgevouwen;
    }
}
