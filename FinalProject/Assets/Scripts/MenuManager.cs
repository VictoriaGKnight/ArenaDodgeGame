using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using TMPro;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private TMP_InputField ipInputField;

    public void StartHost()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.ResetMatchData();
        }

        if (NetworkManager.Singleton.StartHost())
        {
            NetworkManager.Singleton.SceneManager.LoadScene("GameScene", LoadSceneMode.Single);
        }
    }

    public void StartClient()
    {
        if (!string.IsNullOrWhiteSpace(ipInputField.text))
        {
            UnityTransport transport = NetworkManager.Singleton.GetComponent<UnityTransport>();
            transport.ConnectionData.Address = ipInputField.text;
        }

        NetworkManager.Singleton.StartClient();
    }

    public void LoadHighScores()
    {
        SceneManager.LoadScene("HighScores");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}