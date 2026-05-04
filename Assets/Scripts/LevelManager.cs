using System;
using System.ComponentModel.Design;
using UnityEngine;

// Central data hub for the level. Singleton accessed via LevelManager.main.
// Holds the enemy path, the spawn point, and a flag that tracks whether
// any tower ghost (drag preview) is currently active in the scene.
public class LevelManager : MonoBehaviour
{

    // True while a TowerDragHandler has an active ghost in the scene.
    Boolean activeShadow = false;
    // Singleton — set in Awake so other scripts can access it before their Start().
    public static LevelManager main;

    // Ordered array of waypoints enemies follow. Index 0 = first turn after spawn point.
    public Transform[] path;
    // Where enemies are instantiated at the start of each wave.
    public Transform startPoint;

    private void Awake()
    {
        main = this;
    }
    public void UpdateActiveShadow(Boolean value)
    {
        activeShadow = value;
    }
    public Boolean getActiveShadow()
    {
       return activeShadow;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created

}
