using UnityEngine;

// Registry for all buildable tower prefabs. Accessed via BuildManager.main.
// Currently the selected tower index is always 0 — this class is scaffolded
// for when a multi-tower selection UI is added later.
public class BuildManager : MonoBehaviour
{
    // Singleton reference so any script can call BuildManager.main.GetTower().
    public static BuildManager main;

    // All tower prefabs available to place, ordered by shop slot.
    [SerializeField] private GameObject[] buildingPreFabs;

    // Index of the tower the player currently has selected in the shop.
    private int sellectedTower = 0;

    // Returns the prefab for the currently selected tower type.
    public GameObject GetTower()
    {
        return buildingPreFabs[sellectedTower];
    }

    private void Awake()
    {
        main = this;
    }

    void Start() { }
    void Update() { }
}
