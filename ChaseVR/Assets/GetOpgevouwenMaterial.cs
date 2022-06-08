using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetOpgevouwenMaterial : MonoBehaviour
{
    void Start()
    {
        GetComponent<MeshRenderer>().sharedMaterial = transform.parent.GetChild(0).GetComponent<MeshRenderer>().sharedMaterial;
    }
}
