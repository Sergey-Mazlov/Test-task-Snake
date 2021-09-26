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

    private void Start()
    {
        _mainCamera = Camera.main;
        _groundPlane = new Plane(Vector3.up, Vector3.zero);
        _rigidbody = GetComponent<Rigidbody>();
    }


    void FixedUpdate()
    {
        _dir = Vector3.zero;
        if (Input.GetMouseButton(0) && !GameManager.Instance.fever)
        {
            _dir = (WorldPosition() - transform.position).normalized * 30f;
        }
        else if (GameManager.Instance.fever)
        {
            Vector3 pos = transform.position;
            Vector3 target = new Vector3(0, 0.5f, pos.z);
            _dir =  (target - pos).normalized * 30f * Vector3.Distance(target, pos);
        }

        _dir.z = GameManager.Instance.snakeForwardSpeed;
        _dir.y = 0;
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
