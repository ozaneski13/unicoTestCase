using UnityEngine;

public class EnemyVisual : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform visual;
    [SerializeField] private Animator animator;

    [Header("Rotation")]
    [SerializeField] private float turnSpeed = 1080f;

    [Header("Animation States")]
    [SerializeField] private string moveState;
    [SerializeField] private string attackState;
    [SerializeField] private string deathState;
    [SerializeField] private float crossFade = 0.1f;

    private int moveHash;
    private int attackHash;
    private int deathHash;
    private int currentHash;

    private void Awake()
    {
        if (animator == null)
            animator = GetComponentInChildren<Animator>();

        moveHash = Animator.StringToHash(moveState);
        attackHash = Animator.StringToHash(attackState);
        deathHash = Animator.StringToHash(deathState);
    }

    private void OnEnable() => currentHash = 0;

    public void FaceDirection(Vector3 direction)
    {
        if (direction.sqrMagnitude < 0.0001f)
            return;

        Quaternion target = Quaternion.LookRotation(direction);
        visual.rotation = Quaternion.RotateTowards(visual.rotation, target, turnSpeed * Time.deltaTime);
    }

    public void PlayMove()
    {
        if (currentHash == moveHash)
            return;

        currentHash = moveHash;
        animator.CrossFadeInFixedTime(moveHash, crossFade);
    }

    public void PlayAttack()
    {
        currentHash = attackHash;
        animator.Play(attackHash, 0, 0f);
    }

    public void PlayDeath()
    {
        if (currentHash == deathHash)
            return;

        currentHash = deathHash;
        animator.CrossFadeInFixedTime(deathHash, crossFade);
    }
}
