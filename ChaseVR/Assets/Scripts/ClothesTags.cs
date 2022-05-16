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
        switch (Clothes.gameObject.tag)
        {
            case "Torso":

                if (_torsoKleding == null)
                {
                    _torsoKleding = Clothes.gameObject;
                    SetCorrectClothesTransform(Clothes, TorsoPosition);
                    break;
                }

                break;
             

            case "Benen":
                
                if (_benenKleding == null)
                {
                    _benenKleding = Clothes.gameObject;
                    SetCorrectClothesTransform(Clothes, BenenPosition);
                    break;
                }

                break;

            case "Schoenen":
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

        //Debug.Log($"({Clothes.name}, exit) parent: {Clothes.transform.parent?.name ?? "none"}");
        switch (Clothes.tag)
        {
            case "Torso":

                ResetClothesTransform(Clothes, _torsoKleding);
                    
                break;

            case "Benen":
                
                ResetClothesTransform(Clothes, _benenKleding);

                break;

            case "Schoenen":

                ResetClothesTransform(Clothes, _schoenenKleding);

                break;

            default:
                break;
        }
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

    public void ResetClothesTransform(Collider clothes, GameObject clothingGameObject)
    {
        clothingGameObject = null;
        clothes.GetComponent<KledingStuk>().kledingStaat = KledingStuk.kledingStaten.opgevouwen;
    }
}
