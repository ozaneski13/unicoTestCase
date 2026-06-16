using UnityEngine;
using System;

public class EnemyMover : Movable
{
    private int column;
    private float rowPosition;
    private int currentRow;
    private GridController grid;
    private Turret blockingTurret;
    private bool reached;
    private bool stopped;
    private Vector3 moveDirection;

    public Vector2Int CurrentCell => new Vector2Int(column, currentRow);
    public Turret BlockingTurret => blockingTurret;
    public Vector3 MoveDirection => moveDirection;

    public event Action OnReachedBase;

    public void Init(int column, float startRow, GridController grid)
    {
        reached = false;
        stopped = false;

        this.column = column;
        this.rowPosition = startRow;
        this.currentRow = Mathf.RoundToInt(startRow);
        this.grid = grid;

        grid.TryGetTurret(CurrentCell, out blockingTurret);
        transform.position = grid.LaneToWorld(column, rowPosition);

        moveDirection = (grid.LaneToWorld(column, rowPosition - 1f) - transform.position).normalized;
    }

    public void Stop() => stopped = true;

    private void Update()
    {
        if (stopped)
            return;

        if (blockingTurret != null)
            return;

        rowPosition -= Time.deltaTime * Speed;

        int row = Mathf.RoundToInt(rowPosition);

        if (row != currentRow)
        {
            currentRow = row;
            grid.TryGetTurret(CurrentCell, out blockingTurret);
        }

        transform.position = grid.LaneToWorld(column, rowPosition);

        if (!reached && rowPosition <= 0f)
        {
            reached = true;
            OnReachedBase?.Invoke();
        }
    }
}