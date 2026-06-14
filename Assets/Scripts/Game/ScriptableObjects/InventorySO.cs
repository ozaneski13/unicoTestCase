using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TurretInventory
{
    public Turret TurretPrefab;
    public int MaxTurretAmount;
    public int UsedAmount;
}

[CreateAssetMenu(fileName = "InventorySO", menuName = "Turret/InventorySO")]
public class InventorySO : ScriptableObject
{
    [SerializeField] private List<TurretInventory> turretInventory = new List<TurretInventory>();
    public List<TurretInventory> TurretInventory => turretInventory;
}