using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClothesTags : MonoBehaviour
{
    public GameObject TorsoHitbox;
    public GameObject BenenHitbox;
    public GameObject SchoenenHitbox;
    public GameObject Mannequin;

    private GameObject _torsoKleding;
    private GameObject _benenKleding;
    private GameObject _schoenenKleding;


    private void OnTriggerEnter(Collider Clothes)
    {
        Debug.Log($"({Clothes.name}, enter) parent: {Clothes.transform.parent?.name ?? "none"}");
        switch (Clothes.gameObject.tag)
        {
            case "Torso":


                if (_torsoKleding == null && Clothes.tag == "Torso")
                {
                    _torsoKleding = Clothes.gameObject;
                    Clothes.transform.position = TorsoHitbox.transform.position;
                    Clothes.transform.rotation = TorsoHitbox.transform.rotation;
                    Clothes.attachedRigidbody.velocity = new Vector3(0, 0, 0);
                    Clothes.attachedRigidbody.angularVelocity = new Vector3(0, 0, 0);
                    Clothes.gameObject.transform.SetParent(TorsoHitbox.transform.parent.parent, true);
                    
                    break;
                }
                break;
             

            case "Benen":
                
                if (_benenKleding == null && Clothes.tag == "Benen")
                {
                    _benenKleding = Clothes.gameObject;
                    Clothes.transform.position = BenenHitbox.transform.position;
                    Clothes.transform.rotation = BenenHitbox.transform.rotation;
                    Clothes.attachedRigidbody.velocity = new Vector3(0, 0, 0);
                    Clothes.attachedRigidbody.angularVelocity = new Vector3(0, 0, 0);
                    Clothes.gameObject.transform.SetParent(BenenHitbox.transform.parent.parent, true);
                    break;
                }
                else
                    break;

            case "Schoenen":
                if (_schoenenKleding == null && Clothes.tag == "Schoenen")
                {
                    _schoenenKleding = Clothes.gameObject;
                    Clothes.transform.position = SchoenenHitbox.transform.position;
                    Clothes.transform.rotation = SchoenenHitbox.transform.rotation;
                    Clothes.attachedRigidbody.velocity = new Vector3(0, 0, 0);
                    Clothes.attachedRigidbody.angularVelocity = new Vector3(0, 0, 0);
                    Clothes.gameObject.transform.SetParent(SchoenenHitbox.transform.parent.parent, true);
                    break;
                }
                else
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
                Debug.Log("Benen");
                if (_benenKleding == Clothes.gameObject)
                    _benenKleding = null;
                break;

            case "Schoenen":
                Debug.Log("Schoenen");
                if (_schoenenKleding == Clothes.gameObject)
                    _schoenenKleding = null;
                break;


        }

    }

}
