using UnityEngine;

public class PersistentNetworkManager : MonoBehaviour
{
    private static PersistentNetworkManager instance;

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }
}