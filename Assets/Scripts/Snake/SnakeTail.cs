using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeTail : MonoBehaviour
{
    
    public Transform snakePrefab;
    public float sphereDiameter;
    public int lenght = 5;
    public int maxLenght = 10;
    
    private List<Transform> _snakeSphere = new List<Transform>();
    private List<Vector3> _positions = new List<Vector3>();
    private Color _selfColor;
    private Transform _snakeHead;
    private GameManager _gameManager;
    
    
    private void Awake()
    {
        _snakeHead = Instantiate(snakePrefab, new Vector3(0, 1, 0), Quaternion.identity, transform);
        _positions.Add(_snakeHead.position);
        for (int i = 0; i < lenght; i++)
        {
            AddSphere();
        }
    }

    private void Start()
    {
        _gameManager = GameManager.Instance;
    }

    private void FixedUpdate()
    {
        float distance = (_snakeHead.position - _positions[0]).magnitude;
        
        if (distance > sphereDiameter)
        {
            Vector3 direction = (_snakeHead.position - _positions[0]).normalized;
            _positions.Insert(0, _positions[0] + direction * sphereDiameter);
            _positions.RemoveAt(_positions.Count - 1);
            distance -= sphereDiameter;
        }

        for (int i = 0; i < _snakeSphere.Count; i++)
        {
            _snakeSphere[i].position = Vector3.Lerp(_positions[i + 1], _positions[i], distance / sphereDiameter);
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            AddSphere();
            lenght++;
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            RemoveSphere();
            lenght++;
        }
    }

    
    public void AddSphere()
    {
        _snakeSphere.Insert(0, _snakeHead);
        Transform sphere = Instantiate(snakePrefab, transform.localPosition, Quaternion.identity, transform);
        _snakeHead = sphere;
        _positions.Insert(0, _snakeHead.position);
        sphere.GetComponent<Renderer>().material.color = _selfColor;
    }

    public void RemoveSphere()
    {
        if (_snakeSphere.Count <= 1) return;
        Destroy(_snakeHead.gameObject);
        _snakeHead = _snakeSphere[0];
        _snakeHead.position = transform.localPosition;
        _snakeSphere.RemoveAt(0);
        _positions.RemoveAt(_positions.Count - 1);
        lenght--;
    }

    public void SetStartColor(Color color)
    {
        _selfColor = color;
        _snakeHead.GetComponent<Renderer>().material.color = _selfColor;
        foreach (Transform trn in _snakeSphere)
        {
            trn.GetComponent<Renderer>().material.color = _selfColor;
        }
    }
    public void SetColor(Color color)
    {
        _selfColor = color;
        StartCoroutine(ChangeColor());
    }

    private IEnumerator ChangeColor()
    {
        _snakeHead.GetComponent<Renderer>().material.color = _selfColor;
        yield return new WaitForSeconds(1.25f / GameManager.Instance.snakeForwardSpeed);
        foreach (Transform trn in _snakeSphere)
        {
            trn.GetComponent<Renderer>().material.color = _selfColor;
            yield return new WaitForSeconds(1.25f / GameManager.Instance.snakeForwardSpeed);
        }
    }


    private void OnCollisionEnter(Collision other)
    {
        Transform trn = other.collider.transform;
        switch (trn.tag)
        {
            case "Mine":
                _gameManager.ShowGameOverPanel();
                break;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        switch (other.tag)
        {
            case "Diamond":
                _gameManager.AddDeaths();
                if (lenght < maxLenght)
                {
                    AddSphere();
                    lenght++;
                }
                Destroy(other.gameObject);
                break;
            case "Man":
                if (other.GetComponent<Renderer>().material.color == _selfColor)
                {
                    _gameManager.AddDiamonds();
                    if (lenght < maxLenght)
                    {
                        AddSphere();
                        lenght++;
                    }
                    Destroy(other.gameObject);
                }
                else
                { 
                    _gameManager.ShowGameOverPanel();
                }
                break;
            case "Wall":
                SetColor(other.GetComponent<Renderer>().material.color);
                break;
            case "Finish":
                _gameManager.ShowWinPanel();
                break;
        }
    }
}
