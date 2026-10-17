using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SpawnEntry
{
    public EnemyData enemyData;
    public int unlockWave;
    public float weight;
}
[CreateAssetMenu(fileName ="NewWaveData", menuName ="ScriptableObjs/WaveData")]
public class WaveData : ScriptableObject
{
    public List<SpawnEntry> spawnEntries;
    public float timeBetweenWaves;
    public int baseEnemyCount;
    public int extraEnemiesPerWave;
    public float baseSpawnInterval;
    public float intervalReductionPerWave;
    public float minSpawnInterval;
    public int maxAlive = 40;
    public float speedMultPerWave = 0.05f;
    public float maxSpeedMult = 1.25f;
    public float healthMultPerWave = 0.10f;
    public float maxHealthMult = 2f;
    public float damageMultPerWave = 0.08f;
    public float maxDamageMult = 1.5f;
}
