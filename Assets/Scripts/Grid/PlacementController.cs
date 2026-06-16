using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public class PlacementController : MonoBehaviour
{
    [SerializeField] private GridController grid;
    [SerializeField] private Camera mainCam;
    [SerializeField] private Inventory inventory;
    [SerializeField] private bool multiPlace;

    [Header("Transparent Turret Settings")]
    [SerializeField] private Material transparentValidMaterial;
    [SerializeField] private Material transparentInvalidMaterial;

    private Plane groundPlane;
    private GameObject transparent;
    private Turret transparentSource;
    private Renderer[] transparentRenderers;

    private void Start()
    {
        groundPlane = new Plane(Vector3.up, Vector3.zero);
    }

    public void SetMultiPlace(bool value)
    {
        multiPlace = value;
    }

    private void Update()
    {
        Turret selectedTurret = inventory.CanPlaceSelected() ? inventory.SelectedPrefab : null;

        if (selectedTurret == null)
        {
            ClearTransparent();
            return;
        }

        if (selectedTurret != transparentSource)
            BuildTransparent(selectedTurret);

        if (Mouse.current != null && Mouse.current.rightButton.wasPressedThisFrame)
        {
            inventory.Deselect();
            return;
        }

        if (PointerOverUI() || !TryGetHoveredCell(out Vector2Int cell))
        {
            transparent.SetActive(false);
            return;
        }

        transparent.SetActive(true);
        transparent.transform.position = grid.CellToWorld(cell);

        bool valid = grid.CanPlace(cell);
        SetTransparentMaterial(valid ? transparentValidMaterial : transparentInvalidMaterial);

        if (valid && Mouse.current.leftButton.wasPressedThisFrame)
            Place(cell);
    }

    private void ClearTransparent()
    {
        if (transparent != null)
            Destroy(transparent);

        transparent = null;
        transparentSource = null;
        transparentRenderers = null;
    }

    private void BuildTransparent(Turret prefab)
    {
        ClearTransparent();

        transparent = Instantiate(prefab).gameObject;
        transparentSource = prefab;

        foreach (MonoBehaviour behaviour in transparent.GetComponentsInChildren<MonoBehaviour>())
            behaviour.enabled = false;

        foreach (Collider col in transparent.GetComponentsInChildren<Collider>())
            col.enabled = false;

        transparentRenderers = transparent.GetComponentsInChildren<Renderer>();
    }

    private bool PointerOverUI()
    {
        return EventSystem.current != null && EventSystem.current.IsPointerOverGameObject();
    }

    private bool TryGetHoveredCell(out Vector2Int cell)
    {
        cell = default;

        if (Mouse.current == null)
            return false;

        Ray ray = mainCam.ScreenPointToRay(Mouse.current.position.ReadValue());

        if (!groundPlane.Raycast(ray, out float enter))
            return false;

        return grid.TryWorldToCell(ray.GetPoint(enter), out cell);
    }

    private void SetTransparentMaterial(Material material)
    {
        foreach (Renderer renderer in transparentRenderers)
            renderer.sharedMaterial = material;
    }

    private void Place(Vector2Int cell)
    {
        Turret turret = Instantiate(inventory.SelectedPrefab, transform);
        grid.PlaceTurret(turret, cell);
        inventory.ConsumeSelected();

        if (!multiPlace || !inventory.CanPlaceSelected())
            inventory.Deselect();
    }
}