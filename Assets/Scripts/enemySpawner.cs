using System;
using System.Collections.Generic;
using System.Collections;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Events;

// Controls the wave loop: waits between waves, spawns enemies at a set rate,
// and starts the next wave once all enemies are gone (killed or leaked).
public class enemySpawner : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    // Array of enemy prefabs. Index 0 is the common enemy; indices 1+ are rarer variants.
    [SerializeField] private GameObject[] enemyList;
    // How many enemies appear in wave 1. Each wave grows from this via EnemiesPerWave().
    [SerializeField] private int baseEnemys = 8;
    [SerializeField] private float EnemiesPerSecond = 0.5f ;
    [SerializeField] private float timeInBetweenWaves = 5.0f;
    // Exponent used in the wave scaling formula: enemies = base * wave^diffFactor.
    // Lower values = gentler curve; higher values = harder later waves.
    [SerializeField] private float diffFactor = 0.75f;

    // Static so any script (Health, enemyMovement) can call it when an enemy leaves the field,
    // whether killed or leaked — both count as "enemy gone" for the wave end check.
    public static UnityEvent onEnemyDestroy = new UnityEvent();
    private int currentWave = 1;
    private float timeFromLastSpawn;
    private float enemiesAlive;       // decremented by the onEnemyDestroyed listener
    private int enemiesLeftToSpawn;   // counts down from EnemiesPerWave() each wave
    private Boolean isSpawning = false;

    void Start()
    {
        StartCoroutine(StartWave());
    }

    // Awake runs before Start — register the listener here so it's ready before any enemy spawns.
    private void Awake()
    {
        onEnemyDestroy.AddListener(onEnemyDestroyed);
    }

    // Waits the between-wave delay then arms the spawner for the new wave.
    private IEnumerator StartWave()
    {
        yield return new WaitForSeconds(timeInBetweenWaves);
        isSpawning = true;
        enemiesLeftToSpawn = EnemiesPerWave();
    }

    // Update is called once per frame
    void Update()
    {
        if(!isSpawning) return;

        timeFromLastSpawn += Time.deltaTime;

        // Spawn one enemy each time the per-second interval elapses.
        if(timeFromLastSpawn >= (1f / EnemiesPerSecond) && enemiesLeftToSpawn > 0)
        {
            SpawnEnemies();
            enemiesLeftToSpawn--;
            enemiesAlive++;
            timeFromLastSpawn = 0.0f;
        }
        // Wave ends when all enemies have been spawned AND none are still alive.
        if(enemiesAlive == 0 && enemiesLeftToSpawn == 0)
        {
            EndWave();
        }
        // dev tool (not working right)
        if (Input.GetKeyDown(KeyCode.P))
        {
            currentWave += 1;
            enemiesAlive = 0;
            enemiesLeftToSpawn= 0;
        }

    }
    private void EndWave()
    {
        isSpawning = false;
        timeFromLastSpawn = 0.0f;
        currentWave++;
        StartCoroutine(StartWave());
    }

    // Listener registered in Awake. Called whenever an enemy is killed OR leaks.
    private void onEnemyDestroyed()
    {
        enemiesAlive--;
    }

    // 80% chance to spawn the base enemy type (index 0).
    // 20% chance to pick a random variant from the rest of the list (if any exist).
    private void SpawnEnemies()
    {
        GameObject enemyPreFab;
        if (enemyList.Length > 1 && UnityEngine.Random.Range(0, 100) < 20)
        {
            enemyPreFab = enemyList[UnityEngine.Random.Range(1, enemyList.Length)];
        }
        else
        {
            enemyPreFab = enemyList[0];
        }

        Instantiate(enemyPreFab, LevelManager.main.startPoint.position, Quaternion.identity);
    }

    // Power-curve scaling: enemies grow faster in early waves, slower in later ones.
    // Adjust diffFactor in the Inspector to tune the difficulty ramp.
    private int EnemiesPerWave()
    {
        return Mathf.RoundToInt(baseEnemys * Mathf.Pow(currentWave, diffFactor));
    }
    public int getWave()
    {
        return currentWave;
    }
}
