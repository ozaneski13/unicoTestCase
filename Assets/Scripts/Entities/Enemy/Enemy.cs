using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private EnemyHealth health;
    [SerializeField] private EnemyAttacker attacker;
    [SerializeField] private EnemyMover mover;
    [SerializeField] private EEnemyType type;
    public EEnemyType Type => type;

    [Header("Events")]
    [SerializeField] private EnemyReleasedSO enemyReleasedSO;
    [SerializeField] private EnemyReachedBaseSO enemyReachedBaseSO;

    private GridController grid;

    public void Init(int column, float startRow, GridController grid)
    {
        this.grid = grid;
        mover.Init(column, startRow, grid);
    }

    private void OnEnable()
    {
        RegisterToEvents();
    }

    private void RegisterToEvents()
    {
        health.OnDeath += OnEnemyDeath;
        mover.OnReachedBase += OnReachedBase;
    }

    private void OnEnemyDeath()
    {
        enemyReleasedSO.Fire(this);
    }

    private void OnReachedBase()
    {
        enemyReachedBaseSO.Fire();
    }

    private void Update()
    {
        Turret turret = mover.BlockingTurret;

        if (turret != null)
            attacker.TickAttack(turret.Damageable);
    }

    private void OnDisable()
    {
        UnregisterFromEvents();
    }

    private void UnregisterFromEvents()
    {
        health.OnDeath -= OnEnemyDeath;
        mover.OnReachedBase -= OnReachedBase;
    }
}

public enum EEnemyType
{
    Normal,
    Fast,
    Tank
}