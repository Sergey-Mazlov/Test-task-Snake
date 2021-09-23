using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class LevelGenerator : MonoBehaviour
{
    [Header("Color List")]
    [SerializeField] private List<Color> colors;

    [Header("Level Settings")] 
    [SerializeField] private float duration;
    [SerializeField] private float snakeSpeed;
    [SerializeField] private SnakeTail _snakeTail;
    [SerializeField] private int diamondsRow;
    [SerializeField] private int peopleRow;

    [Header("Level Prefabs")] 
    [SerializeField] private GameObject wall;
    [SerializeField] private GameObject ground;
    [SerializeField] private List<GameObject> groups;
    [SerializeField] private GameObject diamond;
    [SerializeField] private GameObject mine;
    [SerializeField] private GameObject changeWall;

    private float _stepPeople = 4.5f;
    private float _stepDiamond = 3.5f;
    private float _betweenDistance = 5f;
    private Color _firstColor;
    private Color _secondColor;

    private void Start()
    {
        float levelLenght = duration * snakeSpeed;

        Transform wallLeftT = Instantiate(wall, transform).transform;
        Transform wallRightT = Instantiate(wall, transform).transform;
        Transform groundT = Instantiate(ground, transform).transform;
        
        wallLeftT.localScale = new Vector3(1, 1, levelLenght / wallLeftT.GetComponent<Renderer>().bounds.size.z);
        wallRightT.localScale = new Vector3(1, 1, levelLenght / wallRightT.GetComponent<Renderer>().bounds.size.z);
        groundT.localScale = new Vector3(1, 1, levelLenght / groundT.GetComponent<Renderer>().bounds.size.z);

        wallLeftT.position = new Vector3(-5.5f, 0.5f, levelLenght / 2);
        wallRightT.position = new Vector3(5.5f, 0.5f, levelLenght / 2);
        groundT.position = new Vector3(0, 0, levelLenght / 2);

        int blockCount = (int)levelLenght / (int)(peopleRow * _stepPeople + diamondsRow * _stepDiamond + _betweenDistance * 2);
        float z = 0;
        for (int i = 0; i < blockCount; i++)
        {
            for (int j = 0; j < peopleRow; j++)
            {
                GameObject rndObj = groups[Random.Range(0, groups.Count)];
                Instantiate(rndObj, new Vector3(0, 0, z), Quaternion.identity, transform);
                z += _stepPeople;
            }
            
            z += _betweenDistance;
            
            for (int j = 0; j < diamondsRow; j++)
            {
                Instantiate(diamond, new Vector3(0, 0, z), Quaternion.identity, transform);
                z += _stepPeople;
            }
            
            z += _betweenDistance;
        }
    }


    private (Color, Color) GetRandomColors(Color first, Color second)
    {
        List<Color> colorsForRnd = new List<Color>();
        foreach (Color color in colors)
        {
            if (color != first && color != second)
            {
                colorsForRnd.Add(color);
            }
        }
        int rndIndex = Random.Range(0, colorsForRnd.Count);
        first = colorsForRnd[rndIndex];
        colorsForRnd.RemoveAt(rndIndex);
        second = colorsForRnd[Random.Range(0, colorsForRnd.Count)];
        return (first, second);
    }
}
