using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRotation : MonoBehaviour
{
    [SerializeField]
    private Transform _transform;
    [SerializeField]
    public float RotaionSpeedOnYAxses;
    [SerializeField]
    public float RotationOfXAngel;

    private void Start()
    {
        RotateBaseOnSpaceWorld(RotationOfXAngel, 0f, 0f);
    }
    public void Update()
    {
        RotateBaseOnSpaceWorld(0f, RotaionSpeedOnYAxses, 0f);
    }
    private void RotateBaseOnSpaceWorld(float xAxsis, float yAxsis, float zAxsis)
    {
        _transform.Rotate(xAxsis, yAxsis, zAxsis, Space.World);
    }
}
