using UnityEngine;

public struct DifficultyScale 
{
    public float speed;
    public float health;
    public float damage;  

    public DifficultyScale(float s, float h, float d)
    {
        speed = s; health = h; damage = d;
    }

    public static DifficultyScale Compute(WaveData waveData, int wave)
    {
        float speed = Mathf.Min(1 + (waveData.speedMultPerWave * (wave - 1)), waveData.maxSpeedMult);
        float health = Mathf.Min(1 + (waveData.healthMultPerWave * (wave - 1)), waveData.maxHealthMult);
        float damage = Mathf.Min(1 + (waveData.damageMultPerWave * (wave - 1)), waveData.maxDamageMult);

        return new DifficultyScale(speed, health, damage);
    }
}

[CreateAssetMenu(fileName = "NewEnemyData", menuName = "ScriptableObjs/EnemyData")]
public class EnemyData : ScriptableObject
{
    public float maxHealth;
    public float moveSpeed;
    public float contactDamage;
    public float contactRange;
    public float damageInterval;
    public int scoreValue;
    public Material mat;
}


