using UnityEngine;

public abstract class TurretAttacker : Attacker
{
    [SerializeField] protected float range;
    [SerializeField] protected LayerMask enemyLayer;

    protected float worldRange;
    protected float cellWorldSize;
    private float cooldown;

    public void Setup(float cellWorldSize)
    {
        this.cellWorldSize = cellWorldSize;
        worldRange = range * cellWorldSize;// range convert to world units.
        cooldown = 0f;

        OnSetup();
    }

    protected virtual void OnSetup() { }

    private void Update()
    {
        cooldown -= Time.deltaTime;

        if (cooldown > 0f)
            return;

        if (TryAttack())
            cooldown = Interval;
    }

    protected abstract bool TryAttack();
}