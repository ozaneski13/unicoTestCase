using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Damageable target;
    [SerializeField] private Image fill;
    [SerializeField] private Gradient gradient;

    private Camera cam;

    private void Awake()
    {
        cam = Camera.main;
    }

    private void OnEnable()
    {
        RegisterToEvents();

        Refresh(target.Health, target.MaxHealth);
    }

    private void RegisterToEvents()
    {
        target.OnHealthChanged += OnHealthChanged;
    }

    private void OnHealthChanged(float current, float max)
    {
        Refresh(current, max);
    }

    private void Refresh(float current, float max)
    {
        float fraction = max > 0f ? current / max : 0f;

        fill.fillAmount = fraction;
        fill.color = gradient.Evaluate(fraction);
    }

    private void LateUpdate()
    {
        if (cam != null)
            transform.forward = cam.transform.forward;
    }

    private void OnDisable()
    {
        UnregisterFromEvents();
    }

    private void UnregisterFromEvents()
    {
        target.OnHealthChanged -= OnHealthChanged;
    }
}