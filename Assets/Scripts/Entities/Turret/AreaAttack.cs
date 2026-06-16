using System.Collections;
using UnityEngine;

public class AreaAttack : TurretAttacker
{
    [SerializeField] private GameObject vfxPrefab;
    [SerializeField] private float vfxHeight;
    [SerializeField] private float impactDelay;
    [SerializeField] private float vfxLifeTime;

    private Collider[] hits = new Collider[16];

    protected override bool TryAttack()
    {
        if (!HasTargetInRange())
            return false;

        SpawnVfx();
        StartCoroutine(ApplyDamageAfterDelay());

        return true;
    }

    private bool HasTargetInRange()
    {
        int count = Physics.OverlapBoxNonAlloc(transform.position, HalfExtents(), hits, Quaternion.identity, enemyLayer);

        for (int i = 0; i < count; i++)
        {
            IDamageable target = hits[i].GetComponentInParent<IDamageable>();

            if (target != null && !target.IsDead)
                return true;
        }

        return false;
    }

    private void SpawnVfx()
    {
        if (vfxPrefab == null)
            return;

        GameObject vfx = Instantiate(vfxPrefab, transform.position + Vector3.up * vfxHeight, Quaternion.identity);
        Destroy(vfx, vfxLifeTime);
    }

    private IEnumerator ApplyDamageAfterDelay()
    {
        yield return new WaitForSeconds(impactDelay);

        DamageEnemiesInRange();
    }

    private void DamageEnemiesInRange()
    {
        int count = Physics.OverlapBoxNonAlloc(transform.position, HalfExtents(), hits, Quaternion.identity, enemyLayer);

        for (int i = 0; i < count; i++)
        {
            IDamageable target = hits[i].GetComponentInParent<IDamageable>();

            if (target != null && !target.IsDead)
                Attack(target);
        }
    }

    private Vector3 HalfExtents()
    {
        float half = worldRange + cellWorldSize * 0.5f;
        return new Vector3(half, half, half);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, HalfExtents() * 2f);
    }
}
