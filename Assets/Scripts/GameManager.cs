using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public int diamondsCount;
    public int deathsCount;
    public float snakeForwardSpeed;

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

}
