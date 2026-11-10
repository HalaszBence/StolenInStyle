using UnityEngine;

[DisallowMultipleComponent]
[RequireComponent(typeof(SpriteRenderer), typeof(PolygonCollider2D))]
public sealed class Building : MonoBehaviour
{
    [SerializeField] private string buildingName;
    [SerializeField, TextArea] private string description;
    [SerializeField] private Color highlightTint = new Color(0.6f, 1f, 0.6f, 1f);

    private SpriteRenderer spriteRenderer;
    private PolygonCollider2D hoverCollider;
    private Color originalColor;

    public string BuildingName => string.IsNullOrWhiteSpace(buildingName) ? name : buildingName;
    public string Description => description;
    public bool IsHovered { get; private set; }
    public SpriteRenderer Visual => spriteRenderer;
    public bool CanHover => isActiveAndEnabled && spriteRenderer != null &&
        spriteRenderer.enabled && !spriteRenderer.forceRenderingOff && spriteRenderer.sprite != null &&
        hoverCollider != null && hoverCollider.enabled;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        hoverCollider = GetComponent<PolygonCollider2D>();
    }

    public void SetHovered(bool hovered)
    {
        hovered = hovered && CanHover;
        if (IsHovered == hovered)
        {
            return;
        }

        IsHovered = hovered;
        if (hovered)
        {
            originalColor = spriteRenderer.color;
            spriteRenderer.color = originalColor * highlightTint;
            Debug.Log($"Hovering building: {BuildingName}", this);
        }
        else if (spriteRenderer != null)
        {
            spriteRenderer.color = originalColor;
        }
    }

    private void OnDisable()
    {
        SetHovered(false);
    }
}
