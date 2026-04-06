using Unity.VisualScripting;
using UnityEngine;

public class TowerDragHandler : MonoBehaviour
{
    public GameObject tower;
    public GameObject ghostTower;
    public bool isDragging = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void BeginDrag()
    {
        isDragging = true;
        ghostTower =Instantiate(tower);
        SpriteRenderer spriteRenderer= ghostTower.GetComponent<SpriteRenderer>();
        spriteRenderer.color =  new Color(1,1,1,0.5f);
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(isDragging == true)
        {
            Vector2 position = new Vector2(Camera.main.ScreenToWorldPoint(Input.mousePosition).x,Camera.main.ScreenToWorldPoint(Input.mousePosition).y);
            ghostTower.transform.position = position;
        }
        if (Input.GetMouseButtonDown(1))
        {
            Destroy(ghostTower);
            isDragging = false;
        }
    }
}
