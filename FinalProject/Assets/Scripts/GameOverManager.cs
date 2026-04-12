using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using Unity.Netcode;

public class GameOverManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI winnerText;
    [SerializeField] private TMP_InputField playerNameInput;

    void Start()
    {
        if (GameManager.Instance != null)
        {
            winnerText.text = GameManager.Instance.WinnerName;
        }
    }

    public void ReturnToMenu()
    {
        string playerName = "Player";

        if (playerNameInput != null && !string.IsNullOrWhiteSpace(playerNameInput.text))
        {
            playerName = playerNameInput.text;
        }

        if (DatabaseManager.Instance != null && GameManager.Instance != null)
        {
            DatabaseManager.Instance.SaveScore(playerName, GameManager.Instance.LocalScore, Time.time);
        }

        if (NetworkManager.Singleton != null)
        {
            NetworkManager.Singleton.Shutdown();
        }

        if (GameManager.Instance != null)
        {
            GameManager.Instance.ResetMatchData();
        }

        SceneManager.LoadScene("MainMenu");
    }
}