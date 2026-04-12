using UnityEngine;

public class DatabaseManager : MonoBehaviour
{
    public static DatabaseManager Instance { get; private set; }

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

    public void SaveScore(string playerName, int score, float time)
    {
        Debug.Log("Saving score: " + playerName + " " + score + " " + time);
    }
}