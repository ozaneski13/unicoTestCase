using UnityEngine;

public class EnemyAttacker : Attacker
{
    private float cooldown;

    private void OnEnable() => cooldown = 0f;

    public void TickAttack(IDamageable target)
    {
        cooldown -= Time.deltaTime;

        if (cooldown > 0f)
            return;

        Attack(target);
        cooldown = Interval;
    }
}