using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class ToggleRay : MonoBehaviour
{
    public InputActionAsset controls;
    void Start()
    {
        GetComponent<XRInteractorLineVisual>().enabled = false;
        InputAction gripAction = controls.FindAction("XRI RightHand Interaction/Select");
        //Debug.Log(gripAction);
        gripAction.started += TurnOn;
        gripAction.canceled += TurnOff;

    }

    public void TurnOff(InputAction.CallbackContext context)
    {
        GetComponent<XRInteractorLineVisual>().enabled = false;
    }

    public void TurnOn(InputAction.CallbackContext context)
    {
        GetComponent<XRInteractorLineVisual>().enabled = true;
        // StartCoroutine(DelayedTurnOn());
    }
}
