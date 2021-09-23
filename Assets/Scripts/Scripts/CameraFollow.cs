using System;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;

    public float smoothSpeed = 0.125f;

    public Vector3 offset;

    private Transform _transform;
    private void Awake()
    {
        _transform = transform;
    }

    private void FixedUpdate()
    {
        Vector3 desiredPos = target.position + offset;
        Vector3 smoothedPos = Vector3.Lerp(_transform.position, desiredPos, smoothSpeed);
        smoothedPos.z = 0;
        _transform.position = smoothedPos;
    }
}
