# Stolen in Style

## Project information

This repository is a Unity game project named **Stolen in Style**. It is currently an early project scaffold; do not assume a genre, gameplay design, target platform, or networking requirement from its name or installed packages.

- Unity Editor: **6000.0.51f1**, as recorded in `ProjectSettings/ProjectVersion.txt`.
- Rendering: Universal Render Pipeline (**17.0.4**), with PC and mobile renderer settings in `Assets/Settings/`.
- Input: Unity Input System (**1.14.0**); actions live in `Assets/InputSystem_Actions.inputactions`.
- Other installed packages include AI Navigation, Timeline, uGUI, Visual Scripting, and Unity Test Framework. Installation does not imply that a feature is used.
- Current scene: `Assets/Scenes/SampleScene.unity`.
- Gameplay code: `Assets/Scripts/`; `GameInitializer.cs` is currently an empty MonoBehaviour scaffold.
- `Assets/TutorialInfo/` and `Assets/Readme.asset` are Unity template content.
- Read `Packages/manifest.json` and relevant project settings for the current configuration; update this overview when intentional project changes make it inaccurate.

## Repository boundaries

- **Do not modify anything outside of this repository's directory.** This includes global skills, user settings, other projects, external caches, and tools. Keep any generated reports, temporary files, downloaded assets, and build outputs inside this repository.
- Resolve write destinations before use. Do not follow a symlink or junction to write outside the repository, or run a command whose side effects violate this boundary.
- Read-only access to installed tools and documentation is allowed. If a required tool writes outside this repository, explain the conflict and obtain an explicit exception before running it; do not silently relax this rule.
- Do not install global tools, upgrade Unity or packages, publish builds, or change remote services unless the user requests it.
- Preserve existing user changes. Inspect the working tree before editing; do not reset, clean, overwrite unrelated files, or discard work to make a check pass.

## Unity development rules

- Prefer small changes consistent with existing code. Avoid speculative frameworks or dependencies.
- Keep runtime code in `Assets/Scripts/` and editor-only code under an `Editor/` directory or an editor-only assembly.
- Preserve Unity `.meta` files and GUIDs. Move or rename an asset together with its `.meta`; do not regenerate metadata for existing assets.
- Preserve serialized references. Renaming serialized fields or changing types requires a migration strategy, such as `FormerlySerializedAs` for a compatible field rename.
- Prefer Unity Editor APIs for complex scene, prefab, and importer edits. For targeted text edits, inspect the serialized structure first and preserve file IDs, GUIDs, and prefab overrides.
- Use the project's Input System actions for new controls. Inspect existing bindings and player settings before changing input behavior.
- Match the installed URP version when changing materials, shaders, or renderer features.
- Do not hand-edit generated `Library/`, `Temp/`, `obj/`, IDE project files, or package caches. Do not commit generated logs, builds, or machine-specific settings.
- Do not infer platform support or performance targets from the presence of both PC and mobile pipeline assets.
- Use atomic functions and add meaningful names to each function to make the code more readable.

## Validation and handoff

- Validate in the pinned Unity version when available. Do not treat a standalone C# build as proof that Unity serialization or scene behavior works.
- Use focused Edit Mode tests for logic and Play Mode tests for lifecycle, physics, and scene integration when those checks materially help. Avoid adding tests that only mirror trivial implementation details.
- Do not launch a second Unity process against a project already open in the Editor. Check first; use the active Editor or report the checks that require manual execution.
- Before invoking Unity, account for its logs and cache side effects under the repository boundary above. Report any checks blocked by that boundary or unavailable tooling.
- In the final handoff, state what changed, checks actually run, and remaining manual setup or validation. Do not claim compilation, playtesting, or profiling without evidence.

## Project skills

Project-local skills live in `.agents/skills/`. Read the relevant `SKILL.md` before applying a workflow; do not load unrelated skills.

- `unity-gameplay`: C# gameplay, input, component lifecycle, and serialized configuration.
- `unity-assets-scenes`: Scenes, prefabs, materials, imports, and Unity asset references.
- `unity-validation`: Debugging, Edit Mode and Play Mode tests, and build verification.
- `unity-performance`: Profiling and measured CPU, memory, physics, and URP improvements.

These skills belong to this repository. Do not copy them to a global skill directory as part of project setup.
