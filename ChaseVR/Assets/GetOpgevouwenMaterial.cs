using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetOpgevouwenMaterial : MonoBehaviour
{
    void Start()
    {
        GetComponent<MeshRenderer>().material = transform.parent.GetChild(0).GetComponent<MeshRenderer>().material;
    }
}
