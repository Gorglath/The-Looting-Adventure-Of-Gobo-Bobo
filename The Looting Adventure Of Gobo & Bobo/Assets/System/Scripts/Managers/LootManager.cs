using Pixelplacement;
using System.Collections.Generic;
using UnityEngine;

public class LootManager : Singleton<LootManager>
{
    [Header("Debug")]
    [SerializeField]
    private bool isDebug = false;
    [Header("Events")]
    [SerializeField]
    private TimedEvent[] whenCollectedLoot;
    [SerializeField]
    private TimedEvent[] whenGoboBagIsFull;

    [Header("References")]
    [SerializeField]
    private GameObject[] loot = new GameObject[0];
    [SerializeField]
    private GameObject[] bonusLoot = new GameObject[0];
    [SerializeField]
    private Transform[] spawnLocations = new Transform[0];
    [SerializeField]
    private Transform[] bonusLootSpawnLocations = new Transform[0];

    [Header("Variables")]
    [SerializeField]
    private int maxNumberOfLootSpawnedAtAnyGivenTime = 10;
    [SerializeField]
    private float timeBetweenSpawningLoot = 3f;
    [SerializeField]
    private float spawnLocationOffsetMultiplier = 2f;
    //helpers
    private List<GameObject> instantiatedLoot = new List<GameObject>();
    private List<Transform> usedSpawnPoints = new List<Transform>();
    private GameObject lootPlaceholder = null;
    private Transform currentSpawnPoint = null;
    private Transform previousTakenLootSpawnPoint = null;
    private Vector3 randomPosition = Vector3.zero;
    private int indexToSpawnLoot = 0;
    private void OnDestroy()
    {
        _instance = null;
    }
    private void Start()
    {
        if (isDebug)
        {
            Invoke("InitializeLootManager",3f);
            Invoke("SpawnLootFirstTime", 6f);
        }
    }
    public void InitializeLootManager()
    {
        instantiateLoot();
    }

    private void instantiateLoot()
    {
        for (int i = 0; i < maxNumberOfLootSpawnedAtAnyGivenTime + BagManager.Instance.MaxNumberOfLootContainedInGoblinBag; i++)
        {
            indexToSpawnLoot = (i < loot.Length)? i : Random.Range(0,loot.Length);
            instantiatedLoot.Add(Instantiate(loot[indexToSpawnLoot],transform));
            instantiatedLoot[i].SetActive(false);
        }
        SpawnLootFirstTime();
    }
    public void SpawnLootFirstTime()
    {
        for (int i = 0; i < maxNumberOfLootSpawnedAtAnyGivenTime; i++)
        {
            SpawnLoot();
        }

        for (int i = 0; i < bonusLoot.Length; i++)
        {
            lootPlaceholder = Instantiate(bonusLoot[i], transform);

            randomPosition = bonusLootSpawnLocations[i].position +
                   new Vector3(Random.Range(-spawnLocationOffsetMultiplier, spawnLocationOffsetMultiplier), 0f,
                   Random.Range(-spawnLocationOffsetMultiplier, spawnLocationOffsetMultiplier));

            lootPlaceholder.transform.position = randomPosition;
            lootPlaceholder.GetComponent<Loot>().LootSpawnPoint = currentSpawnPoint;
        }
    }
    private void SpawnLoot()
    {
        do
        {
            currentSpawnPoint = spawnLocations[Random.Range(0, spawnLocations.Length)];
            //Get random spawn location.
            randomPosition = currentSpawnPoint.position +
                new Vector3(Random.Range(-spawnLocationOffsetMultiplier, spawnLocationOffsetMultiplier), 0f,
                Random.Range(-spawnLocationOffsetMultiplier, spawnLocationOffsetMultiplier));
        } while (usedSpawnPoints.Contains(currentSpawnPoint));

        do
        {
            lootPlaceholder = instantiatedLoot[Random.Range(0, instantiatedLoot.Count)];
        } while (lootPlaceholder.activeSelf);

        lootPlaceholder.SetActive(true);
        lootPlaceholder.transform.position = randomPosition;
        lootPlaceholder.GetComponent<Loot>().LootSpawnPoint = currentSpawnPoint;
        lootPlaceholder.transform.rotation = Quaternion.Euler(Random.Range(0f, 360f), Random.Range(0f, 360f), Random.Range(0f, 360f));
        usedSpawnPoints.Add(currentSpawnPoint);



    }

    public void LootTaken(Loot loot)
    {
        //Take Loot
        if (BagManager.Instance.CanTakeMoreLoot())
        {
            loot.gameObject.SetActive(false);
            if(previousTakenLootSpawnPoint != null)
            {
                usedSpawnPoints.Remove(previousTakenLootSpawnPoint);
            }
            previousTakenLootSpawnPoint = loot.LootSpawnPoint;
            BagManager.Instance.AddLootToBag(loot);
            Invoke("SpawnLoot", timeBetweenSpawningLoot);
        }
    }

    private void OnDrawGizmos()
    {
            if (spawnLocations.Length > 0)
            {
                foreach (Transform spawn in spawnLocations)
                {
                    Gizmos.DrawCube(spawn.position, new Vector3(spawnLocationOffsetMultiplier,0.3f,spawnLocationOffsetMultiplier));
                }
            }
        Gizmos.color = Color.magenta;
        if (bonusLootSpawnLocations.Length > 0)
        {
            foreach (Transform spawn in bonusLootSpawnLocations)
            {
                Gizmos.DrawCube(spawn.position, new Vector3(spawnLocationOffsetMultiplier, 0.3f, spawnLocationOffsetMultiplier));
            }
        }
    }
}