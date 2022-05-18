using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Benen : KledingStuk
{
    private void Start()
    {
        gameObject.tag = "Benen";
        typeKleding = TypeKleding.benen;
    }
}
