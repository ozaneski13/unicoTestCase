using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public class PlacementController : MonoBehaviour
{
    [SerializeField] private GridController grid;
    [SerializeField] private Camera mainCam;
    [SerializeField] private Inventory inventory;

    private Plane groundPlane;

    private void Start()
    {
        groundPlane = new Plane(Vector3.up, Vector3.zero);
    }

    private void Update()
    {
        if (Mouse.current == null || !Mouse.current.leftButton.wasPressedThisFrame)
            return;

        if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
            return;

        if (!inventory.CanPlaceSelected())
            return;

        Ray ray = mainCam.ScreenPointToRay(Mouse.current.position.ReadValue());

        if (!groundPlane.Raycast(ray, out float enter))
            return;

        Vector3 point = ray.GetPoint(enter);

        if (!grid.TryWorldToCell(point, out Vector2Int cell))
            return;

        if (!grid.CanPlace(cell))
            return;

        Turret turret = Instantiate(inventory.SelectedPrefab);
        grid.PlaceTurret(turret, cell);
        inventory.ConsumeSelected();
    }
}