using UnityEngine;

public class Turret : MonoBehaviour
{
    [SerializeField] private TurretHealth health;
    [SerializeField] private TurretAttacker attack;

    [Header("Death")]
    [SerializeField] private GameObject deathVfxPrefab;
    [SerializeField] private float deathVfxLifeTime = 2f;

    public Vector2Int Cell { get; private set; }
    public IDamageable Damageable => health;

    private GridController grid;

    public void Place(Vector2Int cell, GridController grid)
    {
        Cell = cell;
        this.grid = grid;
        transform.position = grid.CellToWorld(cell);
        attack.Setup(grid.CellWorldSize);

        RegisterToEvents();
    }

    private void RegisterToEvents()
    {
        health.OnDeath += OnDeath;
    }

    private void OnDeath()
    {
        grid.RemoveTurret(Cell);

        SpawnDeathVfx();

        Destroy(gameObject);
    }

    private void SpawnDeathVfx()
    {
        if (deathVfxPrefab == null)
            return;

        GameObject vfx = Instantiate(deathVfxPrefab, transform.position, Quaternion.identity);
        Destroy(vfx, deathVfxLifeTime);
    }

    private void OnDestroy()
    {
        UnregisterFromEvents();
    }

    private void UnregisterFromEvents()
    {
        health.OnDeath -= OnDeath;
    }
}