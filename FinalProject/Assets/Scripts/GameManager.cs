using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public event Action<int> onHealthChanged;
    public event Action<int> onScoreChanged;
    public event Action<string> onWinnerDeclared;

    private int localHealth = 100;
    private int localScore = 0;
    private string winnerName = "";

    public int LocalHealth => localHealth;
    public int LocalScore => localScore;
    public string WinnerName => winnerName;

    private int currentLevel = 1;
    public int CurrentLevel => currentLevel;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void SetLocalHealth(int value)
    {
        localHealth = value;
        onHealthChanged?.Invoke(localHealth);
    }

    public void AddLocalScore(int amount)
    {
        localScore += amount;
        onScoreChanged?.Invoke(localScore);
    }

    public void SetWinner(string value)
    {
        winnerName = value;
        onWinnerDeclared?.Invoke(winnerName);
    }

    public void ResetMatchData()
    {
        localHealth = 100;
        localScore = 0;
        winnerName = "";

        onHealthChanged?.Invoke(localHealth);
        onScoreChanged?.Invoke(localScore);
    }

    public void SetCurrentLevel(int level)
    {
        currentLevel = level;
    }

    public void AdvanceToNextLevel()
    {
        currentLevel++;
    }
}