#if (UNITY_STANDALONE_WIN || UNITY_STANDALONE_LINUX || UNITY_STANDALONE_OSX) && !DISABLESTEAMWORKS
#define STEAM_CONNECTION_SUPPORTED
#endif

using System;
using UnityEngine;

#if STEAM_CONNECTION_SUPPORTED
using Steamworks;
#endif

[DisallowMultipleComponent]
public sealed class SteamConnection : MonoBehaviour
{
    [SerializeField]
    [Tooltip("The game's Steam App ID, not the publisher's Partner ID.")]
    private uint appId;

    private bool hasStarted;
    private bool isInitialized;

    // Accessing the owner never creates or initializes a service.
    public static SteamConnection Instance { get; private set; }

    public bool IsInitialized => isInitialized;
    public bool RestartRequested { get; private set; }
    public string LastError { get; private set; } = string.Empty;
    public ulong SteamId { get; private set; }
    public string PlayerName { get; private set; } = string.Empty;

    // Clear bookkeeping for a new Play Mode session; startup stays in GameInitializer.
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    private static void ResetSession()
    {
        Instance = null;
    }

    private void OnEnable()
    {
        if (hasStarted && Instance == null)
        {
            Instance = this;
        }

#if UNITY_EDITOR
        UnityEditor.AssemblyReloadEvents.beforeAssemblyReload += ShutdownBeforeAssemblyReload;
#endif
    }

    private void OnDisable()
    {
#if UNITY_EDITOR
        UnityEditor.AssemblyReloadEvents.beforeAssemblyReload -= ShutdownBeforeAssemblyReload;
#endif
    }

#if UNITY_EDITOR
    private void ShutdownBeforeAssemblyReload()
    {
        if (Instance != this || !isInitialized)
        {
            return;
        }

        Shutdown();
        LastError = "Steam stopped before script recompilation. Restart Play Mode to reconnect through GameInitializer.";
        Debug.LogWarning(LastError, this);
    }
#endif

    public bool Initialize()
    {
        if (Instance != null && Instance != this)
        {
            Debug.LogError("A SteamConnection already exists. Reuse it instead of initializing another.", this);
            Destroy(gameObject);
            return false;
        }

        Instance = this;
        hasStarted = true;
        DontDestroyOnLoad(gameObject);

        if (isInitialized || RestartRequested)
        {
            return isInitialized;
        }

        LastError = string.Empty;

        if (appId == 0)
        {
            return FailInitialization("Set the game's Steam App ID on the SteamConnection prefab.");
        }

#if STEAM_CONNECTION_SUPPORTED
        try
        {
#if !UNITY_EDITOR
            if (SteamAPI.RestartAppIfNecessary(new AppId_t(appId)))
            {
                RestartRequested = true;
                Debug.Log("Restarting the game through Steam.", this);
                Application.Quit();
                return false;
            }
#endif

            ESteamAPIInitResult result = SteamAPI.InitEx(out string error);

            if (result != ESteamAPIInitResult.k_ESteamAPIInitResult_OK)
            {
                return FailInitialization(
                    $"Steam initialization failed ({result}): {error}. " +
                    "Check that Steam is running, your account owns the app, and local testing has steam_appid.txt.");
            }

            isInitialized = true;
            uint activeAppId = SteamUtils.GetAppID().m_AppId;

            if (activeAppId != appId)
            {
                return FailInitialization($"Steam initialized App ID {activeAppId}, but this game expects {appId}.");
            }

            SteamId = SteamUser.GetSteamID().m_SteamID;
            PlayerName = SteamFriends.GetPersonaName();
            Debug.Log($"Steam initialized successfully for App ID {appId}.", this);
            return true;
        }
        catch (Exception exception) when (
            exception is DllNotFoundException ||
            exception is EntryPointNotFoundException ||
            exception is BadImageFormatException)
        {
            return FailInitialization(
                $"Could not load the Steamworks native library: {exception.Message}. " +
                "Check the Steamworks.NET installation and the build architecture.");
        }
#else
        return FailInitialization("Steam integration is disabled or unavailable on this build target.");
#endif
    }

    private void Update()
    {
#if STEAM_CONNECTION_SUPPORTED
        if (isInitialized)
        {
            SteamAPI.RunCallbacks();
        }
#endif
    }

    private bool FailInitialization(string message)
    {
        Shutdown();
        LastError = message;
        Debug.LogWarning(message, this);
        return false;
    }

    private void OnDestroy()
    {
        if (Instance != this)
        {
            return;
        }

        Shutdown();
        Instance = null;
    }

    private void Shutdown()
    {
#if STEAM_CONNECTION_SUPPORTED
        if (isInitialized)
        {
            isInitialized = false;
            SteamAPI.Shutdown();
        }
#endif

        SteamId = 0;
        PlayerName = string.Empty;
    }
}
