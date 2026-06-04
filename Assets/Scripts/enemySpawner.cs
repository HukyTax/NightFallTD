using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Events;

// Manages the wave loop: waits between waves, spawns enemies at a fixed rate,
// and advances to the next wave once all enemies are dead or have leaked through.
public class enemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] enemyList;

    // Base number of enemies in wave 1. Grows each wave via EnemiesPerWave().
    [SerializeField] private int baseEnemys = 8;

    // How many enemies spawn per second while a wave is active.
    [SerializeField] private float EnemiesPerSecond = 0.5f;

    [SerializeField] private float timeInBetweenWaves = 5.0f;

    // Exponent for the wave scaling formula. Higher = harder difficulty curve.
    // Formula: baseEnemys * currentWave ^ diffFactor
    [SerializeField] private float diffFactor = 0.75f;

    // Static event so any system (Health, enemyMovement) can notify the spawner
    // that an enemy left the field — whether killed or leaked.
    public static UnityEvent onEnemyDestroy = new UnityEvent();
    //meant to be 1 but chnaged for testing
    private int howManytypes = 1;

    private int counter = 5;
    //meant to be 1 but changed for testing
    private int currentWave = 1;
    private float timeFromLastSpawn;
    private int enemiesAlive;         // decremented by onEnemyDestroyed listener
    private int enemiesLeftToSpawn;  // decremented each time SpawnEnemies() fires
    private Boolean isSpawning = false;
    private GameObject light;
    private DayNightManager dayNightManager;

    void Start()
    {
        StartCoroutine(StartWave());
        light = GameObject.Find("Light 2D");
        dayNightManager = light.GetComponent<DayNightManager>();

    }

    private void Awake()
    {
        onEnemyDestroy.AddListener(onEnemyDestroyed);
    }

    // Waits the inter-wave delay then arms the spawner for the new wave.
    private IEnumerator StartWave()
    {
        yield return new WaitForSeconds(timeInBetweenWaves);
        isSpawning = true;
        enemiesLeftToSpawn = EnemiesPerWave();
    }

    void Update()
    {
        if (!isSpawning) return;

        timeFromLastSpawn += Time.deltaTime;

        // Spawn one enemy each time the per-second interval elapses.
        if (timeFromLastSpawn >= (1f / EnemiesPerSecond) && enemiesLeftToSpawn > 0)
        {
            SpawnEnemies();
            enemiesLeftToSpawn--;
            enemiesAlive++;
            timeFromLastSpawn = 0.0f;
        }

        // Wave ends when all enemies have been spawned AND none remain alive.
        if (enemiesAlive == 0 && enemiesLeftToSpawn == 0)
        {
            EndWave();
        }

        // Dev shortcut: P skips to the next wave instantly (note: may desync alive count).
        if (Input.GetKeyDown(KeyCode.P))
        {
            currentWave += 1;
            enemiesAlive = 0;
            enemiesLeftToSpawn = 0;
        }
    }

    private void EndWave()
    { 
        isSpawning = false;
        timeFromLastSpawn = 0.0f;
        currentWave++;
        if(currentWave == counter && (enemyList.Length -1) > howManytypes)
        {   Debug.Log("More enemy types");
            howManytypes++;
            EnemiesPerSecond *= 0.9f;
            counter += 5;
        }
        StartCoroutine(StartWave());
        dayNightManager.AdvanceRoundTest();
        
    }

    // Fired by the static onEnemyDestroy event — both kills and leaks call this.
    private void onEnemyDestroyed()
    {
        enemiesAlive--;
    }

    private void SpawnEnemies()
    {
        GameObject enemyPreFab;
        if(howManytypes <= 1){
            enemyPreFab = enemyList[0];
        }
        else
        {
            if (enemyList.Length > 1 && UnityEngine.Random.Range(0, 100) < 45)
            {
                enemyPreFab = enemyList[UnityEngine.Random.Range(1, howManytypes)];
            }
            else
            {
                enemyPreFab = enemyList[0];
            }

        }
        Instantiate(enemyPreFab, LevelManager.main.startPoint.position, Quaternion.identity);
    }

    // Enemy count per wave scales as: base * wave^diffFactor (power curve, not exponential).
    private int EnemiesPerWave()
    {
        return Mathf.RoundToInt(baseEnemys * Mathf.Pow(currentWave, diffFactor));
    }

    public int getWave()
    {
        return currentWave;
    }
}
