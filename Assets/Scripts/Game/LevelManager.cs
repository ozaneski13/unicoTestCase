using UnityEngine;
using System;

public class LevelManager : MonoBehaviour
{
    [Header("Data")]
    [SerializeField] private LevelHolderSO levelHolderSO;

    [Header("Systems")]
    [SerializeField] private Inventory inventory;
    [SerializeField] private EnemySpawner spawner;
    [SerializeField] private GridController grid;

    [Header("Events")]
    [SerializeField] private GameStartedSO gameStartedSO;
    [SerializeField] private EnemyReachedBaseSO enemyReachedBaseSO;

    private int currentLevel = -1;
    private bool running;

    public event Action<int> OnLevelStarted;
    public event Action OnGameWon;
    public event Action OnGameLost;

    public int CurrentLevel => currentLevel;

    private void OnEnable()
    {
        gameStartedSO.Subscribe(StartGame);
        enemyReachedBaseSO.Subscribe(OnEnemyReachedBase);
        spawner.OnLevelCleared += OnLevelCleared;
    }

    private void OnDisable()
    {
        gameStartedSO.Unsubscribe(StartGame);
        enemyReachedBaseSO.Unsubscribe(OnEnemyReachedBase);
        spawner.OnLevelCleared -= OnLevelCleared;
    }

    public void StartGame()
    {
        currentLevel = 0;
        LoadCurrent();
    }

    public void RestartLevel()
    {
        LoadCurrent();
    }

    private void LoadCurrent()
    {
        LevelSO level = levelHolderSO.Levels[currentLevel];

        spawner.DespawnAll();
        grid.ClearTurrets();
        inventory.Load(level.Inventory);
        spawner.Begin(level.Spawn);

        running = true;
        OnLevelStarted?.Invoke(currentLevel);
    }

    private void OnLevelCleared()
    {
        if (!running)
            return;

        if (currentLevel + 1 >= levelHolderSO.Levels.Count)
        {
            running = false;
            spawner.Stop();
            OnGameWon?.Invoke();
            return;
        }

        currentLevel++;
        LoadCurrent();
    }

    private void OnEnemyReachedBase()
    {
        if (!running)
            return;

        running = false;
        spawner.DespawnAll();
        OnGameLost?.Invoke();
    }
}
