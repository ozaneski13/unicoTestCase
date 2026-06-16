using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private EnemyHealth health;
    [SerializeField] private EnemyAttacker attacker;
    [SerializeField] private EnemyMover mover;
    [SerializeField] private EnemyVisual visual;
    [SerializeField] private EEnemyType type;
    public EEnemyType Type => type;

    [Header("Events")]
    [SerializeField] private EnemyReleasedSO enemyReleasedSO;
    [SerializeField] private EnemyReachedBaseSO enemyReachedBaseSO;

    [Header("Death")]
    [SerializeField] private Collider body;
    [SerializeField] private float deathDuration = 1.5f;

    private GridController grid;
    private bool dead;

    public void Init(int column, float startRow, GridController grid)
    {
        this.grid = grid;
        mover.Init(column, startRow, grid);
    }

    private void OnEnable()
    {
        dead = false;

        if (body != null)
            body.enabled = true;

        RegisterToEvents();
    }

    private void RegisterToEvents()
    {
        health.OnDeath += OnEnemyDeath;
        mover.OnReachedBase += OnReachedBase;
    }

    private void OnEnemyDeath()
    {
        dead = true;

        mover.Stop();

        if (body != null)
            body.enabled = false;

        visual.PlayDeath();

        StartCoroutine(ReleaseAfterDeath());
    }

    private IEnumerator ReleaseAfterDeath()
    {
        yield return new WaitForSeconds(deathDuration);

        enemyReleasedSO.Fire(this);
    }

    private void OnReachedBase()
    {
        enemyReachedBaseSO.Fire();
    }

    private void Update()
    {
        if (dead)
            return;

        Turret turret = mover.BlockingTurret;

        if (turret != null)
        {
            if (attacker.TickAttack(turret.Damageable))
                visual.PlayAttack();

            return;
        }

        visual.PlayMove();
        visual.FaceDirection(mover.MoveDirection);
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