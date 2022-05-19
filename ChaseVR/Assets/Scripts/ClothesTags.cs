using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClothesTags : MonoBehaviour
{
    public GameObject TorsoPosition;
    public GameObject BenenPosition;
    public GameObject SchoenenPosition;
    //public GameObject Mannequin;
    [HideInInspector]
    public GameObject _torsoKleding;
    [HideInInspector]
    public GameObject _benenKleding;
    [HideInInspector]
    public GameObject _schoenenKleding;


    private void OnTriggerEnter(Collider Clothes)
    {
        //Debug.Log($"({Clothes.name}, enter) parent: {Clothes.transform.parent?.name ?? "none"}");
        switch (Clothes.gameObject.tag)
        {
            case "Torso":

                if (_torsoKleding == null)
                {
                    _torsoKleding = Clothes.gameObject;
                    SetCorrectClothesTransform(Clothes, TorsoPosition.transform);
                    break;
                }
                break;
            case "Benen":
                if (_benenKleding == null)
                {
                    _benenKleding = Clothes.gameObject;
                    SetCorrectClothesTransform(Clothes, BenenPosition.transform);
                    break;
                }
                break;

            case "Schoenen":
                if (_schoenenKleding == null)
                {
                    _schoenenKleding = Clothes.gameObject;
                    SetCorrectClothesTransform(Clothes, SchoenenPosition.transform);
                    break;
                }
                break;
            default:
                break;
        }
    }

    private void OnTriggerExit(Collider Clothes)
    {
        //Debug.Log($"({Clothes.name}, exit) parent: {Clothes.transform.parent?.name ?? "none"}");
        switch (Clothes.tag)
        {
            case "Torso":
                if (_torsoKleding == Clothes.gameObject)
                    _torsoKleding = null;
                break;
            case "Benen":
                if (_benenKleding == Clothes.gameObject)
                    _benenKleding = null;
                break;
            case "Schoenen":
                if (_schoenenKleding == Clothes.gameObject)
                    _schoenenKleding = null;
                break;
        }
    }

    public void SetCorrectClothesTransform(Collider clothes, Transform clothingTransform)
    {
        clothes.transform.position = clothingTransform.position;
        clothes.transform.rotation = clothingTransform.rotation;
        clothes.attachedRigidbody.velocity = Vector3.zero;
        clothes.attachedRigidbody.angularVelocity = Vector3.zero;
        clothes.gameObject.transform.SetParent(clothingTransform.parent.parent, true);
    }
}
