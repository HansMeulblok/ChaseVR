using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SurfInteractionTrigger : MonoBehaviour
{

    private void Start()
    {
        if (SurfInteractionManager.Instance.allInteraction)
        {
            if (gameObject.tag == "SurfTriggerLeft")
            {
                SurfInteractionManager.Instance.handInteractionTriggers[0] = gameObject;
                SurfInteractionManager.Instance.stateLeftHand = SurfInteractionManager.StateLeftHand.LeftOutTrigger;

            }
        }
        

        if (gameObject.tag == "SurfTriggerRight")
        {
            SurfInteractionManager.Instance.handInteractionTriggers[1] = gameObject;
            SurfInteractionManager.Instance.stateRightHand = SurfInteractionManager.StateRightHand.RightOutTrigger;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (gameObject == SurfInteractionManager.Instance.handInteractionTriggers[0] && other.gameObject.tag == "LeftHand")
        {
            SurfInteractionManager.Instance.stateLeftHand = SurfInteractionManager.StateLeftHand.LeftInTrigger;
        }

        if (gameObject == SurfInteractionManager.Instance.handInteractionTriggers[1] && other.gameObject.tag == "RightHand")
        {
            SurfInteractionManager.Instance.stateRightHand = SurfInteractionManager.StateRightHand.RightInTrigger;
        }

        if (/*SurfInteractionManager.Instance.stateLeftHand == SurfInteractionManager.StateLeftHand.LeftInTrigger &&*/
            SurfInteractionManager.Instance.stateRightHand == SurfInteractionManager.StateRightHand.RightInTrigger &&
            SurfInteractionManager.Instance.stateBothHands == SurfInteractionManager.StateBothHands.BothHandsOutTrigger)
        {
            SurfInteractionManager.Instance.stateBothHands = SurfInteractionManager.StateBothHands.BothHandsInTrigger;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (gameObject == SurfInteractionManager.Instance.handInteractionTriggers[0] && other.gameObject.tag == "LeftHand")
        {
            SurfInteractionManager.Instance.stateLeftHand = SurfInteractionManager.StateLeftHand.LeftOutTrigger;
        }

        if (gameObject == SurfInteractionManager.Instance.handInteractionTriggers[1] && other.gameObject.tag == "RightHand")
        {
            SurfInteractionManager.Instance.stateRightHand = SurfInteractionManager.StateRightHand.RightOutTrigger;
        }

        if (SurfInteractionManager.Instance.interaction)
        {
            if ((/*SurfInteractionManager.Instance.stateLeftHand == SurfInteractionManager.StateLeftHand.LeftOutTrigger ||*/
            SurfInteractionManager.Instance.stateRightHand == SurfInteractionManager.StateRightHand.RightOutTrigger) &&
            !SurfInteractionManager.Instance.isPlaying)
            {
                SurfInteractionManager.Instance.stateBothHands = SurfInteractionManager.StateBothHands.BothHandsOutTrigger;
            }

            if (SurfInteractionManager.Instance.allInteraction)
            {
                if (SurfInteractionManager.Instance.stateLeftHand == SurfInteractionManager.StateLeftHand.LeftOutTrigger &&
                    SurfInteractionManager.Instance.stateRightHand == SurfInteractionManager.StateRightHand.RightOutTrigger &&
                    SurfInteractionManager.Instance.isPlaying)
                {
                    SurfInteractionManager.Instance.stateBothHands = SurfInteractionManager.StateBothHands.BothHandsOutTrigger;
                }
            }
        }       
    }
}
