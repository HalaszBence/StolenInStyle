---
name: unity-assets-scenes
description: Modify Unity scenes, prefabs, materials, import settings, and asset references while preserving serialization and GUIDs.
---

# Unity assets and scenes

Read root `AGENTS.md`. Inspect the target asset, its `.meta`, and its usages before changing it. Determine whether a scene object is a prefab instance, a variant, or a standalone object.

- Prefer available Unity Editor tooling for structural changes. Do not assume a Unity integration is installed. If only text editing is available, limit edits to understood serialized structures and report the need for Editor validation.
- Preserve GUIDs and local file IDs. Move assets with their metadata; do not copy an existing `.meta` to create a distinct asset. For new assets, use Unity-generated metadata when possible; if metadata must be authored, use unique GUIDs and the correct importer structure.
- Make reusable changes in the intended prefab source and instance-specific changes as overrides. Avoid applying unrelated overrides or unpacking instances merely to simplify an edit.
- Resolve script, material, texture, and prefab references before writing serialized links. A GUID without the correct object file ID is not a validated reference.
- Inspect the active quality and render pipeline settings before modifying URP materials or renderer assets. Preserve the distinction between PC and mobile assets unless the requested change applies to both.
- Set texture, audio, and model import options according to actual use and target constraints. Avoid blanket reimports, mass reserialization, or replacing source art without a task-specific reason.

Review the diff for unexpected scene churn. When Editor validation is possible, check missing scripts, broken references, prefab overrides, and the affected scene in Play Mode. Otherwise identify those checks explicitly in the handoff.
