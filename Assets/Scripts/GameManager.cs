using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    [SerializeField] private List<Color> colors;
    [SerializeField] private SnakeTail snakeTail;
    
    private Color _firstColor;
    private Color _secondColor;
    
    #region Singleton
    public static GameManager Instance;
    private void Awake()
    {
        Instance = this;
    }
    #endregion

    private void Start()
    {
        
    }

    private void Update()
    {

    }
    public (Color, Color) SetRandomColors()
    {
        
        List<Color> colorsForRnd = new List<Color>();
        foreach (Color color in colors)
        {
            if (color != _firstColor && color != _secondColor)
            {
                colorsForRnd.Add(color);
            }
        }

        int rndIndex = Random.Range(0, colorsForRnd.Count);
        _firstColor = colorsForRnd[rndIndex];
        colorsForRnd.RemoveAt(rndIndex);
        _secondColor = colorsForRnd[Random.Range(0, colorsForRnd.Count)];
        snakeTail.SetColor(_firstColor);
        return (_firstColor, _secondColor);
    }
}
