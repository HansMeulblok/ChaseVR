using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SurfBoardMovement : MonoBehaviour
{   
    public Material waveMaterial;
    public float speedMultiplier;

    private string finalEnvironment = "FinalEnvironment";

    [HideInInspector]
    public float surfBoardSpeed;
    [HideInInspector]
    public float shaderSpeed;
    [HideInInspector]
    public float radius;

    [HideInInspector]
    public Vector3 waveStartPos;
    [HideInInspector]
    public Vector3 rotatedOffset;

    public float timeValue;
    [HideInInspector]
    public float t;

    private void Start()
    {
        radius = waveMaterial.GetFloat("_Radius");
        waveStartPos = waveMaterial.GetVector("_WaveStartPos");

        timeValue = 0;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        shaderSpeed = waveMaterial.GetFloat("_DeltaSpeed");
        surfBoardSpeed = shaderSpeed * speedMultiplier;

        if (transform.position.x <= 75f)
        {
            transform.position += Vector3.right * surfBoardSpeed;
            transform.position = new Vector3(transform.position.x, transform.position.y/*Mathf.Lerp(1.38f, 0.2f, t)*/, transform.position.z);
        }

        waveMaterial.SetVector("_SurfBoardPos", transform.position);

        t += 0.2f * Time.deltaTime;

        timeValue += Time.deltaTime;
             

        waveMaterial.SetFloat("_TimeValue", timeValue);

        if (transform.position.x >= 75f)
        {
            SceneManager.LoadScene(finalEnvironment);
        }
    }
}
