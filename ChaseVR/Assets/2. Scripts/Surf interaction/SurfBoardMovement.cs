using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SurfBoardMovement : MonoBehaviour
{   
    public Material waveMaterial;
    public float speedMultiplier;

    private string finalEnvironment = "MainScene";

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

    [HideInInspector]
    public bool playing = false;

    private void Start()
    {
        radius = waveMaterial.GetFloat("_Radius");
        waveStartPos = waveMaterial.GetVector("_WaveStartPos");

        waveMaterial.SetVector("_SurfBoardPos", transform.position);
        waveMaterial.SetFloat("_TimeValue", 12);
    }

    void FixedUpdate()
    {
        shaderSpeed = waveMaterial.GetFloat("_DeltaSpeed");
        surfBoardSpeed = shaderSpeed * speedMultiplier;

        if (playing)
        {
            if (transform.position.x <= 75f)
            {
                transform.position += Vector3.right * surfBoardSpeed;
                transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z);
            }
            else if (transform.position.x >= 75f)
            {
                SceneManager.LoadScene(finalEnvironment);
            }

           


            timeValue += Time.deltaTime;

            waveMaterial.SetVector("_SurfBoardPos", transform.position);
            waveMaterial.SetFloat("_TimeValue", timeValue);
        }
    }
}
