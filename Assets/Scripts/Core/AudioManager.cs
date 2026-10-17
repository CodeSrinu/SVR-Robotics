using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioSource sfxSource;
    [SerializeField] private AudioSource gameOverSource;
    [SerializeField] private AudioClip shotClip;
    [SerializeField] private AudioClip hurtClip;
    [SerializeField] private AudioClip enemyDeathClip;
    [SerializeField] private AudioClip gameOverClip;

    [SerializeField] private PlayerShooter shooter;
    [SerializeField] private Health playerHealth;
    [SerializeField] private EnemyDiedChannel enemyDiedChannel;
    [SerializeField] private GameManager gameManager;

    private void OnEnable()
    {
        shooter.ShotFired += OnShotFired;
        playerHealth.Damaged += OnPlayerHurt;
        enemyDiedChannel.Raised += OnEnemyDeath;
        gameManager.OnStateChange += OnGameStateChanged;
    }

    private void OnDisable()
    {
        shooter.ShotFired -= OnShotFired;
        playerHealth.Damaged -= OnPlayerHurt;
        enemyDiedChannel.Raised -= OnEnemyDeath;
        gameManager.OnStateChange -= OnGameStateChanged;
    }

    private void OnShotFired()
    {
        PlaySfx(shotClip);
    }

    private void OnPlayerHurt(float damage)
    {
        PlaySfx(hurtClip);
    }

    private void OnEnemyDeath(EnemyData enemyData)
    {
        PlaySfx(enemyDeathClip);
    }

    private void OnGameStateChanged(GameManager.GameState state)
    {
        if (state != GameManager.GameState.GameOver) return;

        gameOverSource.pitch = 1f;
        gameOverSource.PlayOneShot(gameOverClip);
    }

    private void PlaySfx(AudioClip clip)
    {
        if (clip == null) return;

        sfxSource.pitch = UnityEngine.Random.Range(0.95f, 1.05f);
        sfxSource.PlayOneShot(clip);
    }
}