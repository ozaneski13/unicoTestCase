using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Wave
{
    public Enemy EnemyPrefab;
    [Min(0)] public int Amount;
}

[CreateAssetMenu(fileName = "WaveSO", menuName = "Enemy/WaveSO")]
public class WaveSO : ScriptableObject
{
    [SerializeField] private List<Wave> waves;
    public List<Wave> Waves => waves;
}