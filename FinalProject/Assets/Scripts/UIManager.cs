using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private TextMeshProUGUI scoreText;

    void Start()
    {
        if (GameManager.Instance != null)
        {
            UpdateHealth(GameManager.Instance.LocalHealth);
            UpdateScore(GameManager.Instance.LocalScore);
        }
    }

    void OnEnable()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.onHealthChanged += UpdateHealth;
            GameManager.Instance.onScoreChanged += UpdateScore;
        }
    }

    void OnDisable()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.onHealthChanged -= UpdateHealth;
            GameManager.Instance.onScoreChanged -= UpdateScore;
        }
    }

    void UpdateHealth(int newHealth)
    {
        healthText.text = "Health: " + newHealth;
    }

    void UpdateScore(int newScore)
    {
        scoreText.text = "Wins: " + newScore;
    }
}