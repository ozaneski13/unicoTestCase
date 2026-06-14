using UnityEngine;

public class Turret : MonoBehaviour
{
    [SerializeField] private Damageable health;
    [SerializeField] private TurretAttacker attack;

    public Vector2Int Cell { get; private set; }
    public IDamageable Damageable => health;

    private GridController grid;

    public void Place(Vector2Int cell, GridController grid)
    {
        Cell = cell;
        this.grid = grid;
        transform.position = grid.CellToWorld(cell);
        attack.Setup(grid.CellWorldSize);

        health.OnDeath += OnDeath;
    }

    private void OnDeath()
    {
        grid.RemoveTurret(Cell);
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        health.OnDeath -= OnDeath;
    }
}