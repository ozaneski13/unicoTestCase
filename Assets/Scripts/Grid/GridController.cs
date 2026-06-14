using System.Collections.Generic;
using UnityEngine;

public class GridController : MonoBehaviour
{
    [SerializeField] private GridConfigSO config;
    [SerializeField] private Transform origin;

    public int Columns => config.Columns;
    public int Rows => config.Rows;
    public float CellWorldSize => config.CellSize + config.CellSpacing;

    private Dictionary<Vector2, GameObject> cells = new Dictionary<Vector2, GameObject>();
    private List<Material> cellMaterials = new List<Material>();

    private Turret[,] turrets;

    private void Awake()
    {
        turrets = new Turret[config.Columns, config.Rows];
    }

    private void Start()
    {
        InitGrids();
    }

    private void InitGrids()
    {
        for (int x = 0; x < config.Columns; x++)
        {
            for (int z = 0; z < config.Rows; z++)
            {
                GameObject cell = GameObject.CreatePrimitive(PrimitiveType.Quad);

                cell.transform.SetParent(origin);
                cell.transform.localPosition = new Vector3(x * CellWorldSize, 0, z * CellWorldSize);
                cell.transform.localScale = Vector3.one * config.CellSize;
                cell.transform.rotation = Quaternion.Euler(90, 0, 0);
                Vector2Int cellIndex = new Vector2Int(x, z);

                Material material = cell.GetComponent<MeshRenderer>().material;
                material.color = config.IsInPlacementZone(cellIndex) ? Color.green : Color.white;
                cellMaterials.Add(material);

                cells.Add(cellIndex, cell);
            }
        }
    }

    public Vector3 CellToWorld(Vector2Int cell)
    {
        return origin.position + new Vector3(cell.x * CellWorldSize, 0, cell.y * CellWorldSize);
    }

    public Vector3 LaneToWorld(int column, float row)
    {
        return origin.position + new Vector3(column * CellWorldSize, 0, row * CellWorldSize);
    }

    public bool TryWorldToCell(Vector3 world, out Vector2Int cell)
    {
        Vector3 local = world - origin.position;
        cell = new Vector2Int(Mathf.RoundToInt(local.x / CellWorldSize), Mathf.RoundToInt(local.z / CellWorldSize));

        return config.IsValid(cell);
    }

    public bool CanPlace(Vector2Int cell)
    {
        return config.IsInPlacementZone(cell) && turrets[cell.x, cell.y] == null;
    }

    public void PlaceTurret(Turret turret, Vector2Int cell)
    {
        turrets[cell.x, cell.y] = turret;
        turret.Place(cell, this);
    }

    public bool TryGetTurret(Vector2Int cell, out Turret turret)
    {
        turret = config.IsValid(cell) ? turrets[cell.x, cell.y] : null;

        return turret != null;
    }

    public void RemoveTurret(Vector2Int cell)
    {
        if (config.IsValid(cell))
            turrets[cell.x, cell.y] = null;
    }

    public void ClearTurrets()
    {
        for (int x = 0; x < config.Columns; x++)
        {
            for (int y = 0; y < config.Rows; y++)
            {
                if (turrets[x, y] == null)
                    continue;

                Destroy(turrets[x, y].gameObject);
                turrets[x, y] = null;
            }
        }
    }

    private void OnDestroy()
    {
        foreach (Material material in cellMaterials)
            Destroy(material);
    }
}