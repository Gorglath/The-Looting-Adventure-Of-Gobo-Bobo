using Pixelplacement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawnManager : Singleton<MonsterSpawnManager>
{
    [Header("Debug")]
    [SerializeField]
    private bool isDebug = false;
    [SerializeField]
    private int monstersToSpawn = 3;
    [Header("References")]
    [SerializeField]
    private SlimeFactory slimeFactory;
    [SerializeField]
    private BarbFactory barbFactory;
    [SerializeField]
    private MonsterSpawnPoint[] slimeSpawnPoint;
    [SerializeField]
    private MonsterSpawnPoint[] barbSpawnPoint;
    [Header("Variables")]
    [SerializeField]
    private float timeBetweenSpawningMonster = 10f;
    [SerializeField]
    private int maxNumberOfMonsters = 20;
    //helpers
    private MonsterSpawnPoint currentSpawnPoint = null;
    private List<MonsterSpawnPoint> slimeSpawnPointsThatAlreadySpawned = new List<MonsterSpawnPoint>();
    private List<MonsterSpawnPoint> barbSpawnPointsThatAlreadySpawned = new List<MonsterSpawnPoint>();
    private int barbsSpawned = 0;
    private int slimesSpawned = 0;
    private float counter = 0f;
    private void Update()
    {
        counter += Time.deltaTime;
        if(counter >= timeBetweenSpawningMonster && barbsSpawned + slimesSpawned < maxNumberOfMonsters)
        {
            counter = 0f;
            SpawnMonster();
        }
    }
    public void InitializeMonsterSpawner()
    {
        for (int i = 0; i < monstersToSpawn; i++)
        {
            Invoke("SpawnMonster", 0.1f);
        }
    }
    private void OnDestroy()
    {
        _instance = null;
    }
    private void SpawnMonster()
    {
        if(barbsSpawned > slimesSpawned)
        {
            slimesSpawned++;
            if(slimeSpawnPointsThatAlreadySpawned.Count == slimeSpawnPoint.Length)
            {
                slimeSpawnPointsThatAlreadySpawned.Clear();
            }
            do
            {
                currentSpawnPoint = slimeSpawnPoint[Random.Range(0, slimeSpawnPoint.Length)];
            } while (slimeSpawnPointsThatAlreadySpawned.Contains(currentSpawnPoint));
            slimeSpawnPointsThatAlreadySpawned.Add(currentSpawnPoint);
            currentSpawnPoint.SpawnObjectAtLocation(slimeFactory);
        }
        else
        {
            barbsSpawned++;
            if(barbSpawnPointsThatAlreadySpawned.Count == barbSpawnPoint.Length)
            {
                barbSpawnPointsThatAlreadySpawned.Clear();
            }
            do
            {
                currentSpawnPoint = barbSpawnPoint[Random.Range(0, barbSpawnPoint.Length)];
            } while (barbSpawnPointsThatAlreadySpawned.Contains(currentSpawnPoint));
            barbSpawnPointsThatAlreadySpawned.Add(currentSpawnPoint);
            currentSpawnPoint.SpawnObjectAtLocation(barbFactory,true);
        }
    }

    public void MonsterDied()
    {
        SpawnMonster();
    }
}
