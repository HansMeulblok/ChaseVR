using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SurfBoardMovement : MonoBehaviour
{
    public float speed;
    public bool start = false;
    public Material waveMaterial;

    public float _DeltaSpeed;
    public float _Offset;
    public float _Radius;

    public Vector3 _WaveStartPos;
    public Vector3 _RotatedOffset;

    private void Start()
    {
        speed *= 0.01f;

    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(transform.position.x + speed, transform.position.y, transform.position.z);
    }


    public void Floater()
    {
        //_Radius = 0.7f * Mathf.Log(-(Vector3.Distance(worldSpace, _WaveStartPos) - 120)) - 1;
        //_Radius = Mathf.Clamp(_Radius, 0.1f, 2f);


        //_RotatedOffset.x = Mathf.Sin(Time.time * _DeltaSpeed + worldSpace.x * _Offset) * _Radius;
        //_RotatedOffset.y = Mathf.Cos(Time.time * _DeltaSpeed + worldSpace.x * _Offset) * _Radius;
        //v.vertex.xyz += mul(unity_WorldToObject, rotatedOffset);
    }
}
