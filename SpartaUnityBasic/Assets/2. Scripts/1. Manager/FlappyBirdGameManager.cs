using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlappyBirdGameManager : MonoBehaviour
{
    const string BestScoreKey = "FlappyBirdBestScore";
    public static FlappyBirdGameManager Instance { get; private set; }

    private int currentScore = 0;
    private int bestScore = 0;


    public event Action<int> OnScoreChanged;

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        bestScore = PlayerPrefs.GetInt(BestScoreKey, 0);
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void GameOver()
    {
        if (currentScore > bestScore)
        {
            bestScore = currentScore;
            PlayerPrefs.SetInt(BestScoreKey, bestScore);
        }

        FlappyBirdUIManager.Instance.OpenGameOverPanel(bestScore, currentScore);
    }

    public void AddScore(int score)
    {
        currentScore += score;
        OnScoreChanged?.Invoke(currentScore);
    }

    private void OnDestroy()
    {
        OnScoreChanged = null;
        Instance = null;
    }
}