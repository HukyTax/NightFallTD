using System;
using UnityEngine;
using UnityEngine.UI;

// Attached to each tower button in the shop panel.
// Manages the drag-to-place flow: when the player presses the button (BeginDrag),
// a ghost preview spawns and follows the cursor. On left-click it places the real tower
// if the position is valid and the player can afford it. Right-click cancels.
public class TowerDragHandler : MonoBehaviour
{
    private static GameObject activeGhost;
    private static TowerDragHandler activeDragger;
    // The real tower prefab that gets placed when the player confirms a spot.
    public GameObject tower;
    // Checks whether a given world position is a legal place to build.
    public PlacementValidator validator;
    // The ghost preview instance that follows the cursor during a drag.
    private GameObject ghostTower;
    public bool isDragging = false;
    // Gold cost of this tower type, set per-button in the Inspector.
    [SerializeField] int price;
    public Economy economy;
    GameObject LevelManger;
    private LevelManager levelManager;
    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void BeginDrag()
    {
        // Find and cache LevelManager each time — BeginDrag can fire before Start on some buttons.
        GameObject LevelManager = GameObject.Find("LevelManager");
        levelManager = LevelManager.GetComponentInChildren<LevelManager>();
        Debug.Log("cost: " + price);
        // If another ghost is active, reset the shared flag before setting it again.
        if(activeDragger != this && activeDragger != null)
        {
            activeDragger.isDragging = false;
        }
        if(activeGhost != null)
        {
             Destroy(activeGhost);
        }
            ghostTower = Instantiate(tower);
            activeDragger = this;
            isDragging = true;
            activeGhost = ghostTower;
        // Disable the collider and turret script so the ghost doesn't fight enemies
        // or physically block placement checks.
        ghostTower.GetComponentInChildren<Collider2D>().enabled = false;
        ghostTower.GetComponentInChildren<Turret>().enabled = false;
        // Initial ghost tint: blue = "dragging, not yet placed".
    }
    void Start()
    {
        economy = GameObject.Find("LevelManager").GetComponentInChildren<Economy>();
    }
    public void ChangeColor(float r, float g, float b)
    {
        SpriteRenderer[] renderers = ghostTower.GetComponentsInChildren<SpriteRenderer>();
        foreach (SpriteRenderer sr in renderers)
                {
                    sr.material = new Material(Shader.Find("Sprites/Default"));
                    sr.color = new Color(r, g, b, 0.5f);
                }
    }

    // Update is called once per frame
    void Update()
    {
        if (isDragging)
        {
            // Convert mouse screen position to world space each frame.
            Vector2 position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            ghostTower.transform.position = position;

            // Green tint = valid placement spot.
            if (validator.isValid(position)){
                ChangeColor(0f,1f,0f);
            }
            else
            {
                // Dark red tint = invalid spot (path, existing tower, blocked layer).
                ChangeColor(0.5f,0f,0f);
            }
            // Orange tint overrides green — valid spot but player can't afford it.
            if(economy.getMoney() < price)
            {
                ChangeColor(1f,0.6f,0f);
            }


            // Right-click cancels the drag without placing anything.
            if (Input.GetMouseButtonDown(1))
            {
                Destroy(ghostTower);
                isDragging = false;
            }
            // Left-click attempts placement if position and funds are both valid.
            if (Input.GetMouseButtonDown(0))
            {
                if (economy.getMoney() >= price)
                {
                    if (validator.isValid(position))
                    {
                        // ChangeMoney takes a new absolute value, so subtract price manually.
                        economy.ChangeMoney(economy.getMoney() - price);
                        Instantiate(tower, new Vector3(position.x, position.y, 0), Quaternion.identity);
                        Destroy(ghostTower);
                        isDragging = false;
                    }
                    else
                    {
                        Debug.Log("This Position is not valid");
                    }
                }
                else
                {
                    Debug.Log("Not enough money. Price: " + price);
                }
            }
        }
    }
}

