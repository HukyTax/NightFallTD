using UnityEngine;


// Central scene authority — singleton accessible via LevelManager.main.
// Holds the enemy path waypoints and the spawn start point so any script
// can reference them without needing a direct Inspector link.
// Also owns the active drag handler slot so only one ghost tower can exist at a time.

public class LevelManager : MonoBehaviour
{
    // Singleton — set in Awake so other scripts can access it before their Start().
    public static LevelManager main;

    // Ordered array of waypoints enemies follow. Index 0 = first turn after spawn point.
    public Transform[] path;

    // Where enemies are instantiated at the start of each wave.
    public Transform startPoint;

    // The TowerDragHandler whose ghost is currently in the scene, or null if none.
    public TowerDragHandler activeDragHandler;

    private void Awake()
    {
        main = this;
    }

    // Registers a new drag, cancelling any ghost that's already active.
    // Calling SetActiveDrag(null) just clears the slot without cancelling anything.
    public void SetActiveDrag(TowerDragHandler handler)
    {
        if (activeDragHandler != null && activeDragHandler != handler)
            activeDragHandler.CancelDrag();

        activeDragHandler = handler;
    }
}
