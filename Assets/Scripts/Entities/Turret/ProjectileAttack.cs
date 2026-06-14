using UnityEngine;
using System.Collections.Generic;

public class ProjectileAttack : TurretAttacker
{
    [SerializeField] private Projectile projectilePrefab;
    [SerializeField] private float radius = 0.4f;
    [SerializeField] private float height = 0.5f;
    [SerializeField] private int poolSize = 8;

    private List<Projectile> pool = new List<Projectile>();

    private void Awake()
    {
        FillPool(poolSize);
    }

    private void FillPool(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            Projectile p = Instantiate(projectilePrefab, transform);
            p.gameObject.SetActive(false);
            pool.Add(p);
        }
    }

    protected override bool TryAttack()
    {
        Vector3 dir = transform.forward;
        Vector3 origin = transform.position + Vector3.up * height;

        if (!Physics.SphereCast(origin, radius, dir, out _, worldRange, enemyLayer))
            return false;

        Projectile p = GetProjectile();
        p.Fire(origin, dir, Damage, worldRange, enemyLayer, ReturnToPool);

        return true;
    }

    private Projectile GetProjectile()
    {
        if (pool.Count == 0)
            FillPool(poolSize);

        Projectile p = pool[^1];
        pool.Remove(p);
        p.gameObject.SetActive(true);

        return p;
    }

    private void ReturnToPool(Projectile p)
    {
        p.gameObject.SetActive(false);
        pool.Add(p);
    }
}