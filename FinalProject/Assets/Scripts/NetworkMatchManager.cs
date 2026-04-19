using System.Collections;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NetworkMatchManager : NetworkBehaviour
{
    public static NetworkMatchManager Instance { get; private set; }

    private bool matchEnded = false;

    void Awake()
    {
        Instance = this;
    }

    public void HandlePlayerDeath(ulong attackerId, ulong deadPlayerId)
    {
        if (!IsServer || matchEnded) return;

        matchEnded = true;

        string winnerLabel = attackerId == 0 ? "Player 1 Wins!" : "Player 2 Wins!";

        NotifyMatchEndedClientRpc(winnerLabel, attackerId);
        StartCoroutine(LoadNextScene());
    }

    [ClientRpc]
    private void NotifyMatchEndedClientRpc(string winnerLabel, ulong attackerId)
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.SetWinner(winnerLabel);

            if (NetworkManager.Singleton.LocalClientId == attackerId)
            {
                GameManager.Instance.AddLocalScore(1);
            }
        }
    }

    private IEnumerator LoadNextScene()
    {
        yield return new WaitForSeconds(2f);

        string currentSceneName = SceneManager.GetActiveScene().name;

        if (currentSceneName == "GameScene")
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.SetCurrentLevel(2);
            }

            NetworkManager.Singleton.SceneManager.LoadScene("GameScene2", LoadSceneMode.Single);
        }
        else
        {
            NetworkManager.Singleton.SceneManager.LoadScene("GameOver", LoadSceneMode.Single);
        }
    }
}
