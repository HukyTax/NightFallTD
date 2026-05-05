using System;
using System.ComponentModel.Design;
using UnityEngine;

<<<<<<< Updated upstream
// Central data hub for the level. Singleton accessed via LevelManager.main.
// Holds the enemy path, the spawn point, and a flag that tracks whether
// any tower ghost (drag preview) is currently active in the scene.
=======
// Central scene authority — singleton accessible via LevelManager.main.
// Holds the enemy path waypoints and the spawn start point so any script
// can reference them without needing a direct Inspector link.
// Also owns the active drag handler slot so only one ghost tower can exist at a time.
>>>>>>> Stashed changes
public class LevelManager : MonoBehaviour
{
<<<<<<< Updated upstream

<<<<<<< Updated upstream
    // Singleton — set in Awake so other scripts can access it before their Start().
    public static LevelManager main;

    // Ordered array of waypoints enemies follow. Index 0 = first turn after spawn point.
    public Transform[] path;
    // Where enemies are instantiated at the start of each wave.
=======
=======
    // Singleton reference — set in Awake so it's available before any Start() calls.
>>>>>>> Stashed changes
    public static LevelManager main;

    // Ordered array of waypoints the enemy follows. Index 0 = first turn after spawn.
    public Transform[] path;

    // Where enemies are instantiated at the beginning of each wave.
>>>>>>> Stashed changes
    public Transform startPoint;

    // The TowerDragHandler whose ghost is currently in the scene, or null if none.
    public TowerDragHandler activeDragHandler;

    private void Awake()
    {
        main = this;
    }
<<<<<<< Updated upstream

=======
<<<<<<< Updated upstream
>>>>>>> Stashed changes
    // Start is called once before the first execution of Update after the MonoBehaviour is created
=======
>>>>>>> Stashed changes

    // Registers a new drag, cancelling any ghost that's already active.
    // Calling SetActiveDrag(null) just clears the slot without cancelling anything.
    public void SetActiveDrag(TowerDragHandler handler)
    {
        if (activeDragHandler != null && activeDragHandler != handler)
            activeDragHandler.CancelDrag();

        activeDragHandler = handler;
    }
}
