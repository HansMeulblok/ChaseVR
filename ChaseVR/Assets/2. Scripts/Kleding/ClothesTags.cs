
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class ClothesTags : MonoBehaviour
{

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

        kledingPivotPoint = transform.parent.GetChild(1);
    }


    private void OnTriggerEnter(Collider clothes)
    {
        //Debug.Log($"({Clothes.name}, enter) parent: {Clothes.transform.parent?.name ?? "none"}");
        if (clothes.TryGetComponent(typeof (KledingStuk), out Component kledingStuk))
        {
            switch (clothes.GetComponent<KledingStuk>().typeKleding)
            {
                case KledingStuk.TypeKleding.torso:

                    if (_torsoKleding == null)
                    {
                        _torsoKleding = clothes.gameObject;
                        SetCorrectClothesTransform(clothes, kledingPivotPoint);
                        break;
                    }
                    else if (clothes.GetComponent<KledingStuk>().kledingStaat == KledingStuk.KledingStaten.opgevouwen)
                    {
                        Destroy(_torsoKleding.gameObject);
                        SetCorrectClothesTransform(clothes, kledingPivotPoint);
                    }

                    break;


                case KledingStuk.TypeKleding.benen:

                    if (_benenKleding == null)
                    {
                        _benenKleding = clothes.gameObject;

                        SetCorrectClothesTransform(clothes, kledingPivotPoint);
                        break;
                    }
                    else if (clothes.GetComponent<KledingStuk>().kledingStaat == KledingStuk.KledingStaten.opgevouwen)
                    {
                        Destroy(_benenKleding.gameObject);
                        SetCorrectClothesTransform(clothes, kledingPivotPoint);
                    }

                    break;

                case KledingStuk.TypeKleding.schoenen:
                    if (_schoenenKleding == null)
                    {
                        _schoenenKleding = clothes.gameObject;
                        SetCorrectClothesTransform(clothes, kledingPivotPoint);

                        break;
                    }
                    else if (clothes.GetComponent<KledingStuk>().kledingStaat == KledingStuk.KledingStaten.opgevouwen)
                    {
                        Destroy(_schoenenKleding.gameObject);
                        SetCorrectClothesTransform(clothes, kledingPivotPoint);
                    }

                    break;


                default:
                    break;
            }
        }        
    }

    private void OnTriggerExit(Collider clothes)
    {
        if (clothes.transform.TryGetComponent(typeof(KledingStuk), out Component component) &&
            component.GetComponent<KledingStuk>().kledingStaat != KledingStuk.KledingStaten.opgevouwen)
        {
            ResetClothesTransform(clothes);
        }
        
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


        if (clothes.transform.TryGetComponent(typeof(KledingStuk), out Component component) && 
            component.GetComponent<KledingStuk>().kledingStaat != KledingStuk.KledingStaten.statisch)
        {
            component.GetComponent<KledingStuk>().kledingStaat = KledingStuk.KledingStaten.statisch;
        }
        clothes.transform.gameObject.layer = LayerMask.NameToLayer("KledingHitbox");
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

        clothes.GetComponent<KledingStuk>().kledingStaat = KledingStuk.KledingStaten.opgevouwen;
    }
}
