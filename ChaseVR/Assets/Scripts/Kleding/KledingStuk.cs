using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KledingStuk : MonoBehaviour
{
    public enum kledingStaten 
    {
        geanimeerd,
        statisch,
        opgevouwen
    }

    private kledingStaten _kledingStaat;
    public kledingStaten kledingStaat
    {
        get 
        { 
            return _kledingStaat; 
        }
        set 
        { 
            _kledingStaat = value;
            ChangeModel();
        }
    }

    public int artikelNummer;

    public void ChangeModel()
    {
        Debug.Log(gameObject.name + "       " + kledingStaat);
    }
}
