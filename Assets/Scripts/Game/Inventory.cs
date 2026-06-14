using System.Collections.Generic;
using UnityEngine;
using System;

public class Inventory : MonoBehaviour
{
    private List<TurretInventory> turretInventory = new List<TurretInventory>();
    private int selectedIndex = -1;

    public event Action OnChanged;

    public List<TurretInventory> TurretInventory => turretInventory;
    public int SelectedIndex => selectedIndex;
    public Turret SelectedPrefab => selectedIndex >= 0 ? turretInventory[selectedIndex].TurretPrefab : null;

    public bool CanPlaceSelected() => selectedIndex >= 0 && turretInventory[selectedIndex].UsedAmount > 0;

    public void Load(InventorySO source)
    {
        turretInventory.Clear();

        foreach (TurretInventory ti in source.TurretInventory)
            turretInventory.Add(new TurretInventory { TurretPrefab = ti.TurretPrefab, MaxTurretAmount = ti.MaxTurretAmount, UsedAmount = ti.MaxTurretAmount });

        selectedIndex = -1;
        OnChanged?.Invoke();
    }

    public void Select(int index)
    {
        if (index < 0 || index >= turretInventory.Count)
            return;

        selectedIndex = index;
        OnChanged?.Invoke();
    }

    public void ConsumeSelected()
    {
        if (!CanPlaceSelected())
            return;

        turretInventory[selectedIndex].UsedAmount--;
        OnChanged?.Invoke();
    }
}