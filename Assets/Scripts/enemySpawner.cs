using System;
using System.Collections.Generic;
using System.Collections;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Events;

public class enemySpawner : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] private GameObject[] enemyList;
    [SerializeField] private int baseEnemys = 8;
    [SerializeField] private float EnemiesPerSecond = 0.5f ;
    [SerializeField] private float timeInBetweenWaves = 5.0f;
    [SerializeField] private float diffFactor = 0.75f; 

    public static UnityEvent onEnemyDestroy = new UnityEvent();
    private int currentWave = 1;
    private float timeFromLastSpawn;
    private float enemiesAlive;
    private int enemiesLeftToSpawn;
    private Boolean isSpawning = false;

    void Start()
    {
        StartCoroutine(StartWave());
    }
    private void Awake()
    {
        onEnemyDestroy.AddListener(onEnemyDestroyed);
    }

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

        if(timeFromLastSpawn >= (1f / EnemiesPerSecond) && enemiesLeftToSpawn > 0)
        {
            SpawnEnemies();
            enemiesLeftToSpawn--;
            enemiesAlive++;
            timeFromLastSpawn = 0.0f;
        }
        if(enemiesAlive == 0 && enemiesLeftToSpawn == 0)
        {
            EndWave();
        }
        
    }
    private void EndWave()
    {
        isSpawning = false;
        timeFromLastSpawn = 0.0f;
        currentWave++;
        StartCoroutine(StartWave());
    }
    private void onEnemyDestroyed()
    {
        enemiesAlive--;
    }
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

    private int EnemiesPerWave()
    {
        return Mathf.RoundToInt(baseEnemys * Mathf.Pow(currentWave, diffFactor));
    }
}
