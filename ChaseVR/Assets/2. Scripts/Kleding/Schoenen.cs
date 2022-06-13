using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Schoenen : KledingStuk
{
    private void Start()
    {
        gameObject.tag = "schoenen";
        typeKleding = TypeKleding.schoenen;
    }
}
