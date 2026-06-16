using System.Collections.Generic;
using UnityEngine;
using System;

public class Inventory : MonoBehaviour
{
    private List<TurretInventory> turretInventory = new List<TurretInventory>();
    public List<TurretInventory> TurretInventory => turretInventory;

    private int selectedIndex = -1;
    public int SelectedIndex => selectedIndex;
    public Turret SelectedPrefab => selectedIndex >= 0 ? turretInventory[selectedIndex].TurretPrefab : null;

    public event Action OnChanged;

    public void Load(InventorySO source)
    {
        turretInventory.Clear();

        foreach (TurretInventory ti in source.TurretInventory)
            turretInventory.Add(new TurretInventory { TurretPrefab = ti.TurretPrefab, Icon = ti.Icon, MaxTurretAmount = ti.MaxTurretAmount, UsedAmount = ti.MaxTurretAmount });

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

    public void Deselect()
    {
        selectedIndex = -1;
        OnChanged?.Invoke();
    }

    public void ConsumeSelected()
    {
        if (!CanPlaceSelected())
            return;

        turretInventory[selectedIndex].UsedAmount--;
        OnChanged?.Invoke();
    }

    public bool CanPlaceSelected() => selectedIndex >= 0 && turretInventory[selectedIndex].UsedAmount > 0;
}