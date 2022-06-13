using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GrabCheckSurfBoard : MonoBehaviour
{
    private string surfTestScene = "SurfTestScene";

    // Update is called once per frame
    void Update()
    {
        if (transform.parent == null)
        {
            SceneManager.LoadScene(surfTestScene);
        }        
    }
}
