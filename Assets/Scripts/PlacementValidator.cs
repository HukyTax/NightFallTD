
using UnityEngine;

// Checks whether a world position is legal to build on.
// Uses a Physics2D circle overlap — if any collider on a blocked layer is found,
// placement is rejected.
public class PlacementValidator : MonoBehaviour
{
    // Assign every layer that should block tower placement in the Inspector
    // (e.g. Path, ExistingTower, UI).
    public LayerMask blockedLayers;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    // Returns true if the position is free to build on.
    // The 0.5f radius matches roughly a tower's footprint without needing a ghost collider.
    public bool isValid(Vector2 position)
    {
        Collider2D collider2D = Physics2D.OverlapCircle(position, 0.5f, blockedLayers);
        if(collider2D == null)
        {
            print("true");
            return true;
        }
        print("false");
        return false;
    }
}
