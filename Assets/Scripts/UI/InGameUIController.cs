using UnityEngine;
using System.Collections;
using TMPro;
using System;

public class InGameUIController : MonoBehaviour
{
    [SerializeField]
    private GameObject gameOverPanel;
    [SerializeField]
    private TextMeshProUGUI roundNumberText;
   
    public void OnClickTryAgain()
    {
        MapGenerator.Instance.GenerateMap();
        MapGenerator.Instance.RenderMap();
    }

    private void Start()
    {
        gameOverPanel.SetActive(false);
        GameManager.Instance.GameLost.AddListener(GameOver);
        MapGenerator.Instance.MapRendered.AddListener(MapRendered);
    }

    void MapRendered()
    {
        gameOverPanel.SetActive(false);
    }

    private void GameOver()
    {
        gameOverPanel.SetActive(true);
        roundNumberText.text = WaveManager.Instance.Round.ToString();
    }
}