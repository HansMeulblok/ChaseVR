using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SurfInteractionTriggerLeft : MonoBehaviour
{
    private void Start()
    {
        SurfInteractionManager.Instance.handInteractionTriggers[0] = gameObject;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.name == "LeftHand")
        {
            //SurfInteractionManager.Instance.leftHandInTrigger = true;
            
            SurfInteractionManager.Instance.SetTriggerMaterial(gameObject.GetComponent<MeshRenderer>(),
                                                               SurfInteractionManager.Instance.surfTriggerCorrectLeft);

            Debug.Log("left hand stay");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name == "LeftHand")
        {
            //SurfInteractionManager.Instance.leftHandInTrigger = false;

            SurfInteractionManager.Instance.SetTriggerMaterial(gameObject.GetComponent<MeshRenderer>(),
                                                               SurfInteractionManager.Instance.surfTriggerIncorrectLeft);

            Debug.Log("left hand exit");
        }
    }
}
