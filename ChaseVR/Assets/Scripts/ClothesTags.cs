using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClothesTags : MonoBehaviour
{
    public GameObject TorsoHitbox;
    public GameObject BenenHitbox;
    public GameObject SchoenenHitbox;


    private void Start()
    {
        
    }

    private void Update()
    {
        
    }
    //private GameObject torso = GameObject.FindWithTag("Torso");

    private void OnTriggerEnter(Collider Clothes)
    {
        switch (Clothes.tag)
        {
            case "Torso":
                Debug.Log("Torso");

                Clothes.gameObject.transform.SetParent(SchoenenHitbox.transform.parent.parent, true);
                Clothes.transform.position = TorsoHitbox.transform.position;
                Clothes.transform.rotation = TorsoHitbox.transform.rotation;
                Clothes.attachedRigidbody.velocity = new Vector3(0, 0, 0);
                Clothes.attachedRigidbody.angularVelocity = new Vector3(0, 0, 0);

                break;
            case "Benen":
                Debug.Log("Benen");
                Clothes.gameObject.transform.SetParent(SchoenenHitbox.transform.parent.parent, true);
                Clothes.transform.position = BenenHitbox.transform.position;
                Clothes.transform.rotation = BenenHitbox.transform.rotation;
                Clothes.attachedRigidbody.velocity = new Vector3(0, 0, 0);
                Clothes.attachedRigidbody.angularVelocity = new Vector3(0, 0, 0);

                break;
            case "Schoenen":
                Debug.Log("Schoenen");
                Clothes.gameObject.transform.SetParent(SchoenenHitbox.transform.parent.parent, true);
                Clothes.transform.position = SchoenenHitbox.transform.position;
                Clothes.transform.rotation = SchoenenHitbox.transform.rotation;
                Clothes.attachedRigidbody.velocity = new Vector3(0,0,0) ;
                Clothes.attachedRigidbody.angularVelocity = new Vector3(0, 0, 0);

                break;

        }
    }

}
