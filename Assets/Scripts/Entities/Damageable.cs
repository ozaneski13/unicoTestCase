using System;
using UnityEngine;

public class Damageable : MonoBehaviour, IDamageable
{
    [field: SerializeField] public float MaxHealth { get; private set; }
    public float Health { get; private set; }
    public bool IsDead { get; private set; }

    public event Action<float, float> OnHealthChanged;
    public event Action OnDeath;

    protected void OnEnable()
    {
        Health = MaxHealth;
        IsDead = false;

        OnHealthChanged?.Invoke(Health, MaxHealth);
    }

    public void TakeDamage(float amount)
    {
        if (IsDead)
            return;

        if (amount >= Health)
            Health = 0;
        else
            Health -= amount;

        OnHealthChanged?.Invoke(Health, MaxHealth);

        if (Health <= 0)
        {
            IsDead = true;
            OnDeath?.Invoke();
        }
    }
}