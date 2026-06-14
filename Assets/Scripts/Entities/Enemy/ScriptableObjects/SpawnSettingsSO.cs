using UnityEngine;

[CreateAssetMenu(fileName = "SpawnSettingsSO", menuName = "ScriptableObjects/SpawnSettingsSO")]
public class SpawnSettingsSO : ScriptableObject
{
    [SerializeField] private float firstSpawnDelay;
    [SerializeField] private float spawnInterval;
    [SerializeField] private WaveSO waveSO;

    public float FirstSpawnDelay => firstSpawnDelay;
    public float SpawnInterval => spawnInterval;
    public WaveSO WaveSO => waveSO;
}