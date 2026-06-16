using UnityEngine;
using System;

[CreateAssetMenu(menuName = "Game/GameStartedSO")]
public class GameStartedSO : ScriptableObject
{
    private event Action OnGameStarted;

    private void OnEnable()
    {
        OnGameStarted = null;
    }

    public void Subscribe(Action callback)
    {
        OnGameStarted += callback;
    }

    public void Unsubscribe(Action callback)
    {
        OnGameStarted -= callback;
    }

    public void Fire()
    {
        OnGameStarted?.Invoke();
    }
}