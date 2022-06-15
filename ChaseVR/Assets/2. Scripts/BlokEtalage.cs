using System.Collections;
using System.Collections.Generic;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine;

public class BlokEtalage : MonoBehaviour
{
    public float force = 30;
    public Transform controllerTransform;
    public bool holding;
    public Transform holderPoint;
    private Transform lookAtPoint;
    private GameObject shootingTarget;
    private bool holdingetalage;
    
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
            holdingetalage = true;
            return;
        }
        
        //shootingTarget.GetComponent<MeshRenderer>().enabled = true;
        holding = true;
        force = 30;
        gameObject.AddComponent<CubeScaling>();
    } 

    public void ShootCube(SelectExitEventArgs args)
    {
        holdingetalage = false;
        GetComponent<XRGrabInteractable>().enabled = false;
        GetComponent<Rigidbody>().AddForce(controllerTransform.forward * force, ForceMode.Impulse);
        StartCoroutine(DelayedTurnOn());
    }

    // user can acces the etalage after 3 seconds, after 5 seconds the etalage disapears. if the user picks up the etalage the timer resets.
    IEnumerator DelayedTurnOn()
    {
        yield return new WaitForSeconds(3);
        if (gameObject.GetComponent<XRGrabInteractable>() != null)
        {
            GetComponent<XRGrabInteractable>().enabled = true;
            for (float timer = 5; timer >= 0; timer -= Time.deltaTime)
            {
                if (holdingetalage == true)
                {
                    yield break;
                }
                yield return null;
            }

            gameObject.SetActive(false);
            yield break;
        }

    }
}
