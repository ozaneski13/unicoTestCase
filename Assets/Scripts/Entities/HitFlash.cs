using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitFlash : MonoBehaviour
{
    [SerializeField] private Damageable target;
    [SerializeField] private Color flashColor = Color.red;
    [SerializeField] private float duration = 0.1f;
    [SerializeField] private string colorProperty = "_BaseColor";

    private Renderer[] renderers;
    private MaterialPropertyBlock block;
    private int colorId;
    private Color[] baseColors;
    private float lastHealth;
    private Coroutine routine;

    private void Awake()
    {
        if (renderers == null || renderers.Length == 0)
            renderers = CollectMeshRenderers();

        block = new MaterialPropertyBlock();
        colorId = Shader.PropertyToID(colorProperty);

        baseColors = new Color[renderers.Length];
        for (int i = 0; i < renderers.Length; i++)
            baseColors[i] = renderers[i].sharedMaterial.GetColor(colorId);
    }

    private Renderer[] CollectMeshRenderers()
    {
        List<Renderer> found = new List<Renderer>();

        foreach (Renderer renderer in GetComponentsInChildren<Renderer>())
            if (renderer is MeshRenderer || renderer is SkinnedMeshRenderer)
                found.Add(renderer);

        return found.ToArray();
    }

    private void OnEnable()
    {
        lastHealth = target.Health;

        RegisterToEvents();
    }

    private void RegisterToEvents()
    {
        target.OnHealthChanged += OnHealthChanged;
    }

    private void OnHealthChanged(float current, float max)
    {
        if (current < lastHealth)
            Flash();

        lastHealth = current;
    }

    private void Flash()
    {
        if (routine != null)
            StopCoroutine(routine);

        routine = StartCoroutine(FlashRoutine());
    }

    private IEnumerator FlashRoutine()
    {
        SetColor(flashColor);

        yield return new WaitForSeconds(duration);

        Restore();
    }

    private void SetColor(Color color)
    {
        foreach (Renderer renderer in renderers)
        {
            renderer.GetPropertyBlock(block);
            block.SetColor(colorId, color);
            renderer.SetPropertyBlock(block);
        }
    }

    private void Restore()
    {
        for (int i = 0; i < renderers.Length; i++)
        {
            renderers[i].GetPropertyBlock(block);
            block.SetColor(colorId, baseColors[i]);
            renderers[i].SetPropertyBlock(block);
        }
    }

    private void OnDisable()
    {
        UnregisterFromEvents();

        Restore();
    }

    private void OnDestroy()
    {
        UnregisterFromEvents();

        Restore();
    }

    private void UnregisterFromEvents()
    {
        target.OnHealthChanged -= OnHealthChanged;
    }
}