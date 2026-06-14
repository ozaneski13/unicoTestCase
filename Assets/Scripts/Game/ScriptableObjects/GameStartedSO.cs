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

    public void Subscribe(Action listener)
    {
        OnGameStarted += listener;
    }

    public void Unsubscribe(Action listener)
    {
        OnGameStarted -= listener;
    }

    public void Fire()
    {
        OnGameStarted?.Invoke();
    }
}