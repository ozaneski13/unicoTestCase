using System;

public interface IDamageable
{
    float Health { get; }
    float MaxHealth { get; }
    bool IsDead { get; }
    void TakeDamage(float amount);
    event Action<float, float> OnHealthChanged;// (current, max)
    event Action OnDeath;
}