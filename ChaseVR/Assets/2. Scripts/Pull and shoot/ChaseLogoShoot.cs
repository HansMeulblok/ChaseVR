using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class ChaseLogoShoot : MonoBehaviour
{
    private GameObject extendoPart;
    private XRRayInteractor rayInteractor;
    private RaycastHit hitForExtendoPart;
    private LineRenderer lineRenderer;

    private Transform lineStartPoint;
    private Transform lineEndPoint;
    private Transform extendoPartStartPoint;
    private Transform logoBase;

    public InputActionAsset controls;

    private Coroutine hit;

    private bool startExtendoPartAnimation = true;

    void Start()
    {
        extendoPart = transform.GetChild(0).gameObject;
        rayInteractor = transform.parent.parent.GetChild(4).GetComponent<XRRayInteractor>();

        lineStartPoint = transform.parent.transform.GetChild(2).transform;
        lineEndPoint = transform.GetChild(0).GetChild(0).transform;

        logoBase = transform.GetChild(1);

        lineRenderer = GetComponent<LineRenderer>();


        InputAction shootAction = controls.FindAction("XRI RightHand Interaction/Select");
        shootAction.performed += ShootExtendoPart;
    }

    void Update()
    {
        lineRenderer.SetPosition(0, lineStartPoint.position);
        lineRenderer.SetPosition(1, lineEndPoint.position);
    }

    public void ShootExtendoPart(InputAction.CallbackContext context)
    {
        ResetExtendoPart();

        if (rayInteractor.TryGetCurrent3DRaycastHit(out hitForExtendoPart) &&
            rayInteractor.interactablesSelected.Count == 0 &&
            (hitForExtendoPart.transform.TryGetComponent(out XRGrabInteractable grabbable) && grabbable.enabled))
        {
            extendoPart.transform.position = hitForExtendoPart.point;

            if (startExtendoPartAnimation)
            {
                

                hit = StartCoroutine(ExtendoPartAnimation(hitForExtendoPart));
                startExtendoPartAnimation = false;
            }
        }
    }

    private IEnumerator ExtendoPartAnimation(RaycastHit hitForExtendoPart)
    {
        yield return new WaitUntil(() => rayInteractor.interactablesSelected.Count > 0);

        lineRenderer.enabled = true;

        extendoPart.transform.position = hitForExtendoPart.point;
        extendoPart.transform.SetParent(hitForExtendoPart.collider.gameObject.transform, true);

        while (Vector3.Distance(lineStartPoint.position, lineEndPoint.position) > 0.1f &&
               Vector3.Dot((lineEndPoint.position - lineStartPoint.position), -logoBase.up) > 0)
        {
            yield return null;
        }

        ResetExtendoPart();

        startExtendoPartAnimation = true;
        lineRenderer.enabled = false;
    }

    private void ResetExtendoPart()
    {
        extendoPart.transform.SetParent(transform, true);
        extendoPart.transform.localScale = new Vector3(100, 100, 15);
        extendoPart.transform.localRotation = Quaternion.Euler(-90, 0, -90);

        extendoPart.transform.localPosition = Vector3.zero;
    }
}
