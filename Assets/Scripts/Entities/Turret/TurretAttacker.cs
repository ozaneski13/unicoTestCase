using UnityEngine;

public abstract class TurretAttacker : Attacker
{
    [SerializeField] protected float range;
    [SerializeField] protected LayerMask enemyLayer;

    protected float worldRange;
    private float cooldown;

    public void Setup(float cellWorldSize)
    {
        worldRange = range * cellWorldSize;// range convert to world units.
        cooldown = 0f;
    }

    private void Update()
    {
        cooldown -= Time.deltaTime;

        if (cooldown > 0f)
            return;

        TryAttack();
        cooldown = Interval;
    }

    protected abstract bool TryAttack();
}