using System;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] private EnemyDiedChannel enemyDiedChannel;
    private int score = 0;
    public event Action<int> ScoreChanged;

    private void OnEnable()
    {
        enemyDiedChannel.Raised += OnEnemyDeath;
    }

    private void OnDisable()
    {
        enemyDiedChannel.Raised -= OnEnemyDeath;
    }

    private void OnEnemyDeath(EnemyData enemyData)
    {
        score += enemyData.scoreValue;
        ScoreChanged?.Invoke(score);
    }
}
