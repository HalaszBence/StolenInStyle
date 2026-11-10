# GameObject buildings

Each building is an ordinary scene GameObject with a SpriteRenderer, PolygonCollider2D, and Building component. Its name, description, and highlight tint live directly on that component. Moving or duplicating a building uses Unity's usual GameObject workflow. Roads and pavement can remain tilemaps.

## Setup

1. Drag the original building sprite from the Project window into the scene to create a SpriteRenderer GameObject. Erase its old artwork tile from the building tilemap so it is displayed once.
2. Add **Building**. Unity adds a PolygonCollider2D if needed. Assign the artwork sprite before adding the collider so Unity can generate its initial shape. Use **Edit Collider** to adjust the polygon to the desired hover outline. Keep SpriteRenderer, Building, and PolygonCollider2D on the same GameObject.
3. Enter Building Name and Description on the Building component and choose Highlight Tint. Each GameObject stores its own data. A blank Building Name falls back to the GameObject name.
4. Keep one **BuildingHoverDetector** in the scene. Assign World Camera and Assets/InputSystem_Actions.inputactions. Building Layers defaults to normal raycast layers; an optional dedicated Buildings layer can restrict the query. Include the building GameObjects' layer and ensure the camera renders it.
5. No Rigidbody2D is required. A normal collider works with the default settings. Trigger colliders are detected only if Physics2D Queries Hit Triggers is enabled. Set up gameplay collision layers separately if you need hover colliders excluded from movement collisions.

The detector casts through the pointer and uses the actual polygon, including gaps outside the building outline. It ignores colliders without Building components. For overlapping buildings, higher SpriteRenderer Sorting Layer/Order wins, then the closest hit. This assumes one SpriteRenderer on each building and no nested SortingGroups or custom sorting rules.

Hover entry multiplies the SpriteRenderer's original color by Highlight Tint and logs the building name once. Exit, focus loss, or disabling the detector/building restores the original color. Hover remains active while timeScale is zero. UI blocks mouse world-hover; no tilemap lookup, ID painting, per-cell synchronization, or collider bounds guessing is used. The physics result list reuses its allocated capacity.

Other gameplay/UI code can read detector.HoveredBuilding and that component's BuildingName, Description, and IsHovered properties. Add future per-building fields directly to Building. Authored values are serialized with the GameObject in its scene/prefab. Runtime save/load can be implemented separately when its requirements are known.

## Replacing the previous system

Remove the old BuildingHoverTarget (or Missing Script) component from the scene's BuildingHover object. Keep its BuildingHoverDetector and recheck camera/input assignments. Disable or delete BuildingIdsTilemap and erase the replaced building artwork tiles. New code does not use either building tilemap.

The old BuildingData, BuildingHoverTarget, BuildingSpriteGeometry, BuildingIdTile, custom Inspector, and validation-menu scripts have been removed. Existing ID palette/marker assets were preserved; the old Apartment ID tile was converted to an ordinary Unity Tile so deleting its script does not leave a missing-script asset. These old ID painting assets are unnecessary for the new system.

## Play Mode checks

- Move inside/outside the polygon and around its transparent corners. Check one log on entry, whole-sprite tint, and restoration on exit.
- Hover two buildings sharing the same sprite. Check only one changes color and each displays its own name.
- Check a pre-tinted sprite, overlapping buildings with different sorting orders, UI blocking, focus loss, and disabling/re-enabling either component.
- Stop Play Mode and confirm normal colors return. Check the renderer material supports sprite/vertex colors.

The saved SampleScene still has no city/hover objects in this repository snapshot. Add these components to your current scene and validate it in Unity 6000.0.51f1; standalone reference compilation does not verify scene behavior.
