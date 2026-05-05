using UnityEngine;

// Attached to each tower button in the shop UI.
// Handles the full drag-to-place flow: spawns a ghost preview on BeginDrag,
// colours it based on placement validity and affordability each frame,
// and either places or cancels on mouse-up.
public class TowerDragHandler : MonoBehaviour
{
    // The real tower prefab to instantiate on successful placement.
    public GameObject tower;
    // Shared validator that checks whether a world position is free to build on.
    public PlacementValidator validator;

    private GameObject ghostTower;
    public bool isDragging = false;

    // Gold cost shown in the shop. Checked before placement is allowed.
    [SerializeField] int price;

    public Economy economy;
<<<<<<< Updated upstream
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void BeginDrag()
    {
        Debug.Log("cost: " + price);
        isDragging = true;
        ghostTower = Instantiate(tower);
        ghostTower.GetComponentInChildren<Collider2D>().enabled = false;
        ghostTower.GetComponentInChildren<Turret>().enabled = false;
        SpriteRenderer[] renderers = ghostTower.GetComponentsInChildren<SpriteRenderer>();
        foreach (SpriteRenderer sr in renderers)
        {
            sr.material = new Material(Shader.Find("Sprites/Default"));
            sr.color = new Color(0f, 0.0f, 0.5f, 0.5f);
        }
    }
=======
    private LevelManager levelManager;

>>>>>>> Stashed changes
    void Start()
    {
        levelManager = GameObject.Find("LevelManager").GetComponentInChildren<LevelManager>();
        economy = GameObject.Find("LevelManager").GetComponentInChildren<Economy>();
    }

    // Called by the UI button's EventTrigger (PointerDown).
    // Registers this handler as the active drag — LevelManager cancels any
    // previous ghost before setting the new one, so only one can exist at a time.
    public void BeginDrag()
    {
        levelManager.SetActiveDrag(this);
        isDragging = true;

        ghostTower = Instantiate(tower);
        // Disable physics and AI on the ghost so it doesn't interact with the world.
        ghostTower.GetComponentInChildren<Collider2D>().enabled = false;
        ghostTower.GetComponentInChildren<Turret>().enabled = false;

        // Tint the ghost blue to signal "currently dragging, not yet placed".
        TintGhost(new Color(0f, 0f, 0.5f, 0.5f));
    }

    void Update()
    {
<<<<<<< Updated upstream
        if (isDragging)
=======
        if (!isDragging) return;

        Vector2 position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        ghostTower.transform.position = position;
        UpdateGhostTint(position);

        if (Input.GetMouseButtonDown(1))
            CancelDrag();
        else if (Input.GetMouseButtonDown(0))
            TryPlace(position);
    }

    // Ghost colour feedback:
    //   Green    = valid position AND affordable
    //   Dark red = invalid position (blocked layer / path)
    //   Orange   = valid position but not enough money
    private void UpdateGhostTint(Vector2 position)
    {
        if (!validator.isValid(position))
>>>>>>> Stashed changes
        {
            TintGhost(new Color(0.5f, 0f, 0f, 0.5f));
            return;
        }

        // Orange overrides green when the player can't afford it.
        Color tint = economy.GetMoney() < price
            ? new Color(1f, 0.6f, 0f, 0.5f)
            : new Color(0f, 1f, 0f, 0.5f);
        TintGhost(tint);
    }

    private void TryPlace(Vector2 position)
    {
        if (economy.GetMoney() < price)
        {
            Debug.Log("Not enough money. Price: " + price);
            return;
        }

        if (!validator.isValid(position))
        {
            Debug.Log("This Position is not valid");
            return;
        }

        economy.ChangeMoney(economy.GetMoney() - price);
        Instantiate(tower, new Vector3(position.x, position.y, 0), Quaternion.identity);
        CancelDrag();
    }

    // Public so LevelManager.SetActiveDrag can cancel an in-progress drag when a new
    // one starts. Directly nulls the LevelManager slot instead of calling SetActiveDrag
    // to avoid a recursive cancel loop.
    public void CancelDrag()
    {
        if (ghostTower != null) Destroy(ghostTower);
        isDragging = false;

        if (levelManager != null && levelManager.activeDragHandler == this)
            levelManager.activeDragHandler = null;
    }

    // Applies a single colour to every SpriteRenderer on the ghost tower.
    private void TintGhost(Color color)
    {
        foreach (SpriteRenderer sr in ghostTower.GetComponentsInChildren<SpriteRenderer>())
        {
            sr.material = new Material(Shader.Find("Sprites/Default"));
            sr.color = color;
        }
    }
}
