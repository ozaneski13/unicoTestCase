using UnityEngine;

[CreateAssetMenu(fileName = "LevelSO", menuName = "Level/LevelSO")]
public class LevelSO : ScriptableObject
{
    [SerializeField] private SpawnSettingsSO spawn;
    [SerializeField] private InventorySO inventory;

    public SpawnSettingsSO Spawn => spawn;
    public InventorySO Inventory => inventory;
}