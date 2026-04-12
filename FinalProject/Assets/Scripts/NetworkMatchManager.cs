using System.Collections;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NetworkMatchManager : NetworkBehaviour
{
    public static NetworkMatchManager Instance { get; private set; }

    [SerializeField] private Transform[] spawnPoints;

    private bool matchEnded = false;

    void Awake()
    {
        Instance = this;
    }

    public Vector3 GetSpawnPosition(ulong clientId)
    {
        int index = clientId == 0 ? 0 : 1;
        index = Mathf.Clamp(index, 0, spawnPoints.Length - 1);
        return spawnPoints[index].position;
    }

    public void HandlePlayerDeath(ulong attackerId, ulong deadPlayerId)
    {
        if (!IsServer || matchEnded) return;

        matchEnded = true;

        string winnerLabel = attackerId == 0 ? "Player 1 Wins!" : "Player 2 Wins!";

        NotifyMatchEndedClientRpc(winnerLabel, attackerId);
        StartCoroutine(LoadGameOverScene());
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

    private IEnumerator LoadGameOverScene()
    {
        yield return new WaitForSeconds(2f);
        NetworkManager.Singleton.SceneManager.LoadScene("GameOver", LoadSceneMode.Single);
    }
}