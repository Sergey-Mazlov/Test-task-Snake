using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SnakeTail))]
[RequireComponent(typeof(Rigidbody))]
public class SnakeController : MonoBehaviour
{
    
    private Plane _groundPlane;
    private Rigidbody _rigidbody;
    private Camera _mainCamera;
    private Vector3 _dir;
    private float _forwardSpeed;

    private void Start()
    {
        _mainCamera = Camera.main;
        _groundPlane = new Plane(Vector3.up, Vector3.zero);
        _rigidbody = GetComponent<Rigidbody>();
        _forwardSpeed = GameManager.Instance.snakeForwardSpeed;
    }


    void FixedUpdate()
    {
        _dir = Vector3.zero;
        if (Input.GetMouseButton(0))
        {
            _dir = (WorldPosition() - transform.position).normalized * 30f;
        }
        _dir.y = 0;
        _dir.z = _forwardSpeed;
        _rigidbody.velocity = _dir;
    }
    private Vector3 WorldPosition()
    {
        Vector3 position = new Vector3();
        Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
        
        if (_groundPlane.Raycast(ray, out float pos))
        {
            position = ray.GetPoint(pos);
        }
        
        return position;
    }
    
}
