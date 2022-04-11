using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class TeleportNewInputSystem : MonoBehaviour
{
    static private bool _teleportIsActive = false;

    public enum ControllerType
    {
        RightHand,
        LeftHand
    }

    public ControllerType targetController;

    public InputActionAsset inputAction;
    public XRRayInteractor rayInteractor;
    public TeleportationProvider teleportationProvider;

    private InputAction _thumbstickInputAction;
    private InputAction _teleportActivate;
    private InputAction _teleportcancel;



    void Start()
    {
        rayInteractor.enabled = false;
        Debug.Log("XRI " + targetController.ToString());
        _teleportActivate = inputAction.FindActionMap("XRI " + targetController.ToString()).FindAction("Teleport Mode Activate");
        _teleportActivate.Enable();
        _teleportActivate.performed += OnTeleportActivate;

        _teleportcancel = inputAction.FindActionMap("XRI " + targetController.ToString()).FindAction("Teleport Mode Cancel");
        _teleportcancel.Enable();
        _teleportcancel.performed += OnTeleportCancel;

        _thumbstickInputAction = inputAction.FindActionMap("XRI " + targetController.ToString()).FindAction("Move");
    }

    private void OnDestroy()
    {
        _teleportActivate.performed -= OnTeleportActivate;
        _teleportcancel.performed -= OnTeleportCancel;
        Debug.Log("destroy");
    }

    // Update is called once per frame
    void Update()
    {
        if (!_teleportIsActive)
        {
            Debug.Log("1");
            return;
        }
        if (!rayInteractor.enabled)
        {
            Debug.Log("2");

            return;
        }
        if (_thumbstickInputAction.triggered)
        {
            Debug.Log("3");

            return;
        }
        if(!rayInteractor.TryGetCurrent3DRaycastHit(out RaycastHit raycasthit))
        {
            Debug.Log("4");
            rayInteractor.enabled = false;
            _teleportIsActive = false;
            return;
        }
        Debug.Log("teleport commense");
        TeleportRequest teleportrequest = new TeleportRequest()
        {
            destinationPosition = raycasthit.point,
        };
       
        teleportationProvider.QueueTeleportRequest(teleportrequest);
        rayInteractor.enabled = false;
        _teleportIsActive = false;

    }
    

    public void OnTeleportActivate(InputAction.CallbackContext context)
    {
        Debug.Log("activate");
        if (!_teleportIsActive)
        {
            rayInteractor.enabled = true;
            _teleportIsActive = true;
        }

    }

    public void OnTeleportCancel(InputAction.CallbackContext context)
    {
        if(_teleportIsActive && rayInteractor.enabled == true)
        {
            Debug.Log("de-activate");
            rayInteractor.enabled = false;
            _teleportIsActive = false;
        }

    }


}
