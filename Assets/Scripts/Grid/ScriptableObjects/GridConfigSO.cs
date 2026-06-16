using UnityEngine;

[CreateAssetMenu(fileName = "GridConfigSO", menuName = "Grid/GridConfigSO")]
public class GridConfigSO : ScriptableObject
{
    [SerializeField] private int columns;
    public int Columns => columns;
    [SerializeField] private int rows;
    public int Rows => rows;
    [SerializeField] private float cellSize;
    public float CellSize => cellSize;
    [SerializeField] private float cellSpacing;
    public float CellSpacing => cellSpacing;
    [SerializeField] private int placementRows;

    public bool IsValid(Vector2Int c) => c.x >= 0 && c.x < columns && c.y >= 0 && c.y < rows;
    public bool IsInPlacementZone(Vector2Int c) => IsValid(c) && c.y < placementRows;
}