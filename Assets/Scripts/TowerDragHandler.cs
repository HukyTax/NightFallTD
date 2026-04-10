using System;
using UnityEngine;

public class TowerDragHandler : MonoBehaviour
{
    public GameObject tower;
    public PlacementValidator validator;
    private GameObject ghostTower;
    public bool isDragging = false;
    [SerializeField] int price;
    public Economy economy;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void BeginDrag()
    {
        //this.price = price;
        isDragging = true;
        ghostTower =Instantiate(tower);
        ghostTower.GetComponentInChildren<Collider2D>().enabled = false;
        ghostTower.GetComponentInChildren<Turret>(). enabled = false;
        economy = gameObject.GetComponent<Economy>();
        SpriteRenderer[] renderers = ghostTower.GetComponentsInChildren<SpriteRenderer>();
        foreach (SpriteRenderer sr in renderers)
        {
            sr.material = new Material(Shader.Find("Sprites/Default"));
            sr.color = new Color(1f, 0.5f, 0.5f, 0.5f);
        }
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(isDragging)
        {
            Vector2 position = new Vector2(Camera.main.ScreenToWorldPoint(Input.mousePosition).x,Camera.main.ScreenToWorldPoint(Input.mousePosition).y);
            ghostTower.transform.position = position;
        
            if (Input.GetMouseButtonDown(1))
            {
                Destroy(ghostTower);
                isDragging = false;
            }
            if (Input.GetMouseButtonDown(0))
            {
                if(economy.getMoney() >= price){
                    if (validator.isValid(position))
                    {
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
                    Debug.Log("Not Enough Money, The price for this tower is:"  + price);
                }   
            }
        }
    }
}    


