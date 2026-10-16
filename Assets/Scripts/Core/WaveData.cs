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
}
