using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Events")]
    [SerializeField] private EnemyReleasedSO enemyReleasedSO;

    [Header("Pool")]
    [SerializeField] private int poolSize;
    [SerializeField] private int poolThreshold;
    [SerializeField] private Transform poolHolder;

    [Header("Spawn")]
    [SerializeField] private GridController gridController;

    private Dictionary<EEnemyType, Enemy> prefabByType = new Dictionary<EEnemyType, Enemy>();
    private Dictionary<EEnemyType, List<Enemy>> enemyPool = new Dictionary<EEnemyType, List<Enemy>>();
    private List<Enemy> activeEnemies = new List<Enemy>();
    private Coroutine spawnRoutine;
    private bool spawnFinished;

    public event Action OnLevelCleared;

    private void Awake()
    {
        enemyReleasedSO.Subscribe(OnEnemyRefill);
    }

    public void Begin(SpawnSettingsSO settings)
    {
        Stop();

        spawnFinished = false;

        RegisterPrefabs(settings.WaveSO);

        spawnRoutine = StartCoroutine(SpawnRoutine(settings));
    }

    public void Stop()
    {
        if (spawnRoutine == null)
            return;

        StopCoroutine(spawnRoutine);
        spawnRoutine = null;
    }

    private void RegisterPrefabs(WaveSO waves)
    {
        foreach (Wave wave in waves.Waves)
            prefabByType[wave.EnemyPrefab.Type] = wave.EnemyPrefab;
    }

    private IEnumerator SpawnRoutine(SpawnSettingsSO settings)
    {
        yield return new WaitForSeconds(settings.FirstSpawnDelay);

        foreach (Wave wave in settings.WaveSO.Waves)
        {
            for (int i = 0; i < wave.Amount; i++)
            {
                SpawnEnemy(wave.EnemyPrefab.Type);
                yield return new WaitForSeconds(settings.SpawnInterval);
            }
        }

        spawnFinished = true;
        CheckCleared();
    }

    private void SpawnEnemy(EEnemyType type)
    {
        if (!enemyPool.ContainsKey(type) || enemyPool[type].Count == 0)
            FillPool(type, poolSize);

        Enemy enemy = enemyPool[type][^1];
        enemyPool[type].Remove(enemy);

        enemy.gameObject.SetActive(true);
        enemy.Init(UnityEngine.Random.Range(0, gridController.Columns), gridController.Rows - 1, gridController);
        activeEnemies.Add(enemy);

        if (enemyPool[type].Count < poolThreshold)
            FillPool(type, poolSize);
    }

    private void FillPool(EEnemyType type, int amount)
    {
        Enemy prefab = prefabByType[type];

        for (int i = 0; i < amount; i++)
        {
            Enemy enemy = Instantiate(prefab, poolHolder);
            enemy.gameObject.SetActive(false);

            if (!enemyPool.ContainsKey(type))
                enemyPool[type] = new List<Enemy>();

            enemyPool[type].Add(enemy);
        }
    }

    private void OnEnemyRefill(Enemy enemy)
    {
        enemy.gameObject.SetActive(false);
        activeEnemies.Remove(enemy);
        enemyPool[enemy.Type].Add(enemy);

        if (enemyPool[enemy.Type].Count < poolThreshold)
            FillPool(enemy.Type, poolSize);

        CheckCleared();
    }

    private void CheckCleared()
    {
        if (spawnFinished && activeEnemies.Count == 0)
            OnLevelCleared?.Invoke();
    }

    public void DespawnAll()
    {
        Stop();
        spawnFinished = false;

        for (int i = activeEnemies.Count - 1; i >= 0; i--)
        {
            Enemy enemy = activeEnemies[i];
            enemy.gameObject.SetActive(false);
            enemyPool[enemy.Type].Add(enemy);
        }

        activeEnemies.Clear();
    }

    private void OnDestroy()
    {
        enemyReleasedSO.Unsubscribe(OnEnemyRefill);
    }
}