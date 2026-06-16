using System.Collections;
using UnityEngine;
using TMPro;

public class TransitionBanner : MonoBehaviour
{
    [SerializeField] private TMP_Text label;
    [SerializeField] private RectTransform target;
    [SerializeField] private float slideDuration = 0.5f;
    [SerializeField] private float offScreenOffset = 1200f;

    private float centerX;
    private float leftX;
    private float rightX;
    private Coroutine routine;

    private void Awake()
    {
        centerX = target.anchoredPosition.x;
        leftX = centerX - offScreenOffset;
        rightX = centerX + offScreenOffset;
    }

    public void Play(int level, float duration)
    {
        if (label != null)
            label.text = $"Level {level + 1}";

        if (routine != null)
            StopCoroutine(routine);

        routine = StartCoroutine(PlayRoutine(duration));
    }

    private IEnumerator PlayRoutine(float duration)
    {
        SetX(leftX);

        float hold = Mathf.Max(0f, duration - slideDuration * 2f);

        yield return Slide(leftX, centerX);
        yield return new WaitForSeconds(hold);
        yield return Slide(centerX, rightX);
    }


    private void SetX(float x)
    {
        Vector2 pos = target.anchoredPosition;
        pos.x = x;
        target.anchoredPosition = pos;
    }

    private IEnumerator Slide(float from, float to)
    {
        float elapsed = 0f;

        while (elapsed < slideDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.SmoothStep(0f, 1f, elapsed / slideDuration);
            SetX(Mathf.Lerp(from, to, t));

            yield return null;
        }

        SetX(to);
    }
}