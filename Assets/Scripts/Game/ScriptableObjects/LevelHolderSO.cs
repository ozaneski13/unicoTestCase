using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelHolderSO", menuName = "Level/LevelHolderSO")]
public class LevelHolderSO : ScriptableObject
{
    [SerializeField] private List<LevelSO> levels = new List<LevelSO>();
    public List<LevelSO> Levels => levels;
}