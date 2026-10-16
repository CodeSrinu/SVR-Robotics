using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class WaveManager : MonoBehaviour
{
    [SerializeField] private WaveData waveData;
    [SerializeField] private Enemy EnemyPrefab;
    [SerializeField] private Transform player;
    [SerializeField] private EnemyDiedChannel enemyDiedChannel;
    [SerializeField] private Transform[] spawnPoints;
    public event Action<int> EnemyKilled;


    private int currentWave = 0;
    private int enemiesAlive;
    private int enemiesKilled = 0;

    public event Action<int> WaveChanged;
    private Coroutine wavesCoroutine;
    private void OnEnable()
    {
        enemyDiedChannel.Raised += OnEnemyDeath;
    }
    private void OnDisable()
    {
        enemyDiedChannel.Raised -= OnEnemyDeath;
    }

    private void Start()
    {
        wavesCoroutine = StartCoroutine(RunWaves());
    }

    private void OnEnemyDeath(EnemyData enemyData)
    {
        enemiesAlive--;
        enemiesKilled++;
        EnemyKilled?.Invoke(enemiesKilled);
    }

    private IEnumerator RunWaves()
    {
        while (true)
        {
            currentWave++;
            WaveChanged?.Invoke(currentWave);
            int count = waveData.baseEnemyCount + waveData.extraEnemiesPerWave * (currentWave - 1);
            float interval = Mathf.Max(waveData.minSpawnInterval,
                waveData.baseSpawnInterval - waveData.intervalReductionPerWave * (currentWave - 1));
            for (int i = 0; i < count; i++)
            {
                SpawnEnemy();
                yield return new WaitForSeconds(interval);
            }

            yield return new WaitUntil(()=> enemiesAlive == 0);
            yield return new WaitForSeconds(waveData.timeBetweenWaves);
        }
    }

    private void SpawnEnemy()
    {
        SpawnEntry entry = PickAnEntry();
        if (entry == null) return;

        Transform point = spawnPoints[UnityEngine.Random.Range(0, spawnPoints.Length - 1)];

        if (!NavMesh.SamplePosition(point.position, out NavMeshHit hit, 2, NavMesh.AllAreas)) return;

        Enemy enemy = Instantiate(EnemyPrefab, hit.position, Quaternion.identity);
        enemy.Init(entry.enemyData, player);
        enemiesAlive++;
    }

    private SpawnEntry PickAnEntry()
    {
        float total = 0;
        foreach(SpawnEntry entry in waveData.spawnEntries)
        {
            if(currentWave >= entry.unlockWave)
            {
                total += entry.weight;
            }
        }

        if (total < 0) return null;

        float roll = UnityEngine.Random.Range(0, total);
        SpawnEntry last = null;

        foreach(SpawnEntry entry in waveData.spawnEntries)
        {
            if(entry.unlockWave > currentWave) continue;

            last = entry;
            roll -= entry.weight;
            if (roll < 0) return entry;
        }

        return last;
    }

    public void StopWaves()
    {
        StopCoroutine(wavesCoroutine);
    }
}
