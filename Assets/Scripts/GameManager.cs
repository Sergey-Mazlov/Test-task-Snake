using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public float snakeForwardSpeed;
    private int _diamondsCount;
    private int _deathsCount;
    [SerializeField] private GameObject winPanel;
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private Text diamondText;
    [SerializeField] private Text deathsText;
    #region Singleton
    public static GameManager Instance;
    private void Awake()
    {
        Instance = this;
    }
    #endregion

    public void ShowGameOverPanel()
    {
        gameOverPanel.SetActive(true);
        Time.timeScale = 0;
    }

    public void ShowWinPanel()
    {
        winPanel.SetActive(true);
        Time.timeScale = 0;
    }

    public void AddDiamonds()
    {
        _diamondsCount++;
        diamondText.text = _diamondsCount.ToString();
    }

    public void AddDeaths()
    {
        _deathsCount++;
        deathsText.text = _deathsCount.ToString();
    }
}
