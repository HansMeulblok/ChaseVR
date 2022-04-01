using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SurfInteractionTrigger : MonoBehaviour
{

    private void Start()
    {
        if (gameObject.name.Contains("Left"))
        {
            SurfInteractionManager.Instance.handInteractionTriggers[0] = gameObject;
            SurfInteractionManager.Instance.stateLeftHand = SurfInteractionManager.StateLeftHand.LeftOutTrigger;

        }

        if (gameObject.name.Contains("Right"))
        {
            SurfInteractionManager.Instance.handInteractionTriggers[1] = gameObject;
            SurfInteractionManager.Instance.stateRightHand = SurfInteractionManager.StateRightHand.RightOutTrigger;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (gameObject == SurfInteractionManager.Instance.handInteractionTriggers[0] && other.gameObject.name == "LeftHand")
        {
            SurfInteractionManager.Instance.stateLeftHand = SurfInteractionManager.StateLeftHand.LeftInTrigger;
        }

        if (gameObject == SurfInteractionManager.Instance.handInteractionTriggers[1] && other.gameObject.name == "RightHand")
        {
            SurfInteractionManager.Instance.stateRightHand = SurfInteractionManager.StateRightHand.RightInTrigger;
        }

        if (SurfInteractionManager.Instance.stateLeftHand == SurfInteractionManager.StateLeftHand.LeftInTrigger &&
            SurfInteractionManager.Instance.stateRightHand == SurfInteractionManager.StateRightHand.RightInTrigger)
        {
            SurfInteractionManager.Instance.stateBothHands = SurfInteractionManager.StateBothHands.BothHandsInTrigger;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (gameObject == SurfInteractionManager.Instance.handInteractionTriggers[0] && other.gameObject.name == "LeftHand")
        {
            SurfInteractionManager.Instance.stateLeftHand = SurfInteractionManager.StateLeftHand.LeftOutTrigger;
        }

        if (gameObject == SurfInteractionManager.Instance.handInteractionTriggers[1] && other.gameObject.name == "RightHand")
        {
            SurfInteractionManager.Instance.stateRightHand = SurfInteractionManager.StateRightHand.RightOutTrigger;
        }

        if (SurfInteractionManager.Instance.stateLeftHand == SurfInteractionManager.StateLeftHand.LeftOutTrigger &&
            SurfInteractionManager.Instance.stateRightHand == SurfInteractionManager.StateRightHand.RightOutTrigger)
        {
            SurfInteractionManager.Instance.stateBothHands = SurfInteractionManager.StateBothHands.BothHandsOutTrigger;
        }
    }
}
