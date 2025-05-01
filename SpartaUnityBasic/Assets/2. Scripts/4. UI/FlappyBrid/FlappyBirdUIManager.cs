using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class FlappyBirdUIManager : MonoBehaviour
{
    public static FlappyBirdUIManager Instance { get; private set; }

    [SerializeField] private TextMeshProUGUI scoreText;

    [Header("Game Over Panel")] [SerializeField]
    private GameObject gameOverPanel;

    [SerializeField] private TextMeshProUGUI bestScoreText;
    [SerializeField] private TextMeshProUGUI currentScoreText;
    [SerializeField] private Button exitGameButton;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        FlappyBirdGameManager.Instance.OnScoreChanged += AddScore;

        exitGameButton.onClick.AddListener(() => OnClickExitBtn());
    }

    public void OpenGameOverPanel(int bestScore, int currentScore)
    {
        gameOverPanel.SetActive(true);
        bestScoreText.text = bestScore.ToString();
        currentScoreText.text = currentScore.ToString();
    }

    public void AddScore(int score)
    {
        scoreText.text = $"{score}";
    }

    public void OnClickExitBtn()
    {
        LoadSceneManager.Instance.LoadSceneAsync("MainScene");
    }

    private void OnDestroy()
    {
        Instance = null;
    }
}