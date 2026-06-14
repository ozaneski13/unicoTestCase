using UnityEngine;
using System;

[CreateAssetMenu(menuName = "Game/EnemyReachedBaseSO")]
public class EnemyReachedBaseSO : ScriptableObject
{
    private event Action OnReached;

    private void OnEnable()
    {
        OnReached = null;
    }

    public void Subscribe(Action listener)
    {
        OnReached += listener;
    }

    public void Unsubscribe(Action listener)
    {
        OnReached -= listener;
    }

    public void Fire()
    {
        OnReached?.Invoke();
    }
}
