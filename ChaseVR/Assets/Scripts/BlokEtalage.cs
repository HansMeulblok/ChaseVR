using System.Collections;
using System.Collections.Generic;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine;

/// DISCLAIMER: dit script is geschreven met tegenzin, geen motivatie en 3 uur slaap. 
public class BlokEtalage : MonoBehaviour
{
    public float force = 30;
    public Transform controllerTransform;
    public bool holding;
    public Transform holderPoint;
    private Transform lookAtPoint;
    private GameObject shootingTarget;
    
    void Start()
    {   
        controllerTransform = GameObject.Find("RightHand Controller").transform;
        shootingTarget = GameObject.FindGameObjectWithTag("ShootingTarget");
        lookAtPoint = GameObject.Find("LookAtPoint").transform;
    }

    void FixedUpdate()
    {
        if(holding)
        return;
        
        GetComponent<Rigidbody>().position = holderPoint.position;

        // Calculcate look direction and set Y to 0 so they only rotate towards the look point on 1 axis. 
        var lookDir = lookAtPoint.position - transform.position;
        lookDir.y = 0;
        transform.rotation = Quaternion.LookRotation(lookDir);
    }

    public void Replace(SelectEnterEventArgs args)
    {        
        if(holding) 
        {
            return;
        }
        
        shootingTarget.GetComponent<MeshRenderer>().enabled = true;
        holding = true;
        force = 30;
        gameObject.AddComponent<CubeScaling>();
    } 

    public void ShootCube(SelectExitEventArgs args)
    {
        GetComponent<XRGrabInteractable>().enabled = false;
        GetComponent<Rigidbody>().AddForce(controllerTransform.forward * force, ForceMode.Impulse);
        holding = false;
        StartCoroutine(DelayedTurnOn());
    }

    IEnumerator DelayedTurnOn()
    {
        yield return new WaitForSeconds(5);
        GetComponent<XRGrabInteractable>().enabled = true;
        shootingTarget.GetComponent<MeshRenderer>().enabled = false;
    }
}
