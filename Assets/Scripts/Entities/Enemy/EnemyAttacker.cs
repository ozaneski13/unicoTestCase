using UnityEngine;

public class EnemyAttacker : Attacker
{
    private float cooldown;

    private void OnEnable() => cooldown = 0f;

    public bool TickAttack(IDamageable target)
    {
        cooldown -= Time.deltaTime;

        if (cooldown > 0f)
            return false;

        Attack(target);
        cooldown = Interval;

        return true;
    }
}