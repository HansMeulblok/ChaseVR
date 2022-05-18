using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClothesTags : MonoBehaviour
{
    public Transform TorsoPosition;
    public Transform BenenPosition;
    public Transform SchoenenPosition;
    //public GameObject Mannequin;

    private GameObject _torsoKleding;
    private GameObject _benenKleding;
    private GameObject _schoenenKleding;


    private void OnTriggerEnter(Collider Clothes)
    {
        //Debug.Log($"({Clothes.name}, enter) parent: {Clothes.transform.parent?.name ?? "none"}");
        switch (Clothes.GetComponentInParent<KledingStuk>().typeKleding)
        {
            case KledingStuk.TypeKleding.torso:

                if (_torsoKleding == null)
                {
                    _torsoKleding = Clothes.gameObject;
                    SetCorrectClothesTransform(Clothes, TorsoPosition);
                    break;
                }

                break;
             

            case KledingStuk.TypeKleding.benen:
                
                if (_benenKleding == null)
                {
                    _benenKleding = Clothes.gameObject;
                    SetCorrectClothesTransform(Clothes, BenenPosition);
                    break;
                }

                break;

            case KledingStuk.TypeKleding.schoenen:
                if (_schoenenKleding == null)
                {
                    _schoenenKleding = Clothes.gameObject;
                    SetCorrectClothesTransform(Clothes, SchoenenPosition);
                    break;
                }
                
                break;


            default:
                break;
        }
    }

    private void OnTriggerExit(Collider Clothes)
    {
        ResetClothesTransform(Clothes);
        //Debug.Log($"({Clothes.name}, exit) parent: {Clothes.transform.parent?.name ?? "none"}");
    }

    public void SetCorrectClothesTransform(Collider clothes, Transform clothingTransform)
    {
        clothes.transform.parent.transform.position = clothingTransform.position;
        clothes.transform.parent.transform.rotation = clothingTransform.rotation;
        clothes.transform.parent.SetParent(gameObject.transform.parent, true);

        clothes.attachedRigidbody.velocity = Vector3.zero;
        clothes.attachedRigidbody.angularVelocity = Vector3.zero;

        if (clothes.transform.parent.TryGetComponent(typeof(KledingStuk), out Component component) && 
            component.GetComponent<KledingStuk>().kledingStaat != KledingStuk.kledingStaten.statisch)
        {
            component.GetComponent<KledingStuk>().kledingStaat = KledingStuk.kledingStaten.statisch;
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

        clothes.GetComponentInParent<KledingStuk>().kledingStaat = KledingStuk.kledingStaten.opgevouwen;
    }
}
