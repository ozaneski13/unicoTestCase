using UnityEngine;

public class AreaPulseAttack : TurretAttacker
{
    private Collider[] hits = new Collider[16];// cap hits per pulse; overflow enemies wait for next tick.

    protected override bool TryAttack()
    {
        int count = Physics.OverlapSphereNonAlloc(transform.position, worldRange, hits, enemyLayer);
        bool hitAny = false;

        for (int i = 0; i < count; i++)
        {
            IDamageable target = hits[i].GetComponentInParent<IDamageable>();

            if (target != null && !target.IsDead)
            {
                Attack(target);
                hitAny = true;
            }
        }

        return hitAny;
    }
}