using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SurfInteractionTriggerRight : MonoBehaviour
{
    private void Start()
    {
        SurfInteractionManager.Instance.handInteractionTriggers[1] = gameObject;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.name == "RightHand")
        {
            //SurfInteractionManager.Instance.rightHandInTrigger = true;
            
            SurfInteractionManager.Instance.SetTriggerMaterial(gameObject.GetComponent<MeshRenderer>(),
                                                               SurfInteractionManager.Instance.surfTriggerCorrectLeft);

            Debug.Log("right hand stay");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name == "RightHand")
        {
            //SurfInteractionManager.Instance.rightHandInTrigger = false;

            SurfInteractionManager.Instance.SetTriggerMaterial(gameObject.GetComponent<MeshRenderer>(),
                                                              SurfInteractionManager.Instance.surfTriggerIncorrectLeft);

            Debug.Log("right hand exit");
        }
    }
}
