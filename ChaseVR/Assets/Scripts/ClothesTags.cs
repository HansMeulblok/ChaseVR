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
        switch (Clothes.GetComponent<KledingStuk>().typeKleding)
        {
            case TypeKleding.Torso:

                if (_torsoKleding == null)
                {
                    _torsoKleding = Clothes.gameObject;
                    SetCorrectClothesTransform(Clothes, TorsoPosition);
                    break;
                }

                break;
             

            case TypeKleding.Benen:
                
                if (_benenKleding == null)
                {
                    Debug.Log(Clothes.gameObject.name + " in clothes tags scr");
                    _benenKleding = Clothes.gameObject;
                    SetCorrectClothesTransform(Clothes, BenenPosition);
                    break;
                }

                break;

            case TypeKleding.Schoenen:
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
        clothes.GetComponent<KledingStuk>().kledingStaat = KledingStuk.kledingStaten.statisch;

        clothes.transform.position = clothingTransform.position;
        clothes.transform.rotation = clothingTransform.rotation;
        clothes.attachedRigidbody.velocity = Vector3.zero;
        clothes.attachedRigidbody.angularVelocity = Vector3.zero;
        clothes.gameObject.transform.SetParent(clothingTransform.parent.parent, true);
    }

    public void ResetClothesTransform(Collider clothes)
    {
        

        switch (clothes.GetComponent<KledingStuk>().typeKleding)
        {
            case TypeKleding.Torso:

                _torsoKleding = null;

                break;

            case TypeKleding.Benen:

                _benenKleding = null;
            
                break;

            case TypeKleding.Schoenen:

                _schoenenKleding = null;

                break;

            default:
                break;
        }

        clothes.GetComponent<KledingStuk>().kledingStaat = KledingStuk.kledingStaten.opgevouwen;
    }

    public enum TypeKleding
    {
        Torso,
        Benen,
        Schoenen
    }
}
