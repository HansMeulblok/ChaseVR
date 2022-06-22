using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit;

public class NewTeleportScript : MonoBehaviour
{
    public XRRayInteractor rayInteractor;
    public TeleportationProvider teleportationProvider;

    public GameObject blackOutSquare;
    public int fadespeed = 5;
    private bool isTeleporting;
    private bool JoyWentNorth;
    private RaycastHit raycasthithit;


    private void Start()
    {
        isTeleporting = false;
        JoyWentNorth = false;
        rayInteractor.enabled = false;

    }

    public void startTeleportRay(InputAction.CallbackContext context)
    {
        rayInteractor.enabled = true;
        var test = context.ReadValue<Vector2>();
             if (test.y >= 0.5)
            {
                JoyWentNorth = true;
            }
    }
    public void activateTeleport(InputAction.CallbackContext context)
    {
        rayInteractor.TryGetCurrent3DRaycastHit(out raycasthithit );
        if (raycasthithit.transform.GetComponent<TeleportationArea>() != null)
        {
            if (isTeleporting == false && JoyWentNorth == true && raycasthithit.transform.GetComponent<TeleportationArea>().isActiveAndEnabled)
            {
                isTeleporting = true;
                StartCoroutine(FadeToBlack());
            }
        }

    }

    public IEnumerator FadeToBlack()
    {
        if (!rayInteractor.TryGetCurrent3DRaycastHit(out RaycastHit raycasthit))
        {
        }
        else
        {
            Color objectcolor = blackOutSquare.GetComponent<Image>().color;
            float fadeamount;

            while (blackOutSquare.GetComponent<Image>().color.a < 1)
            {
                fadeamount = objectcolor.a + (fadespeed * Time.deltaTime);
                objectcolor = new Color(objectcolor.r, objectcolor.g, objectcolor.b, fadeamount);
                blackOutSquare.GetComponent<Image>().color = objectcolor;
                yield return null;
            }
        }
        

        StartCoroutine(Teleport());
    }

    public IEnumerator Teleport()
    {
        if (!rayInteractor.TryGetCurrent3DRaycastHit(out RaycastHit raycasthit))
        {
            StopCoroutine(FadeToBlack());
            StartCoroutine(fadeBack());
            yield return null;
        }
        else
        {
            TeleportRequest teleportrequest = new TeleportRequest()
            {
                destinationPosition = raycasthit.point,
            };
            teleportationProvider.QueueTeleportRequest(teleportrequest);
            StopCoroutine(FadeToBlack());
            StartCoroutine(fadeBack());
            yield return null;
        }
    }

    public IEnumerator fadeBack()
    {
        Color objectcolor = blackOutSquare.GetComponent<Image>().color;
        float fadeamount;
        while (blackOutSquare.GetComponent<Image>().color.a > 0)
        {
            fadeamount = objectcolor.a - (fadespeed * Time.deltaTime);
            objectcolor = new Color(objectcolor.r, objectcolor.g, objectcolor.b, fadeamount);
            blackOutSquare.GetComponent<Image>().color = objectcolor;
            yield return null;
        }
        isTeleporting = false;
        yield return JoyWentNorth = false;
    }

    public void stopTeleportRay(InputAction.CallbackContext context)
    {
        rayInteractor.enabled = false;
    }
}
