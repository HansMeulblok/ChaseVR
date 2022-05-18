using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ClothesTags;

public class KledingStuk : MonoBehaviour
{
    public enum kledingStaten 
    {
        statisch,
        opgevouwen,
        geanimeerd
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

             KledingManager.Instance.ChangeKledingModel(gameObject.GetComponent<MeshRenderer>(), _kledingStaat);
        }
    }

    public enum TypeKleding
    {
        torso,
        benen,
        schoenen
    }


    public TypeKleding typeKleding;
    public int artikelNummer;
}
