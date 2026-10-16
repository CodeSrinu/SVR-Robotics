using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Health playerHealth;
    [SerializeField] private WaveManager waveManager;
    [SerializeField] private InputReader inputReader;


    public enum GameState {None, Playing, GameOver}
    public GameState state;

    public event Action<GameState> OnStateChange;

    private void Start()
    {
        state = GameState.None;
        OnStateChange?.Invoke(state);

        Cursor.lockState = CursorLockMode.Locked;
    }

    private void OnEnable()
    {
        playerHealth.Dead += OnPlayerDeath;
    }

    private void OnPlayerDeath()
    {
        state = GameState.GameOver;
        waveManager.StopWaves();
        inputReader.enabled = false;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Time.timeScale = 0f;
        OnStateChange?.Invoke(state);
    }

    public void Restart()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
