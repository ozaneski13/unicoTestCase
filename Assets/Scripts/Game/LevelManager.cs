using UnityEngine;
using System;
using System.Collections;

public class LevelManager : MonoBehaviour
{
    [Header("Data")]
    [SerializeField] private LevelHolderSO levelHolderSO;

    [Header("Controllers")]
    [SerializeField] private Inventory inventory;
    [SerializeField] private EnemySpawner spawner;
    [SerializeField] private GridController grid;

    [Header("Events")]
    [SerializeField] private GameStartedSO gameStartedSO;
    [SerializeField] private EnemyReachedBaseSO enemyReachedBaseSO;

    [Header("Transition")]
    [SerializeField] private float transitionDuration = 3.5f;

    private int currentLevel = -1;
    private bool running;

    public event Action<int> OnLevelStarted;
    public event Action<int> OnLevelTransition;
    public event Action OnGameWon;
    public event Action OnGameLost;

    public int CurrentLevel => currentLevel;
    public float TransitionDuration => transitionDuration;

    private void Awake()
    {
        RegisterToEvents();
    }

    private void RegisterToEvents()
    {
        gameStartedSO.Subscribe(StartGame);
        enemyReachedBaseSO.Subscribe(OnEnemyReachedBase);

        spawner.OnLevelCleared += OnLevelCleared;
    }

    public void StartGame()
    {
        currentLevel = 0;

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

        running = false;
        currentLevel++;

        StartCoroutine(TransitionToCurrent());
    }

    private IEnumerator TransitionToCurrent()
    {
        OnLevelTransition?.Invoke(currentLevel);

        yield return new WaitForSeconds(transitionDuration);

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

    private void OnDestroy()
    {
        UnregisterFromEvents();
    }

    private void UnregisterFromEvents()
    {
        gameStartedSO.Unsubscribe(StartGame);
        enemyReachedBaseSO.Unsubscribe(OnEnemyReachedBase);

        spawner.OnLevelCleared -= OnLevelCleared;
    }
}