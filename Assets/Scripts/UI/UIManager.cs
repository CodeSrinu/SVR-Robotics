using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager: MonoBehaviour
{
    [SerializeField] private ScoreManager scoreManager;
    [SerializeField] private WaveManager waveManager;
    [SerializeField] private Health playerHealth;
    [SerializeField] private GameManager gameManager;
    [SerializeField] private EnemyDiedChannel enemyDiedChannel;

    [SerializeField] private Slider playerHealthbar;
    [SerializeField] private TextMeshProUGUI healthTxt;
    [SerializeField] private TextMeshProUGUI scoreTxt;
    [SerializeField] private TextMeshProUGUI waveTxt;
    [SerializeField] private TextMeshProUGUI killsTxt;
    [SerializeField] private TextMeshProUGUI enemiesTxt;

    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private Button retryBtn;


    private void OnEnable()
    {
        playerHealth.Changed += OnPlayerHelathChange;
        gameManager.OnStateChange += OnGameStateChanged;
        waveManager.WaveChanged += OnWaveChanged;
        scoreManager.ScoreChanged += OnScoreChanged;
        waveManager.EnemyKilled += OnEnemyKill;
        waveManager.EnemiesChanged += OnEnemiesSpawned;
    }

    private void OnEnemiesSpawned(int enemies)
    {
        UpdateEnemiesCountUI(enemies);
    }

    private void OnGameStateChanged(GameManager.GameState state)
    {
        if(state == GameManager.GameState.GameOver) ShowGameOver();
    }

    private void OnEnemyKill(int kills)
    {
        UpdateKillsUI(kills);
    }

    private void OnScoreChanged(int score)
    {
        UpdateScoreUI(score);
    }

    private void OnWaveChanged(int wave)
    {
        UpdateWavesUI(wave);
    }

    private void Start()
    {
        UpdatePlayerHealthUI(100);
        UpdateWavesUI(1);
        UpdateKillsUI(0);
        UpdateScoreUI(0);

        retryBtn.onClick.AddListener(() =>
        {
            gameManager.Restart();
        });
        gameOverPanel.SetActive(false);
    }

    private void OnPlayerHelathChange(float currentHealth, float maxHealth)
    {
        UpdatePlayerHealthUI(currentHealth);
    }

    private void UpdatePlayerHealthUI(float updatedHealth)
    {
        playerHealthbar.value = updatedHealth * 0.01f;
        healthTxt.text = updatedHealth.ToString();
    }

    

    private void UpdateWavesUI(int wave)
    {
        waveTxt.text = $"WAVE {wave}";
    }
    private void UpdateEnemiesCountUI(int enemiesAlive)
    {
        enemiesTxt.text = $"Enemies: {enemiesAlive}";
    }

    private void UpdateKillsUI(int kills)
    {
        killsTxt.text = $"Kills: {kills}";
    }
    private void UpdateScoreUI(int score)
    {
        scoreTxt.text = $"Score: {score}";
    }

    private void ShowGameOver()
    {
        gameOverPanel.SetActive(true);
    }
}
