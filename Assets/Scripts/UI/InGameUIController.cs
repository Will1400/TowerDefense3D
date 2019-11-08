using UnityEngine;
using System.Collections;
using TMPro;
using System;

public class InGameUIController : MonoBehaviour
{
    [SerializeField]
    private GameObject gameWonPanel;
    [SerializeField]
    private TextMeshProUGUI gameWonRoundNumberText;
    [SerializeField]
    private GameObject gameOverPanel;
    [SerializeField]
    private TextMeshProUGUI gameOverRoundNumberText;

    private void Start()
    {
        gameOverPanel.SetActive(false);
        gameWonPanel.SetActive(false);
        GameManager.Instance.GameLost.AddListener(GameOver);
        GameManager.Instance.GameWon.AddListener(GameWon);
        MapGenerator.Instance.MapRendered.AddListener(MapRendered);
    }
   
    public void OnClickTryAgain()
    {
        MapGenerator.Instance.GenerateMap();
        MapGenerator.Instance.RenderMap();
    }

    public void OnClickQuit()
    {
        Application.Quit();
    }

    void MapRendered()
    {
        gameOverPanel.SetActive(false);
        gameWonPanel.SetActive(false);
    }

    private void GameWon()
    {
        gameWonPanel.SetActive(true);
        gameWonRoundNumberText.text = WaveManager.Instance.Round.ToString();
    }

    private void GameOver()
    {
        gameOverPanel.SetActive(true);
        gameOverRoundNumberText.text = WaveManager.Instance.Round.ToString();
    }
}