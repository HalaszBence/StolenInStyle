using UnityEngine;

[DefaultExecutionOrder(-1000)]
[DisallowMultipleComponent]
public class GameInitializer : MonoBehaviour
{
    [SerializeField] private SteamConnection steamConnectionPrefab;

    public SteamConnection Steam { get; private set; }

    private void Awake()
    {
        InitializeSteamConnection();
    }

    private void InitializeSteamConnection()
    {
        Steam = SteamConnection.Instance;

        if (Steam != null)
        {
            return;
        }

        if (steamConnectionPrefab == null)
        {
            Debug.LogError("Assign a SteamConnection prefab to GameInitializer.", this);
            return;
        }

        Steam = Instantiate(steamConnectionPrefab);
        Steam.name = steamConnectionPrefab.name;
        Steam.Initialize();
    }
}
