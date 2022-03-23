using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class Teleportfader : MonoBehaviour
{
    public XRController RightTeleporterRay;
    public XRController LeftTeleporterRay;

    public InputHelpers.Button teleportActivationButting;
    public InputHelpers.Button TeleportFade;
    public float activationThreshold = 0.5f;

    public GameObject blackOutSquare;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (RightTeleporterRay)
        {
            RightTeleporterRay.gameObject.SetActive(CheckIfActivated(RightTeleporterRay));
        }


        if (LeftTeleporterRay)
        {
            LeftTeleporterRay.gameObject.SetActive(CheckIfActivated(LeftTeleporterRay));

            /*
            if (CheckIfActivated(LeftTeleporterRay)){

                Debug.Log("trigger pressed");
            }
            if (CheckForFade(LeftTeleporterRay))
            {
                Debug.Log("triggerbutton pressed");
            }


            if (CheckForFade(LeftTeleporterRay) && !CheckIfActivated(LeftTeleporterRay))
            {
                Debug.Log("3");
                StartCoroutine(fadeBlackOutsquare());
            }
            else
            {
                StartCoroutine(fadeBlackOutsquare(false));
                Debug.Log("4");
            }*/
        }

    }

    public IEnumerator fadeBlackOutsquare (bool fadeToBlack = true, int fadespeed = 5)
    {
        Color objectcolor = blackOutSquare.GetComponent<Image>().color;
        float fadeamount;
        if (fadeToBlack)
        {
            while (blackOutSquare.GetComponent<Image>().color.a < 1)
            {
                fadeamount = objectcolor.a + (fadespeed * Time.deltaTime);
                objectcolor = new Color(objectcolor.r, objectcolor.g, objectcolor.b, fadeamount);
                blackOutSquare.GetComponent<Image>().color = objectcolor;
                yield return null;
            }
        }else
            while (blackOutSquare.GetComponent<Image>().color.a > 0)
            {
                fadeamount = objectcolor.a - (fadespeed * Time.deltaTime);
                objectcolor = new Color(objectcolor.r, objectcolor.g, objectcolor.b, fadeamount);
                blackOutSquare.GetComponent<Image>().color = objectcolor;
                yield return null;
            }

        yield return new WaitForEndOfFrame();
    }

    public bool CheckIfActivated(XRController controller)
    {
        InputHelpers.IsPressed(controller.inputDevice, teleportActivationButting, out bool isActivated, activationThreshold);
        
        return isActivated;
    }

    public bool CheckForFade(XRController controller)
    {
        InputHelpers.IsPressed(controller.inputDevice, TeleportFade, out bool fadeOut, activationThreshold);
        
        return fadeOut;
    }


}
