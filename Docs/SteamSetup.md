# Steam connection

The game uses Steamworks.NET 2025.165.0, pinned in `Packages/manifest.json`.
Unity Package Manager resolves the package when the project is opened. Git must
be available for that dependency. Do not also import another Steamworks wrapper
or the sample SteamManager, since this project owns initialization explicitly.

## Startup and configuration

`GameInitializer` instantiates the `SteamConnection` prefab and calls `Initialize()` early via `DefaultExecutionOrder(-1000)`. The sample camera, light, and volume are ordinary saved objects in `SampleScene`, editable before Play Mode. Scene content is no longer spawned by the initializer.

The App ID is **5586670**, supplied by the project owner. Change `appId` on
`Assets/Prefabs/SteamConnection.prefab` if that ID is corrected. The Partner ID,
product activation keys, and Web API keys are not used for this connection.

The connection persists across scene loads. Subsequent initializers reuse
`SteamConnection.Instance`; that getter never creates objects. The service pumps
Steam callbacks while initialized and shuts down when its owning object is
destroyed. `IsInitialized`, `LastError`, `SteamId`, and `PlayerName` expose its
startup result. Initialization does not imply that Steam is online, and this
does not yet implement achievements, lobbies, or multiplayer networking.

If Steam initialization fails, the error is logged and scene content still
loads, allowing local development. Steam-specific features must check
`IsInitialized` before using the API. A standalone game requesting a Steam relaunch calls `Application.Quit()`; already active scene objects may run their lifecycle before the process exits. The Editor never requests that relaunch. Unsupported platforms skip native Steam calls and log a warning.

## Local verification

1. Allow Unity to resolve Steamworks.NET and confirm that the Console has no
   compilation or import errors.
2. Run Steam and sign into an account with a developer license for this App ID.
3. For Editor testing, put `steam_appid.txt` in the repository root containing
   only `5586670`. That local file is created by this setup and ignored by Git;
   recreate it on a fresh clone. For a standalone build launched outside Steam,
   place it in the executable's working directory, normally beside the `.exe`.
4. Open `Assets/Scenes/SampleScene.unity` and enter Play Mode. Confirm that the
   camera, light, and volume are visible before Play Mode and are not duplicated at runtime,
   and the Console reports successful Steam initialization for the expected App
   ID. Player identity is available through the service properties, not logged.
5. Stop and re-enter Play Mode to verify cleanup. Test a scene reload to confirm
   the existing Steam service is reused and callbacks are not pumped twice.
   If scripts are recompiled during Play Mode, the service shuts down before
   assembly reload. Restart Play Mode to reconnect through the entry point.
6. Test a build installed and launched through Steam. **Exclude
   `steam_appid.txt` from uploaded depots**; Steam supplies the app context.

If initialization fails, check the logged native error, the App ID, developer
package ownership, Steam client availability, and native library architecture.
An App ID mismatch is rejected and the incorrectly initialized API is shut down.

Builds and logs produced by agent tools must stay inside this repository.
Do not launch a second Editor against an already open project. Editor execution
and a live Steam connection remain separate validation steps from static checks.

Sources: [Steamworks.NET installation](https://steamworks.github.io/installation/),
[pinned release](https://github.com/rlabrecque/Steamworks.NET/releases/tag/2025.165.0),
[Valve API lifecycle](https://partner.steamgames.com/doc/sdk/api).
