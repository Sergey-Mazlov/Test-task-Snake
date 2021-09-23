using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(SnakeController))]
public class SnakeTail : MonoBehaviour
{
    private Transform snakeHead;
    public Transform snakePrefab;
    public float sphereDiameter;
    
    private List<Transform> _snakeSphere = new List<Transform>();
    private List<Vector3> _positions = new List<Vector3>();
    private SnakeController _snakeController;
    

    private void Awake()
    {
        snakeHead = Instantiate(snakePrefab, new Vector3(0, 1, 0), Quaternion.identity, transform);
        _positions.Add(snakeHead.position);
    }

    private void Start()
    {
        _snakeController = GetComponent<SnakeController>();
    }

    private void FixedUpdate()
    {
        // Считаю дистанцию между новым и старым положением головы
        float distance = (snakeHead.position - _positions[0]).magnitude; 

        if (distance > sphereDiameter)
        {
            // Направление от старого положения головы, к новому
            Vector3 direction = (snakeHead.position - _positions[0]).normalized;

            _positions.Insert(0, _positions[0] + direction * sphereDiameter);
            _positions.RemoveAt(_positions.Count - 1);

            distance -= sphereDiameter;
            
            /*_positions.Insert(0, snakeHead.position);
            _positions.RemoveAt(_positions.Count - 1);
            distance -= sphereDiameter;*/
        }

        for (int i = 0; i < _snakeSphere.Count; i++)
        {
            _snakeSphere[i].position = Vector3.Lerp(_positions[i + 1], _positions[i], distance / sphereDiameter);
        }
        
        
        if (Input.GetKeyDown(KeyCode.A))
        {
            _snakeController.lenght++;
            AddSphere();
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            _snakeController.lenght--;
            RemoveSphere();
        }
    }

    
    public void AddSphere()
    {
        _snakeSphere.Insert(0, snakeHead);
        
        Transform sphere = Instantiate(snakePrefab, transform.localPosition, Quaternion.identity, transform);
        sphere.gameObject.GetComponent<Renderer>().material.color = new Color(Random.Range(0, 1f), Random.Range(0, 1f), Random.Range(0, 1f));
        
        snakeHead = sphere;
        
        _positions.Insert(0, snakeHead.position);
    }

    public void RemoveSphere()
    {
        if (_snakeSphere.Count <= 1) return;
        Destroy(snakeHead.gameObject);
        snakeHead = _snakeSphere[0];
        snakeHead.position = transform.localPosition;
        _snakeSphere.RemoveAt(0);
        _positions.RemoveAt(_positions.Count - 1);
    }
}
