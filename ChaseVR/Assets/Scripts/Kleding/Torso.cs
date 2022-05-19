using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Torso : KledingStuk
{
    private void Start()
    {
        gameObject.tag = "torso";
        typeKleding = TypeKleding.torso;

        GetComponent<Rigidbody>().isKinematic = true;
        GetComponent<Rigidbody>().useGravity = false;

        kledingStukCollider = GetComponent<BoxCollider>();

        kledingStukCollider.center = new Vector3(0f, 1.3f, 0f);
        kledingStukCollider.size = new Vector3(0.6f, 0.6f, 0.3f);
    }
}
