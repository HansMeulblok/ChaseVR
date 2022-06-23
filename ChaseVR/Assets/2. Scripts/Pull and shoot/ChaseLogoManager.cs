using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class ChaseLogoManager : MonoBehaviour
{
    public void ChaseLogo(SelectEnterEventArgs args)
    {
        Destroy(args.interactableObject.transform.gameObject);

        args.interactorObject.transform.GetChild(1).gameObject.SetActive(true);

        args.interactorObject.transform.parent.GetChild(4).
            GetComponent<XRRayInteractor>().enabled = true;

        args.interactorObject.transform.parent.GetChild(4).
            GetComponent<XRRayInteractor>().interactionLayers = LayerMask.GetMask("Default");
        

        args.interactorObject.transform.GetComponent<ShowOutlines>().OutlineHitboxLayer = LayerMask.GetMask("KledingHitbox", "Etalage");

        args.interactorObject.transform.GetComponent<XRDirectInteractor>().interactionLayers = LayerMask.GetMask("RandomInteractables");
    }
}
