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
    [SerializeField] private Transform enemiesPoolContainer;
    public event Action<int> EnemyKilled;
    public event Action<int> EnemiesChanged;


    private int currentWave = 0;
    private int enemiesAlive;
    private int enemiesKilled = 0;
    private int currentEnemyCount = 0;

    public event Action<int> WaveChanged;
    private Coroutine wavesCoroutine;
    private Pool<Enemy> enemiesPool;

    [SerializeField] private float spawnRingRadius = 5f;
    [SerializeField] private float sampleRadius = 6f;
    [SerializeField] private float minSpawnDistance = 15f;
    [SerializeField] private int maxSpawnAttempts = 10;
    [SerializeField] private float minEdgeDistance = 0.8f;
    

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
        enemiesPool = new Pool<Enemy>(EnemyPrefab, enemiesPoolContainer, 15);
        wavesCoroutine = StartCoroutine(RunWaves());
    }

    private void OnEnemyDeath(EnemyData enemyData)
    {
        currentEnemyCount--;
        enemiesAlive--;
        enemiesKilled++;
        EnemyKilled?.Invoke(enemiesKilled);
        EnemiesChanged?.Invoke(currentEnemyCount);
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
            var scale = DifficultyScale.Compute(waveData, currentWave);

            int spawned = 0;
            int failedInARow = 0;

            while (spawned < count)
            {
                if (enemiesAlive >= waveData.maxAlive)
                {
                    yield return new WaitForSeconds(0.25f);
                    continue;
                }

                if (TrySpawnEnemy(scale))
                {
                    spawned++;
                    currentEnemyCount++;
                    EnemiesChanged?.Invoke(currentEnemyCount);
                    failedInARow = 0;
                    yield return new WaitForSeconds(interval);
                }
                else
                {
                    failedInARow++;
                    if (failedInARow >= 5)
                    {
                        //Debug.LogWarning($"Wave {currentWave}: gave up after {failedInARow} failed spawns ({spawned}/{count} spawned)");
                        break;
                    }
                    yield return new WaitForSeconds(0.2f);
                }
            }

            yield return new WaitUntil(() => enemiesAlive == 0);
            yield return new WaitForSeconds(waveData.timeBetweenWaves);
        }
    }

    private bool TrySpawnEnemy(DifficultyScale scale)
    {
        SpawnEntry entry = PickAnEntry();
        if (entry == null) return false;

        for (int i = 0; i < maxSpawnAttempts; i++)
        {
            Transform point = spawnPoints[UnityEngine.Random.Range(0, spawnPoints.Length)];

            Vector2 ring = UnityEngine.Random.insideUnitCircle * spawnRingRadius;
            Vector3 candidate = point.position + new Vector3(ring.x, 0f, ring.y);

            if (!NavMesh.SamplePosition(candidate, out NavMeshHit hit, sampleRadius, NavMesh.AllAreas))
                continue;

            if ((hit.position - player.position).sqrMagnitude < minSpawnDistance * minSpawnDistance)
                continue;

            if (Mathf.Abs(hit.position.y - point.position.y) > 1f) continue;

            if (NavMesh.FindClosestEdge(hit.position, out NavMeshHit edge, NavMesh.AllAreas)
                && edge.distance < minEdgeDistance) continue;

            Enemy enemy = enemiesPool.Get();
            enemy.Init(entry.enemyData, player, hit.position, enemiesPool.Release,scale);
            enemiesAlive++;
            return true;
        }

        return false;
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

        if (total <= 0) return null;

        float roll = UnityEngine.Random.Range(0, total);
        SpawnEntry last = null;

        foreach(SpawnEntry entry in waveData.spawnEntries)
        {
            if(entry.unlockWave > currentWave) continue;

            last = entry;
            roll -= entry.weight;
            if (roll < 0) return entry;
        }

        return waveData.spawnEntries[0];
    }

    public void StopWaves()
    {
        StopCoroutine(wavesCoroutine);
    }
}
