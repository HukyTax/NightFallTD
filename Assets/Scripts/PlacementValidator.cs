using UnityEngine;

// Determines whether a given world position is a legal tower placement spot.
// Uses a Physics2D circle overlap against a configurable layer mask — any layer
// you mark as "blocked" (path tiles, existing towers, UI, etc.) will reject placement.
public class PlacementValidator : MonoBehaviour
{
    // Assign all layers that should block tower placement (e.g. Path, TowerLayer).
    public LayerMask blockedLayers;

    void Start() { /* no initialization needed */ }
    void Update() { /* validation is on-demand via isValid(), not per-frame */ }

    // Returns true if the position is free to build on.
    // The 0.5f radius keeps the check tight to the tower footprint without
    // needing a collider on the ghost itself.
    public bool isValid(Vector2 position)
    {
        Collider2D collider2D = Physics2D.OverlapCircle(position, 0.5f, blockedLayers);
        if (collider2D == null)
        {
            return true;
        }
        return false;
    }
}
