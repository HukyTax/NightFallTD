using System;
using UnityEngine;

// Attached to each tower button in the shop panel.
// Manages the drag-to-place flow: when the player presses the button (BeginDrag),
// a ghost preview spawns and follows the cursor. On left-click it places the real tower
// if the position is valid and the player can afford it. Right-click cancels.
public class TowerDragHandler : MonoBehaviour
{
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
    private LevelManager levelManager;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void BeginDrag()
    {
        // Find and cache LevelManager each time — BeginDrag can fire before Start on some buttons.
        GameObject LevelManager = GameObject.Find("LevelManager");
        levelManager = LevelManager.GetComponentInChildren<LevelManager>();
        Debug.Log("cost: " + price);
        // If another ghost is active, reset the shared flag before setting it again.
        if(levelManager.getActiveShadow())
        {
            levelManager.UpdateActiveShadow(false);
        }
        levelManager.UpdateActiveShadow(true);
        isDragging = true;
        ghostTower = Instantiate(tower);
        // Disable the collider and turret script so the ghost doesn't fight enemies
        // or physically block placement checks.
        ghostTower.GetComponentInChildren<Collider2D>().enabled = false;
        ghostTower.GetComponentInChildren<Turret>().enabled = false;
        SpriteRenderer[] renderers = ghostTower.GetComponentsInChildren<SpriteRenderer>();
        // Initial ghost tint: blue = "dragging, not yet placed".
        foreach (SpriteRenderer sr in renderers)
        {
            sr.material = new Material(Shader.Find("Sprites/Default"));
            sr.color = new Color(0f, 0.0f, 0.5f, 0.5f);
        }
    }
    void Start()
    {
        economy = GameObject.Find("LevelManager").GetComponentInChildren<Economy>();
    }

    // Update is called once per frame
    void Update()
    {
        // NOTE: this block has an empty body — it currently does nothing.
        if (!levelManager.getActiveShadow()){
        }
        if (isDragging)
        {
            // Convert mouse screen position to world space each frame.
            Vector2 position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            ghostTower.transform.position = position;
            SpriteRenderer[] renderers = ghostTower.GetComponentsInChildren<SpriteRenderer>();

            // Green tint = valid placement spot.
            if (validator.isValid(position)){
                foreach (SpriteRenderer sr in renderers)
                {
                    sr.material = new Material(Shader.Find("Sprites/Default"));
                    sr.color = new Color(0f, 1f, 0f, 0.5f);
                }
            }
            else
            {
                // Dark red tint = invalid spot (path, existing tower, blocked layer).
                foreach (SpriteRenderer sr in renderers)
                {
                    sr.material = new Material(Shader.Find("Sprites/Default"));
                    sr.color = new Color(0.5f, 0f, 0f, 0.5f);
                }
            }
            // Orange tint overrides green — valid spot but player can't afford it.
            if(economy.getMoney() < price)
            {
                foreach (SpriteRenderer sr in renderers)
                {
                    sr.material = new Material(Shader.Find("Sprites/Default"));
                    sr.color = new Color(1f, 0.6f, 0f, 0.5f);
                }
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

