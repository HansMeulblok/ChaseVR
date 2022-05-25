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

    public InputActionAsset controls;

    // Start is called before the first frame update
    void Start()
    {
        extendoPart = transform.GetChild(0).gameObject;
        rayInteractor = transform.parent.parent.GetChild(4).GetComponent<XRRayInteractor>();

        lineStartPoint = transform.parent.transform.GetChild(2).transform;
        lineEndPoint = transform.GetChild(0).GetChild(0).transform;

        lineRenderer = GetComponent<LineRenderer>();


        InputAction shootAction = controls.FindAction("XRI RightHand Interaction/Select");
        shootAction.started += ShootExtendoPart;
    }

    // Update is called once per frame
    void Update()
    {
        lineRenderer.SetPosition(0, lineStartPoint.position);
        lineRenderer.SetPosition(1, lineEndPoint.position);
    }

    public void ShootExtendoPart(InputAction.CallbackContext context)
    {
        rayInteractor.TryGetCurrent3DRaycastHit(out hitForExtendoPart);

        extendoPart.transform.position = hitForExtendoPart.point;


        StartCoroutine(ExtendoPartAnimation(hitForExtendoPart));

       
    }

    private IEnumerator ExtendoPartAnimation(RaycastHit hitForExtendoPart)
    {
        while (Vector3.Distance(lineStartPoint.position, lineEndPoint.position) > 0.1f)
        {
            extendoPart.transform.position = hitForExtendoPart.transform.position;

            yield return null;
        }
    }
}
