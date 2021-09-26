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
    [SerializeField] private bool isSetup;
    [SerializeField] private float duration;
    [SerializeField] private SnakeTail snakeTail;
    [SerializeField] private int diamondsRow;
    [SerializeField] private int peopleRow;
    [SerializeField] private Transform water;
    [SerializeField] private float renderDistance;

    [Header("Level Prefabs")] 
    [SerializeField] private GameObject wall;
    [SerializeField] private GameObject ground;
    [SerializeField] private List<GameObject> groups;
    [SerializeField] private GameObject diamond;
    [SerializeField] private GameObject mine;
    [SerializeField] private GameObject changeWall;
    [SerializeField] private GameObject finish;

    private float _stepPeople = 4.5f;
    private float _stepDiamond = 3.5f;
    private float _betweenDistance = 6f;
    private Color _firstColor;
    private Color _secondColor;

    public Queue<(string, Vector3, Vector3, Color)> objectsQueue;
    private void Start()
    {
        objectsQueue = new Queue<(string, Vector3, Vector3, Color)>();
        float levelLenght = duration * GameManager.Instance.snakeForwardSpeed;

        if (isSetup)
        {
            Transform wallLeftT = Instantiate(wall, transform).transform;
            Transform wallRightT = Instantiate(wall, transform).transform;
            Transform groundT = Instantiate(ground, transform).transform;

            wallLeftT.localScale = new Vector3(1, 1, levelLenght / wallLeftT.GetComponent<Renderer>().bounds.size.z);
            wallRightT.localScale = new Vector3(1, 1, levelLenght / wallRightT.GetComponent<Renderer>().bounds.size.z);
            groundT.localScale = new Vector3(1, 1, levelLenght / groundT.GetComponent<Renderer>().bounds.size.z);
            water.localScale = new Vector3(10, 1, (levelLenght + 20f) / water.GetComponent<Renderer>().bounds.size.z);

            wallLeftT.position = new Vector3(-5.5f, 0.5f, levelLenght / 2);
            wallRightT.position = new Vector3(5.5f, 0.5f, levelLenght / 2);
            groundT.position = new Vector3(0, 0, levelLenght / 2);
            water.position = new Vector3(0, -1, (levelLenght - 20f) / 2);
        }
        
        int blockCount = (int)levelLenght / (int)(peopleRow * _stepPeople + diamondsRow * _stepDiamond + _betweenDistance * 2);
        float z = 0;
        (_firstColor, _secondColor) = GetRandomColors(_firstColor, _secondColor);
        snakeTail.SetStartColor(_firstColor);
        for (int i = 0; i < blockCount; i++)
        {
            int colorCounter = 0;
            int manPosCounter = 0;
            for (int j = 0; j < peopleRow; j++)
            {
                GameObject rndObj = groups[Random.Range(0, groups.Count)];

                Vector3 posScale;
                #region SetPosition
                if (Random.value > 0.5f && manPosCounter < 2 || manPosCounter < 0)
                {
                    posScale = new Vector3(1, 1, 1);
                    manPosCounter++;
                }
                else
                {
                    posScale = new Vector3(-1, 1, 1);
                    manPosCounter--;
                }
                #endregion
                
                #region SetColor
                Color color;
                if (Random.value > 0.5f && colorCounter < 2 || colorCounter < 0)
                {
                    color = _firstColor;
                    colorCounter++;
                }
                else
                {
                    color = _secondColor;
                    colorCounter--;
                }
                #endregion
                
                objectsQueue.Enqueue((rndObj.name, new Vector3(0, 0, z),posScale, color));

                z += _stepPeople;
            }
            
            z += _betweenDistance;

            int diamondCounter = 0;
            int diamondCountCounter = 0;
            for (int j = 0; j < diamondsRow; j++)
            {
                if (Random.Range(0, 3) > 1 && diamondCountCounter < 2)
                {
                    float x = -3.5f;
                    if (Random.value > 0.5f && diamondCounter < 3 || diamondCounter < 1)
                    {
                        objectsQueue.Enqueue(("Diamond", new Vector3(x, 1.5f, z), Vector3.zero, Color.black));
                        x += 3.5f;
                        objectsQueue.Enqueue(("Mine", new Vector3(x, 1.5f, z), Vector3.zero, Color.black));
                        x += 3.5f;
                        objectsQueue.Enqueue(("Diamond", new Vector3(x, 1.5f, z), Vector3.zero, Color.black));
                        diamondCounter++;
                    }
                    else
                    {
                        objectsQueue.Enqueue(("Mine", new Vector3(x, 1.5f, z), Vector3.zero, Color.black));
                        x += 3.5f;
                        objectsQueue.Enqueue(("Diamond", new Vector3(x, 1.5f, z), Vector3.zero, Color.black));
                        x += 3.5f;
                        objectsQueue.Enqueue(("Mine", new Vector3(x, 1.5f, z), Vector3.zero, Color.black));
                        diamondCounter--;
                    }
                    diamondCountCounter++;
                }
                else
                {
                    if (Random.value > 0.5f && diamondCounter < 2 || diamondCounter < 1)
                    {
                        objectsQueue.Enqueue(("Diamond", new Vector3(0, 1.5f, z), Vector3.zero, Color.black));
                        diamondCounter++;
                    }
                    else
                    {
                        objectsQueue.Enqueue(("Mine", new Vector3(0, 1.5f, z), Vector3.zero, Color.black));
                        diamondCounter--;
                    }
                    diamondCountCounter--;
                }
                z += _stepPeople;
            }

            z += _betweenDistance / 2f;
            (_firstColor, _secondColor) = GetRandomColors(_firstColor, _secondColor);
            #region ChangeWall
            if (i != blockCount - 1)
            {
                objectsQueue.Enqueue(("Wall", new Vector3(0, 1.5f, z), Vector3.zero, _firstColor));
            }
            #endregion
            z += _betweenDistance / 2f;
        }
        Instantiate(finish, new Vector3(0, 0, z + _betweenDistance), Quaternion.identity, transform);
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
