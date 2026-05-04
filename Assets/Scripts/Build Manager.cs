using UnityEngine;

// Registry for all buildable tower prefabs. Accessed via BuildManager.main.
// Currently scaffolded — selectedTower is always 0 because there's no UI
// to change the selection yet. Expand this when you add a tower selection system.
public class BuildManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    // Singleton so any script can call BuildManager.main.GetTower() without a direct reference.
    public static BuildManager main;
    // All tower prefabs available to place, ordered to match shop slot indices.
    [SerializeField] private GameObject[] buildingPreFabs;

    // Index of the currently selected tower. Always 0 until selection UI is added.
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




    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
