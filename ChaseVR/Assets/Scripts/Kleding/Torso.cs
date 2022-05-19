using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Torso : KledingStuk
{
    private void Start()
    {
        gameObject.tag = "torso";
        typeKleding = TypeKleding.torso;
    }
}
