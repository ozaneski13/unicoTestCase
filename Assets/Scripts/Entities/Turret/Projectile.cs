using UnityEngine;
using System;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float speed = 12f;

    private LayerMask targetLayer;
    private Action<Projectile> onFinished;

    private float maxDistance;
    private float traveled;
    private float damage;

    private bool isReleased;

    public void Fire(Vector3 position, Vector3 direction, float damage, float maxDistance, LayerMask targetLayer, Action<Projectile> onFinished)
    {
        transform.SetPositionAndRotation(position, Quaternion.LookRotation(direction));

        this.damage = damage;
        this.maxDistance = maxDistance;
        this.targetLayer = targetLayer;
        this.onFinished = onFinished;

        traveled = 0f;
        isReleased = false;
    }

    private void Update()
    {
        if (isReleased)
            return;

        float step = speed * Time.deltaTime;
        transform.position += transform.forward * step;
        traveled += step;

        if (traveled >= maxDistance)
            Release();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isReleased)
            return;

        if ((targetLayer.value & (1 << other.gameObject.layer)) == 0)
            return;

        IDamageable target = other.GetComponentInParent<IDamageable>();

        if (target != null && !target.IsDead)
            target.TakeDamage(damage);

        Release();
    }

    private void Release()
    {
        if (isReleased)
            return;

        isReleased = true;
        onFinished?.Invoke(this);
    }
}