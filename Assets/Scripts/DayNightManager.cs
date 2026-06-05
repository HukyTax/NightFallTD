using System;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class DayNightManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    //[SerializeField] private int peakHour = 24;
    GameObject levelManger;
    enemySpawner enemyspawner;

    public int currentRound = 0;
    public float maxBrightness = 1f;

    // 72 for a 3-day survival window
    public int totalRounds = 72; 

    private Light2D sunLight;
    private Boolean nightTime;
    
    void Start()
    {
        sunLight = GetComponent<Light2D>();
        levelManger = GameObject.Find("LevelManager");
        enemyspawner = levelManger.GetComponent<enemySpawner>();


        

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void updateSun()
    {
        if(currentRound >= totalRounds)
        {
            Debug.Log("YOU WON! :)");
            return;
        }
        int currentHour = (enemyspawner.getWave() + 8) % 24;
        float angle = (2f * Mathf.PI / 24f) * currentHour;
        float sunHeight = -Mathf.Cos(angle);

        sunLight.intensity = Mathf.Max(0.1f, sunHeight) * maxBrightness;
        if(sunLight.intensity <= .3f)
        {
            nightTime = true;
        }
        else
        {
            nightTime = false;
        }
        
    }
    public void AdvanceRoundTest()
    {
        currentRound++;
        updateSun();
    }
    public Boolean GetIsNight()
    {
        return nightTime;
    }
}
