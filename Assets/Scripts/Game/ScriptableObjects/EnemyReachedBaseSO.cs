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

    public void Subscribe(Action callback)
    {
        OnReached += callback;
    }

    public void Unsubscribe(Action callback)
    {
        OnReached -= callback;
    }

    public void Fire()
    {
        OnReached?.Invoke();
    }
}