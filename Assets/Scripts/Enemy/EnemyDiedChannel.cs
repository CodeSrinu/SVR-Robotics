using System;
using UnityEngine;

[CreateAssetMenu(fileName = "NewChannel", menuName = "ScriptableObjs/enemyDiedChannel")]
public class EnemyDiedChannel : ScriptableObject
{
    public event Action<EnemyData> Raised;

    public void EnemyDied(EnemyData data)
    {
        Raised?.Invoke(data);
    }
}
