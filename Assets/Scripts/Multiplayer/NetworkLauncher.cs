using UnityEngine;
using Unity.Netcode;
using UnityEngine.SceneManagement;

public class NetworkLauncher : MonoBehaviour
{
    [Header("Scene Settings")]
    [Tooltip("Name of the game scene to load after starting as host.")]
    [SerializeField] private string gameSceneName = "GameScene";

    private void Awake()
    {
        if (NetworkManager.Singleton == null)
            DontDestroyOnLoad(gameObject);
        else
            Destroy(gameObject);
    }

    public void StartHost()
    {
        if (NetworkManager.Singleton == null)
        {
            Debug.LogError("NetworkManager is not set up in the scene!");
            return;
        }

        NetworkManager.Singleton.StartHost();
        Debug.Log("Started Host");

        if (!string.IsNullOrEmpty(gameSceneName))
        {
            NetworkManager.Singleton.SceneManager.LoadScene(gameSceneName, LoadSceneMode.Single);
        }
        else
        {
            Debug.LogWarning("Game scene name is not set. Please assign it in the inspector.");
        }
    }

    public void StartClient()
    {
        if (NetworkManager.Singleton == null)
        {
            Debug.LogError("NetworkManager is not set up in the scene!");
            return;
        }

        NetworkManager.Singleton.StartClient();
        Debug.Log("Started Client");
    }
}
