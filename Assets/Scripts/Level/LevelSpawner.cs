using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSpawner : MonoBehaviour
{
    [SerializeField] private LevelGenerator generator;
    [SerializeField] private SnakeTail snakeTail;
    
    private Queue<(string, Vector3, Vector3, Color)> objectsQueue;

    private ObjectPooler _pooler;

    private void Start()
    {
        _pooler = ObjectPooler.Instance;
        objectsQueue = generator.objectsQueue;
    }

    private void FixedUpdate()
    {
        if (objectsQueue == null)
        {
            objectsQueue = generator.objectsQueue;
            return;
        }

        if (Vector3.Distance(snakeTail.transform.position, objectsQueue.Peek().Item2) < 60)
        {
            (string tag, Vector3 pos, Vector3 scale, Color color) = objectsQueue.Dequeue();
            Transform obj = _pooler.SpawnFromPool(tag, pos, Quaternion.identity).transform;
            if (scale != Vector3.zero)
            {
                obj.localScale = scale;
            }
            if (color != Color.black && tag != "Wall")
            {
                Renderer[] renderers = obj.GetComponentsInChildren<Renderer>();
                foreach (Renderer rnd in renderers)
                {
                    rnd.material.color = color;
                }
            }
            if (tag == "Wall")
            {
                obj.GetComponent<Renderer>().material.color = color;
            }
        }
    }
}
