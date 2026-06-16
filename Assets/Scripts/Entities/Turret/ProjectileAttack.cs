using UnityEngine;
using System.Collections.Generic;

public class ProjectileAttack : TurretAttacker
{
    [SerializeField] private Projectile projectilePrefab;
    [SerializeField] private GameObject muzzle;
    [SerializeField] private float radius;
    [SerializeField] private float height;
    [SerializeField] private int poolSize;

    private List<Projectile> pool = new List<Projectile>();
    private ParticleSystem[] muzzleSystems;

    protected override void OnSetup()
    {
        FillPool(poolSize);

        if (muzzle != null)
            muzzleSystems = muzzle.GetComponentsInChildren<ParticleSystem>(true);
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

        if (!Physics.BoxCast(origin, new Vector3(cellWorldSize * 0.5f, radius, radius), dir, out _, transform.rotation, worldRange, enemyLayer))
            return false;

        Projectile p = GetProjectile();
        p.Fire(origin, dir, Damage, worldRange, enemyLayer, ReturnToPool);

        PlayMuzzle();

        return true;
    }

    private void PlayMuzzle()
    {
        if (muzzleSystems == null)
            return;

        foreach (ParticleSystem system in muzzleSystems)
            system.Play(false);
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

    private void OnDrawGizmosSelected()
    {
        Vector3 origin = transform.position + Vector3.up * height;
        Vector3 center = origin + transform.forward * (worldRange * 0.5f);
        Vector3 size = new Vector3(cellWorldSize, radius, worldRange);

        Gizmos.color = Color.red;
        Gizmos.matrix = Matrix4x4.TRS(center, transform.rotation, Vector3.one);
        Gizmos.DrawWireCube(Vector3.zero, size);
    }
}