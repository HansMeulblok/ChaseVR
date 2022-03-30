using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SurfBoardMovement : MonoBehaviour
{
    public float speed;
    public Material waveMaterial;

    [HideInInspector]
    public float _DeltaSpeed;
    [HideInInspector]
    public float _Offset;
    [HideInInspector]
    public float _Radius;

    [HideInInspector]
    public Vector3 _WaveStartPos;
    [HideInInspector]
    public Vector3 _RotatedOffset;

    public float timeValue;
    [HideInInspector]
    public float t;

    private void Start()
    {
        speed *= 0.01f;

        _DeltaSpeed = waveMaterial.GetFloat("_DeltaSpeed");
        _Offset = waveMaterial.GetFloat("_Offset");
        _Radius = waveMaterial.GetFloat("_Radius");
        _WaveStartPos = waveMaterial.GetVector("_WaveStartPos");

        timeValue = 0;
        
        
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.x <= 75f)
        {
            transform.position += Vector3.right * speed;
            transform.position = new Vector3(transform.position.x, Mathf.Lerp(1.38f, 0.2f, t), transform.position.z);
        }

        waveMaterial.SetVector("_SurfBoardPos", transform.position);

        t += 0.2f * Time.deltaTime;

        timeValue += Time.deltaTime;
             

        waveMaterial.SetFloat("_TimeValue", timeValue);
    }
}
