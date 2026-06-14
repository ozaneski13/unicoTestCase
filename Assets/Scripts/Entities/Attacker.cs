using UnityEngine;

public class Attacker : MonoBehaviour
{
    [field: SerializeField] public float Damage { get; private set; }
    [field: SerializeField] public float Interval { get; private set; }

    public void Attack(IDamageable target)
    {
        target.TakeDamage(Damage);
    }
}