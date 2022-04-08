using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;


public class Teleportfader : MonoBehaviour
{
    /*public XRController RightTeleporterRay;
    public XRController LeftTeleporterRay;

    public InputHelpers.Button teleportActivationButting;
    public InputHelpers.Button TeleportFade;

    public float activationThreshold = 0.5f;
    public float fadeThreshhold = 0.8f;
    public float maxThreshhold = 0.99f;
    public float minFade = 0.4f, maxFade= 0.85f;
    //;public float ;

    new Vector2 joystick;

    public bool hasHitMax;

    public GameObject blackOutSquare;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        LeftTeleporterRay.inputDevice.TryGetFeatureValue(CommonUsages.primary2DAxis, out joystick);
        //Debug.Log(joystick.y);
        
        
        if (RightTeleporterRay)
        {
            RightTeleporterRay.gameObject.SetActive(CheckIfActivated(RightTeleporterRay));
            
            
        }

        if (LeftTeleporterRay)
        {
            if (joystick.y >= 0.99)
            {
                hasHitMax = true;
            }
            //MaxInput(LeftTeleporterRay);
            LeftTeleporterRay.gameObject.SetActive(CheckIfActivated(LeftTeleporterRay));
            

        }
        if (joystick.y >= minFade && joystick.y <= maxFade && hasHitMax == true)
        {
            StartCoroutine(fadeBlackOutsquare());
            Debug.Log("1");
        }
        else if (joystick.y < minFade && blackOutSquare.GetComponent<Image>().color.a > 0)
        {
            //Debug.Log(blackOutSquare.GetComponent<Image>().color.a);
            hasHitMax = false;
            Debug.Log("2");
            StartCoroutine(fadeBlackOutsquare(false));
        }
        else
        {
            //StopAllCoroutines();
            //Debug.Log("3");
        }



    }

    public IEnumerator fadeBlackOutsquare (bool fadeToBlack = true, int fadespeed = 20)
    {
        Color objectcolor = blackOutSquare.GetComponent<Image>().color;
        float fadeamount;
        if (fadeToBlack)
        {
            while (blackOutSquare.GetComponent<Image>().color.a < 1 && hasHitMax == true)
            {
                fadeamount = objectcolor.a + (fadespeed * Time.deltaTime);
                objectcolor = new Color(objectcolor.r, objectcolor.g, objectcolor.b, fadeamount);
                blackOutSquare.GetComponent<Image>().color = objectcolor;
                Debug.Log("fade to black");
                yield return new WaitForSeconds(0.5f);
            }
            
            

        }
        else
            while (blackOutSquare.GetComponent<Image>().color.a > 0 )
            {
                fadeamount = objectcolor.a - (fadespeed * Time.deltaTime);
                objectcolor = new Color(objectcolor.r, objectcolor.g, objectcolor.b, fadeamount);
                blackOutSquare.GetComponent<Image>().color = objectcolor;
                //hasHitMax = false;
                Debug.Log("no Fade");
                yield return null;
            }
    }
   
    public bool CheckIfActivated(XRController controller)
    {
        InputHelpers.IsPressed(controller.inputDevice, teleportActivationButting, out bool isActivated, activationThreshold);
        
           
        return isActivated;
    }

    public bool CheckForFade(XRController controller)
    {
        InputHelpers.IsPressed(controller.inputDevice, TeleportFade, out bool fadeOut, fadeThreshhold);
        
        return fadeOut;
    }
    *//*public bool MaxInput(XRController controller)
    {
        if (InputHelpers.IsPressed(controller.inputDevice, TeleportFade, out hasHitMax, maxThreshhold))
        {
            Debug.Log(hasHitMax);
            return hasHitMax = true;
        }
        else
        {
            return MaxInput(controller);
        }
       }*/



}
