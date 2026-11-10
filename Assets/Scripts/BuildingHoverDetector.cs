using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

[DisallowMultipleComponent]
public sealed class BuildingHoverDetector : MonoBehaviour
{
    [SerializeField] private Camera worldCamera;
    [Tooltip("Assign InputSystem_Actions. Uses the existing UI/Point action.")]
    [SerializeField] private InputActionAsset inputActions;
    [SerializeField] private LayerMask buildingLayers = Physics2D.DefaultRaycastLayers;
    [SerializeField, Min(0.01f)] private float maxDistance = 1000f;

    private readonly List<RaycastHit2D> hits = new List<RaycastHit2D>(16);
    private InputAction pointerPosition;

    public Building HoveredBuilding { get; private set; }

    private void Awake()
    {
        if (worldCamera == null)
        {
            worldCamera = Camera.main;
        }

        InputAction source = inputActions != null ? inputActions.FindAction("UI/Point") : null;
        if (worldCamera == null || source == null)
        {
            Debug.LogError("BuildingHoverDetector needs a camera and InputSystem_Actions with UI/Point.", this);
            enabled = false;
            return;
        }

        // A separate instance lets this component own input without disabling UI input.
        pointerPosition = source.Clone();
        pointerPosition.wantsInitialStateCheck = true;
    }

    private void OnEnable()
    {
        pointerPosition?.Enable();
    }

    private void LateUpdate()
    {
        ChangeBuilding(FindHoveredBuilding());
    }

    private Building FindHoveredBuilding()
    {
        if (!Application.isPlaying || !Application.isFocused || pointerPosition == null ||
            pointerPosition.activeControl == null || worldCamera == null || !worldCamera.isActiveAndEnabled)
        {
            return null;
        }

        if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
        {
            return null;
        }

        Vector2 position = pointerPosition.ReadValue<Vector2>();
        Rect cameraRect = worldCamera.pixelRect;
        if (float.IsNaN(position.x) || float.IsNaN(position.y) ||
            float.IsInfinity(position.x) || float.IsInfinity(position.y) ||
            cameraRect.width <= 0f || cameraRect.height <= 0f || !cameraRect.Contains(position))
        {
            return null;
        }

        Ray ray = worldCamera.ScreenPointToRay(position);
        int visibleLayers = buildingLayers.value & worldCamera.cullingMask;
        int count = Physics2D.GetRayIntersection(ray, maxDistance, hits, visibleLayers);
        Building selected = null;
        float selectedDistance = float.PositiveInfinity;
        for (int i = 0; i < count; i++)
        {
            Building candidate = hits[i].collider.GetComponent<Building>();
            if (candidate == null || !candidate.CanHover)
            {
                continue;
            }

            if (selected == null || IsInFront(candidate.Visual, selected.Visual, hits[i].distance, selectedDistance))
            {
                selected = candidate;
                selectedDistance = hits[i].distance;
            }
        }
        return selected;
    }

    private static bool IsInFront(SpriteRenderer candidate, SpriteRenderer selected, float distance, float selectedDistance)
    {
        int layer = SortingLayer.GetLayerValueFromID(candidate.sortingLayerID);
        int selectedLayer = SortingLayer.GetLayerValueFromID(selected.sortingLayerID);
        if (layer != selectedLayer) return layer > selectedLayer;
        if (candidate.sortingOrder != selected.sortingOrder) return candidate.sortingOrder > selected.sortingOrder;
        return distance < selectedDistance;
    }

    private void ChangeBuilding(Building building)
    {
        if (HoveredBuilding == building)
        {
            // A building can be disabled and re-enabled while the pointer stays still.
            if (building != null && !building.IsHovered) building.SetHovered(true);
            return;
        }

        if (HoveredBuilding != null) HoveredBuilding.SetHovered(false);
        HoveredBuilding = building;
        if (HoveredBuilding != null) HoveredBuilding.SetHovered(true);
    }

    private void OnApplicationFocus(bool hasFocus)
    {
        if (!hasFocus) ChangeBuilding(null);
    }

    private void OnDisable()
    {
        pointerPosition?.Disable();
        ChangeBuilding(null);
    }

    private void OnDestroy()
    {
        pointerPosition?.Dispose();
    }
}
