using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ClothesTags;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(XRGrabInteractable))]
[RequireComponent(typeof(BoxCollider))]
public class KledingStuk : MonoBehaviour
{
    public enum KledingStaten 
    {
        statisch,
        opgevouwen,
        geanimeerd
    }

    private KledingStaten _kledingStaat;
    
    public KledingStaten kledingStaat
    {
        get 
        { 
            return _kledingStaat; 
        }
        set 
        {
            _kledingStaat = value;

            KledingManager.Instance.ChangeKledingModel(gameObject, _kledingStaat);

        }
    }

    public enum TypeKleding
    {
        torso,
        benen,
        schoenen,
        geenType
    }


    public TypeKleding typeKleding;
    public int artikelNummer;
    [HideInInspector]
    public BoxCollider kledingStukCollider;

}
