using UnityEngine;
using System;

[CreateAssetMenu(menuName = "Game/EnemyReleasedSO")]
public class EnemyReleasedSO : ScriptableObject
{
    private event Action<Enemy> OnEnemyReleased;

    private void OnEnable()
    {
        OnEnemyReleased = null;
    }

    public void Subscribe(Action<Enemy> callback)
    {
        OnEnemyReleased += callback;
    }

    public void Unsubscribe(Action<Enemy> callback)
    {
        OnEnemyReleased -= callback;
    }

    public void Fire(Enemy enemy)
    {
        OnEnemyReleased?.Invoke(enemy);
    }
}