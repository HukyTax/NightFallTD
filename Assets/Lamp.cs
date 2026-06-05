using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Lamp : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
   [SerializeField] private CircleCollider2D circleCollider2D;
    [SerializeField ]private Light2D light2D;
    void Start()
    {
        light2D = GetComponent<Light2D>();
        circleCollider2D = GetComponent<CircleCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void upgradeRange(float upgrade)
    {
        light2D.pointLightOuterRadius += upgrade;
        circleCollider2D.radius = light2D.pointLightOuterRadius;
    }
}
