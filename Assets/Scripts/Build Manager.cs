using UnityEngine;

public class BuildManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public static BuildManager main;
    [SerializeField] private GameObject[] buildingPreFabs;

    private int sellectedTower = 0;

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
