using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRotation : MonoBehaviour
{
    [SerializeField]
    private Transform _transform;
    [SerializeField]
    public float RotaionSpeedOnYAcses;
    [SerializeField]
    public float RotationOfXAngel;

    private void Start()
    {
        RotateBaseOnSpaceWorld(RotationOfXAngel, 0f, 0f);
    }
    public void FixedUpdate()
    {
        RotateBaseOnSpaceWorld(0f, RotaionSpeedOnYAcses, 0f);
    }
    private void RotateBaseOnSpaceWorld(float xAcsis , float yAcsis , float zAcsis)
    {
        _transform.Rotate(xAcsis, yAcsis, zAcsis, Space.World);
    }
}
