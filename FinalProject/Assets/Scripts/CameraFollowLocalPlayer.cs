using UnityEngine;
using Unity.Netcode;

public class CameraFollowLocalPlayer : MonoBehaviour
{
    void LateUpdate()
    {
        PlayerNetworkController[] players = FindObjectsByType<PlayerNetworkController>(FindObjectsSortMode.None);

        foreach (PlayerNetworkController player in players)
        {
            if (player.IsOwner)
            {
                Vector3 playerPos = player.transform.position;
                transform.position = new Vector3(playerPos.x, playerPos.y, -10f);
                return;
            }
        }
    }
}