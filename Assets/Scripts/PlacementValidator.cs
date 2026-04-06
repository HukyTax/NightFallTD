using UnityEngine;
public class PlacementValidator : MonoBehaviour
{
    public LayerMask blockedLayers;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool isValid(Vector2 position)
    {
        Collider2D collider2D = Physics2D.OverlapCircle(position, 2, blockedLayers);
        if(collider2D == null)
        {
            return true;
        }
        return false;
    }
}
