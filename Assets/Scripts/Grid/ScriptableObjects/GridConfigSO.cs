using UnityEngine;

[CreateAssetMenu(fileName = "GridConfigSO", menuName = "Grid/GridConfigSO")]
public class GridConfigSO : ScriptableObject
{
    [SerializeField] private int columns;
    [SerializeField] private int rows;
    [SerializeField] private float cellSize;
    [SerializeField] private float cellSpacing;
    [SerializeField] private int placementRows;

    public int Columns => columns;
    public int Rows => rows;
    public float CellSize => cellSize;
    public float CellSpacing => cellSpacing;
    public int PlacementRows => placementRows;

    public bool IsValid(Vector2Int c) => c.x >= 0 && c.x < columns && c.y >= 0 && c.y < rows;
    public bool IsInPlacementZone(Vector2Int c) => IsValid(c) && c.y < placementRows;
}