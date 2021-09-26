using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public float snakeForwardSpeed;
    public bool fever = false;

    private int _diamondsCount;
    private int _deathsCount;
    [SerializeField] private GameObject winPanel;
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private Text diamondText;
    [SerializeField] private Text deathsText;
    private int _feverCounter;
    #region Singleton
    public static GameManager Instance;
    private void Awake()
    {
        Instance = this;
    }
    #endregion

    private void Start()
    {
        Application.targetFrameRate = 240;
        diamondText.text = "0";
        deathsText.text = "0";
    }
    

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
        if (_feverCounter < 4 && !fever)
        {
            _feverCounter++;
        }
        else if (!fever)
        {
            StartCoroutine(FeverTimer());
            fever = true;
            snakeForwardSpeed *= 3f;
        }
    }

    public void AddDeaths()
    {
        _deathsCount++;
        _feverCounter = 0;
        deathsText.text = _deathsCount.ToString();
    }

    private IEnumerator FeverTimer()
    {
        yield return new WaitForSeconds(3f);
        fever = false;
        _feverCounter = 0;
        snakeForwardSpeed /= 3f;
    }
}
