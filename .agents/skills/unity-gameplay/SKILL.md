---
name: unity-gameplay
description: Implement or change C# gameplay, input, and component behavior in the Stolen in Style Unity project.
---

# Unity gameplay

Read root `AGENTS.md`, the affected scripts, and their scene or prefab usage before editing. Resolve the current Unity version and packages from project files.

- Find the owner of the behavior and its serialized configuration before creating new managers or singletons. Start with the smallest component design that satisfies the feature.
- Inspect `Assets/InputSystem_Actions.inputactions` and existing input usage. Reuse appropriate actions; document any new action, binding, or Inspector assignment needed for the feature.
- Choose lifecycle methods deliberately: establish local references in `Awake`, subscribe and unsubscribe symmetrically in `OnEnable` and `OnDisable`, and avoid depending on unspecified initialization order across objects.
- Match work to Unity timing: frame input and presentation in `Update`, physics changes in `FixedUpdate` when appropriate. Define pause and scaled versus unscaled time behavior where it affects the feature.
- Keep tunable values serialized with sensible defaults. Preserve existing serialized data during field changes and avoid public mutable state solely to expose Inspector settings.
- Handle destruction, scene changes, and disabled components in asynchronous work or callbacks. Do not leave static subscriptions or references holding scene objects alive.
- Keep editor dependencies out of runtime assemblies. Avoid repeated scene searches and allocations in per-frame paths when a cached reference is suitable.

Verify the requested behavior and relevant transitions, such as disabling and re-enabling the component or reloading the scene. Report any scene wiring and checks that could not be performed; do not claim that writing a script made the feature playable.
