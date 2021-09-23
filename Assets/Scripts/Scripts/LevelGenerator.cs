using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class LevelGenerator : MonoBehaviour
{
    public float roadX;
    public float roadZ;
    public GameObject cubePrefab;
    private Renderer _renderer;

    private void Awake()
    {
        _renderer = cubePrefab.GetComponent<Renderer>();
        float cubeZ = _renderer.bounds.size.z;
        for (float i = 10; i < roadX; i += roadX / 10 )
        {
            int maxCube = (int)(roadZ / cubeZ);
            Debug.Log(maxCube);
            for (int j = 0; j < maxCube; j++)
            {
                if (Random.Range(0f, 10f) >= 5f)
                {
                    Vector3 pos = new Vector3(i, 0.5f, j*cubeZ + cubeZ - roadZ);
                    Instantiate(cubePrefab, pos, Quaternion.identity, transform);
                }   
            }
        }
    }
}
