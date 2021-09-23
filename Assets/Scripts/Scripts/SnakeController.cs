using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeController : MonoBehaviour
{
    public GameObject obj;
    public float forwardSpeed = 10f;
    public int lenght = 1;
    public float roadSize;
    
    private Plane _groundPlane;
    private Transform _transform;
    private Rigidbody _rigidbody;
    private Camera _mainCamera;
    private Vector3 _offset;
    private SnakeTail _snakeTail;
    private float _t = 0.2f;
    
    void Awake()
    {
#if UNITY_EDITOR
        //QualitySettings.vSyncCount = 0;  // VSync must be disabled
        Application.targetFrameRate = 60;
#endif
        _mainCamera = Camera.main;
        _transform = obj.GetComponent<Transform>();
        _groundPlane = new Plane(Vector3.up, Vector3.zero);
        _rigidbody = obj.GetComponent<Rigidbody>();

        _snakeTail = GetComponent<SnakeTail>();
        for (int i = 0; i < lenght; i++)
        {
            _snakeTail.AddSphere();
        }
    }
    

    void FixedUpdate()
    {
        if (Input.GetMouseButtonDown(0))
        {
            _offset = _transform.position - WorldPosition();
        }
        else if (Input.GetMouseButton(0))
        {
            Vector3 position = _transform.position;
            if (position.z > -roadSize && position.z < roadSize)
            {
                Vector3 worldPos = new Vector3(position.x, position.y, WorldPosition().z + _offset.z);
                _transform.position = Vector3.Lerp(transform.position, worldPos, _t);
            }
            else if (position.z < -roadSize && WorldPosition().z + _offset.z > -roadSize)
            {
                Vector3 worldPos = new Vector3(position.x, position.y, WorldPosition().z + _offset.z);
                _transform.position = Vector3.Lerp(transform.position, worldPos, _t);
            }
            else if (position.z > roadSize  && WorldPosition().z + _offset.z < roadSize)
            {
                Vector3 worldPos = new Vector3(position.x, position.y, WorldPosition().z + _offset.z);
                _transform.position = Vector3.Lerp(transform.position, worldPos, _t);
            }
            
        }
        _rigidbody.velocity = new Vector3(forwardSpeed, 0, 0);
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
