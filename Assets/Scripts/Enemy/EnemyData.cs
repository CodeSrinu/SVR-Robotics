using UnityEngine;

[CreateAssetMenu(fileName = "NewEnemyData", menuName = "ScriptableObjs/EnemyData")]
public class EnemyData : ScriptableObject
{
    public float maxHealth;
    public float moveSpeed;
    public float contactDamage;
    public float contactRange;
    public float damageInterval;
    public int scoreValue;


}
